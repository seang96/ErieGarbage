using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErieGarbage.Models.DatabaseModels
{
    public partial class Invoice
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int InvoiceID { get; set; }

        public int? CustomerID { get; set; }

        [Column(TypeName = "money")]
        public decimal? AmountOwed { get; set; }

        public bool Paid { get; set; }

        public DateTime InvoiceGenerated { get; set; }

        public DateTime? InvoicePaid { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
