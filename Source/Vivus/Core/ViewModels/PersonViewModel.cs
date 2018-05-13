namespace Vivus.Core.ViewModels
{
    using Vivus.Core.DataModels;
    using Vivus.Core.Validators;

    /// <summary>
    /// Represents a bindable person.
    /// </summary>
    public class PersonViewModel : BaseViewModel
    {
        #region Private Members

        private string firstName;
        private string lastName;
        private string birthDate;
        private string nin;
        private string phoneNo;
        private BasicEntity<string> gender;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the first name of the person.
        /// </summary>
        public string FirstName
        {
            get => firstName;

            set
            {
                if (firstName == value)
                    return;

                firstName = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the last name of the person.
        /// </summary>
        public string LastName
        {
            get => lastName;

            set
            {
                if (lastName == value)
                    return;

                lastName = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the birth date of the person.
        /// </summary>
        public string BirthDate
        {
            get => birthDate;

            set
            {
                if (birthDate == value)
                    return;

                birthDate = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the national identification number of the person.
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
        /// Gets or sets the phone number of the person.
        /// </summary>
        public string PhoneNumber
        {
            get => phoneNo;

            set
            {
                if (phoneNo == value)
                    return;

                phoneNo = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the gender of the person.
        /// </summary>
        public BasicEntity<string> Gender
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
        /// Gets the error string of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns></returns>
        public override string this[string propertyName]
        {
            get
            {
                if (propertyName == nameof(FirstName))
                    return GetErrorString(propertyName, PersonValidator.FirstNameValidation(FirstName));

                if (propertyName == nameof(LastName))
                    return GetErrorString(propertyName, PersonValidator.LastNameValidation(LastName));

                if (propertyName == nameof(BirthDate))
                    return GetErrorString(propertyName, PersonValidator.BirthDateValidation(BirthDate));

                if (propertyName == nameof(NationalIdentificationNumber))
                    return GetErrorString(propertyName, PersonValidator.NinValidation(NationalIdentificationNumber));

                if (propertyName == nameof(PhoneNumber))
                    return GetErrorString(propertyName, PersonValidator.PhoneNumberValidation(PhoneNumber));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonViewModel"/> class with the default values.
        /// </summary>
        public PersonViewModel()
        {
            gender = new BasicEntity<string>(-1, "Not specified");
        }

        #endregion
    }
}
