using System.Diagnostics;
using DrinkShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrinkShop.Controllers
{
    public class HomeController : Controller
    {
        //private CropsShopContext _context;
        public HomeController()//CropsShopContext context)
        {
            //_context = context;
        }
        public IActionResult Index(int page = 1, string sort = null, string search = null)
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult RulesAndConditions()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Questions()
        {
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
