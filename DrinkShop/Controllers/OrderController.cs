using DrinkShop.Data;
using DrinkShop.Models;
using DrinkShop.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace DrinkShop.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class OrderController : Controller
    {
        private DrinkShopDbContext _context;
        private readonly UserManager<User> _userManager;
        public OrderController(DrinkShopDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) { return NotFound(); }

            Buyer? buyer = await _context.buyers.SingleOrDefaultAsync(b => b.userId == user.Id);
            if (buyer == null) { return NotFound(); }

            var userCart = await _context.orderItems.Where(o=>o.Order.buyerId == buyer.id).IgnoreQueryFilters()
                .Include(o=>o.Order)
                .Select(o => new UserOrderViewModel
                {
                    orderId = o.Id,
                    productId = o.ProductId,
                    productName = o.Product.Name,
                    productImage = o.Product.productImage,
                    productPrice = (int)o.UnitPrice,
                    number = o.Quantity,
                    registerDateTime = o.Order.createdAt,
                    OrderStat = o.Order.Status
                }
                ).ToListAsync();
            return View(userCart);
        }
    }
}
