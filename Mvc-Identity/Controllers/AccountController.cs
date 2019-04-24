using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mvc_Identity.ViewModels;
using System.Security.Claims;

namespace Mvc_Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //

        /*      Self-Notes:
         * - Recreated (kind word for copied) most of the work from a recent example by Ulf. 
         *    - Make it yourself with your own style after you've understood how it all functions.
         * - A lot of the Action-methods in the AccountController seems to work in unison. 
         *    - Try to use that to your advantage. (RedirectToAction, PartialViews, etc.)
         * - Signout never really needs a View since it only has 1 row of code. Will this change?
         * - Put notes EVERYWHERE to try to remember what various things does.
         *    - Especially the different managers, what their purpose is, what things they handle, etc.
         * - A switch both looks nicer and functions better than an If-statement when working with users.
         * - [Authorize] above the AccountController means it affects ALL Action-methods in this Controller.
         *    - Except for the Action-methods you put [AllowAnonymous] on.
        */

        /*      REMEMBER:
         * - Remember to use DataAnnotations correctly with users, logged-in or not.
         * - [AllowAnonymous] Allows the user to access the Action-method even if the Controller has [Authorize] on top.
         * - [AllowAnonymous] bypasses ALL [Authorize] statements put.
         * - Keep it simple for once...
         * - Please listen to the above text.
         * - Work on the Identity section AFTER you're done with everything else.
         * - Start off with 1 user to see how it all behaves. Add more at a later stage.
         * - You don't put a specific "tier" on roles like i first thought. you just put restrictions -
         *      - On everything with [Authorize(Roles = "X")]
         */

        /// <summary>
        /// _userManager is for EVERYTHING User related like creating, deleting, assigning roles etc.
        /// </summary>
        private readonly UserManager<AppUser> _userManager;
        /// <summary>
        /// _signInManager is for EVERYTHING related to login and logout. Which is... login and logout.
        /// </summary>
        private readonly SignInManager<AppUser> _signInManager;
        /// <summary>
        /// _roleManager is for EVERYTHING related to roles, like creating, deleting.
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
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

        /// <summary>
        /// A simple Action method to send the user to the SignIn View.
        /// THIS ONE NEEDS TO BE [AllowAnonymous]! Otherwise the non-logged in users can't access this.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        /// <summary>
        /// Checks if the data the user submitted is valid, then checks it in the database to see if
        /// the UserName and Password combination exists. Also needs to be [AllowAnonymous].
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);

                switch (signInResult.ToString())
                {
                    case "Succeeded": // There's no need to add a message since it's a redirect.
                        return RedirectToAction(nameof(Index), "Home");

                    case "Failed": // User gets this message if he/she typed wrong.
                        ViewBag.msg = "Login Failed - Username and/or password was incorrect.";
                        break;
                    case "Lockedout": // User gets this message if he/she typed wrong too many times.
                        ViewBag.msg = "Login Failed - you're locked out. Try again later.";
                        break;
                    default: // The default message corresponding all other mistakes the user might do.
                        ViewBag.msg = signInResult.ToString();
                        break;
                }
            }
            return View();
        }

        /// <summary>
        /// Simple Signout Action method that, as name implies, signs the user out.
        /// This one doesn't need a View unless you want a fancy message for the user.
        /// </summary>
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Index), "Home");
        }

        /// <summary>
        /// Sends the user to the CreateUser View so a account can be created.
        /// Fix this later to be admin-only? Or maybe accessible to all logged in users?
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateUser()
        {
            return View();
        }
        /// <summary>
        /// Gathers the data the user submitted, validates it and if it's up to standard, proceeds to
        /// create the account.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(CreateUserVM createUser)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser() { UserName = createUser.UserName, Email = createUser.Email };
                var result = await _userManager.CreateAsync(user, createUser.Password);

                // This checks if the user checked the checkbox when creating the user.
                // If the user did, then the user will be assigned the Administrator role.
                if (createUser.Admin == true)
                { 
                    await _userManager.AddToRoleAsync(user, "Administrator");
                }

                await _userManager.AddToRoleAsync(user, "NormalUser");

                if (result.Succeeded)
                { // Change this later. this is just to make it easier to create several users quickly.
                    ViewBag.msg = "User was successfully created!";

                    return RedirectToAction(nameof(CreateUser));
                }
                else
                { // A lazy and bad way to inform the user about errors.
                    ViewBag.errorlist = result.Errors.ToString();
                }
            }
            return View(createUser);
        }

        /// <summary>
        /// A Simple list of all roles currently existing.
        /// </summary>
        public IActionResult ListOfRoles()
        {
            return View(_roleManager.Roles.ToList());
        }

        /// <summary>
        /// Sends the user to the AddRole View where they can create a new role.
        /// </summary>
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        /// <summary>
        /// Gets the name of the role the user submitted and proceeds to add it to the database.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string name)
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

        /// <summary>
        /// Makes it possible for the user to assign him/her-self and others to various roles.
        /// </summary>
        [HttpGet]
        public IActionResult AssignUserToRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                ViewBag.msg = "Something went wrong (: - Please try again";

                return View();
            }
            ViewBag.role = role;

            return View(_userManager.Users.ToList());
        }
        /// <summary>
        /// Takes the role selected and the userId of the person that was selected and combines them.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AssignUserToRoleSave(string userId, string role)
        {
            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(role))
            {
                ViewBag.msg = "Something went wrong (:";

                return RedirectToAction(nameof(AssignUserToRole), "Account", role);
            }

            AppUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.AddToRoleAsync(user, role);

            if (result.Succeeded)
            {
                ViewBag.msg = "User was successfully assigned to role";

                return RedirectToAction(nameof(ListOfRoles));
            }
            else
            {
                ViewBag.msg = result.ToString();

                return RedirectToAction(nameof(AssignUserToRole), "Account", role);
            }
        }

        /// <summary>
        /// The Get method for removing a user. NOTE: Haven't added any buttons yet to reach this method.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RemoveUser(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    return View(user);
                }
                return NotFound();
            }
            return BadRequest();
        }
        /// <summary>
        /// The Post method for removing a user. NOTE: Haven't added any buttons yet to reach this method.
        /// </summary>
        [HttpPost, ActionName("Removeuser")]
        public async Task<IActionResult> RemoveUserConfirmed(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var result = await _userManager.DeleteAsync(await _userManager.FindByIdAsync(userId));

                if (result.Succeeded)
                { // Returning to ListOfRoles because I haven't added the correct actionmethods yet.
                    return RedirectToAction(nameof(ListOfRoles));
                }
                else
                {
                    ViewBag.msg = result.Errors.ToString();
                }
            }
            return RedirectToAction(nameof(ListOfRoles));
        }

        /// <summary>
        /// The Get method for removing a role. Asks if the user is sure that the role should be deleted.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> RemoveRole(string roleId)
        {
            if (!string.IsNullOrWhiteSpace(roleId))
            {
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role != null)
                {
                    return View(role);
                }
                return NotFound();
            }
            return BadRequest();
        }
        /// <summary>
        /// The Post method for removing a role. Removes role. Only accessible if the user decided yes.
        /// </summary>
        [HttpPost, ActionName("RemoveRole")]
        public async Task<IActionResult> RemoveRoleConfirmed(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                ViewBag.msg = "Something went wrong (:";

                return RedirectToAction(nameof(ListOfRoles));
            }

            var result = await _roleManager.DeleteAsync(await _roleManager.FindByIdAsync(roleId));

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ListOfRoles));
            }
            else
            {
                ViewBag.msg = result.Errors.ToString();

                return RedirectToAction(nameof(ListOfRoles));
            }
        }
    }
}
