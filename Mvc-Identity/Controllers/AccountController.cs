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
        /// <summary>
        /// _userManager is for EVERYTHING User related like creating, deleting, assigning roles etc.
        /// </summary>
        private readonly UserManager<IdentityUser> _userManager;
        /// <summary>
        /// _signInManager is for EVERYTHING related to login and logout.
        /// </summary>
        private readonly SignInManager<IdentityUser> _signInManager;
        /// <summary>
        /// _roleManager is for EVERYTHING related to roles, like creating, deleting.
        /// </summary>
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

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM createUser)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser() { UserName = createUser.UserName, Email = createUser.Email };
                var result = await _userManager.CreateAsync(user, createUser.Password);

                if (result.Succeeded)
                {
                    ViewBag.msg = "User was successfully created!";

                    return RedirectToAction(nameof(CreateUser));
                }
                else
                {
                    ViewBag.msg = result.ToString();
                }
            }
            return View(createUser);
        }

        public IActionResult ListOfRoles()
        {
            return View(_roleManager.Roles.ToList());
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return View();
            }
            var result = await _roleManager.CreateAsync(new IdentityRole(name));

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ListOfRoles));
            }
            else
            {
                ViewBag.msg = result.ToString();
            }
            return View(name);
        }

        [HttpGet]
        public IActionResult AssignUserToRole()
        {
            return View(_userManager.Users.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> AssignUserToRole(string userId, string role)
        {
            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(role))
            {
                ViewBag.msg = "Something went wrong (:";

                return View(_userManager.Users.ToList());
            }

            IdentityUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.AddToRoleAsync(user, role);

            if (result.Succeeded)
            {
                ViewBag.msg = "User was successfully assigned to role";

                return RedirectToAction(nameof(ListOfRoles));
            }
            else
            {
                ViewBag.msg = result.ToString();

                return View(_userManager.Users.ToList());
            }
        }
    }
}
