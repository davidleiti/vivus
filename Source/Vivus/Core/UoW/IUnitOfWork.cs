namespace Vivus.Core.UoW
{
    using System;
    using System.Threading.Tasks;
    using Vivus.Core.Model;
    using Vivus.Core.Repository;

    /// <summary>
    /// Represents an interface for a Unit of Work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Public Repositories

        // Specific to your application. Add interfaces for your repositories.

        /// <summary>
        /// Gets the accounts repository.
        /// </summary>
        IRepository<Account> Accounts { get; }

        /// <summary>
        /// Gets the addresses repository.
        /// </summary>
        IRepository<Address> Addresses { get; }

        /// <summary>
        /// Gets the administrators repository.
        /// </summary>
        IRepository<Administrator> Administrators { get; }

        /// <summary>
        /// Gets the blood containers repository.
        /// </summary>
        IRepository<BloodContainer> BloodContainers { get; }

        /// <summary>
        /// Gets the blood container types repository.
        /// </summary>
        IRepository<BloodContainerType> BloodContainerTypes { get; }

        /// <summary>
        /// Gets the blood requests repository.
        /// </summary>
        IRepository<BloodRequest> BloodRequests { get; }

        /// <summary>
        /// Gets the blood types repository.
        /// </summary>
        IRepository<BloodType> BloodTypes { get; }

        /// <summary>
        /// Gets the counties repository.
        /// </summary>
        IRepository<County> Counties { get; }

        /// <summary>
        /// Gets the donation centers personnel repository.
        /// </summary>
        IRepository<DCPersonnel> DCPersonnel { get; }

        /// <summary>
        /// Gets the doctors repository.
        /// </summary>
        IRepository<Doctor> Doctors { get; }

        /// <summary>
        /// Gets the donation centers repository.
        /// </summary>
        IRepository<DonationCenter> DonationCenters { get; }

        /// <summary>
        /// Gets the donation forms repository.
        /// </summary>
        IRepository<DonationForm> DonationForms { get; }

        /// <summary>
        /// Gets the donors repository.
        /// </summary>
        IRepository<Donor> Donors { get; }

        /// <summary>
        /// Gets the genders repository.
        /// </summary>
        IRepository<Gender> Genders { get; }

        /// <summary>
        /// Gets the messages repository.
        /// </summary>
        IRepository<Message> Messages { get; }

        /// <summary>
        /// Gets the patients repository.
        /// </summary>
        IRepository<Patient> Patients { get; }

        /// <summary>
        /// Gets the persons repository.
        /// </summary>
        IRepository<Person> Persons { get; }

        /// <summary>
        /// Gets the person statuses repository.
        /// </summary>
        IRepository<PersonStatus> PersonStatuses { get; }

        /// <summary>
        /// Gets the request priorities repository.
        /// </summary>
        IRepository<RequestPriority> RequestPriorities { get; }

        /// <summary>
        /// Gets the rhs repository.
        /// </summary>
        IRepository<RH> RHs { get; }

        #endregion

        /// <summary>
        /// Saves all the changes made to the persistance level.
        /// </summary>
        /// <returns>The number of entities written to the persistance level.</returns>
        int Complete();

        /// <summary>
        /// Saves asynchronously all the changes made to the persistance level.
        /// </summary>
        /// <returns>The number of entities written to the persistance level.</returns>
        Task<int> CompleteAsync();
    }
}
