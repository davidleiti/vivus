namespace Vivus.Core.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.Administration.IoC;
    using Vivus.Core.Administration.Validators;
    using Vivus.Core.DataModels;
    using Vivus.Core.Model;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the donation center personnel page.
    /// </summary>
    public class DCPersonnelViewModel : BaseViewModel
    {
        #region Private Members

        private string email;
        private object password;
        private bool isActive;
        private ButtonType buttonType;
        private bool optionalErrors;
        private bool actionIsRunning;
        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<Administrator> appViewModel;
        private ISecurity security;
        private DCPItemViewModel selectedItem;
        private BasicEntity<string> selectedDonationCenter;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the email address of the DCPersonnel.
        /// </summary>
        public string Email
        {
            get => email;

            set
            {
                if (email == value)
                    return;

                email = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the binding property for the password.
        /// </summary>
        public object Password
        {
            get => password;

            set
            {
                if (password == value)
                    return;

                password = value;

                OnPropertyChanged();
            }
        }


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
        /// Gets the list of donation centers.
        /// </summary>
        public List<BasicEntity<string>> DonationCenters { get; }

        public BasicEntity<string> SelectedDonationCenter
        {
            get => selectedDonationCenter;
            set
            {
                if (selectedDonationCenter == value)
                    return;

                selectedDonationCenter = value;

                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get => isActive;
            set
            {
                if (isActive == value)
                    return;

                isActive = value;

                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Gets the person view model.
        /// </summary>
        public PersonViewModel Person { get; set; }

        /// <summary>
        /// Gets the address
        /// </summary>
        public AddressViewModel Address { get; set; }

        /// <summary>
        /// Gets the list of counties.
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }

        /// <summary>
        /// Gets the add command.
        /// </summary>
        public ICommand AddCommand { get; }

        /// <summary>
        /// Gets the collection of DCPersonnel.
        /// </summary>
        public ObservableCollection<DCPItemViewModel> Items { get; }

        public DCPItemViewModel SelectedTableItem
        {
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
        /// Gets the error string of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns></returns>
        public override string this[string propertyName]
        {
            get
            {

                if (propertyName == nameof(Email))
                    return GetErrorString(propertyName, AdministrationValidator.EmailValidation(Email));

                // if (propertyName == nameof(Password) && ParentPage != null)
                //   return GetErrorString(propertyName, AdministrationValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                if (propertyName == nameof(Password) && ParentPage != null)
                {
                    if (optionalErrors)
                        return GetNotMandatoryErrorString(propertyName, AdministrationValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));

                    return GetErrorString(propertyName, AdministrationValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));
                }

                if (propertyName == nameof(SelectedDonationCenter))
                    return GetErrorString(propertyName, AdministrationValidator.DonationCenterValidation(SelectedDonationCenter));

                return null;
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DCPersonnelViewModel"/> class with the default values.
        /// </summary>
        public DCPersonnelViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            //SelectedDonationCenter = new BasicEntity<string>(-1, "Select donation center");
            DonationCenters = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select donation center") };
            Active = true;

            ButtonType = ButtonType.Add;
            optionalErrors = false;
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<Administrator>>();
            security = IoCContainer.Get<ISecurity>();

            Person = new PersonViewModel();
            Address = new AddressViewModel();
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select county") };


            Items = new ObservableCollection<DCPItemViewModel>();
            AddCommand = new RelayCommand(async () => await AddModifyAsync());
            LoadCountiesAsync();
            LoadDonationCentersAsync();
            LoadDcPersonnelAsync();

        }

        public DCPersonnelViewModel(IUnitOfWork unitOfWork, IApplicationViewModel<Administrator> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security)
        {

            ButtonType = ButtonType.Add;
            optionalErrors = false;
            this.unitOfWork = unitOfWork;
            this.appViewModel = appViewModel;
            this.dispatcherWrapper = dispatcherWrapper;
            this.security = security;

            Person = new PersonViewModel();
            Counties = new List<BasicEntity<string>>();
            Items = new ObservableCollection<DCPItemViewModel>();
            Counties = new List<BasicEntity<string>>();
            Address = new AddressViewModel();

            LoadCountiesAsync();
            LoadDonationCentersAsync();
            LoadDcPersonnelAsync();
            AddCommand = new RelayCommand(async () => await AddModifyAsync());




        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Adds/modifies a dcPersonnel
        /// </summary>
        public async Task AddModifyAsync()
        {
            await RunCommand(() => ActionIsRunning, async () =>
            {
                if (ButtonType == ButtonType.Add)
                    await AddDCPersonnelAsync();
                else
                    await ModifyDCPersonnelAsync();
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
        /// loads the donation centers asynchronously
        /// </summary>
        private async void LoadDonationCentersAsync()
        {
            await Task.Run(() =>
            {
                DonationCenters.Clear();
                DonationCenters.Add(new BasicEntity<string>(-1, "Select Donation Center"));
                unitOfWork.DonationCenters.Entities.ToList().ForEach(center =>
                    dispatcherWrapper.InvokeAsync(() => DonationCenters.Add(new BasicEntity<string>(center.DonationCenterID, center.Name)))
                );
            });
        }

        /// <summary>
        /// loads the DCPersonnel asynchronously
        /// </summary>
        private async void LoadDcPersonnelAsync()
        {
            await Task.Run(() =>
            {
                unitOfWork.DCPersonnel
                .Entities
                .ToList()
                .ForEach(dcPersonnel =>
                {
                    dispatcherWrapper.InvokeAsync(() =>
                    Items.Add(new DCPItemViewModel
                    {

                        Id = dcPersonnel.PersonID,
                        Email = dcPersonnel.Account.Email,
                        IsActive = dcPersonnel.Active,
                        Person = new PersonViewModel
                        {
                            FirstName = dcPersonnel.Person.FirstName,
                            LastName = dcPersonnel.Person.LastName,

                            BirthDate = dcPersonnel.Person.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                            NationalIdentificationNumber = dcPersonnel.Person.Nin,
                            PhoneNumber = dcPersonnel.Person.PhoneNo,
                            Gender = new BasicEntity<string>(dcPersonnel.Person.GenderID, dcPersonnel.Person.Gender.Type)
                        },
                        Address = new AddressViewModel
                        {
                            County = new BasicEntity<string>(dcPersonnel.Person.Address.CountyID, dcPersonnel.Person.Address.County.Name),
                            City = dcPersonnel.Person.Address.City,
                            StreetName = dcPersonnel.Person.Address.Street,
                            StreetNumber = dcPersonnel.Person.Address.StreetNo,
                            ZipCode = dcPersonnel.Person.Address.ZipCode
                        },
                        DonationCenter = dcPersonnel.DonationCenter.Name,
                        DonationCenterId = dcPersonnel.DonationCenterID,
                        Name = dcPersonnel.Person.FirstName + " " + dcPersonnel.Person.LastName,
                        NationalIdentificationNumber = dcPersonnel.Person.Nin

                    })
                    );

                }
                 );
            });

        }

        /// <summary>
        /// Clears all the fields of the viewmodel.
        /// </summary>
        private void ClearFields()
        {
            Email = string.Empty;
            Active = true;

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
            Active = selectedItem.IsActive;

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


            SelectedDonationCenter = new BasicEntity<string>(selectedItem.DonationCenterId, selectedItem.DonationCenter);

        }

        /// <summary>
        /// fills the model fields from the viewmodel for the dcPersonnel
        /// </summary>
        /// <param name="dCPersonnel">reference to dcPersonnel</param>
        /// <param name="fillOptional">wheter to fill or not optional fields</param>
        private void FillModelDCPersonnel(ref DCPersonnel dCPersonnel, bool fillOptional = false)
        {
            IContainPassword parentPage;
            Model.Gender gender;
            County county;

            parentPage = (ParentPage as IContainPassword);
            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            county = unitOfWork.Counties.Find(c => c.Name == Address.County.Value).Single();

            //initialize them if they're null
            if (dCPersonnel is null)
                dCPersonnel = new DCPersonnel();

            if (dCPersonnel.Person is null)
                dCPersonnel.Person = new Person();

            if (dCPersonnel.Person.Address is null)
                dCPersonnel.Person.Address = new Address();

            if (dCPersonnel.Account is null)
                dCPersonnel.Account = new Account();

            /*if (dCPersonnel.DonationCenter is null)
                dCPersonnel.DonationCenter = new DonationCenter();*/

            dCPersonnel.Active = Active;
            //fill account

            dCPersonnel.Account.Email = Email;
            dCPersonnel.Account.Password = parentPage.SecurePasword.Length == 0 && !fillOptional ? dCPersonnel.Account.Password : security.HashPassword(parentPage.SecurePasword.Unsecure());

            //personal details
            dCPersonnel.Person.FirstName = Person.FirstName;
            dCPersonnel.Person.LastName = Person.LastName;
            dCPersonnel.Person.BirthDate = DateTime.Parse(Person.BirthDate);
            dCPersonnel.Person.Nin = Person.NationalIdentificationNumber;
            dCPersonnel.Person.PhoneNo = Person.PhoneNumber;
            dCPersonnel.Person.Gender = gender;

            //address properties
            dCPersonnel.Person.Address.Street = Address.StreetName;
            dCPersonnel.Person.Address.StreetNo = Address.StreetNumber;
            dCPersonnel.Person.Address.City = Address.City;
            dCPersonnel.Person.Address.County = county;
            dCPersonnel.Person.Address.ZipCode = Address.ZipCode;

            dCPersonnel.DonationCenterID = SelectedDonationCenter.Id;

        }


        private void FillViewModelDCPersonnel(ref DCPItemViewModel dCPersonnel)
        {
            Model.Gender gender;
            County county;

            gender = unitOfWork.Genders.Find(g => g.Type == Person.Gender.Value).Single();
            county = unitOfWork.Counties.Find(c => c.Name == Address.County.Value).Single();

            if (dCPersonnel is null)
                dCPersonnel = new DCPItemViewModel();

            if (dCPersonnel.Person is null)
                dCPersonnel.Person = new PersonViewModel();
            if (dCPersonnel.Address is null)
                dCPersonnel.Address = new AddressViewModel();



            //active
            dCPersonnel.IsActive = Active;

            //active
            dCPersonnel.Email = Email;

            dCPersonnel.Person.FirstName = Person.FirstName;
            dCPersonnel.Person.LastName = Person.LastName;
            dCPersonnel.Person.BirthDate = Person.BirthDate;
            dCPersonnel.Person.NationalIdentificationNumber = Person.NationalIdentificationNumber;
            dCPersonnel.Person.PhoneNumber = Person.PhoneNumber;
            dCPersonnel.Person.Gender = new BasicEntity<string>(gender.GenderID, gender.Type);
            //  Address properties
            dCPersonnel.Address.StreetName = Address.StreetName;
            dCPersonnel.Address.StreetNumber = Address.StreetNumber;
            dCPersonnel.Address.City = Address.City;
            dCPersonnel.Address.County = new BasicEntity<string>(county.CountyID, county.Name);
            dCPersonnel.Address.ZipCode = Address.ZipCode;
            //outer properties already bound
            dCPersonnel.NationalIdentificationNumber = dCPersonnel.Person.NationalIdentificationNumber;
            dCPersonnel.Name = dCPersonnel.Person.FirstName + " " + dCPersonnel.Person.LastName;
            dCPersonnel.DonationCenter = SelectedDonationCenter.Value;
            dCPersonnel.DonationCenterId = SelectedDonationCenter.Id;

        }

        /// <summary>
        /// Add a DCPersonnel.
        /// </summary>
        private async Task AddDCPersonnelAsync()
        {

            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                if (Errors + Person.Errors + Address.Errors > 0)
                {
                    Popup("Some errors were found. Fix them before going forward.");
                    return;
                }
                try
                {
                    DCPersonnel dCPersonnel;
                    DCPItemViewModel dCPersonnelViewModel;

                    dCPersonnel = null;
                    dCPersonnelViewModel = null;

                    //fill, add, persist
                    FillModelDCPersonnel(ref dCPersonnel);
                    unitOfWork.DCPersonnel.Add(dCPersonnel);
                    unitOfWork.Complete();

                    FillViewModelDCPersonnel(ref dCPersonnelViewModel);
                    dCPersonnelViewModel.Id = dCPersonnel.PersonID;

                    dispatcherWrapper.InvokeAsync(() => Items.Add(dCPersonnelViewModel));

                    Popup($"DCPersonnel added successfully!", PopupType.Successful);


                }
                catch (Exception ex)
                {
                    Popup($"An error occured while adding the dcPersonnel.");
                    Console.WriteLine("ex" + ex + "\n" + ex.StackTrace);

                }

            });
        }

        /// <summary>
        /// modifies a dcPersonnel
        /// </summary>
        /// <returns></returns>
        private async Task ModifyDCPersonnelAsync()
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
                    DCPersonnel dCPersonnel;

                    dCPersonnel = unitOfWork.Persons[selectedItem.Id].DCPersonnel;

                    //fill and complete
                    FillModelDCPersonnel(ref dCPersonnel);
                    unitOfWork.Complete();

                    FillViewModelDCPersonnel(ref selectedItem);

                    Popup($"DCPersonnel modified successfully!", PopupType.Successful);
                }
                catch
                {
                    Popup("An error occured while modifying dcPersonnel");
                }

            });
        }
        #endregion
    }

    public class DCPItemViewModel : BaseViewModel
    {
        #region Private Members

        private int id;
        private string name;
        private string nin;
        private string donationCenter;
        private string email;
        private bool isActive;
        private int donationCenterId;

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
        public int DonationCenterId
        {
            get => donationCenterId;
            set
            {
                if (donationCenterId == value)
                    return;

                donationCenterId = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the name.
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
        /// Gets or sets the nin.
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
        /// Gets or sets the donation center.
        /// </summary>
        public string DonationCenter
        {
            get => donationCenter;

            set
            {
                if (donationCenter == value)
                    return;

                donationCenter = value;

                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => email;
            set
            {
                if (email == value)
                    return;

                email = value;

                OnPropertyChanged();
            }
        }
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive == value)
                    return;

                isActive = value;

                OnPropertyChanged();
            }
        }
        public AddressViewModel Address { get; set; }

        public PersonViewModel Person { get; set; }

        #endregion

        #region Constructors
        public DCPItemViewModel() { }
        #endregion
    }
}