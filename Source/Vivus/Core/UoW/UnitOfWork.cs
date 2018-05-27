namespace Vivus.Core.UoW
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Vivus.Core.Model;
    using Vivus.Core.Repository;

    /// <summary>
    /// Represents the implementation of a unit of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Members

        private readonly DbContext context;

        #endregion

        #region Public Repositories

        /// <summary>
        /// Gets the accounts repository.
        /// </summary>
        public IRepository<Account> Accounts { get; }

        /// <summary>
        /// Gets the addresses repository.
        /// </summary>
        public IRepository<Address> Addresses { get; }

        /// <summary>
        /// Gets the administrators repository.
        /// </summary>
        public IRepository<Administrator> Administrators { get; }

        /// <summary>
        /// Gets the blood containers repository.
        /// </summary>
        public IRepository<BloodContainer> BloodContainers { get; }

        /// <summary>
        /// Gets the blood container types repository.
        /// </summary>
        public IRepository<BloodContainerType> BloodContainerTypes { get; }

        /// <summary>
        /// Gets the blood requests repository.
        /// </summary>
        public IRepository<BloodRequest> BloodRequests { get; }

        /// <summary>
        /// Gets the blood types repository.
        /// </summary>
        public IRepository<BloodType> BloodTypes { get; }

        /// <summary>
        /// Gets the counties repository.
        /// </summary>
        public IRepository<County> Counties { get; }

        /// <summary>
        /// Gets the donation centers personnel repository.
        /// </summary>
        public IRepository<DCPersonnel> DCPersonnel { get; }

        /// <summary>
        /// Gets the doctors repository.
        /// </summary>
        public IRepository<Doctor> Doctors { get; }

        /// <summary>
        /// Gets the donation centers repository.
        /// </summary>
        public IRepository<DonationCenter> DonationCenters { get; }

        /// <summary>
        /// Gets the donation forms repository.
        /// </summary>
        public IRepository<DonationForm> DonationForms { get; }

        /// <summary>
        /// Gets the donors repository.
        /// </summary>
        public IRepository<Donor> Donors { get; }

        /// <summary>
        /// Gets the genders repository.
        /// </summary>
        public IRepository<Gender> Genders { get; }

        /// <summary>
        /// Gets the messages repository.
        /// </summary>
        public IRepository<Message> Messages { get; }

        /// <summary>
        /// Gets the patients repository.
        /// </summary>
        public IRepository<Patient> Patients { get; }

        /// <summary>
        /// Gets the persons repository.
        /// </summary>
        public IRepository<Person> Persons { get; }

        /// <summary>
        /// Gets the person statuses repository.
        /// </summary>
        public IRepository<PersonStatus> PersonStatuses { get; }

        /// <summary>
        /// Gets the request priorities repository.
        /// </summary>
        public IRepository<RequestPriority> RequestPriorities { get; }

        /// <summary>
        /// Gets the rhs repository.
        /// </summary>
        public IRepository<RH> RHs { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the given value.
        /// </summary>
        /// <param name="context">The Unit of Work for the Entity Framework instance.</param>
        public UnitOfWork(DbContext context)
        {
            this.context = context;
            this.context.Database.Connection.ConnectionString = Configurations.Configurations.GetConnectionString("dbConfigurations.json", "localConfiguration");
            //this.context.Database.Connection.ConnectionString = Configurations.Configurations.GetConnectionString("dbConfigurations.json", "mainConfiguration");
            this.context.Configuration.LazyLoadingEnabled = true;
            this.context.Configuration.ProxyCreationEnabled = true;

            Accounts = new Repository<Account>(this.context);
            Addresses = new Repository<Address>(this.context);
            Administrators = new Repository<Administrator>(this.context);
            BloodContainers = new Repository<BloodContainer>(this.context);
            BloodContainerTypes = new BloodContainerTypesRepository(this.context);
            BloodRequests = new Repository<BloodRequest>(this.context);
            BloodTypes = new BloodTypesRepository(this.context);
            Counties = new CountiesRepository(this.context);
            DCPersonnel = new Repository<DCPersonnel>(this.context);
            Doctors = new Repository<Doctor>(this.context);
            DonationCenters = new Repository<DonationCenter>(this.context);
            DonationForms = new Repository<DonationForm>(this.context);
            Donors = new Repository<Donor>(this.context);
            Genders = new GendersRepository(this.context);
            Messages = new Repository<Message>(this.context);
            Patients = new Repository<Patient>(this.context);
            Persons = new Repository<Person>(this.context);
            PersonStatuses = new PersonStatusesRepository(this.context);
            RequestPriorities = new RequestPrioritiesRepository(this.context);
            RHs = new RhsRepository(this.context);

            this.context.Database.Connection.Open();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves all the changes made to the persistance level.
        /// </summary>
        /// <returns>The number of entities written to the persistance level.</returns>
        public int Complete()
        {
            return context.SaveChanges();
        }

        /// <summary>
        /// Saves asynchronously all the changes made to the persistance level.
        /// </summary>
        /// <returns>The number of entities written to the persistance level.</returns>
        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Calls the protected Dispose method.
        /// </summary>
        public void Dispose()
        {
            context.Dispose();
        }

        #endregion
    }
}
