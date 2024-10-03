using Magti1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Magti1.Controllers
{
    public class AdminController(ApplicationDbContext context) : Controller
    {
        //[Authorize(Roles = "Admin")]        
        public async Task<IActionResult> Index()
        {
            var users = await context.Users
                .Include(x => x.BoughtNumber)
                .ToListAsync();

            return View(users);
        }
    }
}
