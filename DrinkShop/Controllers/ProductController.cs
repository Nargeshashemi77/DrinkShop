using DrinkShop.Data;
using DrinkShop.Models;
using DrinkShop.Models.InputQuery;
using DrinkShop.Models.View_Models;
using DrinkShop.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DrinkShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly DrinkShopDbContext _context;
        public ProductController(DrinkShopDbContext context)
        {
            _context = context;
        }

        // متد عمومی برای ساخت query
        private IQueryable<Product> BuildProductQuery(int? groupId = null, int? subGroupId = null, string search = null, string filter = null)
        {
            var query = _context.products.AsQueryable();

            if (groupId.HasValue)
                query = query.Where(p => p.groupId == groupId.Value);

            if (subGroupId.HasValue)
                query = query.Where(p => p.subGroupId == subGroupId.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search));

            if (!string.IsNullOrEmpty(filter) && filter.ToLower() == "available")
                query = query.Where(p => p.Stock > 0);

            return query;
        }

        private IActionResult ShowProducts(IQueryable<Product> productsQuery, GetProductInputQuery query)
        {
            if (query.page < 1)
                return BadRequest(new { StatusCode = 400, message = "Page number should be greater than 0" });

            if (query.limit < 1)
                return BadRequest(new { StatusCode = 400, message = "Limit should be greater than 0" });

            int skip = (query.page - 1) * query.limit;

            int totalCount = productsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / query.limit);

            if (!string.IsNullOrEmpty(query.sort))
                productsQuery = Filter.ApplySort(productsQuery, query.sort);

            var products = productsQuery.Skip(skip).Take(query.limit).ToList();

            var viewModel = new ProductPageViewModel
            {
                Products = products,
                CurrentPage = query.page,
                TotalPages = totalPages,
                Limit = query.limit
            };

            return View(viewModel);
        }

        // نمایش محصولات بر اساس GroupId
        [HttpGet]
        public IActionResult ShowProductByGroupId(int groupId, [FromQuery] GetProductInputQuery query)
        {
            try
            {
                var productsQuery = BuildProductQuery(groupId: groupId, search: query.search, filter: query.filter);
                return ShowProducts(productsQuery, query);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Caught Error: {e.Message}");
                return StatusCode(500);
            }
        }

        // نمایش محصولات بر اساس SubGroupId
        [HttpGet]
        public IActionResult ShowProductBySubGroupId(int subGroupId, [FromQuery] GetProductInputQuery query)
        {
            try
            {
                var productsQuery = BuildProductQuery(subGroupId: subGroupId, search: query.search, filter: query.filter);
                return ShowProducts(productsQuery, query);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Caught Error: {e.Message}");
                return StatusCode(500);
            }
        }

        // جزئیات محصول
        [HttpGet]
        public IActionResult ProductDetails(int productId)
        {
            try
            {
                var product = _context.products.FirstOrDefault(p => p.id == productId);
                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Caught Error: {e.Message}");
                return StatusCode(500);
            }
        }
    }
}
