using Magti1.Data;
using Magti1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Magti1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]        
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(u => u.BoughtNumber).ToListAsync();

            return View(users);
        }
    }
}
