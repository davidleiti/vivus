namespace Vivus.Core.Doctor.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
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

        #endregion

        #region Constructors

        public PatientsViewModel()
        {
            AllPatients = new ObservableCollection<PatientItemViewModel>();
            MyPatients = new ObservableCollection<PatientItemViewModel>();

            NewPatientCommand = new RelayCommand(NewPatient);
            ChooseCommand = new RelayCommand(ChoosePatient);
            DismissCommand = new RelayCommand(DismissPatient);

            // Test whether the binding was done right or not
            Application.Current.Dispatcher.Invoke(() =>
            {
                AllPatients.Add(new PatientItemViewModel
                {
                    Id = 1,
                    Name = "Patient One",
                    BloodType = "AB4",
                    Age = 18,
                    Gender = "male",
                    Status = "alive"
                });

                MyPatients.Add(new PatientItemViewModel
                {
                    Id = 2,
                    Name = "Patient Two",
                    BloodType = "A2",
                    Age = 20,
                    Gender = "female",
                    Status = "dead"
                });
            });
        }

        #endregion

        #region Private Methods

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
