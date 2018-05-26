namespace Vivus.Core.Doctor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.IoC;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the patients page.
    /// </summary>
    public class PatientsViewModel : BaseViewModel
    {

        #region Private Members

        private string filter;
        private PatientItemViewModel selectedPatient;
        private PatientItemViewModel mySelectedPatient;
        private bool newPatientIsRunning;
        private bool choosePatientIsRunning;
        private bool dismissPatientIsRunning;
        private IUnitOfWork unitOfWork;
        private IApllicationViewModel<Model.Doctor> appViewModel;

        // Lock objects
        private object allPatientsLockObj;
        private object myPatientsLockObj;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the filter applied over the all patients collection.
        /// </summary>
        public string Filter
        {
            get => filter;

            set
            {
                if (filter == value)
                    return;

                filter = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the collection of all the patients
        /// </summary>
        public ObservableCollection<PatientItemViewModel> AllPatients { get; }

        /// <summary>
        /// Gets the new patient command
        /// </summary>
        public ICommand NewPatientCommand { get; }

        /// <summary>
        /// Gets the choose command
        /// </summary>
        public ICommand ChooseCommand { get; }

        /// <summary>
        /// Gets the collection of own patients
        /// </summary>
        public ObservableCollection<PatientItemViewModel> MyPatients { get; }

        /// <summary>
        /// Get the dismiss command
        /// </summary>
        public ICommand DismissCommand { get; }

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
                Vivus.Console.WriteLine(selectedPatient.Name);

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
                Vivus.Console.WriteLine(mySelectedPatient.Name);

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

        #region Constructors

        public PatientsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            AllPatients = new ObservableCollection<PatientItemViewModel>();
            MyPatients = new ObservableCollection<PatientItemViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Model.Doctor>>();

            NewPatientCommand = new RelayCommand(NewPatient);
            ChooseCommand = new RelayCommand(ChoosePatient);
            DismissCommand = new RelayCommand(DismissPatient);

            allPatientsLockObj = new object();
            myPatientsLockObj = new object();
            BindingOperations.EnableCollectionSynchronization(AllPatients, allPatientsLockObj);
            BindingOperations.EnableCollectionSynchronization(MyPatients, myPatientsLockObj);

            LoadPatientsAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads all the patients.
        /// </summary>
        private async void LoadPatientsAsync()
        {
            await Task.Run(() =>
            {
                lock (allPatientsLockObj)
                {
                    AllPatients.Clear();
                }
                lock (myPatientsLockObj)
                {
                    MyPatients.Clear();
                }

                unitOfWork.Patients
                            .Entities
                            .Where(patient => !patient.DoctorID.HasValue || patient.DoctorID == appViewModel.User.PersonID)
                            .ToList()
                            .ForEach(patient =>
                            {
                                PatientItemViewModel patientVM = new PatientItemViewModel
                                {
                                    Id = patient.PersonID,
                                    Name = $"{ patient.Person.FirstName } { patient.Person.LastName }",
                                    BloodType = patient.BloodType.Type + (patient.RH.Type == "Positive" ? "+" : "-"),
                                    Age = DateTime.Now.Year - patient.Person.BirthDate.Year,
                                    Gender = patient.Person.Gender.Type,
                                    Status = patient.PersonStatus.Type
                                };

                                if (patient.DoctorID.HasValue)
                                {
                                    lock (myPatientsLockObj)
                                        MyPatients.Add(patientVM);

                                    return;
                                }

                                lock (allPatientsLockObj)
                                    AllPatients.Add(patientVM);
                            });
            });
        }

        /// <summary>
        /// Add a new patient
        /// </summary>
        private void NewPatient()
        {
            Vivus.Console.WriteLine("PatientsPage: Add new patient!");
            Popup("Successfull operation!", PopupType.Successful);
        }

        /// <summary>
        /// Choose a patient
        /// </summary>
        private void ChoosePatient()
        {
            Vivus.Console.WriteLine("PatientsPage: Choose a patient!");
            Popup("Successfull operation!", PopupType.Successful);
        }

        /// <summary>
        /// Dismiss a patient
        /// </summary>
        private void DismissPatient()
        {
            Vivus.Console.WriteLine("PatientsPage: Dismiss a patient!");
            Popup("Successfull operation!", PopupType.Successful);
        }


        #endregion
    }

    public class PatientItemViewModel : BaseViewModel
    {
        #region Private Members

        private int id;
        private string name;
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
