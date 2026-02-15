using DrinkShop.Data;
using DrinkShop.Models;
using DrinkShop.Models;
using DrinkShop.Models.InputQuery;
using DrinkShop.Models.View_Models;
using DrinkShop.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace DrinkShop.Controllers
{
    public class HomeController : Controller
    {
        private DrinkShopDbContext _context;
        public HomeController(DrinkShopDbContext context)
        {
            _context = context;
        }
		public IActionResult Index([FromQuery] GetProductInputQuery query)
		{
			try
			{
				if (query.page < 1)
					return BadRequest(new { StatusCode = 400, message = "Page number should be greater than 0" });

				if (query.limit < 1)
					return BadRequest(new { StatusCode = 400, message = "Limit should be greater than 0" });

				int skip = (query.page - 1) * query.limit;

				IQueryable<Product> products = _context.products.AsQueryable();

				if (!string.IsNullOrEmpty(query.search))
					products = products.Where(p => p.Name.Contains(query.search));

				if (!string.IsNullOrEmpty(query.filter) && query.filter.ToLower() == "available")
					products = products.Where(p => p.Stock > 0);

				int totalCount = products.Count();
				int totalPages = (int)Math.Ceiling((double)totalCount / query.limit);

				if (!string.IsNullOrEmpty(query.sort))
					products = Filter.ApplySort(products, query.sort);

				var productList = products.Skip(skip).Take(query.limit).ToList();

				var viewModel = new ProductPageViewModel
				{
					Products = productList,
					CurrentPage = query.page,
					TotalPages = totalPages,
					Limit = query.limit
				};

				return View(viewModel);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Caught Error: {e.Message}");
				return StatusCode(500);
			}
		}
		public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Team()
        {
            return View();
        }
        public IActionResult OurService()
        {
            return View();
        }
        public IActionResult AboutUs()
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