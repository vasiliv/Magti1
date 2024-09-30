using Magti1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Magti1.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            //var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = User.Claims
                                     .Where(c => c.Type == ClaimTypes.Role)
                                     .Select(c => c.Value)
                                     .ToList();
            return View();
        }
    }
}
