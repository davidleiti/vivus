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
    
    public partial class RequestPriority
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequestPriority()
        {
            this.BloodRequest = new HashSet<BloodRequest>();
        }
    
        public int RequestPriorityID { get; set; }
        public string Priority { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BloodRequest> BloodRequest { get; set; }
    }
}
