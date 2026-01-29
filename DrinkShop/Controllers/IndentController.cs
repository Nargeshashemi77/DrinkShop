using DrinkShop.Data;
using DrinkShop.Enum;
using DrinkShop.Models;
using DrinkShop.Models.ApiModel;
using DrinkShop.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace DrinkShop.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class IndentController : Controller
    {
        private DrinkShopDbContext _context;
        private readonly UserManager<User> _userManager;
        public IndentController(DrinkShopDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddToOrder model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) { return NotFound(); }

                if (user.Address == null || user.PhoneNumber == null || user.Name == null || user.Family == null)
                {
                    string url = Url.Action("ProductDetails", "Product", new { productId = model.productId });
                    return Ok(new { message = "Should complete user information first", url = $"/Buyer/Profile?message=Order&Url={url}" });
                }
                var buyer = await _context.buyers.SingleOrDefaultAsync(b => b.userId == user.Id);
                if (buyer == null) { return NotFound(); }

                var product = await _context.products.SingleOrDefaultAsync(p => p.id == model.productId);
                if (product == null) { return NotFound(); }

                if (product.Stock)
                {
                    var cartItem = await _context.orders.Where(o => o.productId == product.id && o.buyerId == buyer.id && (o.Status == OrderStatus.Pending || o.Status == OrderStatus.reffered || o.Status == OrderStatus.doing)).SingleOrDefaultAsync();
                    #region Validation

                    #endregion
                    if (cartItem == null)
                    {
                        _context.orders.Add(new Order
                        {
                            productId = product.id,
                            buyerId = buyer.id,
                            createdAt = DateTime.Now
                        });
                    }
                    else
                        return Ok(new { message = "The Product Added already" });
                    _context.SaveChanges();
                }
                else
                    return Ok(new { message = "The product is not available" });

                return Ok(new { message = "Success" });
            }
            catch (Exception e)
            {
                Console.WriteLine($"catche error: {e.Message}");
                return StatusCode(500);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditOrder([FromBody] EditOrder order)
        {
            try
            {
                var Order = await _context.orders.SingleOrDefaultAsync(o => o.Id == order.OrderId);
                if (Order == null)
                    return NotFound();
                _context.orders.Update(Order);
                await _context.SaveChangesAsync();
                return RedirectToAction("UserOrders", "Indent");
            }
            catch (Exception e)
            {
                Console.WriteLine("Catched Error: ", e.Message);
                return StatusCode(500);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UserOrders()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) { return NotFound(); }

            Buyer? buyer = await _context.buyers.SingleOrDefaultAsync(b => b.userId == user.Id);
            if (buyer == null) { return NotFound(); }

            var userCart = await _context.orders.Where(o => o.buyerId == buyer.id).IgnoreQueryFilters()
                .Include(p => p.product)
                .Select(o => new UserOrderViewModel
                {
                    orderId = o.Id,
                    productId = o.productId,
                    productName = o.product.Name,
                    productImage = o.product.productImage,
                    productPrice = o.product.Price,
                    registerDateTime = o.createdAt,
                    OrderStat = o.Status
                }
                ).ToListAsync();
            return View(userCart);
        }
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) { return NotFound(); }

                var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
                if (buyer == null) { return NotFound(); }

                var deleteItem = _context.orders.Where(o => o.Id == orderId).SingleOrDefault();
                if (deleteItem == null) { return NotFound(); }
                deleteItem.Status = OrderStatus.canceled;
                _context.orders.Update(deleteItem);
                _context.SaveChanges();
                return RedirectToAction("UserOrders", "Indent");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
    }
}
