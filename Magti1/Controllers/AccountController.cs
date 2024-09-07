using Magti1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Magti1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                //perform login
                //! - not nullable
                var result = await _signInManager.PasswordSignInAsync(model.PersonalIDNumber!, model.Password!, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                //perform registration
                ApplicationUser user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.PersonalIDNumber,
                    PersonalIDNumber = model.PersonalIDNumber,
                    Email = model.Email,
                    Address = model.Address,
                };
                var result = await _userManager.CreateAsync(user, model.Password!);
                //move to the next page
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    //Go to Home Controller Index Action
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    //key-value pair
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        public async Task<IActionResult> LoginOut()
        {
            return View();
        }
    }
}
