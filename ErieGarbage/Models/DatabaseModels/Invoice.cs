namespace Model.DatabaseModels.ErieGarbage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Invoice
    {
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
