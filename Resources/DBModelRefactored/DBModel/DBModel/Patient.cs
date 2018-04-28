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
    
    public partial class Patient : Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            this.BloodRequest = new HashSet<BloodRequest>();
        }
    
        public int PersonStatusID { get; set; }
        public int RHID { get; set; }
        public int BloodTypeID { get; set; }
        public int DoctorID { get; set; }
    
        public virtual PersonStatus PersonStatus { get; set; }
        public virtual RH RH { get; set; }
        public virtual BloodType BloodType { get; set; }
        public virtual Doctor Doctor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BloodRequest> BloodRequest { get; set; }
    }
}
