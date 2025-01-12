using IleriWeb.Core.Models;
using IleriWeb.Core.Services;
using IleriWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IleriWeb.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class UsersController : Controller
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext appDbContext;
        
        public UsersController(UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            this.appDbContext = appDbContext;
        }

        public IActionResult Index()
		{
            
            var users= appDbContext.maskedaspnetusers.ToList();
            return View(users);
		}

      
	}
}
