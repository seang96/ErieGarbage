using System;
using System.Collections.Generic;
using System.Linq;
using Model.DatabaseModels.ErieGarbage;

namespace ErieGarbage.Models
{
	public class Adminsitrator
	{
		private static readonly Model.DatabaseModels.ErieGarbage.ErieGarbage ErieGarbage = new Model.DatabaseModels.ErieGarbage.ErieGarbage();
		private Administrator _Administrator { get; set; }
		
		public string Username => _Administrator.Username;

		public bool Login(string username, string password)
		{
			var result = (from administrator in ErieGarbage.Administrators
				where administrator.Username == username && administrator.Password == password
				select administrator).First();
			_Administrator = result;
			return result != null;
		}
	
		public bool UpdatePassword(string newPassword)
		{
			if (_Administrator == null) return false;
			if (!_Administrator.Password.Equals(newPassword)) return false;
			
			_Administrator.Password = newPassword;
			return true;
		}

		public IEnumerable<Model.DatabaseModels.ErieGarbage.Customer> GetCustomers()
		{
			return ErieGarbage.Customers.AsEnumerable();
		}

		public IEnumerable<Model.DatabaseModels.ErieGarbage.Customer> GenerateReportActiveCustomers()
		{
			return (from customer in ErieGarbage.Customers
					where customer.Suspended
					select customer).AsEnumerable();
		}

		public IEnumerable<Model.DatabaseModels.ErieGarbage.Customer> GenerateReportPaymentsDue()
		{
			return (from invoice in ErieGarbage.Invoices
				where !invoice.Paid
				select invoice.Customer).AsEnumerable();
		}
		
		public IEnumerable<Model.DatabaseModels.ErieGarbage.Customer> GenerateReportLatePayments()
		{
			return (from invoice in ErieGarbage.Invoices
				where !invoice.Paid && invoice.InvoiceGenerated <= DateTime.Now.AddMonths(-1)
				select invoice.Customer).AsEnumerable();
		}
	}
}