namespace Vivus.Core.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.Administration.IoC;
    using Vivus.Core.Administration.Validators;
    using Vivus.Core.DataModels;
    using Vivus.Core.Model;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the doctors page.
    /// </summary>
    public class DoctorsViewModel : BaseViewModel
    {
        #region Private Members

        private string email;
        private object password;
        private bool status = true;
        private DoctorItemViewModel selectedDoctor;
        private bool optionalErrors;
        private bool actionIsRunning;
        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<Administrator> appViewModel;
        private ISecurity security;

        #endregion

        #region Public Properties

        public IPage ParentPage { get; set; }

        public ButtonType ButtonType { get; set; }

        /// <summary>
        /// Gets or sets the email address of the doctor
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
        /// Gets or sets the binding for the password
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
        /// Gets or sets the binding for the status
        /// </summary>
        public bool Status
        {
            get => status;

            set
            {
                if (status == value)
                    return;

                status = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the person view model for a doctor
        /// </summary>
        public PersonViewModel Person { get; }

        /// <summary>
        /// Gets the home address view model
        /// </summary>
        public AddressViewModel HomeAddress { get; }

        /// <summary>
        /// Gets the work address view model
        /// </summary>
        public AddressViewModel WorkAddress { get; }

        /// <summary>
        /// Gets the list of counties
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }

        /// <summary>
        /// Gets the add doctor command
        /// </summary>
        public ICommand AddDoctorCommand { get; }

        public bool ActionIsRunning
        {
            get => actionIsRunning;

            set
            {
                if (actionIsRunning == value)
                    return;

                actionIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the collection of doctors
        /// </summary>
        public ObservableCollection<DoctorItemViewModel> Doctors { get; }

        /// <summary>
        /// Gets or sets the selected doctor
        /// </summary>
        public DoctorItemViewModel SelectedDoctor
        {
            get => selectedDoctor;

            set
            {
                if (selectedDoctor == value)
                    return;

                selectedDoctor = value;
                if (selectedDoctor is null)
                {
                    ButtonType = ButtonType.Add;
                    Console.WriteLine("add doctor entered");
                    optionalErrors = false;

                    dispatcherWrapper.InvokeAsync(() => ParentPage.DontAllowErrors());

                    ClearFields();
                }
                else
                {
                    ButtonType = ButtonType.Modify;
                    Console.WriteLine("modify doctor entered");
                    optionalErrors = true;

                    dispatcherWrapper.InvokeAsync(() => ParentPage.AllowOptionalErrors());
                    PopulateFields();
                }
                Vivus.Console.WriteLine(selectedDoctor.Name + " was selected");

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(Email))
                    return GetErrorString(propertyName, AdministrationValidator.EmailValidation(Email));


                if (optionalErrors)
                    return GetNotMandatoryErrorString(propertyName, AdministrationValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                if (propertyName == nameof(Password) && ParentPage != null)
                    return GetErrorString(propertyName, AdministrationValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorsViewModel"/> class with the default values
        /// </summary>
        public DoctorsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Person = new PersonViewModel();
            HomeAddress = new AddressViewModel();
            WorkAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };
            //AddDoctorCommand = new RelayCommand(() => { AddDoctorAsync(); });
            AddDoctorCommand = new RelayCommand(async () => await AddModifyAsync());
            Doctors = new ObservableCollection<DoctorItemViewModel>();
            ButtonType = ButtonType.Add;
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<Administrator>>();
            security = IoCContainer.Get<ISecurity>();
            LoadCountiesAsync();
            LoadDoctorsAsync();
        }
        /// <summary>
        /// constructor with params for DoctorViewModel
        /// </summary>
        /// <param name="unitOfWork">the UoW for repository access</param>
        /// <param name="appViewModel">viewModel of the application</param>
        /// <param name="dispatcherWrapper">the ui thread for the dispatcher</param>
        /// <param name="security">collection of security methods</param>
        public DoctorsViewModel(IUnitOfWork unitOfWork, IApplicationViewModel<Administrator> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security)
        {
            ButtonType = ButtonType.Add;
            optionalErrors = false;
            this.unitOfWork = unitOfWork;
            this.appViewModel = appViewModel;
            this.dispatcherWrapper = dispatcherWrapper;
            this.security = security;

            Person = new PersonViewModel();
            Counties = new List<BasicEntity<string>>();
            Doctors = new ObservableCollection<DoctorItemViewModel>();
            HomeAddress = new AddressViewModel();
            WorkAddress = new AddressViewModel();

            LoadCountiesAsync();
            LoadDoctorsAsync();

            AddDoctorCommand = new RelayCommand(async () => await AddModifyAsync());
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Adds/modifies a doctor
        /// </summary>
        public async Task AddModifyAsync()
        {
            await RunCommand(() => ActionIsRunning, async () =>
            {
                if (ButtonType == ButtonType.Add)
                    await AddDoctorAsync();
                else
                    await ModifyDoctorAsync();
            });
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Add a doctor
        /// </summary>


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
        /// loads the doctors asyncronously
        /// </summary>
        private async void LoadDoctorsAsync()
        {
            await Task.Run(() =>
            unitOfWork.Doctors
            .Entities
            .ToList()
            .ForEach(doctor =>
                dispatcherWrapper.InvokeAsync(() =>
                {
                    AddressViewModel work = new AddressViewModel(true);
                    //Console.Console.WriteLine("i got a doctor " + doctor.Person.LastName);
                    Address add = unitOfWork.Addresses
                        .Entities
                        .Where(address => address.AddressID == doctor.WorkAddressID)
                        .Single();

                    Doctors.Add(new DoctorItemViewModel
                    {
                        PersonID = doctor.PersonID,
                        Email = doctor.Account.Email,
                        IsActive = doctor.Active,
                        Person = new PersonViewModel
                        {
                            FirstName = doctor.Person.FirstName,
                            LastName = doctor.Person.LastName,
                            BirthDate = doctor.Person.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                            NationalIdentificationNumber = doctor.Person.Nin,
                            PhoneNumber = doctor.Person.PhoneNo,
                            Gender = new BasicEntity<string>(doctor.Person.GenderID, doctor.Person.Gender.Type)
                        },
                        HomeAddressViewModel = new AddressViewModel
                        {
                            County = new BasicEntity<string>(doctor.Person.Address.CountyID, doctor.Person.Address.County.Name),
                            City = doctor.Person.Address.City,
                            StreetName = doctor.Person.Address.Street,
                            StreetNumber = doctor.Person.Address.StreetNo,
                            ZipCode = doctor.Person.Address.ZipCode

                        },
                        WorkAddressViewModel = new AddressViewModel
                        {
                            County = new BasicEntity<string>(add.CountyID, add.County.Name),
                            City = add.City,
                            StreetName = add.Street,
                            StreetNumber = add.StreetNo,
                            ZipCode = add.ZipCode
                        },
                        WorkAddress = add.County.Name + "," + add.City + "," + add.Street + "," + add.StreetNo.ToString(),
                        HomeAddress = doctor.Person.Address.County.Name + ","
                        + doctor.Person.Address.City + ","
                        + doctor.Person.Address.Street + ","
                        + doctor.Person.Address.StreetNo.ToString(),
                        Name = doctor.Person.FirstName + " " + doctor.Person.LastName,
                        NationalIdentificationNumber=doctor.Person.Nin
                    }
                    );
                    //Console.Console.WriteLine(Doctors.Count.ToString() + "is the len");
                }
                )
            )
            );

        }
        /// <summary>
        /// clears the fields of the viewmodel
        /// </summary>
        private void ClearFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            Status = true;

            Person.FirstName = string.Empty;
            Person.LastName = string.Empty;
            Person.BirthDate = string.Empty;
            Person.NationalIdentificationNumber = string.Empty;
            Person.PhoneNumber = string.Empty;
            Person.Gender = new BasicEntity<string>(-1, "Not specified");

            WorkAddress.StreetName = string.Empty;
            WorkAddress.StreetNumber = string.Empty;
            WorkAddress.City = string.Empty;
            WorkAddress.County = Counties[0];
            WorkAddress.ZipCode = string.Empty;

            HomeAddress.StreetName = string.Empty;
            HomeAddress.StreetNumber = string.Empty;
            HomeAddress.City = string.Empty;
            HomeAddress.County = Counties[0];
            HomeAddress.ZipCode = string.Empty;
        }



        /// <summary>
        /// populates the fields of the viewmodel
        /// </summary>
        private void PopulateFields()
        {
            //populate email and passowrd
            Email = selectedDoctor.Email;
            Status = selectedDoctor.IsActive;
            //populate person-specific fields
            Person.FirstName = selectedDoctor.Person.FirstName;
            Person.LastName = selectedDoctor.Person.LastName;
            Person.BirthDate = selectedDoctor.Person.BirthDate;
            Person.NationalIdentificationNumber = selectedDoctor.Person.NationalIdentificationNumber;
            Person.PhoneNumber = selectedDoctor.Person.PhoneNumber;
            Person.Gender = selectedDoctor.Person.Gender;
            //populate wrk address fields
            WorkAddress.StreetName = selectedDoctor.WorkAddressViewModel.StreetName;
            WorkAddress.StreetNumber = selectedDoctor.WorkAddressViewModel.StreetNumber;
            WorkAddress.City = selectedDoctor.WorkAddressViewModel.City;
            WorkAddress.County = selectedDoctor.WorkAddressViewModel.County;
            WorkAddress.ZipCode = selectedDoctor.WorkAddressViewModel.ZipCode;
            //populate home address fields
            HomeAddress.StreetName = selectedDoctor.HomeAddressViewModel.StreetName;
            HomeAddress.StreetNumber = selectedDoctor.HomeAddressViewModel.StreetNumber;
            HomeAddress.City = selectedDoctor.HomeAddressViewModel.City;
            HomeAddress.County = selectedDoctor.HomeAddressViewModel.County;
            HomeAddress.ZipCode = selectedDoctor.HomeAddressViewModel.ZipCode;

        }


        /// <summary>
        /// adds a doctor ansyncronously
        /// </summary>
        /// <returns></returns>
        private async Task AddDoctorAsync()
        {
            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                if (Errors + Person.Errors + HomeAddress.Errors + WorkAddress.Errors > 0)
                {
                    Popup("Some errors were found! Fix them before going forward.");
                    return;
                }
                try
                {
                    Doctor doctor;
                    DoctorItemViewModel doctorItemViewModel;

                    doctor = null;
                    doctorItemViewModel = null;

                    //fill model, add, persist
                    FillModelDoctor(ref doctor, true);
                    unitOfWork.Doctors.Add(doctor);
                    unitOfWork.Complete();

                    //fill viewModel fields 
                    FillViewModelDoctor(ref doctorItemViewModel);

                    doctorItemViewModel.PersonID = doctor.PersonID;
                    /*doctorItemViewModel.WorkAddress = WorkAddress.County.Value + ","
                        + WorkAddress.City + ","
                        + WorkAddress.StreetName + ","
                        + WorkAddress.StreetNumber.ToString();
                    doctorItemViewModel.HomeAddress = HomeAddress.County.Value + ","
                        + HomeAddress.City + ","
                        + HomeAddress.StreetName + ","
                        + HomeAddress.StreetNumber.ToString();*/



                    dispatcherWrapper.InvokeAsync(() => Doctors.Add(doctorItemViewModel));
                    Popup($"Doctor added successfully!", PopupType.Successful);
                }
                catch (Exception ex)
                {
                    Popup($"ex:" + ex);
                    Vivus.Console.WriteLine("ex:" + ex + " " + ex.StackTrace);
                }
                Vivus.Console.WriteLine("Adminstration: Add Doctor works!");


            });
        }

        /// <summary>
        /// modifies a doctor asyncronously
        /// </summary>
        /// <returns></returns>
        private async Task ModifyDoctorAsync()
        {
            await Task.Run(() =>
            {
                if (selectedDoctor == null)
                    return;
                //dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                Console.WriteLine("errors:"+Errors.ToString() + " " + HomeAddress.Errors.ToString() + " " + WorkAddress.Errors.ToString());
                if (Errors + Person.Errors + HomeAddress.Errors + WorkAddress.Errors > 0)
                {
                    Popup("Some errors were found! Fix them before going forward.");
                    return;
                }
                try
                {
                    Doctor doctor;

                    doctor = unitOfWork.Persons[SelectedDoctor.PersonID].Doctor;

                    //fill model and persist
                    FillModelDoctor(ref doctor);
                    unitOfWork.Complete();

                    FillViewModelDoctor(ref selectedDoctor);

                    Popup($"Doctor modified successfully!", PopupType.Successful);

                }
                catch
                {
                    Popup($"An error occured while modifying doctor.");
                }



            });
        }

        /// <summary>
        /// fills the model fields of a doctor
        /// </summary>
        /// <param name="doctor"></param>
        /// <param name="fillOptional"></param>
        private void FillModelDoctor(ref Doctor doctor, bool fillOptional = false)
        {
            IContainPassword parentPage;
            Model.Gender gender;
            County workCounty;
            County homeCounty;

            parentPage = (ParentPage as IContainPassword);

            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            workCounty = unitOfWork.Counties.Find(c => c.Name == WorkAddress.County.Value).Single();
            homeCounty = unitOfWork.Counties.Find(c => c.Name == HomeAddress.County.Value).Single();

            if (doctor == null)
                doctor = new Doctor();
            if (doctor.Person == null)
                doctor.Person = new Person();
            if (doctor.Person.Address is null)
                doctor.Person.Address = new Address();
            if (doctor.Address == null)
                doctor.Address = new Address();
            if (doctor.Account == null)
                doctor.Account = new Account();
            //if (doctor.WorkAddressID == 0)
            //  doctor.WorkAddressID = 0;//todo


            doctor.Active = true;

            //account properties
            doctor.Account.Email = Email;
            doctor.Account.Password = parentPage.SecurePasword.Length == 0 && !fillOptional ? doctor.Account.Password : security.HashPassword(parentPage.SecurePasword.Unsecure());

            //person related stuff
            doctor.Person.FirstName = Person.FirstName;
            doctor.Person.LastName = Person.LastName;
            doctor.Person.BirthDate = DateTime.Parse(Person.BirthDate);
            doctor.Person.Nin = Person.NationalIdentificationNumber;
            doctor.Person.PhoneNo = Person.PhoneNumber;
            doctor.Person.Gender = gender;

            //home address
            doctor.Person.Address.Street = HomeAddress.StreetName;
            doctor.Person.Address.StreetNo = HomeAddress.StreetNumber;
            doctor.Person.Address.City = HomeAddress.City;
            doctor.Person.Address.County = homeCounty;
            doctor.Person.Address.ZipCode = HomeAddress.ZipCode;

            //work address
            doctor.Address.Street = WorkAddress.StreetName;
            doctor.Address.StreetNo = WorkAddress.StreetNumber;
            doctor.Address.City = WorkAddress.City;
            doctor.Address.County = workCounty;
            doctor.Address.ZipCode = WorkAddress.ZipCode;
        }

        private void FillViewModelDoctor(ref DoctorItemViewModel doctor)
        {
            Model.Gender gender;
            County homeCounty;
            County workCounty;

            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            homeCounty = unitOfWork.Counties.Find(c => c.Name == HomeAddress.County.Value).Single();
            workCounty = unitOfWork.Counties.Find(c => c.Name == WorkAddress.County.Value).Single();

            if (doctor is null)
                doctor = new DoctorItemViewModel();
            if (doctor.Person is null)
                doctor.Person = new PersonViewModel();
            if (doctor.WorkAddressViewModel is null)
                doctor.WorkAddressViewModel = new AddressViewModel();
            if (doctor.HomeAddressViewModel is null)
                doctor.HomeAddressViewModel = new AddressViewModel();

            //account stuff
            doctor.IsActive = true;
            doctor.Email = Email;


            //person properties
            doctor.Person.FirstName = Person.FirstName;
            doctor.Person.LastName = Person.LastName;
            doctor.Person.BirthDate = Person.BirthDate;
            doctor.Person.NationalIdentificationNumber = Person.NationalIdentificationNumber;
            doctor.Person.PhoneNumber = Person.PhoneNumber;
            doctor.Person.Gender = new BasicEntity<string>(gender.GenderID, gender.Type);

            //workAddress properties

            doctor.WorkAddressViewModel.StreetName = WorkAddress.StreetName;
            doctor.WorkAddressViewModel.StreetNumber = WorkAddress.StreetNumber;
            doctor.WorkAddressViewModel.City = WorkAddress.City;
            doctor.WorkAddressViewModel.County = new BasicEntity<string>(workCounty.CountyID, workCounty.Name);
            doctor.WorkAddressViewModel.ZipCode = WorkAddress.ZipCode;
            doctor.WorkAddress = WorkAddress.County.Value + ","
                        + WorkAddress.City + ","
                        + WorkAddress.StreetName + ","
                        + WorkAddress.StreetNumber.ToString();

            //home address propertes
            doctor.HomeAddressViewModel.StreetName = HomeAddress.StreetName;
            doctor.HomeAddressViewModel.StreetNumber = HomeAddress.StreetNumber;
            doctor.HomeAddressViewModel.City = HomeAddress.City;
            doctor.HomeAddressViewModel.County = new BasicEntity<string>(homeCounty.CountyID, homeCounty.Name);
            doctor.HomeAddressViewModel.ZipCode = HomeAddress.ZipCode;
            doctor.HomeAddress= HomeAddress.County.Value + ","
                        + HomeAddress.City + ","
                        + HomeAddress.StreetName + ","
                        + HomeAddress.StreetNumber.ToString();

            //those properties outside person view model
            doctor.Name = doctor.Person.FirstName + " " + doctor.Person.LastName;
            doctor.NationalIdentificationNumber = doctor.Person.NationalIdentificationNumber;

        }



        #endregion
    }

    public class DoctorItemViewModel : BaseViewModel
    {
        #region Private Members

        private int personID;
        private string name;
        private string nationalIdentificationNumber;
        private string email;
        private string homeAddress;
        private string workAddress;
        private AddressViewModel workAddressViewModel;
        private AddressViewModel homeAddressViewModel;
        private PersonViewModel person;

        private bool isActive;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int PersonID
        {
            get => personID;

            set
            {
                if (personID == value)
                    return;

                personID = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name
        {
            get => name;

            set
            {
                if (name == value)
                    return;

                name = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the national identification number
        /// </summary>
        public string NationalIdentificationNumber
        {
            get => nationalIdentificationNumber;

            set
            {
                if (nationalIdentificationNumber == value)
                    return;

                nationalIdentificationNumber = value;

                OnPropertyChanged();
            }
        }

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
        /// Gets or sets the home address
        /// </summary>
        public string HomeAddress
        {
            get => homeAddress;

            set
            {
                if (homeAddress == value)
                    return;

                homeAddress = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the work address
        /// </summary>
        public string WorkAddress
        {
            get => workAddress;

            set
            {
                if (workAddress == value)
                    return;

                workAddress = value;

                OnPropertyChanged();
            }
        }
        public bool IsActive
        {
            get => isActive;

            set
            {
                if (isActive == value)
                    return;

                isActive = value;

                OnPropertyChanged();
            }

        }
        public AddressViewModel WorkAddressViewModel
        {
            get => workAddressViewModel;
            set
            {
                if (workAddressViewModel == value)
                    return;

                workAddressViewModel = value;


                /*if (workAddressViewModel != null)
                {

                    workAddress = workAddressViewModel.County.Value + ","
                        + workAddressViewModel.City + ","
                        + workAddressViewModel.StreetName + ","
                        + workAddressViewModel.StreetNumber.ToString();
                }*/
                OnPropertyChanged();

            }
        }

        public AddressViewModel HomeAddressViewModel
        {
            get => homeAddressViewModel;
            set
            {
                if (homeAddressViewModel == value)
                    return;

                homeAddressViewModel = value;
               /* if (homeAddressViewModel != null)
                {
                    homeAddress = homeAddressViewModel.County.Value + ","
                        + homeAddressViewModel.City + ","
                        + homeAddressViewModel.StreetName + ","
                        + homeAddressViewModel.StreetNumber.ToString();
                }
                OnPropertyChanged();*/
            }
        }

        public PersonViewModel Person
        {
            get => person;

            set
            {
                if (person == value)
                    return;

                person = value;


                //set bound fields from Person
                name = person.FirstName + " " + person.LastName;

                nationalIdentificationNumber = person.NationalIdentificationNumber;


                OnPropertyChanged();
            }
        }


        #endregion

        #region Constructors
        public DoctorItemViewModel()
        {
            workAddress = "";
            homeAddress = "";
            nationalIdentificationNumber = "";
        }
        #endregion
    }
}