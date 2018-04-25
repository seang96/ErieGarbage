using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;
using Model.DatabaseModels.ErieGarbage;

namespace ErieGarbage.Models
{
	public class Customer
	{
		private static readonly Model.DatabaseModels.ErieGarbage.ErieGarbage ErieGarbage = new Model.DatabaseModels.ErieGarbage.ErieGarbage();
		
		private Model.DatabaseModels.ErieGarbage.Customer _customer { get; set; }

		public string CustomerId { get; }

		public string Email
		{
			get => _customer.Email;
			set
			{
				_customer.Email = value;
				ErieGarbage.SaveChanges();
			}
		}

		public string Street
		{
			get => _customer.Street;
			set
			{
				_customer.Street = value;
				ErieGarbage.SaveChanges();
			}
		}

		public string PhoneNumber
		{
			get => _customer.Phone;
			set
			{
				_customer.Phone = value;
				ErieGarbage.SaveChanges();
			}
		}

		public string FirstName
		{
			get => _customer.FirstName;
			set
			{
				_customer.FirstName = value;
				ErieGarbage.SaveChanges();
			}
		}

		public string MiddleName
		{
			get => _customer.MiddleName;
			set
			{
				_customer.MiddleName = value;
				ErieGarbage.SaveChanges();
			}
		}

		public string LastName
		{
			get => _customer.LastName;
			set
			{
				_customer.LastName = value;
				ErieGarbage.SaveChanges();
			}
		}

		public bool Suspended => _customer.Suspended;

		public Customer()
		{
			CustomerId = _customer.AccountID;
		}

		public bool Login(string email, string password)
		{
			var result = (from customer in ErieGarbage.Customers
				where customer.Email == email && customer.Password == password
				select customer).First();
            _customer = result;
			return result != null;
		}

		public BillingInformation GetBillingInformation()
		{
			return _customer?.BillingInformation;
		}

		public PickupTime GetPickupTime()
		{
			return _customer?.PickupTime;
		}

		public ICollection<SupportTicket> GetSupportTicket()
		{
			return _customer?.SupportTickets;
		}

		public ICollection<Invoice> GetInvoices()
		{
			return _customer?.Invoices;
		}

		public bool PayInvoice(Invoice invoice)
		{
			//Paying stuffs
			invoice.Paid = true;
			invoice.InvoicePaid = DateTime.Now;
			ErieGarbage.SaveChanges();

			return (bool) invoice.Paid;
		}

		public bool UpdateBillingInformation()
		{
			ErieGarbage.SaveChanges();
			return true;
		}

		public bool UpdatePassword(string newPassword)
		{
			if (_customer == null) return false;
			if (!_customer.Password.Equals(newPassword)) return false;
			
			_customer.Password = newPassword;
			return true;
		}

		public bool SuspendAccount()
		{
			if (DateTime.Now.Day != 1) return false;
			_customer.Suspended = true;
			ErieGarbage.SaveChanges();
			return true;
		}

		public bool CreateTicket(string title)
		{
			var ticket = new SupportTicket();
			ticket.CustomerID = _customer.CustomerID;
			ticket.Customer = _customer;
			ticket.Title = title;
			ErieGarbage.SaveChanges();
			var ticketMessage = new SupportTicketMessage();
			ticketMessage.CustomerID = _customer.CustomerID;
			ticketMessage.Customer = _customer;
			return true;
		}
	}
}