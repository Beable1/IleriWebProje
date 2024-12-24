using IleriWeb.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Internal;

namespace IleriWeb.Web.ViewComponents
{
	public abstract class BaseComponent : ViewComponent
	{
		protected ApplicationUser CurrentUser;

		public BaseComponent()
		{
			// This will be null until the first request is made, use a method instead
		}

		protected ApplicationUser GetCurrentUser()
		{
			// Safely access HttpContext and retrieve the current user
			return HttpContext?.Items["CurrentUser"] as ApplicationUser;
		}
	}
}
