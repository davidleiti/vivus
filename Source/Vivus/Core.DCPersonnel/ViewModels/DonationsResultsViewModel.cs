namespace Vivus.Core.DCPersonnel.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.DCPersonnel.Validators;
    using Vivus.Core.UoW;
    using Vivus.Core.Model;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using VivusConsole = Core.Console.Console;
    using System.Linq;
    using Vivus.Core.DCPersonnel.IoC;
    using System.Windows.Data;
    using Vivus.Core.Helpers;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a view model for the donation results page.
    /// </summary>
    public class DonationsResultsViewModel : BaseViewModel
    {
        #region Private Members

        private string donor;
        private string nin;
        private string donationDate;
        private string donationResults;
        private string filter;
        private bool actionIsRunning;

        DonationFormItemViewModel selectedDonationForm;

        private object formsLock;

        private IUnitOfWork unitOfWork;
        private IApllicationViewModel<DCPersonnel> appViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the parent page.
        /// </summary>
        public IPage ParentPage { get; set; }

        /// <summary>
        /// Gets or sets the full name of the donor.
        /// </summary>
        public string Donor {
            get => donor;

            set {
                if (donor == value)
                    return;

                donor = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the national identification number of the donor.
        /// </summary>
        public string NationalIdentificationNumber {
            get => nin;

            set {
                if (nin == value)
                    return;

                nin = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the donation date of the donor.
        /// </summary>
        public string DonationDate {
            get => donationDate;

            set {
                if (donationDate == value)
                    return;

                donationDate = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the results of the donation.
        /// </summary>
        public string DonationResults {
            get => donationResults;

            set {
                if (donationResults == value)
                    return;

                donationResults = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the filter value for the donation forms.
        /// </summary>
        public string Filter {
            get => filter;

            set {
                if (filter == value)
                    return;

                filter = value;

                if (!String.IsNullOrEmpty(filter))
                {
                    FilterForms();
                }
                else
                {
                    LoadRequestsAsync();
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected donation form.
        /// </summary>
        public DonationFormItemViewModel SelectedDonationForm {
            get => selectedDonationForm;

            set {
                if (selectedDonationForm == value)
                    return;

                selectedDonationForm = value;

                if (selectedDonationForm is null)
                {
                    dispatcherWrapper.InvokeAsync(() => ParentPage.DontAllowErrors());
                    ClearFields();
                }
                else
                {
                    dispatcherWrapper.InvokeAsync(() => ParentPage.AllowOptionalErrors());
                    PopulateFields();
                }

                OnPropertyChanged();
            }
        }

        private bool ActionIsRunning {
            get => actionIsRunning;
            set {
                if (actionIsRunning == value)
                    return;

                actionIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the collection of the donation forms.
        /// </summary>
        public ObservableCollection<DonationFormItemViewModel> DonationForms { get; }

        /// <summary>
        /// Gets the error string of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns></returns>
        public override string this[string propertyName] {
            get {
                if (propertyName == nameof(DonationDate))
                    return GetErrorString(propertyName, DCPersonnelValidator.DonationDateValidation(DonationDate));

                if (propertyName == nameof(DonationResults))
                    return GetErrorString(propertyName, DCPersonnelValidator.DonationResultsValidation(DonationResults));

                return null;
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the send command of the donation results.
        /// </summary>
        public ICommand SendCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new intance of the <see cref="DonationsResultsViewModel"/> class with the default values.
        /// </summary>
        public DonationsResultsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            DonationForms = new ObservableCollection<DonationFormItemViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<DCPersonnel>>();

            formsLock = new object();
            DonationForms = new ObservableCollection<DonationFormItemViewModel>();
            BindingOperations.EnableCollectionSynchronization(DonationForms, formsLock);
            SendCommand = new RelayCommand(() =>  SendResults());
        }

        #endregion

        #region Public Methods
        public async void SendResults()
        {
            await RunCommand(() => ActionIsRunning, async () => {
                await SendResultsAsync();
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the DonationDate of the currently selected DonationForm and sends a message to the Donor with the results
        /// </summary>
        /// <returns></returns>
        private async Task SendResultsAsync()
        {
            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                if (Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                DonationForm donationForm = unitOfWork.DonationForms
                    .Entities
                    .First(form => form.DonorID == selectedDonationForm.PersonId);
                donationForm.DonationDate = DateTime.Parse(DonationDate);

                string messageContent = "Below are the results of your latest donation:\n" + DonationResults;
                Message message = new Message
                {
                    RecieverID = donationForm.DonorID,
                    SenderID = appViewModel.User.PersonID,
                    SendDate = DateTime.Now,
                    Content = messageContent
                };

                Console.WriteLine(donationForm.DonorID);

                unitOfWork.Persons
                    .Entities
                    .First(person => person.PersonID == donationForm.DonorID)
                    .ReceivedMessages
                    .Add(message);

                unitOfWork.Complete();

                LoadRequestsAsync();
                ClearFields();
                SelectedDonationForm = null;
                VivusConsole.WriteLine("Dontaion results sent successfully!");
                Popup("Successfull operation!", PopupType.Successful);
            });
        }

        /// <summary>
        /// Loads all <see cref="DonationForm"/> items which don't yet have a DonationDate specified
        /// </summary>
        private async void LoadRequestsAsync()
        {
            await Task.Run(() =>
            {
                lock (formsLock)
                {
                    DonationForms.Clear();
                    unitOfWork.DonationForms
                        .Entities
                        .Where(form => form.DonationStatus == true && form.DonationDate is null)
                        .ToList()
                        .ForEach(form =>
                        {
                            DonationForms.Add(FormToViewModel(form));
                        });
                }
            });
        }

        /// <summary>
        /// Loads only the <see cref="DonationForm"/> items which match the specified filtering criteria
        /// </summary>
        private async void FilterForms()
        {
            await Task.Run(() =>
            {

                string[] patterns = Filter.Trim().Split(' ');
                List<DonationFormItemViewModel> allForms = unitOfWork.DonationForms.Entities.ToList()
                                   .Select(form => FormToViewModel(form))
                                   .Where(form => IsValidForm(form, patterns))
                                   .ToList();
                lock (formsLock)
                {
                    DonationForms.Clear();
                    foreach (var form in allForms)
                    {
                        DonationForms.Add(form);
                    }
                }
            });
        }

        /// <summary>
        /// Returns a <see cref="DonationFormItemViewModel"/> representation of the given <see cref="DonationForm"/> item
        /// </summary>
        /// <param name="form">The form which needs to be converted</param>
        /// <returns></returns>
        private DonationFormItemViewModel FormToViewModel(DonationForm form)
        {
            return new DonationFormItemViewModel
            {
                PersonId = form.DonorID,
                Donor = $"{form.Donor.Person.FirstName} {form.Donor.Person.LastName}",
                ApplyDate = form.ApplyDate,
                NationalIdentificationNumber = form.Donor.Person.Nin,
                BloodType = form.Donor.BloodType.Type + (form.Donor.RH.Type == "Positive" ? "+" : "-")
            };
        }

        /// <summary>
        /// Checks whether the given donation form matches the given patterns or not
        /// </summary>
        /// <param name="form">The form which we are checking</param>
        /// <param name="patterns">Array of patterns</param>
        /// <returns></returns>
        private bool IsValidForm(DonationFormItemViewModel form, string[] patterns)
        {
            return form.Donor.Contains(patterns) || form.NationalIdentificationNumber.ContainsFromBegining(patterns);
        }

        /// <summary>
        /// Populate the fields with the information of the selected donation form
        /// </summary>
        private void PopulateFields()
        {
            Donor = SelectedDonationForm.Donor;
            DonationDate = DateTime.Today.ToShortDateString();
            NationalIdentificationNumber = SelectedDonationForm.NationalIdentificationNumber;
        }

        /// <summary>
        /// Clears the fields of the form
        /// </summary>
        private void ClearFields()
        {
            Donor = string.Empty;
            DonationDate = string.Empty;
            NationalIdentificationNumber = string.Empty;
            DonationResults = string.Empty;
        }

        #endregion
    }

    /// <summary>
    /// Represents an item view model for the donation forms table.
    /// </summary>
    public class DonationFormItemViewModel : BaseViewModel
    {
        #region Private Members

        private int personId;
        private DateTime applyDate;
        private string donor;
        private string nin;
        private string bloodType;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identificatior of the person.
        /// </summary>
        public int PersonId {
            get => personId;

            set {
                if (personId == value)
                    return;

                personId = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the apply date of the donor.
        /// </summary>
        public DateTime ApplyDate {
            get => applyDate;

            set {
                if (applyDate == value)
                    return;

                applyDate = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the full name of the donor.
        /// </summary>
        public string Donor {
            get => donor;

            set {
                if (donor == value)
                    return;

                donor = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the national identification number of the donor.
        /// </summary>
        public string NationalIdentificationNumber {
            get => nin;

            set {
                if (nin == value)
                    return;

                nin = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the blood type and the rh of the donor.
        /// </summary>
        public string BloodType {
            get => bloodType;

            set {
                if (bloodType == value)
                    return;

                bloodType = value;

                OnPropertyChanged();
            }
        }

        #endregion
    }
}
