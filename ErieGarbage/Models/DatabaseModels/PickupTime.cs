namespace Model.DatabaseModels.ErieGarbage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PickupTime
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PickupTime()
        {
            Customers = new HashSet<Customer>();
        }

        [Key]
        public int PickupTimesID { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        public TimeSpan? Monday { get; set; }

        public TimeSpan? Tuesday { get; set; }

        public TimeSpan? Wednesday { get; set; }

        public TimeSpan? Thursday { get; set; }

        public TimeSpan? Friday { get; set; }

        public TimeSpan? Saturday { get; set; }

        public TimeSpan? Sunday { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
