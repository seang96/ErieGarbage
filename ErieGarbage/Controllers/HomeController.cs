using System.Web.Mvc;
using System.Web.Security;
using ErieGarbage.Models;

namespace ErieGarbage.Controllers
{
    [AllowAnonymous]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[HttpGet]
		public ActionResult Login()
		{
			return View(new LoginForm());
		}

		[HttpPost]
		public ActionResult Login(LoginForm form)
		{
			var customer = new Customer();
			var login = customer.Login(form.email, form.password);
			if (login)
			{
				FormsAuthentication.SetAuthCookie(customer.Email, false);
				return RedirectToAction("Index", "Customer", new { id = customer.CustomerID });
			}
			
			var administrator = new Administrator();
			login = administrator.Login(form.email, form.password);

			if (login)
			{
				FormsAuthentication.SetAuthCookie(administrator.Username, false);
                return RedirectToAction("Index", "Administrator", new { id = administrator.AdministratorID});
			}

			form.Error = "Login attempt has failed.";
			return View(form);
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Register()
		{
			return View(new RegisterForm());
		}

		[HttpPost]
		public ActionResult Register(RegisterForm form)
		{
			if (!string.Equals(form.password, form.confirmpassword))
			{
				form.Error = "Passwords do not match.";
				return View(form);
			}

			if (Customer.Register(form.account, form.email, form.password))
				return RedirectToAction("Login");
			form.Error = "A user with the account number and or email address already exists.";
			return View(form);
		}
	}
}