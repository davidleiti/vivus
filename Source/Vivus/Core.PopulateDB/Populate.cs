namespace Vivus.Core.PopulateDB
{
    using System;
    using System.Linq;
    using Vivus.Core.Model;
    using Vivus.Core.UoW;
    using VivusConsole = Console.Console;
    using BCrypt.Net;
    using Vivus.Core.Repository;

    /// <summary>
    /// Represents a popualtor for the database entities.
    /// </summary>
    static class Populate
    {
        /// <summary>
        /// The <see cref="IUnitOfWork"/> used in populating the database.
        /// </summary>
        private static IUnitOfWork unitOfWork = new UnitOfWork(new VivusEntities());

        /// <summary>
        /// Populates the counties table.
        /// </summary>
        public static void Counties()
        {
            // Delete all the counties
            unitOfWork.Counties.Entities.ToList().ForEach(c => unitOfWork.Counties.Remove(c));

            // Add all the counties
            unitOfWork.Counties.Add(new County { Name = "Alba" });
            unitOfWork.Counties.Add(new County { Name = "Arad" });
            unitOfWork.Counties.Add(new County { Name = "Argeș" });
            unitOfWork.Counties.Add(new County { Name = "Bacău" });
            unitOfWork.Counties.Add(new County { Name = "Bihor" });
            unitOfWork.Counties.Add(new County { Name = "Bistrița-Năsăud" });
            unitOfWork.Counties.Add(new County { Name = "Botoșani" });
            unitOfWork.Counties.Add(new County { Name = "Brașov" });
            unitOfWork.Counties.Add(new County { Name = "Brăila" });
            unitOfWork.Counties.Add(new County { Name = "București" });
            unitOfWork.Counties.Add(new County { Name = "Buzău" });
            unitOfWork.Counties.Add(new County { Name = "Caraș-Severin" });
            unitOfWork.Counties.Add(new County { Name = "Călărași" });
            unitOfWork.Counties.Add(new County { Name = "Cluj" });
            unitOfWork.Counties.Add(new County { Name = "Constanța" });
            unitOfWork.Counties.Add(new County { Name = "Covasna" });
            unitOfWork.Counties.Add(new County { Name = "Dâmbovița" });
            unitOfWork.Counties.Add(new County { Name = "Dolj" });
            unitOfWork.Counties.Add(new County { Name = "Galați" });
            unitOfWork.Counties.Add(new County { Name = "Giurgiu" });
            unitOfWork.Counties.Add(new County { Name = "Gorj" });
            unitOfWork.Counties.Add(new County { Name = "Harghita" });
            unitOfWork.Counties.Add(new County { Name = "Hunedoara" });
            unitOfWork.Counties.Add(new County { Name = "Ialomița" });
            unitOfWork.Counties.Add(new County { Name = "Iași" });
            unitOfWork.Counties.Add(new County { Name = "Ilfov" });
            unitOfWork.Counties.Add(new County { Name = "Maramureș" });
            unitOfWork.Counties.Add(new County { Name = "Mehedinți" });
            unitOfWork.Counties.Add(new County { Name = "Mureș" });
            unitOfWork.Counties.Add(new County { Name = "Neamț" });
            unitOfWork.Counties.Add(new County { Name = "Olt" });
            unitOfWork.Counties.Add(new County { Name = "Prahova" });
            unitOfWork.Counties.Add(new County { Name = "Satu Mare" });
            unitOfWork.Counties.Add(new County { Name = "Sălaj" });
            unitOfWork.Counties.Add(new County { Name = "Sibiu" });
            unitOfWork.Counties.Add(new County { Name = "Suceava" });
            unitOfWork.Counties.Add(new County { Name = "Teleorman" });
            unitOfWork.Counties.Add(new County { Name = "Timiș" });
            unitOfWork.Counties.Add(new County { Name = "Tulcea" });
            unitOfWork.Counties.Add(new County { Name = "Vaslui" });
            unitOfWork.Counties.Add(new County { Name = "Vâlcea" });
            unitOfWork.Counties.Add(new County { Name = "Vrancea" });

            VivusConsole.WriteLine($"Counties: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the genders table.
        /// </summary>
        public static void Genders()
        {
            // Delete all the genders
            unitOfWork.Genders.Entities.ToList().ForEach(g => unitOfWork.Genders.Remove(g));

            // Add all the genders
            unitOfWork.Genders.Add(new Gender { Type = "Male" });
            unitOfWork.Genders.Add(new Gender { Type = "Female" });
            unitOfWork.Genders.Add(new Gender { Type = "Not specified" });

            VivusConsole.WriteLine($"Genders: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the blood container types table.
        /// </summary>
        public static void BloodContainerTypes()
        {
            // Delete all the blood container types
            unitOfWork.BloodContainerTypes.Entities.ToList().ForEach(t => unitOfWork.BloodContainerTypes.Remove(t));

            // Add all the blood container types
            unitOfWork.BloodContainerTypes.Add(new BloodContainerType { Type = "Thrombocytes" });
            unitOfWork.BloodContainerTypes.Add(new BloodContainerType { Type = "Red cells" });
            unitOfWork.BloodContainerTypes.Add(new BloodContainerType { Type = "Plasma" });
            unitOfWork.BloodContainerTypes.Add(new BloodContainerType { Type = "Blood" });

            VivusConsole.WriteLine($"Blood container types: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the blood types table.
        /// </summary>
        public static void BloodTypes()
        {
            // Delete all the blood types
            unitOfWork.BloodTypes.Entities.ToList().ForEach(t => unitOfWork.BloodTypes.Remove(t));

            // Add all the blood types
            unitOfWork.BloodTypes.Add(new BloodType { Type = "A" });
            unitOfWork.BloodTypes.Add(new BloodType { Type = "B" });
            unitOfWork.BloodTypes.Add(new BloodType { Type = "AB" });
            unitOfWork.BloodTypes.Add(new BloodType { Type = "O" });

            VivusConsole.WriteLine($"Blood types: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the RHs table.
        /// </summary>
        public static void RHs()
        {
            // Delete all the RHs
            unitOfWork.RHs.Entities.ToList().ForEach(rh => unitOfWork.RHs.Remove(rh));

            // Add all the RHs
            unitOfWork.RHs.Add(new RH { Type = "Positive" });
            unitOfWork.RHs.Add(new RH { Type = "Negative" });

            VivusConsole.WriteLine($"RHs: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the person statuses table.
        /// </summary>
        public static void PersonStatuses()
        {
            // Delete all the person statuses
            unitOfWork.PersonStatuses.Entities.ToList().ForEach(s => unitOfWork.PersonStatuses.Remove(s));

            // Add all the person statuses
            unitOfWork.PersonStatuses.Add(new PersonStatus { Type = "Alive" });
            unitOfWork.PersonStatuses.Add(new PersonStatus { Type = "Dead" });

            VivusConsole.WriteLine($"Person statuses: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the request priorities table.
        /// </summary>
        public static void RequestPriorities()
        {
            // Delete all the request priorities
            unitOfWork.RequestPriorities.Entities.ToList().ForEach(p => unitOfWork.RequestPriorities.Remove(p));

            // Add all the request priorities
            unitOfWork.RequestPriorities.Add(new RequestPriority { Type = "Low" });
            unitOfWork.RequestPriorities.Add(new RequestPriority { Type = "Medium" });
            unitOfWork.RequestPriorities.Add(new RequestPriority { Type = "High" });

            VivusConsole.WriteLine($"Request priorities: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the administrators table.
        /// </summary>
        public static void Administrators()
        {
            // Delete all the administrators
            unitOfWork.Administrators.Entities.ToList().ForEach(admin =>
                {
                    unitOfWork.Accounts.Remove(admin.Account);
                    unitOfWork.Addresses.Remove(admin.Person.Address);
                    unitOfWork.Persons.Remove(admin.Person);
                    unitOfWork.Administrators.Remove(admin);
                });

            // Add all the administrators
            unitOfWork.Administrators.Add(new Administrator
            {
                Person = new Person
                {
                    FirstName = "Mihai",
                    LastName = "Nitu",
                    BirthDate = new DateTime(1979, 7, 20),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Male"),
                    Nin = "1790720425218",
                    PhoneNo = "+(40) 727 109 531",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("București"),
                        City = "București",
                        Street = "Remus",
                        StreetNo = "7",
                        ZipCode = "030167"
                    },
                },
                Account = new Account
                {
                    Email = "nitu.mihai@gmail.com",
                    Password = BCrypt.HashPassword("nitu1234")
                },
                IsOwner = true,
                Active = true
            });

            VivusConsole.WriteLine($"Admnistrators: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the doctors table.
        /// </summary>
        public static void Doctors()
        {
            // Delete all the administrators
            unitOfWork.Doctors.Entities.ToList().ForEach(doctor =>
            {
                unitOfWork.Accounts.Remove(doctor.Account);
                unitOfWork.Addresses.Remove(doctor.Person.Address);
                unitOfWork.Addresses.Remove(doctor.Address);
                unitOfWork.Persons.Remove(doctor.Person);
                unitOfWork.Doctors.Remove(doctor);
            });

            // Add all the administrators
            unitOfWork.Doctors.Add(new Doctor
            {
                Person = new Person
                {
                    FirstName = "Camelia",
                    LastName = "Berea",
                    BirthDate = new DateTime(1975, 2, 7),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Female"),
                    Nin = "2750207421129",
                    PhoneNo = "+(40) 767 139 215",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("București"),
                        City = "București",
                        Street = "Căminului",
                        StreetNo = "26",
                        ZipCode = "139423"
                    },
                },
                Address = new Address
                {
                    County = (unitOfWork.Counties as CountiesRepository).County("București"),
                    City = "București",
                    Street = "Șoseaua Iancului",
                    StreetNo = "10",
                    ZipCode = "136920"
                },
                Account = new Account
                {
                    Email = "berea.camelia@yahoo.com",
                    Password = BCrypt.HashPassword("berea123")
                },
                Active = true
            });

            VivusConsole.WriteLine($"Doctors: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the donors table.
        /// </summary>
        public static void Donors()
        {
            // Delete all the administrators
            unitOfWork.Donors.Entities.ToList().ForEach(donor =>
            {
                unitOfWork.Accounts.Remove(donor.Account);
                unitOfWork.Addresses.Remove(donor.Person.Address);
                unitOfWork.Addresses.Remove(donor.Address);
                unitOfWork.Persons.Remove(donor.Person);
                unitOfWork.Donors.Remove(donor);
            });

            // Add all the administrators
            unitOfWork.Donors.Add(new Donor
            {
                Person = new Person
                {
                    FirstName = "Adina",
                    LastName = "Huțanu",
                    BirthDate = new DateTime(1995, 9, 10),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Female"),
                    Nin = "2950910121090",
                    PhoneNo = "+(40) 766 218 996",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Cluj"),
                        City = "Cluj-Napoca",
                        Street = "Colibița",
                        StreetNo = "10",
                        ZipCode = "4000000"
                    },
                },
                Account = new Account
                {
                    Email = "hutanu.adina@hotmail.com",
                    Password = BCrypt.HashPassword("hutanu12")
                }
            });

            VivusConsole.WriteLine($"Doctors: { unitOfWork.Complete() }");
        }
    }
}
