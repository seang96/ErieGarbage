namespace ErieGarbage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ErieGarbage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        AdministratorID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.AdministratorID);
            
            CreateTable(
                "dbo.SupportTicketMessages",
                c => new
                    {
                        SupportTicketMessageID = c.Int(nullable: false, identity: true),
                        SupportTicketID = c.Int(nullable: false),
                        CustomerID = c.Int(),
                        AdministratorID = c.Int(),
                        Message = c.String(nullable: false, maxLength: 500),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SupportTicketMessageID)
                .ForeignKey("dbo.Administrators", t => t.AdministratorID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .ForeignKey("dbo.SupportTickets", t => t.SupportTicketID)
                .Index(t => t.SupportTicketID)
                .Index(t => t.CustomerID)
                .Index(t => t.AdministratorID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        AccountID = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        BillingInfo = c.Int(),
                        Street = c.String(maxLength: 50),
                        PickupTimes = c.Int(),
                        Phone = c.String(maxLength: 11),
                        FirstName = c.String(maxLength: 30),
                        MiddleName = c.String(maxLength: 30),
                        LastName = c.String(maxLength: 30),
                        Suspended = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID)
                .ForeignKey("dbo.BillingInformation", t => t.BillingInfo)
                .ForeignKey("dbo.PickupTimes", t => t.PickupTimes)
                .Index(t => t.BillingInfo)
                .Index(t => t.PickupTimes);
            
            CreateTable(
                "dbo.BillingInformation",
                c => new
                    {
                        BillingInfoID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(),
                        Street = c.String(nullable: false, maxLength: 50),
                        City = c.String(nullable: false, maxLength: 50),
                        State = c.String(nullable: false, maxLength: 50),
                        zip = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.BillingInfoID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(),
                        AmountOwed = c.Decimal(storeType: "money"),
                        Paid = c.Boolean(nullable: false),
                        InvoiceGenerated = c.DateTime(nullable: false),
                        InvoicePaid = c.DateTime(),
                    })
                .PrimaryKey(t => t.InvoiceID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.PickupTimes",
                c => new
                    {
                        PickupTimesID = c.Int(nullable: false, identity: true),
                        Street = c.String(maxLength: 50),
                        Monday = c.Time(precision: 0),
                        Tuesday = c.Time(precision: 0),
                        Wednesday = c.Time(precision: 0),
                        Thursday = c.Time(precision: 0),
                        Friday = c.Time(precision: 0),
                        Saturday = c.Time(precision: 0),
                        Sunday = c.Time(precision: 0),
                    })
                .PrimaryKey(t => t.PickupTimesID);
            
            CreateTable(
                "dbo.SupportTickets",
                c => new
                    {
                        SupportTicketID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(),
                        Status = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false, storeType: "date"),
                        Title = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.SupportTicketID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .Index(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupportTicketMessages", "SupportTicketID", "dbo.SupportTickets");
            DropForeignKey("dbo.SupportTickets", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.SupportTicketMessages", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Customers", "PickupTimes", "dbo.PickupTimes");
            DropForeignKey("dbo.Invoices", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.BillingInformation", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Customers", "BillingInfo", "dbo.BillingInformation");
            DropForeignKey("dbo.SupportTicketMessages", "AdministratorID", "dbo.Administrators");
            DropIndex("dbo.SupportTickets", new[] { "CustomerID" });
            DropIndex("dbo.Invoices", new[] { "CustomerID" });
            DropIndex("dbo.BillingInformation", new[] { "CustomerID" });
            DropIndex("dbo.Customers", new[] { "PickupTimes" });
            DropIndex("dbo.Customers", new[] { "BillingInfo" });
            DropIndex("dbo.SupportTicketMessages", new[] { "AdministratorID" });
            DropIndex("dbo.SupportTicketMessages", new[] { "CustomerID" });
            DropIndex("dbo.SupportTicketMessages", new[] { "SupportTicketID" });
            DropTable("dbo.SupportTickets");
            DropTable("dbo.PickupTimes");
            DropTable("dbo.Invoices");
            DropTable("dbo.BillingInformation");
            DropTable("dbo.Customers");
            DropTable("dbo.SupportTicketMessages");
            DropTable("dbo.Administrators");
        }
    }
}
