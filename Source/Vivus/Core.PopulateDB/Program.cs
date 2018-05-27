namespace Vivus.Core.PopulateDB
{
    using Vivus.Core.Model;
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a console application used in populating the database.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Starts the console application.
        /// </summary>
        /// <param name="args">The console parameters.</param>
        private static void Main(string[] args)
        {
            //Populate.Counties();
            //Populate.Genders();
            //Populate.BloodContainerTypes();
            //Populate.BloodTypes();
            //Populate.RHs();
            //Populate.PersonStatuses();
            //Populate.RequestPriorities();
            //Populate.Administrators();
            //Populate.DonationCenters();
            Populate.DonationCentersPersonnel();
            //Populate.Doctors();
            //Populate.Donors();
            //Populate.Patients();
            Address address=new Address();
            VivusConsole.WriteLine(address.AddressID + " is the address id");
            VivusConsole.WriteLine("Everything is already generated! Be careful if you wanna play with the fire. c;");

            VivusConsole.WriteLine("Press any key to continue...", false);
            VivusConsole.ReadKey();
        }
    }
}
