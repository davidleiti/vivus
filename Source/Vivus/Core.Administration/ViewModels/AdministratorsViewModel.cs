namespace Vivus.Core.Administration.ViewModels {
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Windows.Input;
	using Vivus.Core.Administration.Validators;
	using Vivus.Core.DataModels;
	using Vivus = Console;
	using Vivus.Core.ViewModels;

	/// <summary>
	/// Represents a view model for the administrators page.
	/// </summary>
	public class AdministratorsViewModel : BaseViewModel {

		#region Private Members
		private string email;
		private object password;
		private bool isActive = true;
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
		/// Gets the person view model.
		/// </summary>
		public PersonViewModel Person { get; }

		/// <summary>
		/// Gets the address view model.
		/// </summary>
		public AddressViewModel Address { get; }

		/// <summary>
		/// Gets the list of counties.
		/// </summary>
		public List<BasicEntity<string>> Counties { get; }

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
			get {
				if (propertyName == nameof(Email))
					return GetErrorString(propertyName, AdministrationValidator.EmailValidation(Email));
				if (propertyName == nameof(Password) && ParentPage != null)
					return GetErrorString(propertyName, AdministrationValidator.PasswordValidation((ParentPage as IContainPassword).SecurePasword));
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
		/// Initializes a new instance of the <see cref="SignUpViewModel"/> class with the default values.
		/// </summary>
		public AdministratorsViewModel() {
			Person = new PersonViewModel();
			Counties = new List<BasicEntity<string>> { new BasicEntity<string>(-1, "Select a county") };
			Administrators = new ObservableCollection<AdministratorItemViewModel>();
			Address = new AddressViewModel();
			AddCommand = new RelayCommand(Add);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Registers a donor.
		/// </summary>
		private void Add() {
			ParentPage.AllowErrors();

			if (Errors + Person.Errors + Address.Errors > 0) {
				Popup("Some errors were found. Fix them before going forward.");
				return;
			}

			Vivus.Console.WriteLine("Donor: Registration worked!");
			Popup("Successfull operation!", PopupType.Successful);
		}

		#endregion

		#region Private Inner Classes
		public class AdministratorItemViewModel : BaseViewModel {
			#region Private fields
			private int id;
			private string fullName;
			private string email;
			private string phoneNumber;
			private string address;
			private bool isActive;
			#endregion

			public int Id {
				get => id;
				set {
					if (id == value)
						return;

					id = value;

					OnPropertyChanged();
				}
			}

			#region Public Properties
			public string FullName {
				get => fullName;
				set {
					if (fullName == value)
						return;

					fullName = value;

					OnPropertyChanged();
				}
			}
			public string Email {
				get => email;
				set {
					if (email == value)
						return;

					email = value;

					OnPropertyChanged();
				}
			}

			public string PhoneNumber {
				get => phoneNumber;
				set {
					if (phoneNumber == value)
						return;

					phoneNumber = value;

					OnPropertyChanged();
				}
			}

			public string FullAddress {
				get => address;
				set {
					if (address == value)
						return;

					address = value;

					OnPropertyChanged();
				}
			}

			public bool IsActive {
				get => isActive;
				set {
					if (isActive == value)
						return;

					isActive = value;

					OnPropertyChanged();
				}
			}

			#endregion

		}
	}
	#endregion
}
