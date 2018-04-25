namespace Model.DatabaseModels.ErieGarbage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SupportTicket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupportTicket()
        {
            SupportTicketMessages = new HashSet<SupportTicketMessage>();
        }

        public int SupportTicketID { get; set; }

        public int? CustomerID { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime Created { get; set; }

        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupportTicketMessage> SupportTicketMessages { get; set; }
    }
}
