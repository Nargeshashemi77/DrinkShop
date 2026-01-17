using DrinkShop.Data;
using DrinkShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrinkShop.Controllers
{
    [Authorize(Roles ="Buyer,Service,Admin")]
    public class CommentController : Controller
    {
        private DrinkShopDbContext _context;
        public CommentController(DrinkShopDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult addComment(int productId,string userName,string comment)
        {
            try
            {
                Comment newComment = new Comment
                {
                    productId = productId,
                    userName = userName,
                    comment = comment,
                };
                _context.comments.Add(newComment);
                _context.SaveChanges();
                return RedirectToAction("ProductDetails", "Product", new { productId = productId });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
    }
}
