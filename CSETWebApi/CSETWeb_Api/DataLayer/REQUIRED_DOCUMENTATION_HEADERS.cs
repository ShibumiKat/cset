//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class REQUIRED_DOCUMENTATION_HEADERS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REQUIRED_DOCUMENTATION_HEADERS()
        {
            this.REQUIRED_DOCUMENTATION = new HashSet<REQUIRED_DOCUMENTATION>();
        }
    
        public int RDH_Id { get; set; }
        public string Requirement_Documentation_Header { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REQUIRED_DOCUMENTATION> REQUIRED_DOCUMENTATION { get; set; }
    }
}