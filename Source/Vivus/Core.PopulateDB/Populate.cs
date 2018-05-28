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
                    Gender = unitOfWork.Genders.Entities.First(g => g.Type == "Male"),
                    Nin = "1790720425218",
                    PhoneNo = "+(40) 727 109 531",
                    Address = new Address
                    {
                        County = unitOfWork.Counties.Entities.First(c => c.Name == "București"),
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
        /// Populates the dontaion centers table.
        /// </summary>
        public static void DonationCenters()
        {
            // Delete all the donation centers
            unitOfWork.DonationCenters.Entities.ToList().ForEach(dc =>
            {
                unitOfWork.Addresses.Remove(dc.Address);
                unitOfWork.DonationCenters.Remove(dc);
            });

            // Add all the donation centers
            unitOfWork.DonationCenters.Add(new DonationCenter
            {
                Name = "Spitalul Clinic Județean de Urgență Cluj-Napoca",
                Address = new Address
                {
                    County = unitOfWork.Counties.Entities.First(c => c.Name == "Cluj"),
                    City = "Cluj-Napoca",
                    Street = "Clinicilor",
                    StreetNo = "3-5",
                    ZipCode = "400000"
                }
            });
            unitOfWork.DonationCenters.Add(new DonationCenter
            {
                Name = "Spitalul Universitar de Urgență București",
                Address = new Address
                {
                    County = unitOfWork.Counties.Entities.First(c => c.Name == "București"),
                    City = "București",
                    Street = "Splaiul Independenței",
                    StreetNo = "169",
                    ZipCode = "050098"
                }
            });
            unitOfWork.DonationCenters.Add(new DonationCenter
            {
                Name = "Spitalul Județean de Urgență Alba Iulia",
                Address = new Address
                {
                    County = unitOfWork.Counties.Entities.First(c => c.Name == "Alba"),
                    City = "Alba Iulia",
                    Street = "Revoluției 1989",
                    StreetNo = "23",
                    ZipCode = "510007"
                }
            });
            unitOfWork.DonationCenters.Add(new DonationCenter
            {
                Name = "Spitalul Județean de Urgență Brașov",
                Address = new Address
                {
                    County = unitOfWork.Counties.Entities.First(c => c.Name == "Brașov"),
                    City = "Brașov",
                    Street = "Calea București",
                    StreetNo = "25",
                    ZipCode = "500326"
                }
            });
            unitOfWork.DonationCenters.Add(new DonationCenter
            {
                Name = "Spitalul Clinic Județean de Urgență Sibiu",
                Address = new Address
                {
                    County = unitOfWork.Counties.Entities.First(c => c.Name == "Sibiu"),
                    City = "Sibiu",
                    Street = "Corneliu Coposu",
                    StreetNo = "2-4",
                    ZipCode = "550245"
                }
            });
            unitOfWork.DonationCenters.Add(new DonationCenter
            {
                Name = "Spitalul Județean de Urgență Târgu Jiu",
                Address = new Address
                {
                    County = unitOfWork.Counties.Entities.First(c => c.Name == "Gorj"),
                    City = "Târgu Jiu",
                    Street = "Tudor Vladimirescu",
                    StreetNo = "17",
                    ZipCode = "210132"
                }
            });
            unitOfWork.DonationCenters.Add(new DonationCenter
            {
                Name = "Spitalul Clinic Județean de Urgență Târgu Mureș",
                Address = new Address
                {
                    County = unitOfWork.Counties.Entities.First(c => c.Name == "Mureș"),
                    City = "Târgu Mureș",
                    Street = "Gheorghe Marinescu",
                    StreetNo = "50",
                    ZipCode = "540136"
                }
            });

            VivusConsole.WriteLine($"Donation centers: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the donation centers personnel table.
        /// </summary>
        public static void DonationCentersPersonnel()
        {
            // Delete all the donation centers personnel
            unitOfWork.DCPersonnel.Entities.ToList().ForEach(personnel =>
            {
                unitOfWork.Accounts.Remove(personnel.Account);
                unitOfWork.Addresses.Remove(personnel.Person.Address);
                unitOfWork.Persons.Remove(personnel.Person);
                unitOfWork.DCPersonnel.Remove(personnel);
            });

            // Add all the donation centers personnel
            unitOfWork.DCPersonnel.Add(new DCPersonnel
            {
                Person = new Person
                {
                    FirstName = "Daniel",
                    LastName = "Moldovan",
                    BirthDate = new DateTime(1980, 11, 4),
                    Gender = unitOfWork.Genders.Entities.First(g => g.Type == "Male"),
                    Nin = "1801104123318",
                    PhoneNo = "+(40) 722 129 315",
                    Address = new Address
                    {
                        County = unitOfWork.Counties.Entities.First(c => c.Name == "Cluj"),
                        City = "Cluj-Napoca",
                        Street = "Slatina",
                        StreetNo = "2",
                        ZipCode = "400000"
                    },
                },
                Account = new Account
                {
                    Email = "moldovan.dani@yahoo.com",
                    Password = BCrypt.HashPassword("moldovan")
                },
                Active = true
            });

            VivusConsole.WriteLine($"Donation Centers Personnel: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the doctors table.
        /// </summary>
        public static void Doctors()
        {
            // Delete all the doctors
            unitOfWork.Doctors.Entities.ToList().ForEach(doctor =>
            {
                unitOfWork.Accounts.Remove(doctor.Account);
                unitOfWork.Addresses.Remove(doctor.Person.Address);
                unitOfWork.Addresses.Remove(doctor.Address);
                unitOfWork.Persons.Remove(doctor.Person);
                unitOfWork.Doctors.Remove(doctor);
            });

            // Add all the doctors
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
            // Delete all the donors
            unitOfWork.Donors.Entities.ToList().ForEach(donor =>
            {
                unitOfWork.Accounts.Remove(donor.Account);
                unitOfWork.Addresses.Remove(donor.Person.Address);
                unitOfWork.Addresses.Remove(donor.ResidenceAddress);
                unitOfWork.Persons.Remove(donor.Person);
                unitOfWork.Donors.Remove(donor);
            });

            // Add all the donors
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
            unitOfWork.Donors.Add(new Donor
            {
                Person = new Person
                {
                    FirstName = "Valeria",
                    LastName = "Constantinescu",
                    BirthDate = new DateTime(2006, 7, 3),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Female"),
                    Nin = "6060703322149",
                    PhoneNo = "+(40) 764 551 811",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Sibiu"),
                        City = "Cisnădie",
                        Street = "Vișinilor",
                        StreetNo = "18",
                        ZipCode = "555300"
                    },
                },
                Account = new Account
                {
                    Email = "valeriac@yahoo.com",
                    Password = BCrypt.HashPassword("constantinescu")
                },
                DonationCenter = unitOfWork.DonationCenters.Entities.Single(dc => dc.Name == "Spitalul Clinic Județean de Urgență Cluj-Napoca"),
                BloodType = unitOfWork.BloodTypes.Entities.Single(bt => bt.Type == "B"),
                RH = unitOfWork.RHs.Entities.Single(rh => rh.Type == "Positive"),
            });
            unitOfWork.Donors.Add(new Donor
            {
                Person = new Person
                {
                    FirstName = "Laurențiu",
                    LastName = "Funar",
                    BirthDate = new DateTime(1993, 3, 20),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Not specified"),
                    Nin = "1930320086840",
                    PhoneNo = "+(40) 796 569 360",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Brașov"),
                        City = "Făgăraș",
                        Street = "Teiului",
                        StreetNo = "21"
                    }
                },
                Account = new Account
                {
                    Email = "laur.funar@gmail.com",
                    Password = BCrypt.HashPassword("funar123")
                },
                DonationCenter = unitOfWork.DonationCenters.Entities.Single(dc => dc.Name == "Spitalul Județean de Urgență Brașov"),
                BloodType = unitOfWork.BloodTypes.Entities.Single(bt => bt.Type == "AB"),
                RH = unitOfWork.RHs.Entities.Single(rh => rh.Type == "Negative"),
            });
            unitOfWork.Donors.Add(new Donor
            {
                Person = new Person
                {
                    FirstName = "Zsolt",
                    LastName = "Meggyesfalvi",
                    BirthDate = new DateTime(1995, 2, 5),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Not specified"),
                    Nin = "1950205265074",
                    PhoneNo = "+(40) 759 397 104",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Mureș"),
                        City = "Târgu Mureș",
                        Street = "Grădinarilor",
                        StreetNo = "11B",
                        ZipCode = "540013"
                    }
                },
                Account = new Account
                {
                    Email = "meggy.zsolt@hotmail.com",
                    Password = BCrypt.HashPassword("meggyesfalvi")
                },
                DonationCenter = unitOfWork.DonationCenters.Entities.Single(dc => dc.Name == "Spitalul Clinic Județean de Urgență Cluj-Napoca"),
                BloodType = unitOfWork.BloodTypes.Entities.Single(bt => bt.Type == "A"),
                RH = unitOfWork.RHs.Entities.Single(rh => rh.Type == "Negative"),
            });
            unitOfWork.Donors.Add(new Donor
            {
                Person = new Person
                {
                    FirstName = "Cezar",
                    LastName = "Ionesco",
                    BirthDate = new DateTime(1984, 10, 10),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Male"),
                    Nin = "1841010227210",
                    PhoneNo = "+(40) 741 366 096",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Iași"),
                        City = "Pașcani",
                        Street = "Cosminului",
                        StreetNo = "2",
                        ZipCode = "705203"
                    }
                },
                Account = new Account
                {
                    Email = "icezar@yahoo.com",
                    Password = BCrypt.HashPassword("ionesco1")
                },
                DonationCenter = unitOfWork.DonationCenters.Entities.Single(dc => dc.Name == "Spitalul Clinic Județean de Urgență Târgu Mureș"),
                BloodType = unitOfWork.BloodTypes.Entities.Single(bt => bt.Type == "AB"),
                RH = unitOfWork.RHs.Entities.Single(rh => rh.Type == "Negative"),
            });
            unitOfWork.Donors.Add(new Donor
            {
                Person = new Person
                {
                    FirstName = "Tamara",
                    LastName = "Kerekes",
                    BirthDate = new DateTime(1987, 5, 27),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Female"),
                    Nin = "2870527307703",
                    PhoneNo = "+(40) 762 817 520",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Satu Mare"),
                        City = "Satu Mare",
                        Street = "Trandafirilor",
                        StreetNo = "110"
                    }
                },
                Account = new Account
                {
                    Email = "kerekes.tamara@yahoo.com",
                    Password = BCrypt.HashPassword("kerekes1")
                },
                DonationCenter = unitOfWork.DonationCenters.Entities.Single(dc => dc.Name == "Spitalul Clinic Județean de Urgență Cluj-Napoca"),
                BloodType = unitOfWork.BloodTypes.Entities.Single(bt => bt.Type == "O"),
                RH = unitOfWork.RHs.Entities.Single(rh => rh.Type == "Positive"),
            });

            VivusConsole.WriteLine($"Donors: { unitOfWork.Complete() }");
        }

        /// <summary>
        /// Populates the patients table.
        /// </summary>
        public static void Patients()
        {
            // Delete all the patients
            unitOfWork.Patients.Entities.ToList().ForEach(patient =>
            {
                unitOfWork.Addresses.Remove(patient.Person.Address);
                unitOfWork.Persons.Remove(patient.Person);
                unitOfWork.Patients.Remove(patient);
            });

            // Add all the patients
            unitOfWork.Patients.Add(new Patient
            {
                Person = new Person
                {
                    FirstName = "Magda",
                    LastName = "Petrescu",
                    BirthDate = new DateTime(1988, 7, 2),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Female"),
                    Nin = "2880702243246",
                    PhoneNo = "+(40) 743 425 761",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Maramureș"),
                        City = "Baia Mare",
                        Street = "Melodiei",
                        StreetNo = "3",
                        ZipCode = "4000000"
                    }
                },
                PersonStatus = (unitOfWork.PersonStatuses as PersonStatusesRepository).PersonStatus("Alive"),
                BloodType = (unitOfWork.BloodTypes as BloodTypesRepository).BloodType("AB"),
                RH = (unitOfWork.RHs as RhsRepository).RH("Positive")
            });
            unitOfWork.Patients.Add(new Patient
            {
                Person = new Person
                {
                    FirstName = "Vlad",
                    LastName = "Dumitru",
                    BirthDate = new DateTime(2002, 11, 15),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Male"),
                    Nin = "5021115384320",
                    PhoneNo = "+(40) 752 002 472",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Vâlcea"),
                        City = "Râmnicu Vâlcea",
                        Street = "Știrbei Vodă",
                        StreetNo = "3",
                        ZipCode = "2400184"
                    }
                },
                PersonStatus = (unitOfWork.PersonStatuses as PersonStatusesRepository).PersonStatus("Alive"),
                BloodType = (unitOfWork.BloodTypes as BloodTypesRepository).BloodType("A"),
                RH = (unitOfWork.RHs as RhsRepository).RH("Negative")
            });
            unitOfWork.Patients.Add(new Patient
            {
                Person = new Person
                {
                    FirstName = "Aurel",
                    LastName = "Nicolescu",
                    BirthDate = new DateTime(1992, 3, 1),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Male"),
                    Nin = "1920301056591",
                    PhoneNo = "+(40) 736 959 637",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Bihor"),
                        City = "Oradea",
                        Street = "Carașului",
                        StreetNo = "26",
                        ZipCode = null
                    }
                },
                PersonStatus = (unitOfWork.PersonStatuses as PersonStatusesRepository).PersonStatus("Dead"),
                BloodType = (unitOfWork.BloodTypes as BloodTypesRepository).BloodType("O"),
                RH = (unitOfWork.RHs as RhsRepository).RH("Negative")
            });
            unitOfWork.Patients.Add(new Patient
            {
                Person = new Person
                {
                    FirstName = "Sergiu",
                    LastName = "Ionescu",
                    BirthDate = new DateTime(1986, 4, 10),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Male"),
                    Nin = "1860410173657",
                    PhoneNo = "+(40) 769 161 432",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Galați"),
                        City = "Tecuci",
                        Street = "Focșani",
                        StreetNo = "25",
                        ZipCode = "805300"
                    }
                },
                PersonStatus = (unitOfWork.PersonStatuses as PersonStatusesRepository).PersonStatus("Alive"),
                BloodType = (unitOfWork.BloodTypes as BloodTypesRepository).BloodType("B"),
                RH = (unitOfWork.RHs as RhsRepository).RH("Negative")
            });
            unitOfWork.Patients.Add(new Patient
            {
                Person = new Person
                {
                    FirstName = "Oana",
                    LastName = "Șerban",
                    BirthDate = new DateTime(1971, 10, 6),
                    Gender = (unitOfWork.Genders as GendersRepository).Gender("Female"),
                    Nin = "2711006227892",
                    PhoneNo = "+(40) 729 973 344",
                    Address = new Address
                    {
                        County = (unitOfWork.Counties as CountiesRepository).County("Iași"),
                        City = "Iași",
                        Street = "Brândușa",
                        StreetNo = "35",
                        ZipCode = null
                    }
                },
                PersonStatus = (unitOfWork.PersonStatuses as PersonStatusesRepository).PersonStatus("Alive"),
                BloodType = (unitOfWork.BloodTypes as BloodTypesRepository).BloodType("O"),
                RH = (unitOfWork.RHs as RhsRepository).RH("Positive")
            });

            VivusConsole.WriteLine($"Patients: { unitOfWork.Complete() }");
        }
    }
}
