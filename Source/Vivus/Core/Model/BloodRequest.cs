//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vivus.Core.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class BloodRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BloodRequest()
        {
            this.BloodContainers = new HashSet<BloodContainer>();
            this.DonationCenters = new HashSet<DonationCenter>();
        }
    
        public int BloodRequestID { get; set; }
        public Nullable<int> ThrombocytesQuantity { get; set; }
        public Nullable<int> RedCellsQuantity { get; set; }
        public Nullable<int> PlasmaQuantity { get; set; }
        public Nullable<int> BloodQuantity { get; set; }
        public int RequestPriorityID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public bool IsFinished { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BloodContainer> BloodContainers { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual RequestPriority RequestPriority { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonationCenter> DonationCenters { get; set; }
    }
}
