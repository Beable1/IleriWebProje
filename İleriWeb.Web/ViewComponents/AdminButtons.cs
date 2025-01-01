using IleriWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace IleriWeb.Web.ViewComponents
{
	public class AdminButtons : BaseComponent
	{
		

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var CurrentUser = GetCurrentUser();
			ViewBag.CurrentUser = CurrentUser;
			

			return View();
		}
	}
}
