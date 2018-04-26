using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErieGarbage
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"CustomerNoAction",
				"Customer/{id}",
				new { controller = "Customer", action = "Index" }
			);
			
			routes.MapRoute(
				"Customer",
				"Customer/{action}/{id}",
				new { controller = "Customer", action = "Index"}
			);
			
			routes.MapRoute(
				"Home",
				"{action}",
				new {controller = "Home", action = "Index"}
			);
			
			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}