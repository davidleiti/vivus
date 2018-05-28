namespace Vivus.Core.Donor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.IoC;
    using Vivus.Core.Donor.Validators;
    using Vivus.Core.Helpers;
    using Vivus.Core.Model;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;

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
        private IApllicationViewModel<Donor> appViewModel;
        private ISecurity security;
        BasicEntity<string> selectedDonationCenter;

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
        /// Gets or sets the selected favourite donation center.
        /// </summary>
        public BasicEntity<string> SelectedDonationCenter
        {
            get => selectedDonationCenter;

            set
            {
                if (selectedDonationCenter == value)
                    return;

                selectedDonationCenter = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the collection of donation centers.
        /// </summary>
        public ObservableCollection<BasicEntity<string>> DonationCenters { get; }

        /// <summary>
        /// Gets the collection of counties.
        /// </summary>
        public ObservableCollection<BasicEntity<string>> Counties { get; }

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
                    return GetNotMandatoryErrorString(propertyName, DonorValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                if (propertyName == nameof(SelectedDonationCenter) && SelectedDonationCenter != null)
                    return GetErrorString(propertyName, DonorValidator.FavouriteDonationCenterValidation(SelectedDonationCenter));

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
            DonationCenters = new ObservableCollection<BasicEntity<string>>();
            Counties = new ObservableCollection<BasicEntity<string>>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Donor>>();
            security = IoCContainer.Get<ISecurity>();

            UpdateCommand = new RelayCommand(async () => await UpdateAsync());

            //ClearFields();
            Task.Run(async () =>
            {
                await LoadDonationCentersAsync();
                await LoadCountiesAsync();
                PopulateFields();
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministratorsViewModel"/> class with the given values.
        /// </summary>
        /// <param name="unitOfWork">The UoW used to access repositories.</param>
        /// <param name="appViewModel">The viewmodel for the application.</param>
        /// <param name="dispatcherWrapper">The ui thread dispatcher.</param>
        /// <param name="security">The collection of security methods.</param>
        public ProfileViewModel(IUnitOfWork unitOfWork, IApllicationViewModel<Donor> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security)
        {
            this.unitOfWork = unitOfWork;
            this.appViewModel = appViewModel;
            this.dispatcherWrapper = dispatcherWrapper;
            this.security = security;

            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            ResidenceAddress = new AddressViewModel(false);
            DonationCenters = new ObservableCollection<BasicEntity<string>>();
            Counties = new ObservableCollection<BasicEntity<string>>();
           
            Task.Run(async () =>
            {
                await LoadDonationCentersAsync();
                await LoadCountiesAsync();
                PopulateFields();
            });

            UpdateCommand = new RelayCommand(async () => await UpdateAsync());
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Adds an adminstrator.
        /// </summary>
        public async Task UpdatePublicAsync()
        {
            await RunCommand(() => UpdateIsRunning, async () =>
            {               
                    await UpdateAsync();
            });
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Loads all the donation centers asynchronously sorted by their location.
        /// </summary>
        /// <returns></returns>
        private async Task LoadDonationCentersAsync()
        {
            Donor donor;
            Address originAddress;
            List<DistanceMatrixApiHelpers.RouteDetails> routes;

            // Clear the donation centers collection and add the default one
            dispatcherWrapper.InvokeAsync(() =>
            {
                DonationCenters.Clear();
                DonationCenters.Add(new BasicEntity<string>(-1, "Select favourite donation center"));
            }).Wait();
                
            // Get the donor
            donor = unitOfWork.Persons[IoCContainer.Get<IApllicationViewModel<Donor>>().User.PersonID].Donor;
            // Get donor's address from the national identification card
            originAddress = donor.Person.Address;
            // If th donor has a residence address
            if (donor.ResidenceID.HasValue)
                // Use residence address
                originAddress = donor.ResidenceAddress;
            // Clear the donor
            donor = null;
            // Get the routes
            routes = (await DistanceMatrixApiHelpers.GetDistancesAsync(originAddress, unitOfWork.DonationCenters.Entities.Select(dc => dc.Address))).ToList();
            // Sort them by the distance ascending
            routes.Sort((r1, r2) =>
                {
                    if (r1.Distance < r2.Distance)
                        return -1;

                    if (r1.Distance > r2.Distance)
                        return 1;

                    if (r1.Duration < r2.Duration)
                        return -1;

                    if (r1.Duration > r2.Duration)
                        return 1;

                    return 0;
                });
            // Foreach route
            routes.ForEach(r =>
            {
                // Get the donation center
                DonationCenter dc = r.DestinationAddress.DonationCenters.ToList()[0];
                // Add the donation center to the collection
                dispatcherWrapper.InvokeAsync(() => DonationCenters.Add(new BasicEntity<string>(dc.DonationCenterID, dc.Name))).Wait();
            });
        }

        /// <summary>
        /// Loads all the counties asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task LoadCountiesAsync()
        {
            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() =>
                {
                    Counties.Clear();
                    Counties.Add(new BasicEntity<string>(-1, "Select county"));
                }).Wait();
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
            SelectedDonationCenter = new BasicEntity<string>(-1, "Select favourite donation center");
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
            Donor donor;

            donor = unitOfWork.Persons[appViewModel.User.PersonID].Donor;

            Email = donor.Account.Email;
            //SelectedDonationCenter = new BasicEntity<string>(donor.DonationCenterID.Value, donor.DonationCenter.Name);
            Person.FirstName = donor.Person.FirstName;
            Person.LastName = donor.Person.LastName;
            Person.BirthDate = donor.Person.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            Person.NationalIdentificationNumber = donor.Person.Nin;
            Person.PhoneNumber = donor.Person.PhoneNo;
            Person.Gender = new BasicEntity<string>(donor.Person.GenderID, donor.Person.Gender.Type);

            IdentificationCardAddress.StreetName = donor.Person.Address.Street;
            IdentificationCardAddress.StreetNumber = donor.Person.Address.StreetNo;
            IdentificationCardAddress.City = donor.Person.Address.City;
            IdentificationCardAddress.County = new BasicEntity<string>(donor.Person.Address.CountyID, donor.Person.Address.County.Name);
            IdentificationCardAddress.ZipCode = donor.Person.Address.ZipCode;

            ResidenceAddress.StreetName = donor.ResidenceAddress.Street;
            ResidenceAddress.StreetNumber = donor.ResidenceAddress.StreetNo;
            ResidenceAddress.City = donor.ResidenceAddress.City;
            ResidenceAddress.County =  new BasicEntity<string>(donor.ResidenceAddress.CountyID, donor.ResidenceAddress.County.Name);
            ResidenceAddress.ZipCode = donor.ResidenceAddress.ZipCode;
        }

        /// <summary>
        /// Registers a donor.
        /// </summary>
        private async Task UpdateAsync()
        {
            await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

            if (Errors + Person.Errors + IdentificationCardAddress.Errors + ResidenceAddress.Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            await Task.Run(() =>
            {
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
       
        /// <summary>
        /// Fills the fields of an administrator.
        /// </summary>
        /// <param name="donor">The administrator instance.</param>
        /// <param name="fillOptional">Whether to fill the optional field also or not.</param>
        private void FillModelAdministrator(ref Donor donor, bool fillOptional = false)
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
            // If the person's national identification card's address instance is null, initialize it
            if (donor.Person.Address is null)
                donor.Person.Address = new Address();
            // If the person's residence address instance is null, initialize it
            if (donor.ResidenceAddress is null)
                donor.ResidenceAddress = new Address();
            // If the account instance is null, initialize it
            if (donor.Account is null)
                donor.Account = new Account();

            // Update the database
            // Account properties
            donor.Account.Email = Email;
            donor.Account.Password = parentPage.SecurePasword.Length == 0 ? donor.Account.Password : security.HashPassword(parentPage.SecurePasword.Unsecure());
            donor.DonationCenterID = SelectedDonationCenter.Id;
            //  Person properties
            donor.Person.FirstName = Person.FirstName;
            donor.Person.LastName = Person.LastName;
            donor.Person.BirthDate = DateTime.Parse(Person.BirthDate);
            donor.Person.Nin = Person.NationalIdentificationNumber;
            donor.Person.PhoneNo = Person.PhoneNumber;
            donor.Person.Gender = gender;
            //  Identification card address properties
            donor.Person.Address.Street = IdentificationCardAddress.StreetName;
            donor.Person.Address.StreetNo = IdentificationCardAddress.StreetNumber;
            donor.Person.Address.City = IdentificationCardAddress.City;
            donor.Person.Address.County = idCardAddressCounty;
            donor.Person.Address.ZipCode = IdentificationCardAddress.ZipCode;
            //  Residence address properties
            donor.ResidenceAddress.Street = ResidenceAddress.StreetName;
            donor.ResidenceAddress.StreetNo = ResidenceAddress.StreetNumber;
            donor.ResidenceAddress.City = ResidenceAddress.City;
            donor.ResidenceAddress.County = residenceAddressCounty;
            donor.ResidenceAddress.ZipCode = ResidenceAddress.ZipCode;
        }

        #endregion
    }
}
