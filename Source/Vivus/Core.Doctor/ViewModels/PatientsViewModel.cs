namespace Vivus.Core.Doctor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
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
        private IApllicationViewModel<Doctor> appViewModel;
        private List<PatientItemViewModel> allPatients;

        #endregion

        #region Public Properties

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
        public ObservableCollection<PatientItemViewModel> MyPatients { get; }

        /// <summary>
        /// Gets or sets the selected patient
        /// </summary>
        public PatientItemViewModel SelectedPatient {
            get => selectedPatient;

            set
            {
                if (selectedPatient == value)
                    return;

                selectedPatient = value;
                VivusConsole.WriteLine(selectedPatient.Name);

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or set the my selected patient
        /// </summary>
        public PatientItemViewModel MySelectedPatient {
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
        /// Gets the new patient command
        /// </summary>
        public ICommand NewPatientCommand { get; }

        /// <summary>
        /// Gets the choose command
        /// </summary>
        public ICommand ChooseCommand { get; }

        /// <summary>
        /// Get the dismiss command
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

            AllPatients = new ObservableCollection<PatientItemViewModel>();
            MyPatients = new ObservableCollection<PatientItemViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Doctor>>();

            NewPatientCommand = new RelayCommand(NewPatient);
            ChooseCommand = new RelayCommand(ChoosePatient);
            DismissCommand = new RelayCommand(DismissPatient);

            BindingOperations.EnableCollectionSynchronization(AllPatients, allPatientsLockObj);
            BindingOperations.EnableCollectionSynchronization(MyPatients, myPatientsLockObj);

            LoadPatientsAsync();
        }

        #endregion

        #region Private Methods

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

                foreach (var patient in patients)
                {
                    person = await unitOfWork.Persons.SingleAsync(patientPerson => patientPerson.PersonID == patient.PersonID);

                    patientVM = new PatientItemViewModel
                    {
                        Id = patient.PersonID,
                        Name = $"{ patient.Person.FirstName } { patient.Person.LastName }",
                        NationalIdentificationNumber = patient.Person.Nin,
                        BloodType = patient.BloodType.Type + (patient.RH.Type == "Positive" ? "+" : "-"),
                        Age = DateTime.Now.Year - patient.Person.BirthDate.Year,
                        Gender = patient.Person.Gender.Type,
                        Status = patient.PersonStatus.Type
                    };

                    if (patient.DoctorID.HasValue)
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
        /// Add a new patient
        /// </summary>
        private void NewPatient()
        {
            VivusConsole.WriteLine("PatientsPage: Add new patient!");
            Popup("Successfull operation!", PopupType.Successful);
        }

        /// <summary>
        /// Choose a patient
        /// </summary>
        private void ChoosePatient()
        {
            VivusConsole.WriteLine("PatientsPage: Choose a patient!");
            Popup("Successfull operation!", PopupType.Successful);
        }

        /// <summary>
        /// Dismiss a patient
        /// </summary>
        private void DismissPatient()
        {
            VivusConsole.WriteLine("PatientsPage: Dismiss a patient!");
            Popup("Successfull operation!", PopupType.Successful);
        }
        
        #endregion
    }

    public class PatientItemViewModel : BaseViewModel
    {
        #region Private Members

        private int id;
        private string name;
        private string nin;
        private string bloodType;
        private int age;
        private string gender;
        private string status;

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
        /// Gets or sets the national identification number of the patient.
        /// </summary>
        public string NationalIdentificationNumber
        {
            get => nin;

            set
            {
                if (nin == value)
                    return;

                nin = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the blood type
        /// </summary>
        public string BloodType
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
        /// Gets or sets the age
        /// </summary>
        public int Age
        {
            get => age;

            set
            {
                if (age == value)
                    return;

                age = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        public string Gender
        {
            get => gender;

            set
            {
                if (gender == value)
                    return;

                gender = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string Status
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
    }
}
