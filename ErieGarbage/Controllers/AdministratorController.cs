using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using ErieGarbage.Models;

namespace ErieGarbage.Controllers
{
	public class AdministratorController : Controller
	{
		private Models.DatabaseModels.ErieGarbage ErieGarbage => new Models.DatabaseModels.ErieGarbage();
		public ActionResult Index(int id)
		{
			if (!IsValid(id)) return new HttpUnauthorizedResult();
			var administrator = new Administrator(id);
			return View(administrator);
		}
		
		public ActionResult Profile(int id)
		{
			if (!IsValid(id)) return new HttpUnauthorizedResult();
			var administrator = new Administrator(id);
			var form = new ProfileForm {Email = administrator.Username};
			administrator.ProfileForm = form;
			return View(administrator);
		}

		[System.Web.Mvc.HttpPost]
		public ActionResult Profile(int id, Administrator admin)
		{
			if (!IsValid(id)) return new HttpUnauthorizedResult();
			var form = admin.ProfileForm;
			admin = new Administrator(id);
			if (string.Equals(form.Password, form.ConfirmPassword))
			{
				admin.UpdatePassword(form.Password);
			}
			admin.ProfileForm = form;

			return View(admin);
		}

		[System.Web.Mvc.HttpGet]
		public ActionResult CreateAdmin(int id)
		{
			if (!IsValid(id)) return new HttpUnauthorizedResult();
			var admin = new Administrator(id);
			var form = new RegisterForm();
			admin.RegisterForm = form;

			return View(admin);
		}
		
		[System.Web.Mvc.HttpPost]
		public ActionResult CreateAdmin(int id, Administrator admin)
		{
			if (!IsValid(id)) return new HttpUnauthorizedResult();
			var form = admin.RegisterForm;
			if (!string.Equals(form.Password, form.Confirmpassword))
			{
				form.Error = "Passwords do not match.";
				return View(admin);
			}

			if (!admin.CreateAdmin(form.Email, form.Password))
                form.Error = "An admin with this username already exists";
			
			admin = new Administrator(id);
			admin.RegisterForm = form;
			
			return View(admin);
		}
		
		private bool IsValid(int id)
		{
			var cookieName = FormsAuthentication.FormsCookieName;
			var authCookie = HttpContext.Request.Cookies[cookieName];
			if (authCookie == null) return false;
			
			var ticket = FormsAuthentication.Decrypt(authCookie.Value);
			var userName = ticket.Name;
			var adminQuery = (from admin in ErieGarbage.Administrators
				where string.Equals(admin.Username, userName ) && admin.AdministratorID == id
				select admin).First();
			return adminQuery != null;
		}
	}
}