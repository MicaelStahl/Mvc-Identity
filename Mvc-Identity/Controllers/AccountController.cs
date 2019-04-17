using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc_Identity.ViewModels;

namespace Mvc_Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);

                switch (signInResult.ToString())
                {
                    case "Succeeded":
                        return RedirectToAction(nameof(Index), "Home");

                    case "Failed":
                        ViewBag.msg = "Failed - Username and/or password was incorrect.";
                        break;
                    case "Lockedout":
                        ViewBag.msg = "Locked out";
                        break;
                    default:
                        ViewBag.msg = signInResult.ToString();
                        break;
                }
            }
            return View();
        }
    }
}