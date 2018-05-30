namespace Vivus.Core.Donor.ViewModels
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.IoC;
    using Vivus.Core.Donor.Validators;
    using Vivus.Core.Helpers;
    using Vivus.Core.Model;
    using Vivus.Core.Security;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using static Vivus.Core.Helpers.DistanceMatrixApiHelpers;
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a view model for the apply page.
    /// </summary>
    public class ApplyViewModel : BaseViewModel
    {
        #region Private Members

        private int? weight;
        private int? heartRate;
        private int? systolicBP;
        private int? diastolicBP;
        private bool pregnant;
        private bool menstruating;
        private bool postBirth;
        private bool? alcohol;
        private bool? highFatFood;
        private bool? medicalTreatment;
        private string pastSurgeries;
        private string travelStatus;
        private bool applyIsRunning;
        private IApplicationViewModel<Donor> appViewModel;
        private IUnitOfWork unitOfWork;
        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets the parent page of the current <see cref="ApplyViewModel"/>.
        /// </summary>
        public IPage ParentPage { get; set; }

        /// <summary>
        /// Gets or sets the weight of the donor.
        /// </summary>
        public int? Weight
        {
            get => weight;

            set
            {
                if (weight == value)
                    return;

                weight = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the heart rate of the donor.
        /// </summary>
        public int? HeartRate
        {
            get => heartRate;

            set
            {
                if (heartRate == value)
                    return;

                heartRate = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the systolic blood pressure of the donor.
        /// </summary>
        public int? SystolicBloodPressure
        {
            get => systolicBP;

            set
            {
                if (systolicBP == value)
                    return;

                systolicBP = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the diastolic blood pressure of the donor.
        /// </summary>
        public int? DiastolicBloodPressure
        {
            get => diastolicBP;

            set
            {
                if (diastolicBP == value)
                    return;

                diastolicBP = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the pregnant status of the donor.
        /// </summary>
        public bool IsPregnant
        {
            get => pregnant;

            set
            {
                if (pregnant == value)
                    return;

                pregnant = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the menstruating status of the donor.
        /// </summary>
        public bool IsMenstruating
        {
            get => menstruating;

            set
            {
                if (menstruating == value)
                    return;

                menstruating = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether the donor is past birth or not.
        /// </summary>
        public bool IsPostBirth
        {
            get => postBirth;

            set
            {
                if (postBirth == value)
                    return;

                postBirth = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the status of the alcohol consumption of the donor.
        /// </summary>
        public bool? ConsumedAlcohol
        {
            get => alcohol;

            set
            {
                if (alcohol == value)
                    return;

                alcohol = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the status of the high fat food consumption of the donor.
        /// </summary>
        public bool? ConsumedHighFatFood
        {
            get => highFatFood;

            set
            {
                if (highFatFood == value)
                    return;

                highFatFood = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether the donor uses medical treatment or not.
        /// </summary>
        public bool? UnderMedicalTreatment
        {
            get => medicalTreatment;

            set
            {
                if (medicalTreatment == value)
                    return;

                medicalTreatment = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the diseases the donor is suffering of.
        /// </summary>
        public HashSet<string> Diseases { get; }

        /// <summary>
        /// Gets or sets a message that indicates whether the donor had past surgeries or not.
        /// </summary>
        public string HadPastSurgeries
        {
            get => pastSurgeries;

            set
            {
                if (pastSurgeries == value)
                    return;

                pastSurgeries = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a message that indicates whether the donor has been travelling lately or not.
        /// </summary>
        public string HadTravelled
        {
            get => travelStatus;

            set
            {
                if (travelStatus == value)
                    return;

                travelStatus = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the flag that indicates whether the apply command is running or not.
        /// </summary>
        public bool ApplyIsRunning
        {
            get => applyIsRunning;

            set
            {
                if (applyIsRunning == value)
                    return;

                applyIsRunning = value;

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
                if (propertyName == nameof(Weight))
                    return GetErrorString(propertyName, DonorValidator.WeightValidation(Weight));

                if (propertyName == nameof(HeartRate))
                    return GetErrorString(propertyName, DonorValidator.HearRateValidation(HeartRate));

                if (propertyName == nameof(HadPastSurgeries))
                    return GetErrorString(propertyName, DonorValidator.PastSurgeriesValidation(HadPastSurgeries));

                if (propertyName == nameof(HadTravelled))
                    return GetErrorString(propertyName, DonorValidator.TravelStatusValidation(HadTravelled));

                return null;
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the apply command.
        /// </summary>
        public ICommand ApplyCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyViewModel"/> class with the default values.
        /// </summary>
        public ApplyViewModel(): base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Diseases = new HashSet<string>();
            
            ApplyCommand = new RelayCommand(async () => await ApplyAsync());

            appViewModel = IoCContainer.Get<IApplicationViewModel<Model.Donor>>();
            unitOfWork = IoCContainer.Get<IUnitOfWork>();

        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyViewModel"/> class with the given values
        /// </summary>
        /// <param name="unitOfWork">The UoW used to acess repossitories</param>
        /// <param name="appViewModel">The viewmodel for the application</param>
        /// <param name="dispathcerWrapper">The ui thread dispatcher</param>
        public ApplyViewModel(IUnitOfWork unitOfWork, IApplicationViewModel<Donor> appViewModel, IDispatcherWrapper dispathcerWrapper)
        {
            this.unitOfWork = unitOfWork;
            this.appViewModel = appViewModel;
            this.dispatcherWrapper = dispathcerWrapper;

            Diseases = new HashSet<string>();

            ApplyCommand = new RelayCommand(async () => await ApplyAsync());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Apply for a donation.
        /// </summary>
        public async Task ApplyAsync()
        {
            await RunCommand(() => ApplyIsRunning, async () =>
            {
                List<string> popupErrors = new List<string>();
               await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

                if (Errors > 0)
                    popupErrors.Add("Some errors were found. Fix them before going forward.");

                if (!ConsumedAlcohol.HasValue || !ConsumedHighFatFood.HasValue || !UnderMedicalTreatment.HasValue)
                    popupErrors.Add("Not all questions have been answered. Answer all of them before going forward.");

                ParentPage.AllowErrors();

                if (popupErrors.Count > 0)
                {
                    Popup(string.Join(Environment.NewLine, popupErrors));
                    return;
                }

                if (IsPregnant)
                    popupErrors.Add("Pregnant women are not allowed to donate.");

                if (IsMenstruating)
                    popupErrors.Add("Menstruating women are not allowed to donate.");

                if (IsPostBirth)
                    popupErrors.Add("Post birth women are not allowed to donate.");

                if (ConsumedAlcohol.Value)
                    popupErrors.Add("Person that consumed alcohol in the past 48 hours are not allowed to donate.");

                if (ConsumedHighFatFood.Value)
                    popupErrors.Add("Person that consumed foods high in fat in the past 48 hours are not allowed to donate.");

                if (UnderMedicalTreatment.Value)
                    popupErrors.Add("Person that are under medical treatment are not allowed to donate.");

                if (Diseases.Count > 0)
                    popupErrors.Add("Person that suffer from one or many of the mentioned diseases is not allowed to donate.");

                if (popupErrors.Count > 0)
                {
                    Popup(string.Join(Environment.NewLine, popupErrors));
                    return;
                }

                await AddDonationForm();
                await Task.Delay(3000);

                VivusConsole.WriteLine("Apply done!");
                Popup("Successfull operation!", PopupType.Successful);
            });
        }

        /// <summary>
        /// Returns a donation form
        /// </summary>
        /// <param name="fillOptional">Whether to fill the optional fields or not</param>
        private async Task<DonationForm> CreateDonationForm(bool fillOptional = false)
        {
            Donor donor = appViewModel.User;
            DonationForm donationForm = new DonationForm();

            // Fill mandatory fields
            donationForm.DonorID = donor.PersonID;
            donationForm.DCPersonnelID = null;
            donationForm.Weight = weight.GetValueOrDefault();
            donationForm.HeartRate = heartRate.GetValueOrDefault();
            donationForm.DonationDate = null;
            donationForm.DonationStatus = null;
            donationForm.ApplyDate = new DateTime();

            // Set the id of the donation center
            if (donor.DonationCenterID != null)
            {
                donationForm.DonationCenterID = donor.DonationCenterID;
            }
            else
            {
                RouteDetails bestRoute = await getBestRouteToDonationCenters(donor);
                DonationCenter closestDonationCenter = unitOfWork.DonationCenters.Entities.Single(singleDonationCenter => singleDonationCenter.AddressID == bestRoute.DestinationAddress.AddressID);
                donationForm.DonationCenterID = closestDonationCenter.DonationCenterID;
            }

            // Fill optional fields
            if (fillOptional)
            {
                donationForm.SystolicBloodPressure = systolicBP;
                donationForm.DiastolicBloodPressure = diastolicBP;
                donationForm.PastSurgeries = string.IsNullOrEmpty(pastSurgeries) ? null : pastSurgeries;
                donationForm.TravelStatus = string.IsNullOrEmpty(travelStatus) ? null : travelStatus;               
            }

            return donationForm;
        }

        /// <summary>
        /// Gets the route to the closest donation center from the donor 
        /// </summary>
        /// <param name="donor">The donor from whom we compute the route</param>
        /// <returns></returns>
        private async Task<RouteDetails> getBestRouteToDonationCenters(Donor donor)
        {
            List<Address> donationCentersAddresses = new List<Address>();
            Address donorAddress;

            List<DonationCenter> donationCenters;

            donationCenters = (List<DonationCenter>)await unitOfWork.DonationCenters.GetAllAsync();
            
            Address donationCenterAddress;
            foreach (var donationCenter in donationCenters)
            {
                donationCenterAddress = await unitOfWork.Addresses.SingleAsync(singleAddress => singleAddress.AddressID == donationCenter.AddressID);

                donationCentersAddresses.Add(donationCenterAddress);
            }

            if (donor.ResidenceID != null)
            {
                donorAddress = await unitOfWork.Addresses.SingleAsync(singleAddress => singleAddress.AddressID == donor.ResidenceID);
            }
            else
            {
                Person person = await unitOfWork.Persons.SingleAsync(singlePerson => singlePerson.PersonID == donor.PersonID);
                donorAddress = await unitOfWork.Addresses.SingleAsync(singleAddress => singleAddress.AddressID == person.AddressID);
            }
            RouteDetails bestRoute = (await DistanceMatrixApiHelpers.GetDistancesAsync(donorAddress, donationCentersAddresses)).OrderBy(RouteDetails => RouteDetails.Distance).First();
            return bestRoute;
        }

        private async Task AddDonationForm()
        {
            DonationForm donationForm = await CreateDonationForm(true);
            unitOfWork.DonationForms.Add(donationForm);
            // Make changes persistent
            await unitOfWork.CompleteAsync();
        }

        #endregion
    }
}
