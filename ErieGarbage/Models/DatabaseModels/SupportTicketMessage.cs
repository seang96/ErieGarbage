using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErieGarbage.Models.DatabaseModels
{
    public partial class SupportTicketMessage
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SupportTicketMessageID { get; set; }

        public int SupportTicketID { get; set; }

        public int? CustomerID { get; set; }

        public int? AdministratorID { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        public DateTime Created { get; set; }

        public virtual Administrator Administrator { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual SupportTicket SupportTicket { get; set; }
    }
}
