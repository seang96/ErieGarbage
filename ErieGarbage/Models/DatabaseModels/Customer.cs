namespace Model.DatabaseModels.ErieGarbage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            BillingInformations = new HashSet<BillingInformation>();
            Invoices = new HashSet<Invoice>();
            SupportTicketMessages = new HashSet<SupportTicketMessage>();
            SupportTickets = new HashSet<SupportTicket>();
        }

        public int CustomerID { get; set; }

        [Required]
        public string AccountID { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public int? BillingInfo { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        public int? PickupTimes { get; set; }

        [StringLength(11)]
        public string Phone { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string MiddleName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        public bool Suspended { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingInformation> BillingInformations { get; set; }

        public virtual BillingInformation BillingInformation { get; set; }

        public virtual PickupTime PickupTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupportTicketMessage> SupportTicketMessages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupportTicket> SupportTickets { get; set; }
    }
}
