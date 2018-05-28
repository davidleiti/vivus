namespace Vivus.Core.DCPersonnel.ViewModels
{
    using System.Windows.Input;
    using System.Windows;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    using Vivus.Core.DCPersonnel.Validators;
    using Vivus = Console;
    using Vivus.Core.ViewModels.Base;
    using Vivus.Core.Model;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.DCPersonnel.IoC;
    using Vivus.Core.ViewModels;
    using Vivus.Core.DataModels;
    
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a view model for the profile page.
    /// </summary>
    public class ProfileViewModel : BaseViewModel
    {
        #region Private members

        private string email;
        private object password;
        private bool updateIsRunning;
        private IApplicationViewModel<DCPersonnel> appViewModel;
        private IUnitOfWork unitOfWork;
        private ISecurity security;

        #endregion

        #region Public Properties

        public IPage ParentPage { get; set; }

        public bool UpdateIsRunning
        {
            get => updateIsRunning;

            set
            {
                if (updateIsRunning == value)
                    return;

                updateIsRunning = value;

                OnPropertyChanged();
            }
        }

        /*
         * Gets or sets the email address of the dc's employee
         */
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

        /*
         * Gets or sets the password of the dc's employee
         */
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

        // Gets the person view model
        public PersonViewModel Person { get; set; }

        // Gets the identification card adress view model
        public AddressViewModel IdentificationCardAddress { get; }

        // Gets the list of counties
        public List<BasicEntity<string>> Counties { get; }

        // Gets the update command
        public ICommand UpdateCommand { get; }

        /*
         * Gets the error string of a property
         * param Name: The name of the property
         */
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(Email))
                    return GetErrorString(propertyName, DCPersonnelValidator.EmailValidation(Email));

                if (propertyName == nameof(Password) && ParentPage != null)
                    return GetErrorString(propertyName, DCPersonnelValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpViewModel"/> class with the default values.
        /// </summary>
        public ProfileViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>>();
            UpdateCommand = new RelayCommand(async() => await UpdateAsync());

            appViewModel = IoCContainer.Get<IApplicationViewModel<DCPersonnel>>();
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            security = IoCContainer.Get<ISecurity>();

            Task.Run(async() => 
            {
                await LoadCountiesAsync();
                PopulateFields();
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpViewModel"/> class with the given values.
        /// </summary>
        /// <param name="unitOfWork">The UoW used to access repositories.</param>
        /// <param name="appViewModel">The viewmodel for the application.</param>
        /// <param name="dispatcherWrapper">The ui thread dispatcher.</param>
        /// <param name="security">The collection of security methods.</param>
        public ProfileViewModel(IUnitOfWork unitOfWork, IApplicationViewModel<DCPersonnel> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security)
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>>();
            UpdateCommand = new RelayCommand(async () => await UpdateAsync());

            appViewModel = IoCContainer.Get<IApplicationViewModel<DCPersonnel>>();
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            security = IoCContainer.Get<ISecurity>();

            Task.Run(async () =>
            {
                await LoadCountiesAsync();
                PopulateFields();
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads all the counties asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task LoadCountiesAsync()
        {
            await Task.Run(() =>
            {
                Counties.Clear();
                Counties.Add(new BasicEntity<string>(-1, "Select county"));
                unitOfWork.Counties.Entities.ToList().ForEach(county =>
                    dispatcherWrapper.InvokeAsync(() => Counties.Add(new BasicEntity<string>(county.CountyID, county.Name)))
                );
            });
        }

        /// <summary>
        /// Fills the fields of a Donation Center emplyee.
        /// </summary>
        /// <param name="admin">The administrator instance.</param>
        /// <param name="fillOptional">Whether to fill the optional field also or not.</param>
        private void FillModelDCPersonnel(ref DCPersonnel dcPersonnel)
        {
            IContainPassword parentPage;
            Model.Gender gender;
            County idCardAddressCounty;

            parentPage = (ParentPage as IContainPassword);
            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            idCardAddressCounty = unitOfWork.Counties.Find(c => c.Name == IdentificationCardAddress.County.Value).Single();

            // if the instance is null, initialize it
            if (dcPersonnel is null)
                dcPersonnel = new DCPersonnel();

            // If the person intance is null, initialize it
            if (dcPersonnel.Person is null)
                dcPersonnel.Person = new Person();

            // If the donation center empoyee address is null, initialize it
            if (dcPersonnel.Person.Address is null)
                dcPersonnel.Person.Address = new Address();

            // If the account instance is null, initialize it
            if (dcPersonnel.Account is null)
                dcPersonnel.Account = new Account();

            // Update the database
            // Account properties
            dcPersonnel.Account.Email = Email;
            dcPersonnel.Account.Password = parentPage.SecurePasword.Length == 0 ? dcPersonnel.Account.Password : security.HashPassword(parentPage.SecurePasword.Unsecure());
            // Person properties
            dcPersonnel.Person.FirstName = Person.FirstName;
            dcPersonnel.Person.LastName = Person.LastName;
            dcPersonnel.Person.BirthDate = System.DateTime.Parse(Person.BirthDate);
            dcPersonnel.Person.Nin = Person.NationalIdentificationNumber;
            dcPersonnel.Person.PhoneNo = Person.PhoneNumber;
            dcPersonnel.Person.Gender = gender;
            // Address properties
            dcPersonnel.Person.Address.Street = IdentificationCardAddress.StreetName;
            dcPersonnel.Person.Address.StreetNo = IdentificationCardAddress.StreetNumber;
            dcPersonnel.Person.Address.City = IdentificationCardAddress.City;
            dcPersonnel.Person.Address.County = idCardAddressCounty;
            dcPersonnel.Person.Address.ZipCode = IdentificationCardAddress.ZipCode;
        }

        // <summary>
        /// Clears all the fields of the viewmodel.
        /// </summary>
        private void ClearFields()
        {
            Email = string.Empty;
            Person.FirstName = string.Empty;
            Person.LastName = string.Empty;
            Person.BirthDate = string.Empty;
            Person.NationalIdentificationNumber = string.Empty;
            Person.PhoneNumber = string.Empty;
            Person.Gender = new BasicEntity<string>(-1, "Not specified");
            IdentificationCardAddress.StreetName = string.Empty;
            IdentificationCardAddress.StreetNumber = string.Empty;
            IdentificationCardAddress.City = string.Empty;
            IdentificationCardAddress.County = Counties[0];
            IdentificationCardAddress.ZipCode = string.Empty;
        }

        /// <summary>
        /// Populates all the fields of the viewmodel.
        /// </summary>
        private void PopulateFields()
        {
            DCPersonnel dcPersonnel; ;
            dcPersonnel = unitOfWork.Persons[appViewModel.User.PersonID].DCPersonnel;

            Email = dcPersonnel.Account.Email;
            Person.FirstName = dcPersonnel.Person.FirstName;
            Person.LastName = dcPersonnel.Person.LastName;
            Person.BirthDate = dcPersonnel.Person.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            Person.NationalIdentificationNumber = dcPersonnel.Person.Nin;
            Person.PhoneNumber = dcPersonnel.Person.PhoneNo;
            Person.Gender = new BasicEntity<string>(dcPersonnel.Person.GenderID, dcPersonnel.Person.Gender.Type);

            IdentificationCardAddress.StreetName = dcPersonnel.Person.Address.Street;
            IdentificationCardAddress.StreetNumber = dcPersonnel.Person.Address.StreetNo;
            IdentificationCardAddress.City = dcPersonnel.Person.Address.City;
            IdentificationCardAddress.County = new BasicEntity<string>(dcPersonnel.Person.Address.CountyID, dcPersonnel.Person.Address.County.Name);
            IdentificationCardAddress.ZipCode = dcPersonnel.Person.Address.ZipCode;
        }

        /// <summary>
        /// Updates the profile of a dc's employee
        /// </summary>
        private async Task UpdateAsync()
        {
            await RunCommand(() => UpdateIsRunning, async() =>
            {
                await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowOptionalErrors());

                if (Errors + Person.Errors + IdentificationCardAddress.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                await Task.Run(() =>
                {
                    try
                    {
                        DCPersonnel dcPersonnel = unitOfWork.Persons[appViewModel.User.PersonID].DCPersonnel;

                        FillModelDCPersonnel(ref dcPersonnel);
                        unitOfWork.Complete();

                        Popup("Profile update was successful!", PopupType.Successful);
                    }
                    catch
                    {
                        Popup("An error occured while updating the profile.", PopupType.Warning);
                    }
                });
            });

        }

        #endregion
    }
}
