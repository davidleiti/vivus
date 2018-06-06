namespace Vivus.Core.Doctor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.IoC;
    using Vivus.Core.Helpers;
    using Vivus.Core.Model;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a view model for the patients page.
    /// </summary>
    public class PatientsViewModel : BaseViewModel
    {
        #region Private Members

        private string filter;
        private object currentFilterId;
        private object allPatientsLockObj;
        private object myPatientsLockObj;
        private PatientItemViewModel selectedPatient;
        private PatientItemViewModel mySelectedPatient;
        private bool newPatientIsRunning;
        private bool choosePatientIsRunning;
        private bool dismissPatientIsRunning;
        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<Doctor> appViewModel;
        private List<PatientItemViewModel> allPatients;
        private PatientItemViewModel lastSelectedPatient;
        private ObservableCollection<BasicEntity<string>> counties;
        private ObservableCollection<BasicEntity<string>> bloodTypes;
        private ObservableCollection<BasicEntity<string>> rhs;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the popup for the patient details.
        /// </summary>
        public IPopup PatientDetailsPopup { get; set; }

        /// <summary>
        /// Gets or sets the filter value.
        /// </summary>
        public string Filter
        {
            get => filter;

            set
            {
                if (filter == value)
                    return;

                filter = value;

                FilterPatients();

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the collection of all the patients
        /// </summary>
        public ObservableCollection<PatientItemViewModel> AllPatients { get; private set; }

        /// <summary>
        /// Gets the collection of own patients
        /// </summary>
        public ObservableCollection<PatientItemViewModel> MyPatients { get; private set; }

        /// <summary>
        /// Gets or sets the selected patient
        /// </summary>
        public PatientItemViewModel SelectedPatient
        {
            get => selectedPatient;

            set
            {
                if (selectedPatient == value)
                    return;

                selectedPatient = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or set the my selected patient
        /// </summary>
        public PatientItemViewModel MySelectedPatient
        {
            get => mySelectedPatient;

            set
            {
                if (mySelectedPatient == value)
                    return;

                mySelectedPatient = value;
                VivusConsole.WriteLine(mySelectedPatient.Name);

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the last selected patient from both of the tables.
        /// </summary>
        public PatientItemViewModel LastSelectedPatient
        {
            get => lastSelectedPatient;

            set
            {
                if (lastSelectedPatient == value)
                    return;

                lastSelectedPatient = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether a new patient action is running or not.
        /// </summary>
        public bool NewPatientIsRunning
        {
            get => newPatientIsRunning;

            set
            {
                if (newPatientIsRunning == value)
                    return;

                newPatientIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether a choose patient action is running or not.
        /// </summary>
        public bool ChoosePatientIsRunning
        {
            get => choosePatientIsRunning;

            set
            {
                if (choosePatientIsRunning == value)
                    return;

                choosePatientIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether a dismiss patient action is running or not.
        /// </summary>
        public bool DismissPatientIsRunning
        {
            get => dismissPatientIsRunning;

            set
            {
                if (dismissPatientIsRunning == value)
                    return;

                dismissPatientIsRunning = value;

                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the new patient command.
        /// </summary>
        public ICommand NewPatientCommand { get; }

        /// <summary>
        /// Gets the modify patient command.
        /// </summary>
        public ICommand ModifyPatientCommand { get; }

        /// <summary>
        /// Gets the choose command.
        /// </summary>
        public ICommand ChooseCommand { get; }

        /// <summary>
        /// Get the dismiss command.
        /// </summary>
        public ICommand DismissCommand { get; }

        #endregion

        #region Constructors

        public PatientsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            currentFilterId = 0;
            allPatientsLockObj = new object();
            myPatientsLockObj = new object();
            allPatients = new List<PatientItemViewModel>();
            LastSelectedPatient = null;
            counties = new ObservableCollection<BasicEntity<string>>();
            bloodTypes = new ObservableCollection<BasicEntity<string>>();
            rhs = new ObservableCollection<BasicEntity<string>>();

            AllPatients = new ObservableCollection<PatientItemViewModel>();
            MyPatients = new ObservableCollection<PatientItemViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<Doctor>>();

            NewPatientCommand = new RelayCommand<Action>(action => NewPatient(action));
            ModifyPatientCommand = new RelayCommand<Action>(action => ModifyPatient(action));
            ChooseCommand = new RelayCommand(ChoosePatientAsync);
            DismissCommand = new RelayCommand(DismissPatientAsync);

            BindingOperations.EnableCollectionSynchronization(AllPatients, allPatientsLockObj);
            BindingOperations.EnableCollectionSynchronization(MyPatients, myPatientsLockObj);

            Task.Run(async () =>
            {
                await LoadCounties();
                await LoadBloodTypes();
                await LoadRhs();
            }).Wait();

            LoadPatientsAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Filters the patients from the all patients table based on the components of the <see cref="Filter"/> property.
        /// </summary>
        private void FilterPatients()
        {
            int filterId;
            string[] patterns;

            lock (currentFilterId)
            {
                // Generate current identificator
                currentFilterId = (int)currentFilterId + 1;
                // Assign the identificator to the current method
                filterId = (int)currentFilterId;
            }

            patterns = filter.Trim().Split(' ');

            lock (allPatientsLockObj)
            {
                lock (currentFilterId)
                    // If other method changed the identificator, return
                    if ((int)currentFilterId != filterId)
                        return;

                AllPatients.Clear();
            }

            foreach (var patient in allPatients.Where(p => IsValid(p, patterns)))
            {
                lock (allPatientsLockObj)
                {
                    lock (currentFilterId)
                        // If other method changed the identificator, return
                        if ((int)currentFilterId != filterId)
                            return;

                    AllPatients.Add(patient);
                }
            };
        }

        /// <summary>
        /// Loads all the patients.
        /// </summary>
        private async void LoadPatientsAsync()
        {
            lock (allPatientsLockObj)
                AllPatients.Clear();

            lock (myPatientsLockObj)
                MyPatients.Clear();

            await unitOfWork.Patients.GetAllAsync().ContinueWith(async patientsTask =>
            {
                IEnumerable<Patient> patients = patientsTask.Result;
                Person person;
                PatientItemViewModel patientVM;
                int doctorId;

                doctorId = appViewModel.User.PersonID;

                foreach (var patient in patients)
                {
                    person = await unitOfWork.Persons.SingleAsync(patientPerson => patientPerson.PersonID == patient.PersonID);

                    patientVM = new PatientItemViewModel
                    {
                        Id = patient.PersonID,
                        FirstName = patient.Person.FirstName,
                        LastName = patient.Person.LastName,
                        BirthDate = patient.Person.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        NationalIdentificationNumber = patient.Person.Nin,
                        PhoneNumber = patient.Person.PhoneNo,
                        Gender = new BasicEntity<string>(patient.Person.Gender.GenderID, patient.Person.Gender.Type),
                        Address = new AddressViewModel
                        {
                            StreetName = patient.Person.Address.Street,
                            StreetNumber = patient.Person.Address.StreetNo,
                            City = patient.Person.Address.City,
                            County = new BasicEntity<string>(patient.Person.Address.County.CountyID, patient.Person.Address.County.Name),
                            ZipCode = patient.Person.Address.ZipCode
                        },
                        BloodType = new BasicEntity<string>(patient.BloodType.BloodTypeID, patient.BloodType.Type),
                        RH = new BasicEntity<string>(patient.RH.RhID, patient.RH.Type),
                        Status = new BasicEntity<string>(patient.PersonStatus.PersonStatusID, patient.PersonStatus.Type)
                    };

                    if (patient.DoctorID.HasValue && patient.DoctorID == doctorId)
                    {
                        lock (myPatientsLockObj)
                            MyPatients.Add(patientVM);

                        continue;
                    }

                    lock (allPatientsLockObj)
                        lock (currentFilterId)
                            // If filter is active, skip all patients
                            if ((int)currentFilterId == 0 || IsValid(patientVM, filter.Trim().Split(' ')))
                                AllPatients.Add(patientVM);

                    lock (allPatients)
                        allPatients.Add(patientVM);
                }
            });
        }

        /// <summary>
        /// Checks whether a patient viewmodel passes the filter or not.
        /// </summary>
        /// <param name="patient">The patient viewmodel.</param>
        /// <param name="patterns">The patterns to check.</param>
        /// <returns></returns>
        private bool IsValid(PatientItemViewModel patient, string[] patterns)
        {
            return patient.Name.Contains(patterns) || patient.NationalIdentificationNumber.ContainsFromBegining(patterns);
        }

        /// <summary>
        /// Adds a new patient.
        /// </summary>
        /// <param name="newPopup">The create popup action.</param>
        private async void NewPatient(Action newPopup)
        {
            await RunCommand(() => NewPatientIsRunning, async () =>
            {
                await dispatcherWrapper.InvokeAsync(() =>
                {
                    PatientDetailsViewModel patientDetailsVM;
                    PatientItemViewModel patientItemVM;

                    // Create new popup instance
                    newPopup();

                    // Populate the fields
                    patientDetailsVM = new PatientDetailsViewModel
                    {
                        Counties = counties,
                        BloodTypes = bloodTypes,
                        RhTypes = rhs,
                        IdentificationCardAddress = new AddressViewModel
                        {
                            County = counties[0]
                        },
                        SelectedBloodType = bloodTypes[0],
                        SelectedRh = rhs[0]
                    };

                    // Show the popup
                    PatientDetailsPopup.ShowDialog(patientDetailsVM);

                    // If the popup was closed, return
                    if (patientDetailsVM.EndState == FinishState.Closed)
                        return;

                    // If the popup failed, show message and return
                    if (patientDetailsVM.EndState == FinishState.Failed)
                    {
                        Popup("An unexpected error occured.");
                        return;
                    }

                    // Create the patient viewmodel
                    patientItemVM = new PatientItemViewModel
                    {
                        FirstName = patientDetailsVM.Person.FirstName,
                        LastName = patientDetailsVM.Person.LastName,
                        BirthDate = patientDetailsVM.Person.BirthDate,
                        NationalIdentificationNumber = patientDetailsVM.Person.NationalIdentificationNumber,
                        PhoneNumber = patientDetailsVM.Person.PhoneNumber,
                        Gender = patientDetailsVM.Person.Gender,
                        Address = new AddressViewModel
                        {
                            StreetName = patientDetailsVM.IdentificationCardAddress.StreetName,
                            StreetNumber = patientDetailsVM.IdentificationCardAddress.StreetNumber,
                            City = patientDetailsVM.IdentificationCardAddress.City,
                            County = patientDetailsVM.IdentificationCardAddress.County,
                            ZipCode = patientDetailsVM.IdentificationCardAddress.ZipCode
                        },
                        BloodType = patientDetailsVM.SelectedBloodType,
                        RH = patientDetailsVM.SelectedRh,
                        Status = new BasicEntity<string>(-1, (patientDetailsVM.Status ? "Alive" : "Dead")),
                    };

                    // Add the patient to all patients table
                    lock (allPatientsLockObj)
                        AllPatients.Add(patientItemVM);

                    // Add the patient to the hidden list of patient
                    allPatients.Add(patientItemVM);

                    // Show successful message
                    Popup("Patient added successfully!", PopupType.Successful);
                });
            });
        }

        /// <summary>
        /// Modifies an existing patient.
        /// </summary>
        /// <param name="newPopup">The create popup action.</param>
        private async void ModifyPatient(Action newPopup)
        {
            // If there is no selected patient, return
            if (LastSelectedPatient is null)
                return;

            await dispatcherWrapper.InvokeAsync(() =>
            {
                PatientDetailsViewModel patientDetailsVM;

                // Create new popup instance
                newPopup();

                // Populate the fields
                patientDetailsVM = new PatientDetailsViewModel
                {
                    Id = LastSelectedPatient.Id,
                    Counties = counties,
                    BloodTypes = bloodTypes,
                    RhTypes = rhs,
                    Person = new PersonViewModel
                    {
                        FirstName = LastSelectedPatient.FirstName,
                        LastName = LastSelectedPatient.LastName,
                        BirthDate = LastSelectedPatient.BirthDate,
                        NationalIdentificationNumber = LastSelectedPatient.NationalIdentificationNumber,
                        PhoneNumber = LastSelectedPatient.PhoneNumber,
                        Gender = LastSelectedPatient.Gender
                    },
                    IdentificationCardAddress = new AddressViewModel
                    {
                        StreetName = lastSelectedPatient.Address.StreetName,
                        StreetNumber = lastSelectedPatient.Address.StreetNumber,
                        City = lastSelectedPatient.Address.City,
                        County = lastSelectedPatient.Address.County,
                        ZipCode = lastSelectedPatient.Address.ZipCode
                    },
                    SelectedBloodType = LastSelectedPatient.BloodType,
                    SelectedRh = LastSelectedPatient.RH,
                    Status = LastSelectedPatient.Status.Value == "Alive" ? true : false,
                    ButtonType = ButtonType.Modify
                };

                // Show the popup
                PatientDetailsPopup.ShowDialog(patientDetailsVM);

                // If the popup was closed, return
                if (patientDetailsVM.EndState == FinishState.Closed)
                    return;

                // If the popup failed, show message and return
                if (patientDetailsVM.EndState == FinishState.Failed)
                {
                    Popup("An unexpected error occured.");
                    return;
                }

                // Update the patient details
                LastSelectedPatient.FirstName = patientDetailsVM.Person.FirstName;
                LastSelectedPatient.LastName = patientDetailsVM.Person.LastName;
                LastSelectedPatient.BirthDate = patientDetailsVM.Person.BirthDate;
                LastSelectedPatient.NationalIdentificationNumber = patientDetailsVM.Person.NationalIdentificationNumber;
                LastSelectedPatient.PhoneNumber = patientDetailsVM.Person.PhoneNumber;
                LastSelectedPatient.Gender = patientDetailsVM.Person.Gender;
                LastSelectedPatient.Address.StreetName = patientDetailsVM.IdentificationCardAddress.StreetName;
                LastSelectedPatient.Address.StreetNumber = patientDetailsVM.IdentificationCardAddress.StreetNumber;
                LastSelectedPatient.Address.City = patientDetailsVM.IdentificationCardAddress.City;
                LastSelectedPatient.Address.County = patientDetailsVM.IdentificationCardAddress.County;
                LastSelectedPatient.Address.ZipCode = patientDetailsVM.IdentificationCardAddress.ZipCode;
                LastSelectedPatient.BloodType = patientDetailsVM.SelectedBloodType;
                LastSelectedPatient.RH = patientDetailsVM.SelectedRh;
                LastSelectedPatient.Status = new BasicEntity<string>(-1, (patientDetailsVM.Status ? "Alive" : "Dead"));

                // Show successful message
                Popup("Patient details modified successfully!", PopupType.Successful);
            });
        }

        /// <summary>
        /// Chooses asynchronously a patient.
        /// </summary>
        private async void ChoosePatientAsync()
        {
            int allPatientsIndex;

            if (SelectedPatient is null)
            {
                Popup("No patient selected. Before choosing a patient, select one.");
                return;
            }

            try
            {
                // Get the selected item
                PatientItemViewModel selectedItem = SelectedPatient;

                // Get the index inside the all patients list
                allPatientsIndex = await FirstIndexAsync(allPatients, patient => patient.Id == SelectedPatient.Id);

                // Remove the patient from the all patients list
                allPatients.RemoveAt(allPatientsIndex);
                // Change patient's doctor identificator
                unitOfWork.Persons[selectedItem.Id].Patient.DoctorID = appViewModel.User.PersonID;
                await unitOfWork.CompleteAsync();
                // Remove the patient from the all patients table
                AllPatients.Remove(selectedItem);
                // Add the patient to doctor's patients table
                MyPatients.Add(selectedItem);

                Popup("Patient chosen successfully!", PopupType.Successful);
            }
            catch
            {
                Popup("Unexpected error occured. Try again later.");
            }
        }

        /// <summary>
        /// Dismisses a patient.
        /// </summary>
        private async void DismissPatientAsync()
        {
            if (MySelectedPatient is null)
            {
                Popup("No patient selected. Before dismissing a patient, select one.");
                return;
            }

            try
            {
                // Get the selected item
                PatientItemViewModel selectedItem = MySelectedPatient;

                // Change patient's doctor identificator
                unitOfWork.Persons[selectedItem.Id].Patient.DoctorID = null;
                await unitOfWork.CompleteAsync();
                // Add the patient to the all patients list
                allPatients.Add(selectedItem);
                // Remove the patient from the doctor's patients table
                MyPatients.Remove(selectedItem);
                // Add the patient to all patients table
                AllPatients.Add(selectedItem);

                Popup("Patient dismissed successfully!", PopupType.Successful);
            }
            catch
            {
                Popup("Unexpected error occured. Try again later.");
            }
        }

        /// <summary>
        /// Gets asynchronously the index of an element from a collection, based on a condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="collection">The collection to parse.</param>
        /// <param name="predicate">The find condition.</param>
        /// <returns></returns>
        private async Task<int> FirstIndexAsync<T>(IEnumerable<T> collection, Expression<Func<T, bool>> predicate)
        {
            List<T> elements = collection.ToList();

            return await Task.Run(() =>
            {
                for (int i = 0; i < elements.Count; i++)
                    if (predicate.Compile().Invoke(elements[i]))
                        return i;

                throw new InvalidOperationException("Item not found.");
            });
        }

        /// <summary>
        /// Loads asynchronously all the RHs.
        /// </summary>
        /// <returns></returns>
        private async Task LoadRhs()
        {
            List<RH> rhs;

            rhs = (List<RH>)await unitOfWork.RHs.GetAllAsync();

            this.rhs.Add(new BasicEntity<string>(-1, "Select rh"));
            rhs.ForEach(rh =>
            {
                this.rhs.Add(new BasicEntity<string>(rh.RhID, rh.Type));
            });
        }

        /// <summary>
        /// Loads asynchronously all the blood types.
        /// </summary>
        /// <returns></returns>
        private async Task LoadBloodTypes()
        {
            List<BloodType> bloodTypes;

            bloodTypes = (List<BloodType>)await unitOfWork.BloodTypes.GetAllAsync();

            this.bloodTypes.Add(new BasicEntity<string>(-1, "Select blood type"));
            bloodTypes.ForEach(bloodType =>
            {
                this.bloodTypes.Add(new BasicEntity<string>(bloodType.BloodTypeID, bloodType.Type));
            });
        }

        /// <summary>
        /// Loads asynchronously all the counties. 
        /// </summary>
        /// <returns></returns>
        private async Task LoadCounties()
        {
            List<County> counties;

            counties = (List<County>)await unitOfWork.Counties.GetAllAsync();

            this.counties.Add(new BasicEntity<string>(-1, "Select county"));
            counties.ForEach(county =>
            {
                this.counties.Add(new BasicEntity<string>(county.CountyID, county.Name));
            });
        }

        #endregion
    }

    public class PatientItemViewModel : BaseViewModel
    {
        #region Private Members

        private PersonViewModel personVM;
        private int id;
        private BasicEntity<string> bloodType;
        private BasicEntity<string> rh;
        private BasicEntity<string> status;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the id
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
        /// Gets or sets the first name of the patient.
        /// </summary>
        public string FirstName
        {
            get => personVM.FirstName;

            set
            {
                if (personVM.FirstName == value)
                    return;

                personVM.FirstName = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Gets or sets the las name of the patient.
        /// </summary>
        public string LastName
        {
            get => personVM.LastName;

            set
            {
                if (personVM.LastName == value)
                    return;

                personVM.LastName = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Gets the full name of the patient.
        /// </summary>
        public string Name
        {
            get => $"{ FirstName.Trim() } { LastName.Trim() }";
        }

        /// <summary>
        /// Gets or sets the birth date of the patient.
        /// </summary>
        public string BirthDate
        {
            get => personVM.BirthDate;

            set
            {
                if (personVM.BirthDate == value)
                    return;

                personVM.BirthDate = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(Age));
            }
        }

        /// <summary>
        /// Gets the age of the patient.
        /// </summary>
        public int Age
        {
            get => DateTime.Now.AddYears(-DateTime.Parse(BirthDate).Year).Year;
        }

        /// <summary>
        /// Gets or sets the national identification number of the patient.
        /// </summary>
        public string NationalIdentificationNumber
        {
            get => personVM.NationalIdentificationNumber;

            set
            {
                if (personVM.NationalIdentificationNumber == value)
                    return;

                personVM.NationalIdentificationNumber = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the phone number of the patient.
        /// </summary>
        public string PhoneNumber
        {
            get => personVM.PhoneNumber;

            set
            {
                if (personVM.PhoneNumber == value)
                    return;

                personVM.PhoneNumber = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        public BasicEntity<string> Gender
        {
            get => personVM.Gender;

            set
            {
                if (personVM.Gender == value)
                    return;

                personVM.Gender = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the address of the patient.
        /// </summary>
        public AddressViewModel Address { get; set; }

        /// <summary>
        /// Gets or sets the blood type of the patient.
        /// </summary>
        public BasicEntity<string> BloodType
        {
            get => bloodType;

            set
            {
                if (bloodType == value)
                    return;

                bloodType = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(BloodTypeRH));
            }
        }

        /// <summary>
        /// Gets or sets the RH of the patient.
        /// </summary>
        public BasicEntity<string> RH
        {
            get => rh;

            set
            {
                if (rh == value)
                    return;

                rh = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(BloodTypeRH));
            }
        }

        public string BloodTypeRH
        {
            get => $"{ BloodType.Value }{ (RH.Value == "Positive" ? "+" : "-") }";
        }

        /// <summary>
        /// Gets or sets the status of the patient.
        /// </summary>
        public BasicEntity<string> Status
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initalizes a new insance of the <see cref="PatientItemViewModel"/> class with the default values.
        /// </summary>
        public PatientItemViewModel()
        {
            personVM = new PersonViewModel();
        }

        #endregion
    }
}
