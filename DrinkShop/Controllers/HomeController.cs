using System.Diagnostics;
using DrinkShop.Models;
using DrinkShop.Models.Dtos;
using DrinkShop.Models.Entities;
using DrinkShop.Service;
using Microsoft.AspNetCore.Mvc;

namespace DrinkShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;
        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }
        public async Task<IActionResult> Index(PagedListQuery pagedListQuery, CancellationToken cancellationToken, string sort = null, string search = null)
        {
            if (pagedListQuery.PageNumber < 1)
                return BadRequest(new { StatusCode = 400, message = "page number should be greater than 0" });

            ViewData["page"] = pagedListQuery.PageNumber;

            var result = await productService.GetAllProducts(pagedListQuery, cancellationToken);

            int pageCount = (int)Math.Ceiling((decimal)result.Count);

            ViewData["pagesCount"] = pageCount;

            List<Product> products = result.Values;

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return View(products);
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
