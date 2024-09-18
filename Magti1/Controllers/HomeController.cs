using Magti1.Data;
using Magti1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;

namespace Magti1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //to get user id of currently logged user
            int ApplicationUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));            
            var numbers = _context.BoughtNumbers
                .Where(b => b.ApplicationUserId == ApplicationUserId)
                .ToList();             
            ViewBag.BoughtNumbers = numbers.Count();
            return View(numbers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.numbers = new SelectList(_context.FreeNumbers, "Id", "PhoneNumber");
            return View();
        }
        [HttpPost]
        public IActionResult Create(FreeNumber freeNumber)
        {
            if (ModelState.IsValid)
            {
                //if (_context.BoughtNumbers.Count() >= 10)
                //{

                //}
                var selectedNumber = _context.FreeNumbers.Find(freeNumber.Id);

                var boughtNumber = new BoughtNumber
                {
                    PhoneNumber = selectedNumber.PhoneNumber,
                    //to get user id of currently logged user
                    ApplicationUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
                };
                _context.BoughtNumbers.Add(boughtNumber);
                //Remove bought number from freenumbers list
                _context.FreeNumbers.Remove(selectedNumber);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.numbers = new SelectList(_context.FreeNumbers, "Id", "PhoneNumber");
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
