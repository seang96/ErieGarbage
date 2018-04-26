using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using ErieGarbage.Models.DatabaseModels;
using Konscious.Security.Cryptography;

namespace ErieGarbage.Models
{
	public class Administrator
	{
		private readonly DatabaseModels.ErieGarbage ErieGarbage = new DatabaseModels.ErieGarbage();
		private static readonly Regex PasswordValidation = new Regex("^((?=.*\\d)(?=.*[A-Z])(?=.*(_|\\W))(?!.*\\s).{8,50})$");
		private DatabaseModels.Administrator _Administrator { get; set; }

		public int? AdministratorID => _Administrator?.AdministratorID;
		public string Username => _Administrator?.Username;

		public ProfileForm ProfileForm  { get; set; }
		public RegisterForm RegisterForm { get; set; }
		
		public Administrator()
		{
		}

		public Administrator(int id)
		{
			var firstAdministrator = (from administrator in ErieGarbage.Administrators
				where administrator.AdministratorID == id
				select administrator).FirstOrDefault();
			if (firstAdministrator != null)
				_Administrator = firstAdministrator;

		}

		public bool Login(string username, string password)
		{
			var result = (from administrator in ErieGarbage.Administrators
				where administrator.Username == username
				select administrator).FirstOrDefault();
			if (result == null) return false;
			
			_Administrator = result;
			if (string.Equals(_Administrator.Username, "admin") && string.Equals(_Administrator.Password, password))
				return true;
			return string.Equals(_Administrator.Password, HashedPassword(password, _Administrator.Salt));
		}
	
		public bool UpdatePassword(string newPassword)
		{
			if (_Administrator == null) return false;
			if (!PasswordValidation.IsMatch(newPassword)) return false;

			var salt = Crypto.GenerateSalt();
			_Administrator.Password = HashedPassword(newPassword, salt);
			_Administrator.Salt = salt;
			ErieGarbage.SaveChanges();
			return true;
		}

		public IEnumerable<DatabaseModels.Customer> GetCustomers()
		{
			return new DatabaseModels.ErieGarbage().Customers.AsEnumerable();
		}

		public IEnumerable<DatabaseModels.Customer> GenerateReportActiveCustomers()
		{
			return (from customer in new DatabaseModels.ErieGarbage().Customers
					where customer.Suspended
					select customer).AsEnumerable();
		}

		public IEnumerable<DatabaseModels.Customer> GenerateReportPaymentsDue()
		{
			return (from invoice in new DatabaseModels.ErieGarbage().Invoices
				where !invoice.Paid
				select invoice.Customer).AsEnumerable();
		}
		
		public IEnumerable<DatabaseModels.Customer> GenerateReportLatePayments()
		{
			return (from invoice in new DatabaseModels.ErieGarbage().Invoices
				where !invoice.Paid && invoice.InvoiceGenerated <= DateTime.Now.AddMonths(-1)
				select invoice.Customer).AsEnumerable();
		}

		public bool CreateAdmin(string username, string password)
		{
			if (string.IsNullOrEmpty(username)) return false;
			if (string.IsNullOrEmpty(password)) return false;
			if (!PasswordValidation.IsMatch(password)) return false;
			var salt = Crypto.GenerateSalt();
			var hashedPassword = HashedPassword(password, salt);
			var oldAdmin = (from admin in ErieGarbage.Administrators
				where string.Equals(admin.Username, username)
				select admin).FirstOrDefault();
			if (oldAdmin != null) return false;

			var newAdmin = new DatabaseModels.Administrator
			{
				Username = username,
				Password = hashedPassword,
				Salt = salt,
			};

			ErieGarbage.Administrators.Add(newAdmin);
			ErieGarbage.SaveChanges();
			return true;
		}
		
		private static string HashedPassword(string password, string salt)
		{
			var task = Task.Run( () =>
			{
				var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password))
				{
					DegreeOfParallelism = 2,
					MemorySize = 8192,
					Iterations = 40,
					Salt = Encoding.UTF8.GetBytes(salt),
				};
				return argon2.GetBytesAsync(128);
			});
			return Encoding.UTF8.GetString(task.Result);
		}
	}
}