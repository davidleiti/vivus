namespace Vivus.Core.DCPersonnel.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Vivus.Core.DataModels;

    class ContainerInfoValidator
    {
        public static List<string> ContainerTypeValidation(BasicEntity<string> containerType)
        {
            if (containerType is null || containerType.Id < 0)
                return new List<string> { "Container type is mandatory." };

            return null;
        }

        public static List<string> ContainerCodeValidation(string containerCode)
        {
            if (string.IsNullOrEmpty(containerCode))
                return new List<string> { "Container code is mandatory." };

            if (containerCode.Length < 12)
                return new List<string> { "Container code must have at least 12 characters." };

            return null;
        }

        public static List<string> BloodTypeValidation(BasicEntity<string> bloodType)
        {
            if (bloodType is null || bloodType.Id < 0)
                return new List<string> { "Blood type is mandatory." };

            return null;
        }

        public static List<string> RHValidation(BasicEntity<string> rh)
        {
            if (rh is null || rh.Id < 0)
                return new List<string> { "RH is mandatory." };

            return null;
        }

        public static List<string> HarvestDateValidation(string harvestDate)
        {
            if (string.IsNullOrEmpty(harvestDate))
                return new List<string> { "Harvest date is mandatory." };

            if (!DateTime.TryParse(harvestDate, new CultureInfo("ro-RO"), DateTimeStyles.AdjustToUniversal, out DateTime date))
                return new List<string> { "Harvest date is not valid. Use dd/mm/yyyy format." };

            return null;
        }

    }
}
