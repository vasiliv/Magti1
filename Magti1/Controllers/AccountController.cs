using Microsoft.AspNetCore.Mvc;

namespace Magti1.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> LoginOut()
        {
            return View();
        }
    }
}
