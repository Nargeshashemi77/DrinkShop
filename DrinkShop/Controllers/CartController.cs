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
using DrinkShop.Enum;

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
				.Include(p => p.product).ToListAsync();

			var result = userCart.Select(c =>
			{
				int finalPrice = CalculateFinalPrice(c.product);

				return new CartResultViewModel
				{
					CartId = c.Id,
					ProductId = c.productId,
					ProductName = c.product.Name,
					ProductImage = c.product.productImage,
					UnitPrice = finalPrice,
					Quantity = c.Number,
					TotalPrice = finalPrice * c.Number
				};
			}).ToList();

			return View(result);
		}

		[HttpPut("items/{itemId}")]
		public async Task<IActionResult> Update(int itemId, [FromBody] UpdateInputQuery model)
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

				return Ok(new { message = "Update successfully" });
			}
			catch (Exception e)
			{
				Console.WriteLine($"Catched Error: {e.Message}");
				return StatusCode(500);
			}
		}

		[HttpPost("checkout")]
		public async Task<IActionResult> Checkout()
		{
			using var transaction = await _context.Database.BeginTransactionAsync();

			try
			{
				var user = await _userManager.FindByNameAsync(User.Identity?.Name);
				if (user == null)
					return Unauthorized();

				var buyer = await _context.buyers
					.FirstOrDefaultAsync(b => b.userId == user.Id);

				if (buyer == null)
					return BadRequest(new { message = "Buyer profile not found" });

				var cartItems = await _context.carts
					.Include(c => c.product)
					.Where(c => c.buyerId == buyer.id)
					.ToListAsync();

				if (!cartItems.Any())
					return BadRequest(new { message = "Cart is empty" });

				foreach (var item in cartItems)
				{
					if (item.Number > item.product.Stock)
					{
						return BadRequest(new
						{
							message = $"Product '{item.product.Name}' does not have enough stock"
						});
					}
				}

				var order = new Order
				{
					buyerId = buyer.id,
					createdAt = DateTime.UtcNow,
					Status = OrderStatus.Pending,
					TotalPrice = 0
				};

				await _context.orders.AddAsync(order);
				await _context.SaveChangesAsync();

				int totalPrice = 0;

				foreach (var item in cartItems)
				{
					var product = item.product;

					int finalPrice = CalculateFinalPrice(product);

					var orderItem = new OrderItems
					{
						OrderId = order.Id,
						ProductId = product.id,
						Quantity = item.Number,
						OriginalPrice = product.Price,
						UnitPrice = finalPrice,
						TotalPrice = finalPrice * item.Number
					};

					await _context.orderItems.AddAsync(orderItem);

					totalPrice += orderItem.TotalPrice;

					product.Stock -= item.Number;
				}

				order.TotalPrice = totalPrice;

				_context.carts.RemoveRange(cartItems);

				await _context.SaveChangesAsync();
				await transaction.CommitAsync();

				return RedirectToAction("Invoice", "Cart", new { orderId = order.Id });
			}
			catch (Exception e)
			{
				await transaction.RollbackAsync();
				Console.WriteLine($"Checkout Error: {e.Message}");
				return StatusCode(500);
			}
		}

		[HttpGet("Invoice")]
		public async Task<IActionResult> Invoice(int orderId)
		{
			try
			{
				var order = await _context.orders
				.Include(o => o.orderItems)
				.ThenInclude(oi => oi.Product)
				.FirstOrDefaultAsync(o => o.Id == orderId);

				if (order == null)
					return NotFound();

				return View(order);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Catched Error: {e.Message}");
				return StatusCode(500);
			}
		}

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

		private int CalculateFinalPrice(Product product)
		{
			if (product.Discount > 0)
			{
				return product.Price - product.Discount;
			}

			return product.Price;
		}
	}
}
