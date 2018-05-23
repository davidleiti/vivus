namespace Vivus.Core.Donor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.IoC;
    using Vivus.Core.Donor.Validators;
    using Vivus.Core.Model;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a view model for the profile page.
    /// </summary>
    public class ProfileViewModel : BaseViewModel
    {
        #region Private Members

        private string email;
        private object password;
        private bool updateIsRunning;
        private IUnitOfWork unitOfWork;
        private IApllicationViewModel<Model.Donor> appViewModel;
        private ISecurity security;


        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the parent page of the current <see cref="SignUpViewModel"/>.
        /// </summary>
        public IPage ParentPage { get; set; }

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
        /// Gets or sets the flag that indicates whether the upadte command is running or not.
        /// </summary>
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
        /// Gets the update command.
        /// </summary>
        public ICommand UpdateCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class with the default values.
        /// </summary>
        public ProfileViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            ResidenceAddress = new AddressViewModel(false);
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Model.Donor>>();
            security = IoCContainer.Get<ISecurity>();
            LoadCountiesAsync();

            UpdateCommand = new RelayCommand(async () => await UpdateAsync());

            ClearFields();
            //PopulateFields();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministratorsViewModel"/> class with the given values.
        /// </summary>
        /// <param name="unitOfWork">The UoW used to access repositories.</param>
        /// <param name="appViewModel">The viewmodel for the application.</param>
        /// <param name="dispatcherWrapper">The ui thread dispatcher.</param>
        /// <param name="security">The collection of security methods.</param>
        public ProfileViewModel(IUnitOfWork unitOfWork, IApllicationViewModel<Model.Donor> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security) : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            ResidenceAddress = new AddressViewModel(false);
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };

            this.unitOfWork = unitOfWork;
            this.appViewModel = appViewModel;
            this.security = security;
            LoadCountiesAsync();

            UpdateCommand = new RelayCommand(async () => await UpdateAsync());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads all the counties asynchronously.
        /// </summary>
        /// <returns></returns>
        private async void LoadCountiesAsync()
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

            ResidenceAddress.StreetName = string.Empty;
            ResidenceAddress.StreetNumber = string.Empty;
            ResidenceAddress.City = string.Empty;
            ResidenceAddress.County = Counties[0];
            ResidenceAddress.ZipCode = string.Empty;
        }

        /// <summary>
        /// Populates all the fields of the viewmodel.
        /// </summary>
        private void PopulateFields()
        {
            Model.Donor donor;
            Model.Donor donorResidence;

            donor = unitOfWork.Persons[appViewModel.User.PersonID].Donor;
            donorResidence = unitOfWork.Persons[appViewModel.User.ResidenceID].Donor;

            Email = donor.Account.Email;
            Person.FirstName = donor.Person.FirstName;
            Person.LastName = donor.Person.LastName;
            Person.BirthDate = donor.Person.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            Person.NationalIdentificationNumber = donor.Person.Nin;
            Person.PhoneNumber = donor.Person.PhoneNo;
            Person.Gender = new BasicEntity<string>(donor.Person.GenderID, donor.Person.Gender.Type);
            IdentificationCardAddress.StreetName = donor.Address.Street;
            IdentificationCardAddress.StreetNumber = donor.Address.StreetNo;
            IdentificationCardAddress.City = donor.Address.City;
            IdentificationCardAddress.County = new BasicEntity<string>(donor.Address.CountyID, donor.Address.County.Name);
            IdentificationCardAddress.ZipCode = donor.Address.ZipCode;

            ResidenceAddress.StreetName = donor.Person.Address.Street;
            ResidenceAddress.StreetNumber = donor.Person.Address.StreetNo;
            ResidenceAddress.City = donor.Person.Address.City;
            ResidenceAddress.County =  new BasicEntity<string>(donor.Person.Address.CountyID, donor.Person.Address.County.Name);
            ResidenceAddress.ZipCode = donor.Person.Address.ZipCode;
        }

        /// <summary>
        /// Registers a donor.
        /// </summary>
        private async Task UpdateAsync()
        {
            await RunCommand(() => updateIsRunning, async () =>
            {
                await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                if (Errors + Person.Errors + IdentificationCardAddress.Errors + ResidenceAddress.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                try
                {
                    Donor donor = unitOfWork.Persons[appViewModel.User.PersonID].Donor;

                    FillModelAdministrator(ref donor, true);
                    // Make changes persistent
                    unitOfWork.Complete();


                    Popup($"Donor updated successfully!", PopupType.Successful);
                }
                catch
                {
                    Popup($"An error occured while updating the donor.");
                }
            });
        }
        /*
        await RunCommand(() => UpdateIsRunning, async () =>
        {
            ParentPage.AllowErrors();

            if (Errors + Person.Errors + IdentificationCardAddress.Errors + ResidenceAddress.Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            await Task.Delay(3000);

            VivusConsole.WriteLine("Profile update done!");
            Popup("Successfull operation!", PopupType.Successful);
        });
        */

        /// <summary>
        /// Fills the fields of an administrator.
        /// </summary>
        /// <param name="donor">The administrator instance.</param>
        /// <param name="fillOptional">Whether to fill the optional field also or not.</param>
        private void FillModelAdministrator(ref Model.Donor donor, bool fillOptional = false)
        {
            IContainPassword parentPage;
            Model.Gender gender;
            County idCardAddressCounty;
            County residenceAddressCounty;

            parentPage = (ParentPage as IContainPassword);
            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            idCardAddressCounty = unitOfWork.Counties.Find(c => c.Name == IdentificationCardAddress.County.Value).Single();
            residenceAddressCounty = unitOfWork.Counties.Find(c => c.Name == ResidenceAddress.County.Value).Single();


            // If the instance is null, initialize it
            if (donor is null)
                donor = new Donor();
            // If the person instance is null, initialize it
            if (donor.Person is null)
                donor.Person = new Person();
            // If the person's address instance is null, initialize it
            if (donor.Person.Address is null)
                donor.Person.Address = new Address();
            // If the account instance is null, initialize it
            if (donor.Account is null)
                donor.Account = new Account();

            // Update the database
            // Account properties
            donor.Account.Email = Email;
            donor.Account.Password = parentPage.SecurePasword.Length == 0 && !fillOptional ? donor.Account.Password : security.HashPassword(parentPage.SecurePasword.Unsecure());
            //  Person properties
            donor.Person.FirstName = Person.FirstName;
            donor.Person.LastName = Person.LastName;
            donor.Person.BirthDate = DateTime.Parse(Person.BirthDate);
            donor.Person.Nin = Person.NationalIdentificationNumber;
            donor.Person.PhoneNo = Person.PhoneNumber;
            donor.Person.Gender = gender;
            //  Identification card address properties
            donor.Address.Street = IdentificationCardAddress.StreetName;
            donor.Address.StreetNo = IdentificationCardAddress.StreetNumber;
            donor.Address.City = IdentificationCardAddress.City;
            donor.Address.County = idCardAddressCounty;
            donor.Address.ZipCode = IdentificationCardAddress.ZipCode;
            //  Residence address properties
            donor.Person.Address.Street = ResidenceAddress.StreetName;
            donor.Person.Address.StreetNo = ResidenceAddress.StreetNumber;
            donor.Person.Address.City = ResidenceAddress.City;
            donor.Person.Address.County = residenceAddressCounty;
            donor.Person.Address.ZipCode = ResidenceAddress.ZipCode;
        }

        #endregion

       
    }
}
