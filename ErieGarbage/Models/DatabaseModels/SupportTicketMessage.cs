namespace Model.DatabaseModels.ErieGarbage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SupportTicketMessage
    {
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
