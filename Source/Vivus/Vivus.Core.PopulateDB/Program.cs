namespace Vivus.Core.PopulateDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Populate.Couties();

            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }
}
