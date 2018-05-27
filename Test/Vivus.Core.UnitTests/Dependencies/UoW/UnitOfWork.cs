namespace Vivus.Core.UnitTests.Dependencies.UoW
{
    using Vivus.Core.Model;
    using VivusRepository = Core.Repository;
    using Vivus.Core.UnitTests.Dependencies.Repository;
    using Vivus.Core.UoW;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {

        #region Public Repositories

        /// <summary>
        /// Gets the accounts repository.
        /// </summary>
        public VivusRepository.IRepository<Account> Accounts { get; }

        /// <summary>
        /// Gets the addresses repository.
        /// </summary>
        public VivusRepository.IRepository<Address> Addresses { get; }

        /// <summary>
        /// Gets the administrators repository.
        /// </summary>
        public VivusRepository.IRepository<Administrator> Administrators { get; }

        /// <summary>
        /// Gets the blood containers repository.
        /// </summary>
        public VivusRepository.IRepository<BloodContainer> BloodContainers { get; }

        /// <summary>
        /// Gets the blood container types repository.
        /// </summary>
        public VivusRepository.IRepository<BloodContainerType> BloodContainerTypes { get; }

        /// <summary>
        /// Gets the blood requests repository.
        /// </summary>
        public VivusRepository.IRepository<BloodRequest> BloodRequests { get; }

        /// <summary>
        /// Gets the blood types repository.
        /// </summary>
        public VivusRepository.IRepository<BloodType> BloodTypes { get; }

        /// <summary>
        /// Gets the counties repository.
        /// </summary>
        public VivusRepository.IRepository<County> Counties { get; }

        /// <summary>
        /// Gets the donation centers personnel repository.
        /// </summary>
        public VivusRepository.IRepository<DCPersonnel> DCPersonnel { get; }

        /// <summary>
        /// Gets the doctors repository.
        /// </summary>
        public VivusRepository.IRepository<Doctor> Doctors { get; }

        /// <summary>
        /// Gets the donation centers repository.
        /// </summary>
        public VivusRepository.IRepository<DonationCenter> DonationCenters { get; }

        /// <summary>
        /// Gets the donation forms repository.
        /// </summary>
        public VivusRepository.IRepository<DonationForm> DonationForms { get; }

        /// <summary>
        /// Gets the donors repository.
        /// </summary>
        public VivusRepository.IRepository<Donor> Donors { get; }

        /// <summary>
        /// Gets the genders repository.
        /// </summary>
        public VivusRepository.IRepository<Gender> Genders { get; }

        /// <summary>
        /// Gets the messages repository.
        /// </summary>
        public VivusRepository.IRepository<Message> Messages { get; }

        /// <summary>
        /// Gets the patients repository.
        /// </summary>
        public VivusRepository.IRepository<Patient> Patients { get; }

        /// <summary>
        /// Gets the persons repository.
        /// </summary>
        public VivusRepository.IRepository<Person> Persons { get; }

        /// <summary>
        /// Gets the person statuses repository.
        /// </summary>
        public VivusRepository.IRepository<PersonStatus> PersonStatuses { get; }

        /// <summary>
        /// Gets the request priorities repository.
        /// </summary>
        public VivusRepository.IRepository<RequestPriority> RequestPriorities { get; }

        /// <summary>
        /// Gets the rhs repository.
        /// </summary>
        public VivusRepository.IRepository<RH> RHs { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the given value.
        /// </summary>
        /// <param name="context">The Unit of Work for the Entity Framework instance.</param>
        public UnitOfWork()
        {
            Accounts = new Repository<Account>();
            Addresses = new Repository<Address>();
            Administrators = new Repository<Administrator>();
            BloodContainers = new Repository<BloodContainer>();
            BloodContainerTypes = new Repository<BloodContainerType>();
            BloodRequests = new Repository<BloodRequest>();
            BloodTypes = new Repository<BloodType>();
            Counties = new Repository<County>();
            DCPersonnel = new Repository<DCPersonnel>();
            Doctors = new Repository<Doctor>();
            DonationCenters = new Repository<DonationCenter>();
            DonationForms = new Repository<DonationForm>();
            Donors = new Repository<Donor>();
            Genders = new Repository<Gender>();
            Messages = new Repository<Message>();
            Patients = new Repository<Patient>();
            Persons = new Repository<Person>();
            PersonStatuses = new Repository<PersonStatus>();
            RequestPriorities = new Repository<RequestPriority>();
            RHs = new Repository<RH>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves all the changes made to the persistance level.
        /// </summary>
        /// <returns>The number of entities written to the persistance level.</returns>
        public int Complete()
        {
            return 0;
        }

        /// <summary>
        /// Saves asynchronously all the changes made to the persistance level.
        /// </summary>
        /// <returns>The number of entities written to the persistance level.</returns>
        public async Task<int> CompleteAsync()
        {
            return await Task.Run(() => 0);
        }

        /// <summary>
        /// Calls the protected Dispose method.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }
}
