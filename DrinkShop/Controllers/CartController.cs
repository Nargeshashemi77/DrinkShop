using DrinkShop.Data;
using DrinkShop.Models;
using DrinkShop.Models.ApiModel;
using DrinkShop.Models.ApiModel.Cart;
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
    [Route("carts")]
    public class CartController : Controller
    {
        private DrinkShopDbContext _context;
        private readonly UserManager<User> _userManager;
        public CartController(DrinkShopDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("items")]
		public async Task<IActionResult> Add([FromBody] AddInputQuery model)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var user = await _userManager.FindByNameAsync(User.Identity?.Name);
				if (user == null)
					return Unauthorized();

				if (user.Address == null || user.PhoneNumber == null || user.Name == null || user.Family == null)
				{
					string url = Url.Action("ProductDetails", "Product", new { productId = model.productId });
					return Ok(new
					{
						message = "Should complete user information first",
						url = $"/Buyer/Profile?message=Order&Url={url}"
					});
				}

				var buyer = await _context.buyers
					.FirstOrDefaultAsync(b => b.userId == user.Id);

				if (buyer == null)
					return BadRequest(new { message = "Buyer profile not found" });

				var product = await _context.products
					.FirstOrDefaultAsync(p => p.id == model.productId);

				if (product == null)
					return NotFound(new { message = "Product not found" });

				if (model.number > product.Stock)
					return BadRequest(new { message = "Requested quantity exceeds stock" });

				var cartItem = await _context.carts
					.FirstOrDefaultAsync(c =>
						c.productId == product.id &&
						c.buyerId == buyer.id);

				if (cartItem == null)
				{
					await _context.carts.AddAsync(new Cart
					{
						Number = model.number,
						productId = product.id,
						buyerId = buyer.id
					});
				}
				else
				{
					int newQuantity = cartItem.Number + model.number;

					if (newQuantity > product.Stock)
						return BadRequest(new { message = "Requested quantity exceeds stock" });

					if (newQuantity > 999)
						return BadRequest(new { message = "Cart limit exceeded" });

					cartItem.Number = newQuantity;
				}

				await _context.SaveChangesAsync();

				return Ok(new { message = "Success" });
			}
			catch (Exception e)
			{
				Console.WriteLine($"Catched Error: {e.Message}");
				return StatusCode(500);
			}
		}

		[HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) { return NotFound(); }

            var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
            if (buyer == null) { return NotFound(); }

            var userCart = await _context.carts.Where(o => o.buyerId == buyer.id).IgnoreQueryFilters()
                .Include(p => p.product)
                .Select(o => new CartResultViewModel
                {
                    CartId = o.Id,
                    ProductId = o.productId,
                    ProductName = o.product.Name,
                    ProductImage = o.product.productImage,
                    UnitPrice = o.product.Price,
                    TotalPrice = o.Number * o.product.Price,
                    Quantity = o.Number
                }
                ).ToListAsync();
            return View(userCart);
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> Update(int itemId, [FromForm] UpdateInputQuery model)
        {
            try
            {
                // بررسی مدل
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // پیدا کردن آیتم سبد خرید
                var cartItem = await _context.carts
                    .Include(c => c.product)
                    .FirstOrDefaultAsync(c => c.Id == itemId);

                if (cartItem == null)
                    return NotFound(new { message = "Cart item not found" });

                // بررسی موجودی محصول
                if (model.number > cartItem.product.Stock)
                    return BadRequest(new { message = "Requested quantity exceeds stock" });

                // به‌روزرسانی تعداد
                cartItem.Number = model.number;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Cart item updated successfully" });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        //public async Task<IActionResult> CompletePurchaseAndPayment()
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //        if (user == null) { return NotFound(); }

        //        var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
        //        if (buyer == null) { return NotFound(); }

        //        if (user.PhoneNumber == null || user.Address == null)
        //            return RedirectToAction("Profile", "User", new { message = "Buy" });

        //        var buyerCart = _context.carts.Where(c => c.buyerId == buyer.id)
        //            .Include(c => c.product)
        //            .ToList();

        //        List<Order> orders = new List<Order>();
        //        for (int i = 0; i < buyerCart.Count; i++)
        //        {
        //            orders.Add(new Order
        //            {
        //                buyerId = buyer.id,
        //                productId = buyerCart[i].productId,
        //                Price = buyerCart[i].product.Price,
        //                Number = buyerCart[i].Number,
        //                orderDateTime = DateTime.Now,
        //            });
        //        }
        //        List<Product> products = new List<Product>();
        //        var buyerCartProductIdlist = buyerCart.Select(c => c.productId).ToList();
        //        products = _context.products.Where(p => buyerCartProductIdlist.Contains(p.id)).ToList();
        //        var sortBuyerCart = buyerCart.OrderBy(c => c.productId).ToList();
        //        for (int i = 0; i < sortBuyerCart.Count; i++)
        //        {
        //            products[i].Stock -= sortBuyerCart[i].Number; //* products[i].Weight);
        //        }
        //        _context.products.UpdateRange(products);
        //        _context.orders.AddRange(orders);
        //        _context.SaveChangesAsync();
        //        return RedirectToAction("Factor");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Catched Error: {e.Message}");
        //        return StatusCode(500);
        //    }
        //}
        //public async Task<IActionResult> Factor()
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //        if (user == null) { return NotFound(); }

        //        var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
        //        if (buyer == null) { return NotFound(); }

        //        var userCart = _context.carts.Where(c => c.buyerId == buyer.id).ToList();
        //        var buyerCart = _context.carts.Where(c => c.buyerId == buyer.id)
        //            .Include(p => p.product)
        //            .Select(i => new CartViewModel
        //            {
        //                productId = i.productId,
        //                productName = i.product.Name,
        //                productPrice = i.product.Price,
        //                selectedNumberOfProducts = i.Number.ToString()
        //            }
        //            ).ToList();
        //        var factor = new FactorViewModel
        //        {
        //            Address = user.Address,
        //            Telphone = user.PhoneNumber,
        //            cartItems = buyerCart
        //        };
        //        _context.RemoveRange(userCart);
        //        _context.SaveChanges();
        //        return View(factor);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Catched Error: {e.Message}");
        //        return StatusCode(500);
        //    }
        //}

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> Delete(int itemId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) { return NotFound(); }

                var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
                if (buyer == null) { return NotFound(); }

                var deleteItem = _context.carts.Where(c => c.Id == itemId).SingleOrDefault();
                if (deleteItem == null) { return NotFound(); }

                _context.carts.Remove(deleteItem);
                _context.SaveChanges();
                return RedirectToAction("userCart", "userCart");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
    }
}
