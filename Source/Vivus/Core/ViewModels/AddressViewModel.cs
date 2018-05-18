namespace Vivus.Core.ViewModels
{
    using Vivus.Core.DataModels;
    using Vivus.Core.Validators;
	using Vivus = Console;

    /// <summary>
    /// Represents a bindable address.
    /// </summary>
    public class AddressViewModel : BaseViewModel
    {
        #region Private Members

        private string streetName;
        private string streetNumber;
        private string city;
        private BasicEntity<string> county;
        private string zipCode;
        private bool ready;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets whether the validation is mandatory or not.
        /// </summary>
        public bool Validate { get; set; }

        /// <summary>
        /// Gets or sets the name of the street.
        /// </summary>
        public string StreetName
        {
            get => streetName;

            set
            {
                if (streetName == value)
                    return;

                streetName = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the street number.
        /// </summary>
        public string StreetNumber
        {
            get => streetNumber;

            set
            {
                if (streetNumber == value)
                    return;

                streetNumber = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City
        {
            get => city;

            set
            {
                if (city == value)
                    return;

                city = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the county.
        /// </summary>
        public BasicEntity<string> County
        {
            get => county;

            set
            {
                if (county == value)
                    return;

                county = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        public string ZipCode
        {
            get => zipCode;

            set
            {
                if (zipCode == value)
                    return;

                zipCode = value;
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
                // If the validation is optional and there is no field filled, clear errors
                if (!Validate && string.IsNullOrEmpty(StreetName) && string.IsNullOrEmpty(StreetNumber) && string.IsNullOrEmpty(City) && (County is null || County.Id == -1) && string.IsNullOrEmpty(zipCode))
                {
                    if (ready)
                    {
                        ready = false;
                        
                        OnPropertyChanged(nameof(StreetName));
                        OnPropertyChanged(nameof(StreetNumber));
                        OnPropertyChanged(nameof(City));
                        OnPropertyChanged(nameof(County));
                        OnPropertyChanged(nameof(ZipCode));

                        ready = true;
                    }

                    return GetErrorString(propertyName, null);
                }
                
                if (propertyName == nameof(StreetName))
                    return GetErrorString(propertyName, AddressValidator.StreetNameValidation(StreetName));

                if (propertyName == nameof(StreetNumber))
                    return GetErrorString(propertyName, AddressValidator.StreetNumberValidation(StreetNumber));

                if (propertyName == nameof(City))
                    return GetErrorString(propertyName, AddressValidator.CityValidation(City));

                if (propertyName == nameof(County))
                    return GetErrorString(propertyName, AddressValidator.CountyValidation(County));

                if (propertyName == nameof(ZipCode))
                    return GetErrorString(propertyName, AddressValidator.ZipCodeValidation(ZipCode));

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressViewModel"/> class with the given value.
        /// </summary>
        /// <param name="validate">The validation status. Mandatory or not.</param>
        public AddressViewModel(bool validate = true)
        {
            Validate = validate;
            ready = true;
        }

        #endregion
    }
}
