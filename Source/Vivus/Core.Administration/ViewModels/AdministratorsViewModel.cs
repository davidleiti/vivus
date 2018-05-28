namespace Vivus.Core.Administration.ViewModels
{
	using System.Collections.ObjectModel;
	using System.Windows.Input;
	using Vivus.Core.Administration.Validators;
	using Vivus.Core.DataModels;
	using Vivus.Core.ViewModels;
    using Vivus.Core.Administration.IoC;
    using Vivus.Core.UoW;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Vivus.Core.Model;
    using Vivus.Core.Security;
    using System;
    using System.Globalization;
    using Vivus.Core.ViewModels.Base;

    /// <summary>
    /// Represents a view model for the administrators page.
    /// </summary>
    public class AdministratorsViewModel : BaseViewModel {
		#region Private Members

		private string email;
		private object password;
		private bool isActive = true;
        private bool isEnabled;
		private AdministratorItemViewModel selectedItem;
        private ButtonType buttonType;
        private bool optionalErrors;
        private bool actionIsRunning;
        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<Administrator> appViewModel;
        private ISecurity security;

        #endregion

        #region Public Members

        public IPage ParentPage { get; set; }

		///<summary>
		///Gets or sets the email address of the adminstrator
		/// </summary>
		public string Email {
			get => email;

			set {
				if (email == value)
					return;

				email = value;

				OnPropertyChanged();
			}
		}

		///<summary>
		///Gets or sets the password of the adminstrator
		/// </summary>
		public object Password {
			get => password;

			set {
				if (password == value)
					return;

				password = value;
				
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the active status of the adminstrator
		/// </summary>
		public bool IsActive {
			get => isActive;

			set {
				if (isActive == value)
					return;

				isActive = value;

				OnPropertyChanged();
			}
		}

        /// <summary>
        /// Gets or sets the enable status of the active status of the administrator.
        /// </summary>
        public bool IsActiveIsEnabled
        {
            get => isEnabled;

            set
            {
                if (isEnabled == value)
                    return;

                isEnabled = value;

                OnPropertyChanged();
            }
        }

		/// <summary>
		/// Gets the person view model.
		/// </summary>
		public PersonViewModel Person { get; }

		/// <summary>
		/// Gets the address view model.
		/// </summary>
		public AddressViewModel Address { get; }

        /// <summary>
        /// Gets or sets the type of the table button.
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
        /// Gets or sets the flag that indicates whether an action is running or not.
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
        /// Gets the list of counties.
        /// </summary>
        public ObservableCollection<BasicEntity<string>> Counties { get; }
        
		/// <summary>
		/// Gets or sets the selected item in the administrators table
		/// </summary>
		public AdministratorItemViewModel SelectedItem {
			get => selectedItem; 

			set
            {
                if (value == selectedItem)
                    return;

                selectedItem = value;

                if (selectedItem is null)
                {
                    ButtonType = ButtonType.Add;
                    optionalErrors = false;

                    dispatcherWrapper.InvokeAsync(() => ParentPage.DontAllowErrors());

                    ClearFields();
                }
                else
                {
                    ButtonType = ButtonType.Modify;
                    optionalErrors = true;

                    dispatcherWrapper.InvokeAsync(() => ParentPage.AllowOptionalErrors());
                    PopulateFields();
                }

				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets the add administrator command.
		/// </summary>
		public ICommand AddCommand { get; }

		/// <summary>
		/// Gets the error string of a property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns></returns>
		public override string this[string propertyName] {
			get
            {
				if (propertyName == nameof(Email))
					return GetErrorString(propertyName, AdministrationValidator.EmailValidation(Email));

                if (propertyName == nameof(Password) && ParentPage != null)
                {
                    if (optionalErrors)
                        return GetNotMandatoryErrorString(propertyName, AdministrationValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                    return GetErrorString(propertyName, AdministrationValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));
                }

				return null;
			}
		}

		/// <summary>
		/// Gets the list of administrator items
		/// </summary>
		public ObservableCollection<AdministratorItemViewModel> Administrators { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministratorsViewModel"/> class with the default values.
        /// </summary>
        public AdministratorsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher)) {
            IsActiveIsEnabled = true;
            ButtonType = ButtonType.Add;
            optionalErrors = false;
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<Administrator>>();
            security = IoCContainer.Get<ISecurity>();

            Person = new PersonViewModel();
            Counties = new ObservableCollection<BasicEntity<string>>();
            Administrators = new ObservableCollection<AdministratorItemViewModel>();
            Address = new AddressViewModel();

            LoadCountiesAsync();
            LoadAdministratorsAsync();

            AddCommand = new RelayCommand(async () => await AddModifyAsync());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministratorsViewModel"/> class with the given values.
        /// </summary>
        /// <param name="unitOfWork">The UoW used to access repositories.</param>
        /// <param name="appViewModel">The viewmodel for the application.</param>
        /// <param name="dispatcherWrapper">The ui thread dispatcher.</param>
        /// <param name="security">The collection of security methods.</param>
        public AdministratorsViewModel(IUnitOfWork unitOfWork, IApplicationViewModel<Administrator> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security)
        {
            IsActiveIsEnabled = true;
            ButtonType = ButtonType.Add;
            optionalErrors = false;
            this.unitOfWork = unitOfWork;
            this.appViewModel = appViewModel;
            this.dispatcherWrapper = dispatcherWrapper;
            this.security = security;

            Person = new PersonViewModel();
            Counties = new ObservableCollection<BasicEntity<string>>();
            Administrators = new ObservableCollection<AdministratorItemViewModel>();
            Address = new AddressViewModel();

            LoadCountiesAsync();
            LoadAdministratorsAsync();

            AddCommand = new RelayCommand(async () => await AddModifyAsync());
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Adds an adminstrator.
        /// </summary>
        public async Task AddModifyAsync()
        {
            await RunCommand(() => ActionIsRunning, async () =>
            {
                if (ButtonType == ButtonType.Add)
                    await AddAdministratorAsync();
                else
                    await ModifyAdministratorAsync();
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads all the counties asynchronously.
        /// </summary>
        /// <returns></returns>
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
        /// Loads all the administrators (based on current administrator privileges) asynchronously.
        /// </summary>
        /// <returns></returns>
        private async void LoadAdministratorsAsync()
        {
            await Task.Run(() =>
                unitOfWork.Administrators
                        .Entities
                        .Where(admin => !admin.IsOwner || admin.IsOwner && appViewModel.User.IsOwner)
                        .ToList()
                        .ForEach(admin =>
                            dispatcherWrapper.InvokeAsync(() =>
                                Administrators.Add(new AdministratorItemViewModel
                                {
                                    PersonID = admin.PersonID,
                                    Email = admin.Account.Email,
                                    IsActive = admin.Active,
                                    IsAdmininistrator = !admin.IsOwner,
                                    Person = new PersonViewModel
                                    {
                                        FirstName = admin.Person.FirstName,
                                        LastName = admin.Person.LastName,
                                        BirthDate = admin.Person.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        NationalIdentificationNumber = admin.Person.Nin,
                                        PhoneNumber = admin.Person.PhoneNo,
                                        Gender = new BasicEntity<string>(admin.Person.GenderID, admin.Person.Gender.Type)
                                    },
                                    Address = new AddressViewModel
                                    {
                                        County = new BasicEntity<string>(admin.Person.Address.CountyID, admin.Person.Address.County.Name),
                                        City = admin.Person.Address.City,
                                        StreetName = admin.Person.Address.Street,
                                        StreetNumber = admin.Person.Address.StreetNo,
                                        ZipCode = admin.Person.Address.ZipCode
                                    }
                                })
                            )
                        )
            );
        }

        /// <summary>
        /// Clears all the fields of the viewmodel.
        /// </summary>
        private void ClearFields()
        {
            Email = string.Empty;
            IsActive = true;
            IsActiveIsEnabled = true;
            Person.FirstName = string.Empty;
            Person.LastName = string.Empty;
            Person.BirthDate = string.Empty;
            Person.NationalIdentificationNumber = string.Empty;
            Person.PhoneNumber = string.Empty;
            Person.Gender = new BasicEntity<string>(-1, "Not specified");
            Address.StreetName = string.Empty;
            Address.StreetNumber = string.Empty;
            Address.City = string.Empty;
            Address.County = Counties[0];
            Address.ZipCode = string.Empty;
        }

        /// <summary>
        /// Populates all the fields of the viewmodel.
        /// </summary>
        private void PopulateFields()
        {
            Email = selectedItem.Email;
            IsActive = selectedItem.IsActive;
            IsActiveIsEnabled = selectedItem.IsAdmininistrator;
            Person.FirstName = selectedItem.Person.FirstName;
            Person.LastName = selectedItem.Person.LastName;
            Person.BirthDate = selectedItem.Person.BirthDate;
            Person.NationalIdentificationNumber = selectedItem.Person.NationalIdentificationNumber;
            Person.PhoneNumber = selectedItem.Person.PhoneNumber;
            Person.Gender = selectedItem.Person.Gender;
            Address.StreetName = selectedItem.Address.StreetName;
            Address.StreetNumber = selectedItem.Address.StreetNumber;
            Address.City = selectedItem.Address.City;
            Address.County = selectedItem.Address.County;
            Address.ZipCode = selectedItem.Address.ZipCode;
        }

        /// <summary>
        /// Adds a new administrator asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task AddAdministratorAsync()
        {
            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                if (Errors + Address.Errors + Person.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                try
                {
                    Administrator admin;
                    AdministratorItemViewModel adminViewModel;

                    admin = null;
                    adminViewModel = null;

                    FillModelAdministrator(ref admin, true);
                    unitOfWork.Administrators.Add(admin);
                    // Make changes persistent
                    unitOfWork.Complete();

                    // Update the table
                    FillViewModelAdministrator(ref adminViewModel);
                    adminViewModel.PersonID = admin.PersonID;

                    dispatcherWrapper.InvokeAsync(() => Administrators.Add(adminViewModel));

                    Popup($"Administrator added successfully!", PopupType.Successful);
                }
                catch
                {
                    Popup($"An error occured while adding the administrator.");
                }
            });
        }

        /// <summary>
        /// Modifies the attributes of an adminsitrator/owner asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task ModifyAdministratorAsync()
        {
            await Task.Run(() =>
            {
                if (selectedItem is null)
                    return;

                if (Errors + Address.Errors + Person.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }

                try
                {
                    Administrator admin;
                    string role;
                    
                    admin = unitOfWork.Persons[SelectedItem.PersonID].Administrator;

                    FillModelAdministrator(ref admin);
                    // Make changes persistent
                    unitOfWork.Complete();

                    // Update the table
                    FillViewModelAdministrator(ref selectedItem);

                    role = "Administrator";

                    if (!SelectedItem.IsAdmininistrator)
                        role = "Owner";

                    Popup($"{ role } modified successfully!", PopupType.Successful);
                }
                catch
                {
                    string role = "administrator";

                    if (!SelectedItem.IsAdmininistrator)
                        role = "owner";

                    Popup($"An error occured while modifing the { role }.");
                }

                ButtonType = ButtonType.Add;
                SelectedItem = null;
            });
        }

        /// <summary>
        /// Fills the fields of an administrator.
        /// </summary>
        /// <param name="admin">The administrator instance.</param>
        /// <param name="fillOptional">Whether to fill the optional field also or not.</param>
        private void FillModelAdministrator(ref Administrator admin, bool fillOptional = false)
        {
            IContainPassword parentPage;
            Model.Gender gender;
            County county;
            
            parentPage = (ParentPage as IContainPassword);
            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            county = unitOfWork.Counties.Find(c => c.Name == Address.County.Value).Single();

            // If the instance is null, initialize it
            if (admin is null)
                admin = new Administrator();
            // If the person instance is null, initialize it
            if (admin.Person is null)
                admin.Person = new Person();
            // If the person's address instance is null, initialize it
            if (admin.Person.Address is null)
                admin.Person.Address = new Address();
            // If the account instance is null, initialize it
            if (admin.Account is null)
                admin.Account = new Account();

            // Update the database
            //  Administrator properties
            admin.Active = IsActive;
            //  Account properties
            admin.Account.Email = Email;
            admin.Account.Password = parentPage.SecurePasword.Length == 0 && !fillOptional ? admin.Account.Password : security.HashPassword(parentPage.SecurePasword.Unsecure());
            //  Person properties
            admin.Person.FirstName = Person.FirstName;
            admin.Person.LastName = Person.LastName;
            admin.Person.BirthDate = DateTime.Parse(Person.BirthDate);
            admin.Person.Nin = Person.NationalIdentificationNumber;
            admin.Person.PhoneNo = Person.PhoneNumber;
            admin.Person.Gender = gender;
            //  Address properties
            admin.Person.Address.Street = Address.StreetName;
            admin.Person.Address.StreetNo = Address.StreetNumber;
            admin.Person.Address.City = Address.City;
            admin.Person.Address.County = county;
            admin.Person.Address.ZipCode = Address.ZipCode;
        }

        /// <summary>
        /// Fills the fields of an administrator viewmodel.
        /// </summary>
        /// <param name="admin">The administrator viewmodel instance.</param>
        private void FillViewModelAdministrator(ref AdministratorItemViewModel admin)
        {
            Model.Gender gender;
            County county;

            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            county = unitOfWork.Counties.Find(c => c.Name == Address.County.Value).Single();

            // If the instance is null, initialize it
            if (admin is null)
                admin = new AdministratorItemViewModel();
            // If the person viewmodel is null, initialize it
            if (admin.Person is null)
                admin.Person = new PersonViewModel();
            // If the address viewmodel is null, initialize it
            if (admin.Address is null)
                admin.Address = new AddressViewModel();

            //  Administrator properties
            admin.IsActive = IsActive;
            //  Account properties
            admin.Email = Email;
            //  Person properties
            admin.Person.FirstName = Person.FirstName;
            admin.Person.LastName = Person.LastName;
            admin.Person.BirthDate = Person.BirthDate;
            admin.Person.NationalIdentificationNumber = Person.NationalIdentificationNumber;
            admin.Person.PhoneNumber = Person.PhoneNumber;
            admin.Person.Gender = new BasicEntity<string>(gender.GenderID, gender.Type);
            //  Address properties
            admin.Address.StreetName = Address.StreetName;
            admin.Address.StreetNumber = Address.StreetNumber;
            admin.Address.City = Address.City;
            admin.Address.County = new BasicEntity<string>(county.CountyID, county.Name);
            admin.Address.ZipCode = Address.ZipCode;
        }

        #endregion

        #region Public Inner Classes

        public class AdministratorItemViewModel : BaseViewModel {
			#region Private fields

			private int personID;
            private int accountID;
			private string email;
			private bool isActive;
            private bool isAdmin;

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets or sets the identificator of the person.
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
            /// Gets or sets the identificator of the account.
            /// </summary>
            public int AccountID
            {
                get => accountID;

                set
                {
                    if (accountID == value)
                        return;

                    OnPropertyChanged();
                }
            }

            /// <summary>
            /// Gets or sets the person details.
            /// </summary>
            public PersonViewModel Person { get; set; }

            /// <summary>
            /// Gets or sets the address details.
            /// </summary>
            public AddressViewModel Address { get; set; }

            /// <summary>
            /// Gets or sets the email address.
            /// </summary>
			public string Email {
				get => email;
				set {
					if (email == value)
						return;

					email = value;

					OnPropertyChanged();
				}
			}

            /// <summary>
            /// Gets or sets whether the administrator is still active or not.
            /// </summary>
			public bool IsActive {
				get => isActive;
				set {
					if (isActive == value)
						return;

					isActive = value;

					OnPropertyChanged();
				}
			}

            /// <summary>
            /// Gets or sets whether the current person is either an administrator or an owner.
            /// </summary>
            public bool IsAdmininistrator
            {
                get => isAdmin;

                set
                {
                    if (isAdmin == value)
                        return;

                    isAdmin = value;

                    OnPropertyChanged();
                }
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="AdministratorItemViewModel"/> class with the given values.
            /// </summary>
            public AdministratorItemViewModel()
            {
                IsAdmininistrator = true;
            }

            #endregion
        }

        #endregion
    }
}
