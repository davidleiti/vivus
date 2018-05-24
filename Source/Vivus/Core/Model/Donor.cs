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
    
    public partial class Donor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Donor()
        {
            this.DonationForms = new HashSet<DonationForm>();
        }
    
        public int PersonID { get; set; }
        public int AccountID { get; set; }
        public int ResidenceID { get; set; }
        public Nullable<int> DonationCenterID { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Address Residence { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonationForm> DonationForms { get; set; }
        public virtual Person Person { get; set; }
        public virtual DonationCenter DonationCenter { get; set; }
    }
}
