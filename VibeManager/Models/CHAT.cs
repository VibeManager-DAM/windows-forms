//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VibeManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CHAT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CHAT()
        {
            this.MESSAGES = new HashSet<MESSAGES>();
        }
    
        public int id { get; set; }
        public int id_user { get; set; }
        public int id_event { get; set; }
    
        public virtual EVENTS EVENTS { get; set; }
        public virtual USERS USERS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MESSAGES> MESSAGES { get; set; }
    }
}
