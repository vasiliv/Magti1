using Magti1.Data;
using Magti1.Models;
using Magti1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Magti1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager,
                                 IWebHostEnvironment environment,
                                 ApplicationDbContext context,
                                 RoleManager<IdentityRole<int>> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _environment = environment;
            _context = context;
            _roleManager = roleManager;
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

                // Perform login
                var result = await _signInManager.PasswordSignInAsync(model.PersonalIDNumber!, model.Password!, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // Get the currently logged-in user
                    var user = await _userManager.FindByNameAsync(model.PersonalIDNumber);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "User not found");
                        return View(model);
                    }

                    // Get the user's roles (this method is more reliable than querying manually)
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles == null || !roles.Any())
                    {
                        ModelState.AddModelError("", "User has no roles assigned");
                        return View(model);
                    }

                    // Add the user to the first role found (or handle multiple roles as necessary)
                    var role = roles.FirstOrDefault();

                    if (!string.IsNullOrEmpty(role))
                    {
                        // Check if the user is in a specific role
                        if (role == "Admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);



                ////perform login
                ////! - not nullable
                //var result = await _signInManager.PasswordSignInAsync(model.PersonalIDNumber!, model.Password!, model.RememberMe, false);

                //if (result.Succeeded)
                //{
                //    // Get the currently logged-in user
                //    var user = await _userManager.GetUserAsync(User);                    

                //    //to get user id of currently logged user
                //    int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                //    // Query to join AspNetUserRoles, AspNetRoles, and AspNetUsers
                //    var role = await (from ur in _context.UserRoles
                //                      join r in _context.Roles on ur.RoleId equals r.Id
                //                      where ur.UserId == userId
                //                      select r.Name).FirstOrDefaultAsync();

                //    await _userManager.AddToRoleAsync(user, role);
                //    // Check if the user is in a specific role
                //    if (await _userManager.IsInRoleAsync(user, "Admin"))
                //    {
                //        // Redirect to Admin dashboard or perform actions based on Admin role
                //        return RedirectToAction("Index", "Admin");
                //    }
                //    return RedirectToAction("Index", "Home");
                //}
                //ModelState.AddModelError("", "Invalid login attempt");
                //return View(model);
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
                // Retrieve the role claims from the current user's claims
                //var roleClaims = User.Claims
                //                     .Where(c => c.Type == ClaimTypes.Role)
                //                     .Select(c => c.Value)
                //                     .ToList();

                // Query to join AspNetUserRoles, AspNetRoles, and AspNetUsers
                //var roles = await (from ur in _context.UserRoles
                //                   join r in _context.Roles on ur.RoleId equals r.Id
                //                   where ur.UserId == userId
                //                   select r.Name).ToListAsync();

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
            // Get the currently logged-in user
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
            ViewData["ImageFileName"] = user.ImageFileName;
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
            string newFileName = user.ImageFileName;
            if (model.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                newFileName += Path.GetExtension(model.ImageFile.FileName);

                string imageFullPath = _environment.WebRootPath + "/images/" + newFileName;

                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                //delete the old image
                string oldImageFullPath = _environment.WebRootPath + "/images/" + user.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.PersonalIDNumber;
            user.Email = model.Email;
            user.PersonalIDNumber = model.PersonalIDNumber;
            user.Address = model.Address;
            user.ImageFileName = newFileName;

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

        public async Task<IActionResult> AssignRoles()
        {
            var users = await _userManager.Users.ToListAsync(); // Use ToListAsync() to get users
            var roles = await _roleManager.Roles.ToListAsync(); // Use ToListAsync() to get roles

            var model = new AssignRolesViewModel
            {
                Users = users,
                Roles = roles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID '{userId}' not found.");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                ModelState.AddModelError("", "Role does not exist.");
                return RedirectToAction(nameof(AssignRoles));
            }

            // Assign the role to the user
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Role '{roleName}' assigned to user '{user.UserName}'.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to assign role '{roleName}' to user '{user.UserName}'.";
            }

            return RedirectToAction(nameof(AssignRoles));
        }
    }
}
