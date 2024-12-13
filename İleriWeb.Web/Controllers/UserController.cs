using Microsoft.AspNetCore.Mvc;

namespace IleriWeb.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
