namespace Vivus.Core.Donor.ViewModels
{
    using Vivus.Core.ViewModels;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using Vivus.Core.DataModels;
    using Vivus.Core.Donor.IoC;
    using Vivus.Core.UoW;
    using Vivus.Core.ViewModels.Base;
    using System.Threading.Tasks;
    using System.Linq;

    /// <summary>
    /// Represents a view model for the history page.
    /// </summary>
    public class HistoryViewModel : BaseViewModel
    {
        #region private fields

        private IUnitOfWork unitOfWork;
        private IApllicationViewModel<Model.Donor> appViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the collection of the donation history.
        /// </summary>
        public ObservableCollection<HistoryItemViewModel> Items { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryViewModel"/> class with the default values.
        /// </summary>
        public HistoryViewModel() : base(new DispatcherWrapper(Application.Current.Dispatcher))
        {
            Items = new ObservableCollection<HistoryItemViewModel>();
            unitOfWork = IoCContainer.Get<IUnitOfWork>();
            appViewModel = IoCContainer.Get<IApllicationViewModel<Model.Donor>>();

            Task.Run(async () =>
            {
                await LoadHistoryAsync();
            });
        }

        #endregion

        /// <summary>
        /// Loads all the History asynchronously.
        /// </summary>
        private async Task LoadHistoryAsync()
        {
            await Task.Run(() =>
            {
                dispatcherWrapper.InvokeAsync(() => Items.Clear());
                unitOfWork.DonationForms
                .Entities
                .ToList()
                .ForEach(history =>
                    dispatcherWrapper.InvokeAsync(() =>
                        Items.Add(new HistoryItemViewModel
                        {
                            Id = history.DonationFormID,
                            Date = history.ApplyDate,
                            Weight = history.Weight,
                            HeartRate = history.HeartRate,
                            SystolicBP = history.SystolicBloodPressure,
                            DiastolicBP = history.DiastolicBloodPressure,
                            Approved = !(history.DonationDate is null)
                        })));

            });
        }
    }

    /// <summary>
    /// Represents an item view model for the history table.
    /// </summary>
    public class HistoryItemViewModel : BaseViewModel
    {
        #region Private Members

        private int id;
        private DateTime date;
        private int weight;
        private int heartRate;
        private int? systolicBP;
        private int? diastolicBP;
        private bool approved;

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
        /// Gets or sets the apply date.
        /// </summary>
        public DateTime Date
        {
            get => date;

            set
            {
                if (date == value)
                    return;

                date = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the weight of the donor.
        /// </summary>
        public int Weight
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
        public int HeartRate
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
        public int? SystolicBP
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
        public int? DiastolicBP
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
        /// Gets or sets the donation status.
        /// </summary>
        public bool Approved
        {
            get => approved;

            set
            {
                if (approved == value)
                    return;

                approved = value;

                OnPropertyChanged();
            }
        }

        #endregion
    }
}
