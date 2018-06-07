namespace Vivus.Core.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.Administration.IoC;
    using Vivus.Core.DataModels;
    using Vivus.Core.Model;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using Vivus = Console;

    /// <summary>
    /// Represents a view model for the donation centers page.
    /// </summary>
    public class DonationCentersViewModel : BaseViewModel
    {
        #region Private Members

        private string donationCenterName;
        private bool status = true;
        private DonationCenterItemViewModel selectedDonationCenter;
        private bool optionalErrors;
        private bool actionIsRunning;
        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<Administrator> appViewModel;
        private ISecurity security;


        #endregion

        #region Public Properties
        
        /// <summary>
        /// Gets or sets the name of donation center
        /// </summary>
        public string DonationCenterName
        {
            get => donationCenterName;

            set
            {
                if (donationCenterName == value)
                    return;

                donationCenterName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the donation center address view model
        /// </summary>
        public AddressViewModel ResidencyAddress { get; }


        /// <summary>
        /// Gets the list of counties
        /// </summary>
        public List<BasicEntity<string>> Counties { get; }


        /// <summary>
        /// Command for adding the new donation center
        /// </summary>
        public ICommand Add { get;  }


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
        /// Command forgetting donation centers
        /// </summary>
        public ObservableCollection<DonationCenterItemViewModel> DonationCenters { get; }

        public DonationCenterItemViewModel SelectedDonationCenter
        {
            get => selectedDonationCenter;

            set
            {
                if (selectedDonationCenter == value)
                    return;
                selectedDonationCenter = value;

                if ( selectedDonationCenter is null)
                {
                    ButtonType = ButtonType.Add;
                    Console.WriteLine("add donation center entered");
                    optionalErrors = false;

                    dispatcherWrapper.InvokeAsync(() => ParentPage.DontAllowErrors());
                    ClearFields();
                } 
                else
                {
                    ButtonType = ButtonType.Modify;
                    Console.WriteLine("Modify donation center entered");
                    optionalErrors = true;

                    dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());
                    PopulateFields();
                }

                Vivus.Console.WriteLine(selectedDonationCenter.DonationCenterNAme + " was selected");
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
                return null;
            }
        }

        public ButtonType ButtonType { get; set; }

        #endregion

        #region Constructors

        public DonationCentersViewModel():base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            ResidencyAddress = new AddressViewModel();
            Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Choose a county") };
            Add = new RelayCommand(async () => await AddModifyAsync());
            DonationCenters = new ObservableCollection<DonationCenterItemViewModel>();
            ButtonType = ButtonType.Add;
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<Administrator>>();
            security = IoCContainer.Get<ISecurity>();
            LoadCountiesAsync();
            LoadDonationCentersAsync();
        }


        public DonationCentersViewModel(IUnitOfWork unitOfWork, IApplicationViewModel<Administrator> appViewModel, IDispatcherWrapper dispatcherWrapper, ISecurity security)
        {
            ResidencyAddress = new AddressViewModel();
            DonationCenters = new ObservableCollection<DonationCenterItemViewModel>();

            // TODO add entity for test

            ButtonType = ButtonType.Add;
            optionalErrors = false;
            this.unitOfWork = unitOfWork;
            this.appViewModel = appViewModel;
            this.dispatcherWrapper = dispatcherWrapper;
            this.security = security;

            Counties = new List<BasicEntity<string>>();

            LoadCountiesAsync();
            LoadDonationCentersAsync();

            Add = new RelayCommand(async () => await AddModifyAsync());
        }

        #endregion

        #region Public methods

        public async Task AddModifyAsync()
        {
            await RunCommand(() => ActionIsRunning, async () =>
            {
                if (ButtonType == ButtonType.Add)
                    await AddDonationCenterAsync();
                else
                    await ModifyDonationCenterAsync();
            });
        }

        #endregion


        #region Private methods

        private async Task AddDonationCenterAsync()
        {
            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                if (Errors + ResidencyAddress.Errors > 0)
                {
                    Popup("Some errors encountered here!");
                    return;
                }

                try
                {
                    DonationCenter donationCenter;
                    DonationCenterItemViewModel donationCenterItemViewModel;

                    donationCenter = null;
                    donationCenterItemViewModel = null;

                   // fill model, add, persist
                    FillModelDonationCenter(ref donationCenter, true);
                    unitOfWork.DonationCenters.Add(donationCenter);
                    unitOfWork.Complete();

                    FillViewModelDonationCenter(ref donationCenterItemViewModel);

                    donationCenterItemViewModel.DonationCenterNAme = donationCenter.Name;

                    dispatcherWrapper.InvokeAsync(() => DonationCenters.Add(donationCenterItemViewModel));
                    Popup($"Donation center added successfully!", PopupType.Successful);

                }
                catch (Exception ex)
                {
                    Popup($"exception: " + ex);
                    Vivus.Console.WriteLine("exception: " + ex + " " + ex.StackTrace);
                }

            });
        }


        private async Task ModifyDonationCenterAsync()
        {
            await Task.Run(() =>
            {
                if (selectedDonationCenter == null)
                    return;

                Console.WriteLine("errors: " + Errors.ToString() + " " + ResidencyAddress.ToString());

                if( Errors + ResidencyAddress.Errors > 0)
                {
                    Popup("Some errors were encountered!");
                    return;
                }

                try
                {
                    DonationCenter donationCenter;
                    donationCenter = unitOfWork.DonationCenters[SelectedDonationCenter.DonationCenterID];

                    FillModelDonationCenter(ref donationCenter);

                    unitOfWork.Complete();

                    FillViewModelDonationCenter(ref selectedDonationCenter);

                    Popup($"Donation center modified successfully!", PopupType.Successful);
                }
                catch
                {
                    Popup($"An error was encountered!");
                }
            });
        }

        private void FillModelDonationCenter(ref DonationCenter donationCenter, bool fillOptional = false)
        {
            County residencyCounty;
            residencyCounty = unitOfWork.Counties.Find(c => c.Name == ResidencyAddress.County.Value).Single();
            if (donationCenter.Name == null)
            {
                donationCenter.Name = "";
            }
            if (donationCenter.Address is null)
            {
                donationCenter.Address = new Address();
            }

            donationCenter.Name = DonationCenterName;
            donationCenter.Address.Street = ResidencyAddress.StreetName;
            donationCenter.Address.StreetNo = ResidencyAddress.StreetNumber;
            donationCenter.Address.City = ResidencyAddress.City;
            donationCenter.Address.County = residencyCounty;
            donationCenter.Address.ZipCode = ResidencyAddress.ZipCode;
        }


        private void FillViewModelDonationCenter(ref DonationCenterItemViewModel donationCenter)
        {
            County residencyCounty;
            residencyCounty = unitOfWork.Counties.Find(c => c.Name == ResidencyAddress.County.Value).Single();
            if (donationCenter is null)
                donationCenter = new DonationCenterItemViewModel();
            if (donationCenter.ResidencyAddressViewModel is null)
                donationCenter.ResidencyAddressViewModel = new AddressViewModel();

            donationCenter.DonationCenterNAme = DonationCenterName;
            donationCenter.ResidencyAddressViewModel.StreetName = ResidencyAddress.StreetName;
            donationCenter.ResidencyAddressViewModel.StreetNumber = ResidencyAddress.StreetNumber;
            donationCenter.ResidencyAddressViewModel.City = ResidencyAddress.City;
            donationCenter.ResidencyAddressViewModel.County = new BasicEntity<string>(residencyCounty.CountyID, residencyCounty.Name);
            donationCenter.ResidencyAddressViewModel.ZipCode = ResidencyAddress.ZipCode;
            donationCenter.ResidencyAddress = ResidencyAddress.County.Value + ","
                + ResidencyAddress.City + ","
                + ResidencyAddress.StreetName + ","
                + ResidencyAddress.StreetNumber.ToString() + ","
                + ResidencyAddress.ZipCode.ToString();
            donationCenter.DonationCenterNAme = donationCenter.DonationCenterNAme;
        }

        private async void LoadDonationCentersAsync()
        {
            await Task.Run(() =>
            unitOfWork.DonationCenters
            .Entities
            .ToList()
            .ForEach(donationCenter =>
                dispatcherWrapper.InvokeAsync(() =>
                {
                    AddressViewModel residency = new AddressViewModel(true);
                    //Console.Console.WriteLine("i got a doctor " + doctor.Person.LastName);
                    Address add = unitOfWork.Addresses
                        .Entities
                        .Where(address => address.AddressID == donationCenter.AddressID)
                        .Single();

                    DonationCenters.Add(new DonationCenterItemViewModel
                    {

                        
                        ResidencyAddressViewModel = new AddressViewModel
                        {
                            County = new BasicEntity<string>(add.CountyID, add.County.Name),
                            City = add.City,
                            StreetName = add.Street,
                            StreetNumber = add.StreetNo,
                            ZipCode = add.ZipCode
                        },
                        ResidencyAddress = add.County.Name + "," + add.City + "," + add.Street + "," + add.StreetNo.ToString(),
                        DonationCenterNAme = donationCenter.Name
                    }
                    );

                    Console.WriteLine(DonationCenters.Count.ToString() + "is the len");
                }
                )
            )
            );

        }


        private void ClearFields()
        {
            DonationCenterName = string.Empty;
            ResidencyAddress.StreetName = string.Empty;
            ResidencyAddress.StreetNumber = string.Empty;
            ResidencyAddress.City = string.Empty;
            ResidencyAddress.County = Counties[0];
            ResidencyAddress.ZipCode = string.Empty;
        }
        
        private void PopulateFields()
        {
            /// populate name
            DonationCenterName = selectedDonationCenter.DonationCenterNAme;

            /// populate residency address
            ResidencyAddress.StreetName = selectedDonationCenter.ResidencyAddressViewModel.StreetName;
            ResidencyAddress.StreetNumber = selectedDonationCenter.ResidencyAddressViewModel.StreetNumber;
            ResidencyAddress.City = selectedDonationCenter.ResidencyAddressViewModel.City;
            ResidencyAddress.County = selectedDonationCenter.ResidencyAddressViewModel.County;
            ResidencyAddress.ZipCode = selectedDonationCenter.ResidencyAddressViewModel.ZipCode;
        }

        private void AddDonationCenter()
        {
            if (Errors + ResidencyAddress.Errors > 0)
            {
                Popup("Some errors here ");
                return;
            }

            Vivus.Console.WriteLine("Adminstration: Add Dodation Center works!");
            Popup("Successful operation!", PopupType);
        }


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

        
        #endregion


        public class DonationCenterItemViewModel: BaseViewModel
        {
            #region Private members
            private int donationCenterID;
            private string donationCenterName;
            private string residencyAddress;
            private AddressViewModel residencyAddressViewModel;
            #endregion

            #region Public Properties

            public int DonationCenterID
            {
                get => donationCenterID;

                set
                {
                    if (donationCenterID == value)
                        return;
                    donationCenterID = value;
                    OnPropertyChanged();
                }
            }

            public string DonationCenterNAme
            {
                get => donationCenterName;

                set
                {
                    if (donationCenterName == value)
                        return;

                    donationCenterName = value;

                    OnPropertyChanged();
                }
            }

            public string ResidencyAddress
            {
                get => residencyAddress;

                set
                {
                    if (residencyAddress == value)
                        return;

                    residencyAddress = value;

                    OnPropertyChanged();
                }
            }

            
            public AddressViewModel ResidencyAddressViewModel
            {
                get => residencyAddressViewModel;

                set
                {
                    if (residencyAddressViewModel == value)
                        return;

                    residencyAddressViewModel = value;
                    OnPropertyChanged();
                }

            }

            #endregion
        }

    }
}
