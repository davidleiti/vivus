namespace Vivus.Core.DCPersonnel.ViewModels {
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Windows.Input;
	using Vivus.Core.DataModels;
	using Vivus.Core.ViewModels;
	using Vivus.Core.ViewModels.Notifications;
	using Vivus = Console;

	/// <summary>
	/// Represents a view model for the notifications page.
	/// </summary>
	public class NotificationsViewModel : BaseViewModel {

		#region Private fields
		private string nin;
		private BasicEntity<string> personType;
		private BasicEntity<string> personName;
		private string message;
		#endregion

		#region Public Properties

		public IPage ParentPage { get; set; }

		/// <summary>
		/// Gets gets the list of person types (Donor, Doctor, or DCPersonnel)
		/// </summary>
		public List<BasicEntity<string>> PersonTypes { get; }

		/// <summary>
		/// Gets or sets the type of the selected person 
		/// </summary>
		public BasicEntity<string> PersonType {
			get => personType;
			set {
				if (value == personType)
					return;

				personType = value;

				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets gets the list of person names
		/// </summary>
		public ObservableCollection<BasicEntity<string>> Persons { get; }

		/// <summary>
		/// Gets or sets the name of the selected person 
		/// </summary>
		public BasicEntity<string> PersonName {
			get => personName;
			set {
				if (value == personName)
					return;

				personName = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the message of the notification
		/// </summary>
		public string Message {
			get => message;
			set {
				if (value == message)
					return;

				message = value;

				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets or sets the National Identification Number of the person
		/// </summary>
		public string NationalIdentificationNumber {
			get => nin;
			set {
				if (value == nin)
					return;

				nin = value;

				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Gets the list of notifications 
		/// </summary>
		public ObservableCollection<NotificationViewModel> Items { get; }

		/// <summary>
		/// Gets the error string of a property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns></returns>
		public override string this[string propertyName] {
			get {
				if (propertyName == nameof(PersonType)) {
					if (PersonType is null || PersonType.Id < 0)
						return GetErrorString(propertyName, new List<string> { "Person type field is mandatory." });
				}
				if (propertyName == nameof(PersonName)) {
					if (PersonName is null || PersonName.Id < 0)
						return GetErrorString(propertyName, new List<string> { "Person name field is mandatory." });
				}
				if (propertyName == nameof(Message))
					if (string.IsNullOrEmpty(Message))
						return GetErrorString(propertyName, new List<string> { "Message field is mandatory." });
				return null;
			}
		}

		/// <summary>
		/// Gets the send notification command.
		/// </summary>
		public ICommand SendCommand { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NotificationsViewModel"/> class with the default values.
		/// </summary>
		public NotificationsViewModel() {
			PersonTypes = new List<BasicEntity<string>>();
			PersonTypes.Add(new BasicEntity<string>(-1, "Select person type"));
			PersonTypes.Add(new BasicEntity<string>(0, "Donor"));
			PersonTypes.Add(new BasicEntity<string>(1, "Doctor"));
			PersonTypes.Add(new BasicEntity<string>(2, "DC Personnel"));
			Persons = new ObservableCollection<BasicEntity<string>>();
			Persons.Add(new BasicEntity<string>(-1, "Select a person"));
			Items = new ObservableCollection<NotificationViewModel>();
			Application.Current.Dispatcher.Invoke(() => {
				Items.Add(new NotificationViewModel(new ArgbColor(255, 0, 123, 255), "AP", "andreipopescu", new DateTime(2018, 4, 28), "This is dă message."));
			});
			SendCommand = new RelayCommand(Send);
		}

		#endregion

		/// <summary>
		/// Sends a notification
		/// </summary>
		#region Public Methods
		public void Send() {
			ParentPage.AllowErrors();

			if (Errors > 0) {
				Popup("Some errors were found. Fix them before going forward.");
				return;
			}

			Vivus.Console.WriteLine("DCPersonnel: Notification sent successfully!");
			Popup("Successfull operation!", PopupType.Successful);
		}
		#endregion

	}
}
