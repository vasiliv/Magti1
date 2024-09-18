using Magti1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Magti1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        public AccountController(SignInManager<ApplicationUser> signInManager,
                                UserManager<ApplicationUser> userManager,
                                IWebHostEnvironment environment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _environment = environment;
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
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                newFileName += Path.GetExtension(model.ImageFile!.FileName);

                string imageFullPath = _environment.WebRootPath + "/images/" + newFileName;

                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                //perform registration
                ApplicationUser user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.PersonalIDNumber,
                    PersonalIDNumber = model.PersonalIDNumber,
                    Email = model.Email,
                    Address = model.Address,
                    ImageFileName = newFileName
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
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new Register
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                //UserName = user.PersonalIDNumber,
                PersonalIDNumber = user.PersonalIDNumber,
                Email = user.Email,
                Address = user.Address,
                //ImageFileName = newFileName
            };
            return View(model);
        }
        [HttpPost]        
        public async Task<IActionResult> Edit(Register model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.PersonalIDNumber;
            user.Email = model.Email;
            user.PersonalIDNumber = model.PersonalIDNumber;
            user.Address = model.Address;

            var result = await _userManager.UpdateAsync(user);            
            if (result.Succeeded)
            {
                // Optionally, sign the user in again to refresh the claims
                await _userManager.UpdateSecurityStampAsync(user);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
