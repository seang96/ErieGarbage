using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using ErieGarbage.Models.DatabaseModels;
using Konscious.Security.Cryptography;

namespace ErieGarbage.Models
{
	public class Customer
	{
		private readonly DatabaseModels.ErieGarbage ErieGarbage = new DatabaseModels.ErieGarbage();
		private static readonly Regex PasswordValidation = new Regex("^((?=.*\\d)(?=.*[A-Z])(?=.*(_|\\W))(?!.*\\s).{8,50})$");
		
		private DatabaseModels.Customer _customer { get; set; }

		public int? CustomerID => _customer?.CustomerID;
		
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

		public BillingInformation BillingInformation
		{
			get => _customer.BillingInformation;
			set
			{
				_customer.BillingInformation = value;
				ErieGarbage.SaveChanges();
			}
		}

		public Account Account => _customer?.Account;
		public bool? Suspended => _customer?.Suspended;
		
		public ProfileForm ProfileForm { get; set; }

		public Customer()
		{
		}
		
		public Customer(int id)
		{
			var firstCustomer = (from customer in ErieGarbage.Customers
				where customer.CustomerID == id
				select customer).FirstOrDefault();
			if (firstCustomer != null)
				_customer = firstCustomer;

		}
		public bool Login(string email, string password)
		{
			if (string.IsNullOrEmpty(email)) return false;
			if (string.IsNullOrEmpty(password)) return false;
			var result = (from customer in ErieGarbage.Customers
				where customer.Email == email
				select customer).FirstOrDefault();
			if (result == null) return false;
			
            _customer = result;
			return string.Equals(_customer.Password,
				HashedPassword(password, _customer.Salt, _customer.Account.AccountNumber));
		}

		public PickupTime GetPickupTime()
		{
			return _customer?.PickupTime;
		}

		public void UpdatePickupTime()
		{
			var pickupTime = (from pickup in ErieGarbage.PickupTimes
				where string.Equals(pickup.Street, Street)
				select pickup).FirstOrDefault();
			if (pickupTime == null) return;
			
			_customer.PickupTime = pickupTime;
			_customer.PickupTimes = pickupTime.PickupTimesID;
			pickupTime.Customers.Add(_customer);
			ErieGarbage.SaveChanges();
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

			return invoice.Paid;
		}

		public bool UpdatePassword(string newPassword)
		{
			if (_customer == null) return false;
			if (string.IsNullOrEmpty(newPassword)) return false;
			if (!PasswordValidation.IsMatch(newPassword)) return false;

			var salt = Crypto.GenerateSalt();
			_customer.Password = HashedPassword(newPassword, salt, Account.AccountNumber);
			_customer.Salt = salt;
			ErieGarbage.SaveChanges();
			return true;
		}

		public bool SuspendAccount()
		{
			if (DateTime.Now.Day != 1) return false;
			_customer.Suspended = true;
			ErieGarbage.SaveChanges();
			return true;
		}

		public bool CreateTicket(string title, string body)
		{
			var ticket = new SupportTicket
			{
				Customer = _customer,
				CustomerID = _customer.CustomerID,
				Title = title,
			};
			
			ErieGarbage.SupportTickets.Add(ticket);
			ErieGarbage.SaveChanges();
			
			var ticketMessage = new SupportTicketMessage
			{
				SupportTicket = ticket,
				SupportTicketID = ticket.SupportTicketID,
				Customer = _customer,
				CustomerID = _customer.CustomerID,
				Message = body,
			};
			
			ticket.SupportTicketMessages.Add(ticketMessage);
			ErieGarbage.SaveChanges();
			return true;
		}

		public static bool Register(string accountID, string email, string password)
		{
			if (string.IsNullOrEmpty(accountID)) return false;
			if (string.IsNullOrEmpty(email)) return false;
			if (string.IsNullOrEmpty(password)) return false;
			if (!PasswordValidation.IsMatch(password)) return false;
			try
			{
				email = new MailAddress(email).Address;
			}
			catch (FormatException)
			{
				return false;
			}

			var ErieGarbage = new DatabaseModels.ErieGarbage();
			var oldCustomer = (from customer in ErieGarbage.Customers
				where string.Equals(customer.Email, email)
				select customer).FirstOrDefault();
			if (oldCustomer != null) return false;
			var firstAccount = (from account in ErieGarbage.Accounts
				where string.Equals(account.AccountNumber, accountID) && account.Customer == null
				select account).FirstOrDefault();
			if (firstAccount == null) return false;
			var salt = Crypto.GenerateSalt();
			var hashedPassword = HashedPassword(password, salt, accountID);

			var newCustomer = new Models.DatabaseModels.Customer
			{
				Email = email,
				Password = hashedPassword,
				Account = firstAccount,
				AccountID = firstAccount.AccountID,
				Salt = salt,
			};
			ErieGarbage.Customers.Add(newCustomer);
			ErieGarbage.SaveChanges();
			firstAccount.Customer = newCustomer;
			firstAccount.CustomerID = newCustomer.CustomerID;
			ErieGarbage.SaveChanges();
			return true;
		}

		private static string HashedPassword(string password, string salt, string associatedData)
		{
			var task = Task.Run( () =>
			{
				var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password))
				{
					DegreeOfParallelism = 2,
					MemorySize = 8192,
					Iterations = 40,
					Salt = Encoding.UTF8.GetBytes(salt),
					AssociatedData = Encoding.UTF8.GetBytes(associatedData)
				};
				return argon2.GetBytesAsync(128);
			});
			return Encoding.UTF8.GetString(task.Result);
		}
	}
}