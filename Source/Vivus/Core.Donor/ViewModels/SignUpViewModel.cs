namespace Vivus.Core.Donor.ViewModels
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.IoC;
    using Vivus.Core.UoW;
    using Vivus.Core.Donor.Validators;
    using Vivus.Core.ViewModels;
    using VivusConsole = Console.Console;
    using Vivus.Core.ViewModels.Base;
    using Vivus.Core.Security;
    using System.Collections.ObjectModel;
    using Vivus.Core.Model;


    /// <summary>
    /// Represents a view model for the sign up page.
    /// </summary>
    public class SignUpViewModel : BaseViewModel
    {
        #region Private Members

        private string email;
        private object password;
        private bool registerIsRunning;
        private bool loginIsRunning;
        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<Model.Donor> applicationViewModel;
        private ISecurity security;
        private ButtonType buttonType;

        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets the parent page of the current <see cref="SignUpViewModel"/>.
        /// </summary>
        public IContainPassword ParentPage { get; set; }

        /// <summary>
        /// Gets or sets the email address of the donor.
        /// </summary>
        public string Email
        {
            get => email;

            set
            {
                if (email == value)
                    return;

                email = value;

                OnPropertyChanged();
            }
        }

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

        public bool RegisterIsRunning
        {
            get => registerIsRunning;

            set
            {
                if (registerIsRunning == value)
                    return;
                registerIsRunning = value;
                OnPropertyChanged();
            }
        }



        /// <summary>
        /// Gets the person view model.
        /// </summary>
        public PersonViewModel Person { get; }

        /// <summary>
        /// Gets the identification card address view model.
        /// </summary>
        public AddressViewModel IdentificationCardAddress { get; }

        /// <summary>
        /// Gets the residence address view model.
        /// </summary>
        public AddressViewModel ResidenceAddress { get; }

        /// <summary>
        /// Gets the list of counties.
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }

        /// <summary>
        /// Gets or sets the flag that indicates whether the register command is running or not.
        /// </summary>
        
        /// <summary>
        /// Gets or sets the flag that indicates whether the login command is running or not.
        /// </summary>
        public bool LoginIsRunning
        {
            get => loginIsRunning;

            set
            {
                if (loginIsRunning == value)
                    return;

                loginIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the error string of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns></returns>
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(Email))
                    return GetErrorString(propertyName, DonorValidator.EmailValidation(Email));

                if (propertyName == nameof(Password) && ParentPage != null)
                    return GetErrorString(propertyName, DonorValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                return null;
            }
        }

        #endregion

        #region Public Commands
        
        /// <summary>
        /// Gets the register command.
        /// </summary>
        public ICommand RegisterCommand { get; }

        /// <summary>
        /// Gets the login command.
        /// </summary>
        public ICommand LoginCommand { get; }


        public AddressViewModel Address { get;  }

        #endregion


        #region Constructors

        public SignUpViewModel(IUnitOfWork unitOfWork, IApplicationViewModel<Model.Donor> applicationViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security)
        {
            this.unitOfWork = unitOfWork;
            this.applicationViewModel = applicationViewModel;
            this.dispatcherWrapper = dispatcherWrapper;
            this.security = security;

            Person = new PersonViewModel();
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };
            Address = new AddressViewModel();

            LoadCountiesAsync();


        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpViewModel"/> class with the default values.
        /// </summary>
        public SignUpViewModel():base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            /*
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            ResidenceAddress = new AddressViewModel(false);
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };
            */

            RegisterCommand = new RelayCommand(async () => await RegisterAsync());
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }
        

        #endregion

        #region Private Methods

        private async void LoadCountiesAsync()
        {
            await Task.Run(() =>
            {
                Counties.Clear();
                Counties.Add(new BasicEntity<string>(-1, "Select county"));
                unitOfWork.Counties.Entities.ToList().ForEach(county =>
                dispatcherWrapper.InvokeAsync(() => Counties.Add(new BasicEntity<string>(county.CountyID, county.Name))));
            });
        }

        private async Task RegisterAsync()
        {
            await RunCommand(() => RegisterIsRunning, async () =>
            {
                if (string.IsNullOrEmpty(Email) && ParentPage.SecurePasword.Length == 0)
                {
                    Popup("Both fields are mandatory!");
                    return;
                }

                if(string.IsNullOrEmpty(Email))
                {
                    Popup("Email address field is mandatory!");
                    return;
                }

                if(ParentPage.SecurePasword.Length == 0)
                {
                    Popup("Password field is mandatory!");
                    return;
                }

                await Task.Run(() =>
                {
                    try
                    {
                        Model.Donor donor = IoCContainer.Get<IUnitOfWork>().Donors.Entities.First(d => d.Account.Email == Email);

                        if (donor != null)
                        {
                            Popup("Another user is registered with the same email address!");
                            return;
                        }

                        donor = null;
                        DonorItemViewModel donorItemViewModel = null;

                        FillModelDonor(ref donor, true);
                        unitOfWork.Donors.Add(donor);

                        /// make changes persistent
                        unitOfWork.Complete();

                        /// todo

                        Popup($"Congratulations! You registered successfully!");
                    }
                    catch
                    {
                        Popup("Invalid resistration!");
                        VivusConsole.WriteLine("Can not register!");
                    }

                });

            });
        }

        /// <summary>
        /// Registers a donor.
        /// </summary>
        
        /*private async void RegisterAsync()
        {
            await RunCommand(() => RegisterIsRunning, async () =>
            {
                ParentPage.AllowErrors();

                if (Errors + Person.Errors + IdentificationCardAddress.Errors + ResidenceAddress.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                await Task.Delay(3000);

                VivusConsole.WriteLine("Registration done!");
                Popup("Successfull operation!", PopupType.Successful);
            });
        }
        */
        /// <summary>
        /// Goes back to the login page.
        /// </summary>
        private async Task LoginAsync()
        {
            await RunCommand(() => LoginIsRunning, async () =>
            {
                await Task.Run(() =>
                {
                    // This time, on load, animate the login page
                    IoCContainer.Get<WindowViewModel>().OnLoadAnimateLoginPage = true;

                    IoCContainer.Get<WindowViewModel>().CurrentPage = DataModels.ApplicationPage.Login;
                    IoCContainer.Get<WindowViewModel>().HideMenu();
                    VivusConsole.WriteLine("Moved to the login page.");
                });
            });
        }

        private void FillModelDonor(ref Model.Donor donor, bool fillOptional = false)
        {
            IContainPassword parentPage;
            parentPage = (ParentPage as IContainPassword);

            Model.Gender gender;
            County county;
            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            county = unitOfWork.Counties.Find(c => c.Name == Address.County.Value).Single();

            if (donor is null)
            {
                donor = new Donor();
            }

            if (donor.Person is null)
            {
                donor.Person = new Person();
            }

            if (donor.Person.Address is null)
            {
                donor.Person.Address = new Address();
            }

            if(donor.Account is null)
            {
                donor.Account = new Account();
            }

            donor.Account.Email = Email;
            donor.Account.Password = parentPage.SecurePasword.Length == 0 && !fillOptional ? donor.Account.Password : security.HashPassword(parentPage.SecurePasword.Unsecure());
            donor.Person.FirstName = Person.FirstName;
            donor.Person.LastName = Person.LastName;
            donor.Person.BirthDate = System.DateTime.Parse(Person.BirthDate);
            donor.Person.Nin = Person.NationalIdentificationNumber;
            donor.Person.PhoneNo = Person.PhoneNumber;
            donor.Person.Gender = gender;

            /// address properties
            donor.Person.Address.Street = Address.StreetName;
            donor.Person.Address.StreetNo = Address.StreetNumber;
            donor.Person.Address.City = Address.City;
            donor.Person.Address.County = county;
            donor.Person.Address.ZipCode = Address.ZipCode;
        }

        private void FillViewModelDonor(ref DonorItemViewModel donor)
        {
            Model.Gender gender;
            County county;
            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            county = unitOfWork.Counties.Find(c => c.Name == Address.County.Value).Single();

            if (donor is null)
                donor = new DonorItemViewModel();
            if (donor.Person is null)
                donor.Person = new PersonViewModel();
            if (donor.Address is null)
                donor.Address = new AddressViewModel();

            // person properties
            donor.Email = Email;
            donor.Person.FirstName = Person.FirstName;
            donor.Person.LastName = Person.LastName;
            donor.Person.BirthDate = Person.BirthDate;
            donor.Person.NationalIdentificationNumber = Person.NationalIdentificationNumber;
            donor.Person.PhoneNumber = Person.PhoneNumber;
            donor.Person.Gender = new BasicEntity<string>(gender.GenderID, gender.Type);

            // address properties
            donor.Address.StreetName = Address.StreetName;
            donor.Address.StreetNumber = Address.StreetNumber;
            donor.Address.City = Address.City;
            donor.Address.County = new BasicEntity<string>(county.CountyID, county.Name);
            donor.Address.ZipCode = Address.ZipCode;
        }

        #endregion

        #region Public Inner Classes

        public class DonorItemViewModel : BaseViewModel
        {
            #region Private fields
            private int personID;
            private int accountID;
            private string email;
            private object password;
            #endregion

            #region Public Properties

            public PersonViewModel Person { get; set; }

            public AddressViewModel Address { get; set; }

            public string Email
            {
                get => email;
                set
                {
                    if (email == value)
                        return;
                    email = value;
                    OnPropertyChanged();
                }
            }

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
            #endregion

            #region Constructors

            public DonorItemViewModel() { }

            #endregion
        }

        #endregion

    }
}
