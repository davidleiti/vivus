namespace Vivus.Core.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Twilio.Exceptions;
    using Vivus.Core.DataModels;
    using Vivus.Core.Administration.IoC;
    using Vivus.Core.Helpers;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using VivusConsole = Console.Console;
    using BCrypt.Net;
    using Vivus.Core.Security;

    /// <summary>
    /// Represents a view model for the forgot password page.
    /// </summary>
    public class ForgotPasswordViewModel : BaseViewModel
    {
        #region Private Members

        private string phoneNumber;
        private string resetCode;
        private object password;
        private PageStep pageStep;
        private FinishState finishState;
        private bool sendIsRunning;
        private bool checkIsRunning;
        private bool resetIsRunning;
        private IUnitOfWork unitOfWork;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets or sets the binding property for the password.
        /// </summary>
        public object Password
        {
            get => password;

            set
            {
                if (password == value)
                    return;

                password = value;

                OnPropertyChanged();
            }
        }

        public PageStep PageStep
        {
            get => pageStep;

            set
            {
                if (pageStep == value)
                    return;

                pageStep = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether the send command is running or not.
        /// </summary>
        public bool SendIsRunning
        {
            get => sendIsRunning;

            set
            {
                if (sendIsRunning == value)
                    return;

                sendIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether the check command is running or not.
        /// </summary>
        public bool CheckIsRunning
        {
            get => checkIsRunning;

            set
            {
                if (checkIsRunning == value)
                    return;

                checkIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether the reset password command is running or not.
        /// </summary>
        public bool ResetIsRunning
        {
            get => resetIsRunning;

            set
            {
                if (resetIsRunning == value)
                    return;

                resetIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the state at the end of the operation.
        /// </summary>
        public FinishState EndState
        {
            get => finishState;

            set
            {
                if (finishState == value)
                    return;

                finishState = value;

                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the send command.
        /// </summary>
        public ICommand SendCommand { get; }

        /// <summary>
        /// Gets the resend command.
        /// </summary>
        public ICommand ResendCommand { get; }

        /// <summary>
        /// Gets the check command.
        /// </summary>
        public ICommand CheckCommand { get; }

        /// <summary>
        /// Gets the reset password command.
        /// </summary>
        public ICommand ResetCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordViewModel"/> class with the default values.
        /// </summary>
        public ForgotPasswordViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            PageStep = PageStep.Step1;
            unitOfWork = IoCContainer.Get<IUnitOfWork>();

            SendCommand = new RelayCommand<string>(phoneNo => SendCodeAsync(phoneNo));
            ResendCommand = new RelayCommand<string>(phoneNo =>
            {
                SendCodeAsync(phoneNo);
                Popup("Code resent. Check your phone.", PopupType.Successful);
            });
            CheckCommand = new RelayCommand<string>(code => CheckCodeAsync(code));
            ResetCommand = new RelayCommand(ResetPasswordAsync);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Resets asynchronously the password of the donor.
        /// </summary>
        private async void ResetPasswordAsync()
        {
            await RunCommand(() => ResetIsRunning, async () =>
            {
                await Task.Run(async () =>
                {
                    IContainPassword parentPage = (IContainPassword)ParentPage;

                    if (parentPage.SecurePasword.Length == 0)
                    {
                        Popup("Password field is mandatory.");
                        return;
                    }

                    if (parentPage.SecurePasword.Length < 8)
                    {
                        Popup("Password field cannot have less than 8 characters.");
                        return;
                    }

                    try
                    {
                        Model.Account account = null;

                        await Task.Run(async () => account = ((List<Model.Person>)await unitOfWork
                            .Persons
                            .GetAllAsync())
                            ?.Single(p => p.PhoneNo.FormatPhoneNumber() == phoneNumber).Administrator.Account);

                        account.Password = BCrypt.HashPassword(parentPage.SecurePasword.Unsecure());

                        await unitOfWork.CompleteAsync();

                        EndState = FinishState.Succeded;
                    }
                    catch
                    {
                        Popup("Invalid password.");
                        VivusConsole.WriteLine("No user found.");
                    }

                    await dispatcherWrapper.InvokeAsync(() => ParentPage.Close());
                });
            });
        }

        /// <summary>
        /// Checks asynchronously whether the typed code is correct or not and changes the step if needed.
        /// </summary>
        /// <param name="code">The code to check.</param>
        private async void CheckCodeAsync(string code)
        {
            await RunCommand(() => CheckIsRunning, async () =>
            {
                await Task.Run(() =>
                {
                    if (code != resetCode)
                    {
                        Popup("Code is not valid.");
                        return;
                    }

                    NextStep();
                });
            });
        }

        /// <summary>
        /// Sends asynchronously the reset password code to a phone number.
        /// </summary>
        /// <param name="phoneNo">Phone number to send the code to.</param>
        private async void SendCodeAsync(string phoneNo)
        {
            await RunCommand(() => SendIsRunning, async () =>
            {
                try
                {
                    int? count;

                    count = null;
                    phoneNumber = phoneNo.FormatPhoneNumber();

                    await Task.Run(async () => count = ((List<Model.Donor>)await unitOfWork
                        .Administrators
                        .GetAllAsync())
                        ?.Where(d => d.Person.PhoneNo.FormatPhoneNumber() == phoneNumber)
                        .Count());

                    if (count.HasValue && count.Value == 0)
                    {
                        Popup("No profile associated with the given phone number.");
                        return;
                    }

                    resetCode = GenerateCode();

                    await TwilioApiHelpers.SendSmsAsync(phoneNumber, $"Your reset password code is: { resetCode }");

                    // Change the step only when the first step is active
                    if (PageStep == PageStep.Step1)
                        NextStep();
                }
                catch (InvalidOperationException)
                {
                    Popup("No profile associated with the given phone number.");
                }
                catch (ApiException)
                {
                    Popup("Couldn't send the reset password code to the phone number.");
                }
                catch (Exception)
                {
                    Popup("An unexpected error occured. Try again later.");
                }
            });
        }

        /// <summary>
        /// Changes the current step on the page.
        /// </summary>
        private void NextStep()
        {
            PageStep += 1;
        }

        /// <summary>
        /// Generates a 6 digits number to recover the password.
        /// </summary>
        /// <returns></returns>
        private string GenerateCode()
        {
            Random random = new Random();

            return random.Next(100000, 1000000).ToString();
        }

        #endregion
    }
}
