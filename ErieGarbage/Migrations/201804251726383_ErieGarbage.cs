namespace ErieGarbage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ErieGarbage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountID = c.Int(nullable: false, identity: true),
                        AccountNumber = c.String(nullable: false),
                        CustomerID = c.Int(),
                    })
                .PrimaryKey(t => t.AccountID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        AccountID = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 128),
                        Salt = c.String(nullable: false, maxLength: 24),
                        BillingInfo = c.Int(),
                        Street = c.String(maxLength: 50),
                        PickupTimes = c.Int(),
                        Phone = c.String(maxLength: 11),
                        FirstName = c.String(maxLength: 30),
                        MiddleName = c.String(maxLength: 30),
                        LastName = c.String(maxLength: 30),
                        Suspended = c.Boolean(nullable: false),
                        Account_AccountID = c.Int(),
                        BillingInformation_BillingInfoID = c.Int(),
                        Account_AccountID1 = c.Int(),
                    })
                .PrimaryKey(t => t.CustomerID)
                .ForeignKey("dbo.Accounts", t => t.Account_AccountID)
                .ForeignKey("dbo.BillingInformation", t => t.BillingInformation_BillingInfoID)
                .ForeignKey("dbo.PickupTimes", t => t.PickupTimes)
                .ForeignKey("dbo.Accounts", t => t.Account_AccountID1)
                .Index(t => t.PickupTimes)
                .Index(t => t.Account_AccountID)
                .Index(t => t.BillingInformation_BillingInfoID)
                .Index(t => t.Account_AccountID1);
            
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
                "dbo.Administrators",
                c => new
                    {
                        AdministratorID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 128),
                        Salt = c.String(nullable: false, maxLength: 24),
                    })
                .PrimaryKey(t => t.AdministratorID);
            
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
            DropForeignKey("dbo.Customers", "Account_AccountID1", "dbo.Accounts");
            DropForeignKey("dbo.SupportTicketMessages", "SupportTicketID", "dbo.SupportTickets");
            DropForeignKey("dbo.SupportTickets", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.SupportTicketMessages", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.SupportTicketMessages", "AdministratorID", "dbo.Administrators");
            DropForeignKey("dbo.Customers", "PickupTimes", "dbo.PickupTimes");
            DropForeignKey("dbo.Invoices", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.BillingInformation", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Customers", "BillingInformation_BillingInfoID", "dbo.BillingInformation");
            DropForeignKey("dbo.Customers", "Account_AccountID", "dbo.Accounts");
            DropIndex("dbo.SupportTickets", new[] { "CustomerID" });
            DropIndex("dbo.SupportTicketMessages", new[] { "AdministratorID" });
            DropIndex("dbo.SupportTicketMessages", new[] { "CustomerID" });
            DropIndex("dbo.SupportTicketMessages", new[] { "SupportTicketID" });
            DropIndex("dbo.Invoices", new[] { "CustomerID" });
            DropIndex("dbo.BillingInformation", new[] { "CustomerID" });
            DropIndex("dbo.Customers", new[] { "Account_AccountID1" });
            DropIndex("dbo.Customers", new[] { "BillingInformation_BillingInfoID" });
            DropIndex("dbo.Customers", new[] { "Account_AccountID" });
            DropIndex("dbo.Customers", new[] { "PickupTimes" });
            DropTable("dbo.SupportTickets");
            DropTable("dbo.Administrators");
            DropTable("dbo.SupportTicketMessages");
            DropTable("dbo.PickupTimes");
            DropTable("dbo.Invoices");
            DropTable("dbo.BillingInformation");
            DropTable("dbo.Customers");
            DropTable("dbo.Accounts");
        }
    }
}
