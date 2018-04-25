namespace Model.DatabaseModels.ErieGarbage
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class ErieGarbage : DbContext
	{
		public ErieGarbage()
			: base("name=ErieGarbage")
		{
		}

		public virtual DbSet<Administrator> Administrators { get; set; }
		public virtual DbSet<BillingInformation> BillingInformations { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<Invoice> Invoices { get; set; }
		public virtual DbSet<PickupTime> PickupTimes { get; set; }
		public virtual DbSet<SupportTicketMessage> SupportTicketMessages { get; set; }
		public virtual DbSet<SupportTicket> SupportTickets { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BillingInformation>()
				.HasMany(e => e.Customers)
				.WithOptional(e => e.BillingInformation)
				.HasForeignKey(e => e.BillingInfo);

			modelBuilder.Entity<Customer>()
				.HasMany(e => e.BillingInformations)
				.WithOptional(e => e.Customer)
				.HasForeignKey(e => e.CustomerID);

			modelBuilder.Entity<Invoice>()
				.Property(e => e.AmountOwed)
				.HasPrecision(19, 4);

			modelBuilder.Entity<PickupTime>()
				.Property(e => e.Monday)
				.HasPrecision(0);

			modelBuilder.Entity<PickupTime>()
				.Property(e => e.Tuesday)
				.HasPrecision(0);

			modelBuilder.Entity<PickupTime>()
				.Property(e => e.Wednesday)
				.HasPrecision(0);

			modelBuilder.Entity<PickupTime>()
				.Property(e => e.Thursday)
				.HasPrecision(0);

			modelBuilder.Entity<PickupTime>()
				.Property(e => e.Friday)
				.HasPrecision(0);

			modelBuilder.Entity<PickupTime>()
				.Property(e => e.Saturday)
				.HasPrecision(0);

			modelBuilder.Entity<PickupTime>()
				.Property(e => e.Sunday)
				.HasPrecision(0);

			modelBuilder.Entity<PickupTime>()
				.HasMany(e => e.Customers)
				.WithOptional(e => e.PickupTime)
				.HasForeignKey(e => e.PickupTimes);

			modelBuilder.Entity<SupportTicket>()
				.HasMany(e => e.SupportTicketMessages)
				.WithRequired(e => e.SupportTicket)
				.WillCascadeOnDelete(false);
		}
	}
}
