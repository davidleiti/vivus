namespace Vivus.Core.Doctor.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Doctor.Validators;
    using Vivus.Core.ViewModels;
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a view model for the patient details page.
    /// </summary>
    public class PatientDetailsViewModel : BaseViewModel
    {
        #region Private Members

        ButtonType buttonType;
        private BasicEntity<string> bloodType;
        private BasicEntity<string> rhType;
        private bool status;
        private bool actionIsRunning;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the parent page of the viewmodel.
        /// </summary>
        public IPage ParentPage { get; set; }

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
            AddModifyCommand = new RelayCommand(AddModify);
        }

        #endregion

        #region Private Methods

        private void AddModify()
        {
            ParentPage.AllowErrors();

            if (Errors + Person.Errors + IdentificationCardAddress.Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            VivusConsole.WriteLine("Doctor: Add successfull");
            Popup("Successfull operation!", PopupType.Successful);
        }

        #endregion
    }
}
