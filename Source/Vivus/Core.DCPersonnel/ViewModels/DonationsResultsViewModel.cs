﻿namespace Vivus.Core.DCPersonnel.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Vivus.Core.DataModels;
    using Vivus.Core.DCPersonnel.Validators;
    using Vivus.Core.UoW;
    using Vivus.Core.Model;
    using Vivus.Core.ViewModels;
    using Vivus.Core.ViewModels.Base;
    using VivusConsole = Core.Console.Console;
    using System.Linq;
    using Vivus.Core.DCPersonnel.IoC;
    using System.Windows.Data;
    using Vivus.Core.Helpers;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a view model for the donation results page.
    /// </summary>
    public class DonationsResultsViewModel : BaseViewModel
    {
        #region Private Members

        private string donor;
        private string nin;
        private string donationDate;
        private string donationResults;
        private string filter;
        private bool actionIsRunning;
        private BasicEntity<string> selectedBloodType;
        private BasicEntity<string> selectedRH;

        DonationFormItemViewModel selectedDonationForm;

        private object formsLock;

        private IUnitOfWork unitOfWork;
        private IApplicationViewModel<DCPersonnel> appViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the full name of the donor.
        /// </summary>
        public string Donor {
            get => donor;

            set {
                if (donor == value)
                    return;

                donor = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the national identification number of the donor.
        /// </summary>
        public string NationalIdentificationNumber {
            get => nin;

            set {
                if (nin == value)
                    return;

                nin = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the donation date of the donor.
        /// </summary>
        public string DonationDate {
            get => donationDate;

            set {
                if (donationDate == value)
                    return;

                donationDate = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the results of the donation.
        /// </summary>
        public string DonationResults {
            get => donationResults;

            set {
                if (donationResults == value)
                    return;

                donationResults = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the filter value for the donation forms.
        /// </summary>
        public string Filter {
            get => filter;

            set {
                if (filter == value)
                    return;

                filter = value;

                if (!String.IsNullOrEmpty(filter))
                {
                    dispatcherWrapper.InvokeAsync(async () => await FilterForms());
                }
                else
                {
                    dispatcherWrapper.InvokeAsync(async () => await LoadRequestsAsync());
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected donation form.
        /// </summary>
        public DonationFormItemViewModel SelectedDonationForm {
            get => selectedDonationForm;

            set {
                if (selectedDonationForm == value)
                    return;

                selectedDonationForm = value;

                if (selectedDonationForm is null)
                {
                    dispatcherWrapper.InvokeAsync(() => ParentPage.DontAllowErrors());
                    ClearFields();
                }
                else
                {
                    dispatcherWrapper.InvokeAsync(() => ParentPage.AllowOptionalErrors());
                    PopulateFields();
                }

                OnPropertyChanged();
            }
        }

        private bool ActionIsRunning {
            get => actionIsRunning;
            set {
                if (actionIsRunning == value)
                    return;

                actionIsRunning = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected blood type.
        /// </summary>
        public BasicEntity<string> SelectedBloodType
        {
            get => selectedBloodType;

            set
            {
                if (selectedBloodType == value)
                    return;

                selectedBloodType = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected rh.
        /// </summary>
        public BasicEntity<string> SelectedRH
        {
            get => selectedRH;

            set
            {
                if (selectedRH == value)
                    return;

                selectedRH = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the collection of all the blood types.
        /// </summary>
        public ObservableCollection<BasicEntity<string>> BloodTypes { get; }

        /// <summary>
        /// Gets the collection of all the rhs.
        /// </summary>
        public ObservableCollection<BasicEntity<string>> RHs { get; }

        /// <summary>
        /// Gets the collection of the donation forms.
        /// </summary>
        public ObservableCollection<DonationFormItemViewModel> DonationForms { get; }

        /// <summary>
        /// Gets the error string of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns></returns>
        public override string this[string propertyName] {
            get
            {
                if (propertyName == nameof(SelectedBloodType))
                    return GetErrorString(propertyName, ContainerInfoValidator.BloodTypeValidation(SelectedBloodType));

                if (propertyName == nameof(SelectedRH))
                    return GetErrorString(propertyName, ContainerInfoValidator.RHValidation(SelectedRH));

                if (propertyName == nameof(DonationDate))
                    return GetErrorString(propertyName, DCPersonnelValidator.DonationDateValidation(DonationDate));

                if (propertyName == nameof(DonationResults))
                    return GetErrorString(propertyName, DCPersonnelValidator.DonationResultsValidation(DonationResults));

                return null;
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the send command of the donation results.
        /// </summary>
        public ICommand SendCommand { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new intance of the <see cref="DonationsResultsViewModel"/> class with the default values.
        /// </summary>
        public DonationsResultsViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            BloodTypes = new ObservableCollection<BasicEntity<string>> { new BasicEntity<string>(-1, "Select blood type") };
            RHs = new ObservableCollection<BasicEntity<string>> { new BasicEntity<string>(-1, "Select rh") };
            DonationForms = new ObservableCollection<DonationFormItemViewModel>();

            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApplicationViewModel<DCPersonnel>>();

            formsLock = new object();
            DonationForms = new ObservableCollection<DonationFormItemViewModel>();
            BindingOperations.EnableCollectionSynchronization(DonationForms, formsLock);
            SendCommand = new RelayCommand(async () => await SendResults());

            Task.Run(async () =>
            {
                await LoadBloodTypesAsync();
                await LoadRHTypesAsync();
            });
        }

        #endregion

        #region Public Methods
        public async Task SendResults()
        {
            await RunCommand(() => ActionIsRunning, async () => {
                await SendResultsAsync();
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the DonationDate of the currently selected DonationForm and sends a message to the Donor with the results
        /// </summary>
        /// <returns></returns>
        private async Task SendResultsAsync()
        {
            Donor donor;
            string messageContent;

            if (selectedDonationForm is null)
            {
                Popup("No donation request has been selected. Select one from the list and try again.");
                return;
            }

            await dispatcherWrapper.InvokeAsync(() => ParentPage.AllowErrors());

            if (Errors > 0)
            {
                Popup("Some errors were found. Fix them before going forward.");
                return;
            }

            try
            {
                DonationForm donationForm = unitOfWork.DonationForms
                .Entities
                .First(form => form.DonorID == selectedDonationForm.PersonId);
                donationForm.DonationDate = DateTime.Parse(DonationDate);

                donor = await unitOfWork.Donors.SingleAsync(d => d.PersonID == donationForm.DonorID);
                donor.RhID = SelectedRH.Id;
                donor.BloodTypeID = selectedBloodType.Id;

                messageContent = "Below are the results of your latest donation:\n" + DonationResults;
                SendMessage(donationForm.DonorID, messageContent);

                LoadRequestsAsync();
                ClearFields();
                SelectedDonationForm = null;

                Popup("Successfull operation!", PopupType.Successful);
            }
            catch
            {
                Popup("Something went wrong when handling the request");
            }
        }

        /// <summary>
        /// Sends a message to the person with the given ID
        /// </summary>
        /// <param name="receiverID">ID of the receiver Person</param>
        /// <param name="messageContent">Text of the message</param>
        private async Task SendMessage(int receiverID, string messageContent)
        {
            await Task.Run(() =>
            {
                Message message = new Message
                {
                    RecieverID = receiverID,
                    SenderID = appViewModel.User.PersonID,
                    SendDate = DateTime.Now,
                    Content = messageContent
                };

                unitOfWork.Persons
                    .Entities
                    .First(person => person.PersonID == receiverID)
                    .ReceivedMessages
                    .Add(message);

                unitOfWork.Complete();
            });
        }

        /// <summary>
        /// Loads all <see cref="DonationForm"/> items which don't yet have a DonationDate specified
        /// </summary>
        private async Task LoadRequestsAsync()
        {
            await Task.Run(() =>
            {
                lock (formsLock)
                {
                    dispatcherWrapper.InvokeAsync(() => DonationForms.Clear());
                    unitOfWork.DonationForms
                        .Entities
                        .Where(form => form.DonationStatus == true && form.DonationDate is null)
                        .ToList()
                        .ForEach(form =>
                        {
                            dispatcherWrapper.InvokeAsync(() => DonationForms.Add(FormToViewModel(form)));
                        });
                }
            });
        }

        /// <summary>
        /// Loads only the <see cref="DonationForm"/> items which match the specified filtering criteria
        /// </summary>
        private async Task FilterForms()
        {
            await Task.Run(() =>
            {
                string[] patterns = Filter.Trim().Split(' ');
                List<DonationFormItemViewModel> allForms = unitOfWork.DonationForms.Entities.ToList()
                                   .Where(form => form.DonationStatus == true && form.DonationDate is null)
                                   .Select(form => FormToViewModel(form))
                                   .Where(form => IsValidForm(form, patterns))
                                   .ToList();
                lock (formsLock)
                {
                    DonationForms.Clear();
                    foreach (var form in allForms)
                    {
                        DonationForms.Add(form);
                    }
                }
            });
        }

        /// <summary>
        /// Returns a <see cref="DonationFormItemViewModel"/> representation of the given <see cref="DonationForm"/> item
        /// </summary>
        /// <param name="form">The form which needs to be converted</param>
        /// <returns></returns>
        private DonationFormItemViewModel FormToViewModel(DonationForm form)
        {
            return new DonationFormItemViewModel
            {
                PersonId = form.DonorID,
                Donor = $"{form.Donor.Person.FirstName} {form.Donor.Person.LastName}",
                ApplyDate = form.ApplyDate,
                NationalIdentificationNumber = form.Donor.Person.Nin,
                BloodType = form.Donor.BloodType.Type + (form.Donor.RH.Type == "Positive" ? "+" : "-")
            };
        }

        /// <summary>
        /// Checks whether the given donation form matches the given patterns or not
        /// </summary>
        /// <param name="form">The form which we are checking</param>
        /// <param name="patterns">Array of patterns</param>
        /// <returns></returns>
        private bool IsValidForm(DonationFormItemViewModel form, string[] patterns)
        {
            return form.Donor.Contains(patterns) || form.NationalIdentificationNumber.ContainsFromBegining(patterns);
        }

        /// <summary>
        /// Populate the fields with the information of the selected donation form
        /// </summary>
        private void PopulateFields()
        {
            Donor = SelectedDonationForm.Donor;
            DonationDate = DateTime.Today.ToShortDateString();
            NationalIdentificationNumber = SelectedDonationForm.NationalIdentificationNumber;
        }

        /// <summary>
        /// Clears the fields of the form
        /// </summary>
        private void ClearFields()
        {
            Donor = string.Empty;
            DonationDate = string.Empty;
            NationalIdentificationNumber = string.Empty;
            DonationResults = string.Empty;
        }

        /// <summary>
        /// Loads asynchronously all the blood types.
        /// </summary>
        /// <returns></returns>
        private async Task LoadBloodTypesAsync()
        {
            await unitOfWork.BloodTypes.GetAllAsync().ContinueWith(async bloodTypes =>
            {
                await dispatcherWrapper.InvokeAsync(() =>
                {
                    BloodTypes.Clear();
                    BloodTypes.Add(new BasicEntity<string>(-1, "Select blood type"));
                    SelectedBloodType = BloodTypes[0];
                });

                foreach (BloodType bt in bloodTypes.Result)
                    await dispatcherWrapper.InvokeAsync(() => BloodTypes.Add(new BasicEntity<string>(bt.BloodTypeID, bt.Type)));
            });
        }

        /// <summary>
        /// Loads asynchronously all the types of RH.
        /// </summary>
        /// <returns></returns>
        private async Task LoadRHTypesAsync()
        {
            await unitOfWork.RHs.GetAllAsync().ContinueWith(async rhs =>
            {
                await dispatcherWrapper.InvokeAsync(() =>
                {
                    RHs.Clear();
                    RHs.Add(new BasicEntity<string>(-1, "Select rh"));
                    SelectedRH = RHs[0];
                });

                foreach (RH rh in rhs.Result)
                    await dispatcherWrapper.InvokeAsync(() => RHs.Add(new BasicEntity<string>(rh.RhID, rh.Type)));
            });
        }

        #endregion
    }

    /// <summary>
    /// Represents an item view model for the donation forms table.
    /// </summary>
    public class DonationFormItemViewModel : BaseViewModel
    {
        #region Private Members

        private int personId;
        private DateTime applyDate;
        private string donor;
        private string nin;
        private string bloodType;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identificatior of the person.
        /// </summary>
        public int PersonId {
            get => personId;

            set {
                if (personId == value)
                    return;

                personId = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the apply date of the donor.
        /// </summary>
        public DateTime ApplyDate {
            get => applyDate;

            set {
                if (applyDate == value)
                    return;

                applyDate = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the full name of the donor.
        /// </summary>
        public string Donor {
            get => donor;

            set {
                if (donor == value)
                    return;

                donor = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the national identification number of the donor.
        /// </summary>
        public string NationalIdentificationNumber {
            get => nin;

            set {
                if (nin == value)
                    return;

                nin = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the blood type and the rh of the donor.
        /// </summary>
        public string BloodType {
            get => bloodType;

            set {
                if (bloodType == value)
                    return;

                bloodType = value;

                OnPropertyChanged();
            }
        }

        #endregion
    }
}
