//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonationStatus
    {
        public int DonationStatusID { get; set; }
        public decimal Weight { get; set; }
        public short Pulse { get; set; }
        public decimal BloodPressure { get; set; }
        public bool HasPastSurgeries { get; set; }
        public bool HasAlcoholConsumption { get; set; }
        public bool HasFatConsumption { get; set; }
        public bool IsInTreatment { get; set; }
        public bool HasDiseases { get; set; }
    
        public virtual Donor Donor { get; set; }
    }
}
