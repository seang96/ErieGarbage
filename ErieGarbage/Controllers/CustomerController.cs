using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using ErieGarbage.Models;

namespace ErieGarbage.Controllers
{
	[System.Web.Mvc.Authorize]
	public class CustomerController : Controller
	{
	
		private Models.DatabaseModels.ErieGarbage ErieGarbage => new Models.DatabaseModels.ErieGarbage();
		public ActionResult Index(int id)
		{
			if (!IsValid(id)) return new HttpUnauthorizedResult();
			var customer = new Customer(id);
			return View(customer);
		}

		[System.Web.Mvc.HttpGet]
		public ActionResult Profile(int id)
		{
			if (!IsValid(id)) return new HttpUnauthorizedResult();
			var customer = new Customer(id);
			var form = new ProfileForm()
			{
				Account = customer.Account.AccountNumber,
				Email = customer.Email,
				FirstName = customer.FirstName,
				LastName = customer.LastName,
				MiddleName = customer.MiddleName,
				PhoneNumber = customer.PhoneNumber,
				Street = customer.Street,
				Suspended = customer.Suspended,
			};
			customer.ProfileForm = form;
			return View(customer);
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult Profile(int id, Customer customer)
		{
			if (!IsValid(id)) return new HttpUnauthorizedResult();
			var form = customer.ProfileForm;
			customer = new Customer(id);
			customer.FirstName = form.FirstName;
			customer.MiddleName = form.MiddleName;
			customer.LastName = form.LastName;
			customer.PhoneNumber = form.PhoneNumber;
			customer.Street = form.Street;
			customer.UpdatePickupTime();
			if (!string.Equals(customer.Email, form.Email))
			{
				var oldEmail = customer.Email;
                customer.Email = form.Email;
				
				var cookieName = FormsAuthentication.FormsCookieName;
				var authCookie = HttpContext.Request.Cookies[cookieName];
			
				var ticket = FormsAuthentication.Decrypt(authCookie.Value);
				var userName = ticket.Name;
				if (string.Equals(oldEmail, userName))
				{
                    FormsAuthentication.SignOut();
                    FormsAuthentication.SetAuthCookie(customer.Email, false);
				}
			}
			if (form.Suspended != null && (bool) form.Suspended)
				if (!customer.SuspendAccount())
				{
					form.Error = "This action can only be done on the first of the month.";
				}

			if (string.Equals(form.Password, form.ConfirmPassword))
			{
				customer.UpdatePassword(form.Password);
			}

			ErieGarbage.SaveChanges();
			customer.ProfileForm = form;

			return View(customer);
		}

		private bool IsValid(int id)
		{
			var cookieName = FormsAuthentication.FormsCookieName;
			var authCookie = HttpContext.Request.Cookies[cookieName];
			if (authCookie == null) return false;
			
			var ticket = FormsAuthentication.Decrypt(authCookie.Value);
			var userName = ticket.Name;
			var customerQuery = (from customer in ErieGarbage.Customers
				where string.Equals(customer.Email, userName ) && customer.CustomerID == id
				select customer).FirstOrDefault();
			if (customerQuery != null)
				return true;
			var adminQuery = (from admin in ErieGarbage.Administrators
				where string.Equals(admin.Username, userName )
				select admin).FirstOrDefault();
			if (adminQuery != null)
				return true;
			return false;
		}
	}
}