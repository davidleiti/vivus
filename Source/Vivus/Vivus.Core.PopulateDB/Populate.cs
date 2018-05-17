namespace Vivus.Core.PopulateDB
{
    using System.Linq;
    using Vivus.Core.Model;
    using Vivus.Core.UoW;
    using VivusConsole = Console.Console;

    /// <summary>
    /// Represents a popualtor for the database entities.
    /// </summary>
    static class Populate
    {
        private static IUnitOfWork unitOfWork = new UnitOfWork(new VivusEntities());

        /// <summary>
        /// Populates the counties.
        /// </summary>
        public static void Couties()
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
    }
}
