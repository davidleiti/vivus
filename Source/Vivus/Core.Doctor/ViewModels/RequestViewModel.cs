namespace Vivus.Core.Doctor.ViewModels
{
    using System.Collections.ObjectModel;
    using Vivus.Core.ViewModels;
    using System.Windows;
    using System;
    using Vivus.Core.DataModels;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Vivus = Console;
    using Vivus.Core.Doctor.Validators;

    /// <summary>
    /// Represents a view model for the request page.
    /// </summary>

    public class RequestViewModel : BaseViewModel
    {
        #region Private Members

        private BasicEntity<string> selectedPatientName;
        private BasicEntity<string> selectedPriority;
        private int? requestThrombocytes;
        private int? requestRedCells;
        private int? requestPlasma;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the collection of the donation history.
        /// </summary>
        public ObservableCollection<RequestItemViewModel> Items { get; }

        public RequestItemViewModel SelectedTableItem { get; set; }

        public List<BasicEntity<string>> PatientNames { get; }

        public List<BasicEntity<string>> Priorities { get; }

        public IPage ParentPage { get; set; }

        /// <summary>
        /// Gets the register command.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Gets the register command.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Gets or sets the requestPatientName.
        /// </summary>
        public BasicEntity<string> SelectedPatientName
        {
            get => selectedPatientName;

            set
            {
                if (selectedPatientName == value)
                    return;

                selectedPatientName = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestPriority.
        /// </summary>
        public BasicEntity<string> SelectedPriority
        {
            get => selectedPriority;

            set
            {
                if (selectedPriority == value)
                    return;

                selectedPriority = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestThrombocytes.
        /// </summary>
        public int? RequestThrombocytes
        {
            get => requestThrombocytes;

            set
            {
                if (requestThrombocytes == value)
                    return;

                requestThrombocytes = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestRedCells.
        /// </summary>
        public int? RequestRedCells
        {
            get => requestRedCells;

            set
            {
                if (requestRedCells == value)
                    return;

                requestRedCells = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestPlasma.
        /// </summary>
        public int? RequestPlasma
        {
            get => requestPlasma;

            set
            {
                if (requestPlasma == value)
                    return;

                requestPlasma = value;

                OnPropertyChanged();
            }
        }

        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(SelectedPatientName))
                    return GetErrorString(propertyName, DoctorValidator.NotNullStringFieldValidation(SelectedPatientName, "Patient"));

                if (propertyName == nameof(SelectedPriority))
                    return GetErrorString(propertyName, DoctorValidator.NotNullStringFieldValidation(SelectedPriority, "Priority"));

                if (propertyName == nameof(RequestThrombocytes))
                    return GetErrorString(propertyName, DoctorValidator.NotNullIntegerFieldValidation(RequestThrombocytes, "Thrombocytes"));

                if (propertyName == nameof(RequestRedCells))
                    return GetErrorString(propertyName, DoctorValidator.NotNullIntegerFieldValidation(RequestRedCells, "Red cells"));

                if (propertyName == nameof(RequestPlasma))
                    return GetErrorString(propertyName, DoctorValidator.NotNullIntegerFieldValidation(RequestPlasma, "Plasma"));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestViewModel"/> class with the default values.
        /// </summary>
        public RequestViewModel()
        {

            Items = new ObservableCollection<RequestItemViewModel>();
            PatientNames = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select patient") };
            Priorities = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select priority") };
            AddCommand = new RelayCommand(Add);
            CancelCommand = new RelayCommand(Cancel);




            // Test whether the binding was done right or not
            Application.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(new RequestItemViewModel
                {
                    Id = 39,
                    PatientName = "Ionut",
                    Priority = "easy",
                    Thrombocytes = 1,
                    RedCells = 213,
                    Plasma = 555,
                    RequestStatus = "done",
                    Cdc = "???",
                    Dcn = "dcn"
                });

            });
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Add.
        /// </summary>
        private void Add()
        {
            ParentPage.AllowErrors();

            if(Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            Vivus.Console.WriteLine("Doctor: Add works!");
            Popup("Add works!", PopupType.Successful);
        }

        /// <summary>
        /// Cancel.
        /// </summary>
        private void Cancel()
        {
            /*ParentPage.AllowErrors();

            if (Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }*/

            Vivus.Console.WriteLine("Doctor: Cancel works!");
            Popup("Cancel works!", PopupType.Successful);
        }

        #endregion
    }

    /// <summary>
    /// Represents an item view model for the requests table.
    /// </summary>

    public class RequestItemViewModel : BaseViewModel
    {
        #region Private Members

        private int id;
        private string patientName;
        private string priority;
        private int thrombocytes;
        private int redCells;
        private int plasma;
        private string requestStatus;
        private string cdc;
        private string dcn;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identificator.
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
        /// Gets or sets the patientName.
        /// </summary>
        public string PatientName
        {
            get => patientName;

            set
            {
                if (patientName == value)
                    return;

                patientName = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public string Priority
        {
            get => priority;

            set
            {
                if (priority == value)
                    return;

                priority = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the thrombocytes.
        /// </summary>
        public int Thrombocytes
        {
            get => thrombocytes;

            set
            {
                if (thrombocytes == value)
                    return;

                thrombocytes = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the red cells.
        /// </summary>
        public int RedCells
        {
            get => redCells;

            set
            {
                if (redCells == value)
                    return;

                redCells = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the plasma.
        /// </summary>
        public int Plasma
        {
            get => plasma;

            set
            {
                if (plasma == value)
                    return;

                plasma = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the requestStatus.
        /// </summary>
        public string RequestStatus
        {
            get => requestStatus;

            set
            {
                if (requestStatus == value)
                    return;

                requestStatus = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the cdc.
        /// </summary>
        public string Cdc
        {
            get => cdc;

            set
            {
                if (cdc == value)
                    return;

                cdc = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the patientName.
        /// </summary>
        public string Dcn
        {
            get => dcn;

            set
            {
                if (dcn == value)
                    return;

                dcn = value;

                OnPropertyChanged();
            }
        }

        #endregion

    }

}
