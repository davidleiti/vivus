namespace Vivus.Core.Doctor.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.IoC;
    using Vivus.Core.Doctor.Validators;
    using Vivus.Core.Model;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;

    /// <summary>
    /// Represents a view model for the patient details page.
    /// </summary>
    public class PatientDetailsViewModel : BaseViewModel
    {
        #region Private Members

        ButtonType buttonType;
        private int id;
        private BasicEntity<string> bloodType;
        private BasicEntity<string> rhType;
        private bool status;
        private bool actionIsRunning;
        private FinishState finishState;
        private IUnitOfWork unitOfWork;

        #endregion

        #region Public Enumeration

        /// <summary>
        /// Represents an enumeration of finish states of an operation.
        /// </summary>
        public enum FinishState
        {
            /// <summary>
            /// The operation was not performed.
            /// </summary>
            Closed,

            /// <summary>
            /// The operation failed.
            /// </summary>
            Failed,

            /// <summary>
            /// The operation succeded.
            /// </summary>
            Succeded
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the type of the button on the page.
        /// </summary>
        public ButtonType ButtonType
        {
            get => buttonType;

            set
            {
                if (buttonType == value)
                    return;

                buttonType = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the identificator of the patient.
        /// </summary>
        public int Id
        {
            get => id;

            set
            {
                if (id == value)
                    return;

                id = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the person view model.
        /// </summary>
        public PersonViewModel Person { get; set; }

        /// <summary>
        /// Gets the identification card address view model.
        /// </summary>
        public AddressViewModel IdentificationCardAddress { get; set; }

        /// <summary>
        /// Gets the list of counties.
        /// </summary>
        public ObservableCollection<BasicEntity<string>> Counties { get; set; }

        /// <summary>
        /// Gets the list of blood types.
        /// </summary>
        public ObservableCollection<BasicEntity<string>> BloodTypes { get; set; }

        /// <summary>
        /// Gets the list of RHs.
        /// </summary>
        public ObservableCollection<BasicEntity<string>> RhTypes { get; set; }

        /// <summary>
        /// Gets or sets the blood type of the patient.
        /// </summary>
        public BasicEntity<string> SelectedBloodType
        {
            get => bloodType;

            set
            {
                if (bloodType == value)
                    return;

                bloodType = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the RH type of the patient.
        /// </summary>
        public BasicEntity<string> SelectedRh
        {
            get => rhType;

            set
            {
                if (rhType == value)
                    return;

                rhType = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the status
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
        /// Gets the error string of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns></returns>
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(SelectedBloodType))
                    return GetErrorString(propertyName, DoctorValidator.NotNullStringFieldValidation(SelectedBloodType, "Blood type"));

                if (propertyName == nameof(SelectedRh))
                    return GetErrorString(propertyName, DoctorValidator.NotNullStringFieldValidation(SelectedBloodType, "RH"));

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether the add/modify command is running or not.
        /// </summary>
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
        /// Gets the add and modify command.
        /// </summary>
        public ICommand AddModifyCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientDetailsViewModel"/> class with the default values.
        /// </summary>
        public PatientDetailsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Status = true;
            Person = new PersonViewModel();
            IdentificationCardAddress = new AddressViewModel();
            Counties = new ObservableCollection<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };
            BloodTypes = new ObservableCollection<BasicEntity<string>> { new BasicEntity<string>(-1, "Select blood type") };
            RhTypes = new ObservableCollection<BasicEntity<string>> { new BasicEntity<string>(-1, "Select rh") };
            AddModifyCommand = new RelayCommand(async() => await AddModify());
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds or modifies a patient.
        /// </summary>
        private async Task AddModify()
        {
            await RunCommand(() => ActionIsRunning, async () =>
            {
                ParentPage.AllowErrors();

                if (Errors + Person.Errors + IdentificationCardAddress.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                try
                {
                    if (ButtonType == ButtonType.Add)
                        await AddPatient();
                    else
                        await ModifyPatient();

                    EndState = FinishState.Succeded;
                }
                catch
                {
                    EndState = FinishState.Failed;
                }

                ParentPage.Close();
            });
        }

        /// <summary>
        /// Modifies a patient.
        /// </summary>
        private async Task ModifyPatient()
        {
            Patient patient = await unitOfWork.Patients.SingleAsync(p => p.PersonID == Id);

            patient.Person.FirstName = Person.FirstName;
            patient.Person.LastName = Person.LastName;
            patient.Person.BirthDate = DateTime.Parse(Person.BirthDate);
            patient.Person.Nin = Person.NationalIdentificationNumber;
            patient.Person.PhoneNo = Person.PhoneNumber;
            patient.Person.Gender = await unitOfWork.Genders.SingleAsync(g => g.Type == Person.Gender.Value);
            patient.Person.Address.Street = IdentificationCardAddress.StreetName;
            patient.Person.Address.StreetNo = IdentificationCardAddress.StreetNumber;
            patient.Person.Address.City = IdentificationCardAddress.City;
            patient.Person.Address.CountyID = IdentificationCardAddress.County.Id;
            patient.Person.Address.ZipCode = IdentificationCardAddress.ZipCode;
            patient.BloodTypeID = SelectedBloodType.Id;
            patient.RhID = SelectedRh.Id;
            patient.PersonStatus = await unitOfWork.PersonStatuses.SingleAsync(ps => ps.Type == (Status ? "Alive" : "Dead"));

            //await unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// Adds a patient.
        /// </summary>
        /// <returns></returns>
        private async Task AddPatient()
        {
            unitOfWork.Patients.Add(new Patient
            {
                Person = new Person
                {
                    FirstName = Person.FirstName,
                    LastName = Person.LastName,
                    BirthDate = DateTime.Parse(Person.BirthDate),
                    Nin = Person.NationalIdentificationNumber,
                    PhoneNo = Person.PhoneNumber,
                    Gender = await unitOfWork.Genders.SingleAsync(g => g.Type == Person.Gender.Value),
                    Address = new Address
                    {
                        Street = IdentificationCardAddress.StreetName,
                        StreetNo = IdentificationCardAddress.StreetNumber,
                        City = IdentificationCardAddress.City,
                        CountyID = IdentificationCardAddress.County.Id,
                        ZipCode = IdentificationCardAddress.ZipCode
                    }
                },
                BloodTypeID = SelectedBloodType.Id,
                RhID = SelectedRh.Id,
                PersonStatus = await unitOfWork.PersonStatuses.SingleAsync(ps => ps.Type == (Status ? "Alive" : "Dead"))
            });
            
            //await unitOfWork.CompleteAsync();
        }

        #endregion
    }
}
