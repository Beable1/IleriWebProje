using AutoMapper;
using IleriWeb.Core.DTOs;
using IleriWeb.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IleriWeb.Web.Controllers
{
    public class UserController : Controller
    {

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper _mapper;
        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserSignUpDto u)
        {
            if (ModelState.IsValid)
            {




                ApplicationUser user = new ApplicationUser()
                {
                    Email = u.Mail,
                    UserName = u.UserName,
                    FullName = u.NameSurname

                };

                

                var result = await userManager.CreateAsync(user, u.Password);

                if (result.Succeeded)
                {
                    

                    return RedirectToAction("login", "user");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);

                    }
                }

            }



            return View(u);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserSignInViewModel s)
        {



            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(s.username, s.password, true, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    return View();
                }


            }

            return View();
        }


        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();
            return RedirectToAction("login", "user");
        }


        public async Task<IActionResult> AccessDenied()
        {

            return View();
        }


    }
}
