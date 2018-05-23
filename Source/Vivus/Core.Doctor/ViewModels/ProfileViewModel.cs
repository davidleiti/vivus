namespace Vivus.Core.Doctor.ViewModels
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.IoC;
    using Vivus.Core.Doctor.Validators;
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
        private IApllicationViewModel<Doctor> appViewModel;
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

        /// <summary>
        /// Gets or sets the email address of the doctor.
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
        public AddressViewModel WorkAddress { get; }

        /// <summary>
        /// Gets the list of counties.
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }

        /// <summary>
        /// Gets the register command.
        /// </summary>
        public ICommand UpdateCommand { get; }

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
                    return GetErrorString(propertyName, DoctorValidator.EmailValidation(Email));

                if (propertyName == nameof(Password) && ParentPage != null)
                    return GetNotMandatoryErrorString(propertyName, DoctorValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class with the default values.
        /// </summary>
        public ProfileViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            WorkAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>>();
            UpdateCommand = new RelayCommand(async () => await UpdateAsync());

            appViewModel = IoCContainer.Get<IApllicationViewModel<Doctor>>();
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            security = IoCContainer.Get<ISecurity>();

            Task.Run(async () =>
            {
                await LoadCountiesAsync();
                PopulateFields();
            });
            //PopulateFields();
            //ClearFields();

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileViewModel"/> class with the given values.
        /// </summary>
        /// <param name="unitOfWork">The UoW used to access repositories.</param>
        /// <param name="appViewModel">The viewmodel for the application.</param>
        /// <param name="dispatcherWrapper">The ui thread dispatcher.</param>
        /// <param name="security">The collection of security methods.</param>
        public ProfileViewModel(IUnitOfWork unitOfWork, IApllicationViewModel<Doctor> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security)
        {
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            WorkAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>>();
            UpdateCommand = new RelayCommand(async () => await UpdateAsync());

            this.appViewModel = appViewModel;
            this.unitOfWork = unitOfWork;
            this.security = security;
            this.dispatcherWrapper = dispatcherWrapper;

            //LoadCountiesAsync();
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
        /// Fills the fields of an doctor.
        /// </summary>
        /// <param name="admin">The administrator instance.</param>
        /// <param name="fillOptional">Whether to fill the optional field also or not.</param>
        private void FillModelDoctor(ref Doctor doctor)
        {
            IContainPassword parentPage;
            Model.Gender gender;
            County idCardAddressCounty;
            County workAddressCounty;

            parentPage = (ParentPage as IContainPassword);
            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            idCardAddressCounty = unitOfWork.Counties.Find(c => c.Name == IdentificationCardAddress.County.Value).Single();
            workAddressCounty = unitOfWork.Counties.Find(c => c.Name == WorkAddress.County.Value).Single();

            // If the instance is null, initialize it
            if (doctor is null)
                doctor = new Doctor();
            // If the person instance is null, initialize it
            if (doctor.Person is null)
                doctor.Person = new Person();
            // If the person's address instance is null, initialize it
            if (doctor.Person.Address is null)
                doctor.Person.Address = new Address();
            //If the doctor's workaddress instance is null, initialize it
            if (doctor.Address is null)
                doctor.Address = new Address();
            // If the account instance is null, initialize it
            if (doctor.Account is null)
                doctor.Account = new Account();

            // Update the database
            //  Account properties
            doctor.Account.Email = Email;
            doctor.Account.Password = parentPage.SecurePasword.Length == 0 ? doctor.Account.Password : security.HashPassword(parentPage.SecurePasword.Unsecure());
            //  Person properties
            doctor.Person.FirstName = Person.FirstName;
            doctor.Person.LastName = Person.LastName;
            doctor.Person.BirthDate = System.DateTime.Parse(Person.BirthDate);
            doctor.Person.Nin = Person.NationalIdentificationNumber;
            doctor.Person.PhoneNo = Person.PhoneNumber;
            doctor.Person.Gender = gender;
            //  Home Address properties
            doctor.Person.Address.Street = IdentificationCardAddress.StreetName;
            doctor.Person.Address.StreetNo = IdentificationCardAddress.StreetNumber;
            doctor.Person.Address.City = IdentificationCardAddress.City;
            doctor.Person.Address.County = idCardAddressCounty;
            doctor.Person.Address.ZipCode = IdentificationCardAddress.ZipCode;
            //  Work Address properties
            doctor.Address.Street = WorkAddress.StreetName;
            doctor.Address.StreetNo = WorkAddress.StreetNumber;
            doctor.Address.City = WorkAddress.City;
            doctor.Address.County = workAddressCounty;
            doctor.Address.ZipCode = WorkAddress.ZipCode;
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
            WorkAddress.StreetName = string.Empty;
            WorkAddress.StreetNumber = string.Empty;
            WorkAddress.City = string.Empty;
            WorkAddress.County = Counties[0];
            WorkAddress.ZipCode = string.Empty;
        }

        /// <summary>
        /// Populates all the fields of the viewmodel.
        /// </summary>
        private void PopulateFields()
        {
            Doctor doctor;
            doctor = unitOfWork.Persons[appViewModel.User.PersonID].Doctor;

            Email = doctor.Account.Email;
            Person.FirstName = doctor.Person.FirstName;
            Person.LastName = doctor.Person.LastName;
            Person.BirthDate = doctor.Person.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            Person.NationalIdentificationNumber = doctor.Person.Nin;
            Person.PhoneNumber = doctor.Person.PhoneNo;
            Person.Gender = new BasicEntity<string>(doctor.Person.GenderID, doctor.Person.Gender.Type);

            IdentificationCardAddress.StreetName = doctor.Person.Address.Street;
            IdentificationCardAddress.StreetNumber = doctor.Person.Address.StreetNo;
            IdentificationCardAddress.City = doctor.Person.Address.City;
            IdentificationCardAddress.County = new BasicEntity<string>(doctor.Person.Address.CountyID, doctor.Person.Address.County.Name);
            IdentificationCardAddress.ZipCode = doctor.Person.Address.ZipCode;

            WorkAddress.StreetName = doctor.Address.Street;
            WorkAddress.StreetNumber = doctor.Address.StreetNo;
            WorkAddress.City = doctor.Address.City;
            WorkAddress.County = new BasicEntity<string>(doctor.Address.CountyID, doctor.Address.County.Name);
            WorkAddress.ZipCode = doctor.Address.ZipCode;
        }

        /// <summary>
        /// Updates a doctor.
        /// </summary>
        private async Task UpdateAsync()
        {
            await RunCommand(() => UpdateIsRunning, async () =>
            {
                await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowOptionalErrors());

                if (Errors + Person.Errors + IdentificationCardAddress.Errors + WorkAddress.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                try
                {
                    Doctor doctor = unitOfWork.Persons[appViewModel.User.PersonID].Doctor;

                    FillModelDoctor(ref doctor);
                    unitOfWork.Complete();

                    //PopulateFields();

                    Popup("Update was successful!", PopupType.Successful);
                    VivusConsole.WriteLine("Doctor: Update worked!");
                }
                catch
                {
                    Popup("An error occured while updating.", PopupType.Warning);
                }
            });

        }

        #endregion
    }
}
