using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Internal;
using IleriWeb.Core.Models;

namespace IleriWeb.Web.Controllers
{
	public class BaseController : Controller
	{
		protected ApplicationUser CurrentUser;

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			CurrentUser = HttpContext.Items["CurrentUser"] as ApplicationUser;
			base.OnActionExecuting(context);
		}
	}
}
