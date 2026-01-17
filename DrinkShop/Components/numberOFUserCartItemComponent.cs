using DrinkShop.Data;
using DrinkShop.Enum;
using DrinkShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DrinkShop.Components
{
    public class numberOFUserCartItemComponent : ViewComponent
    {
        private DrinkShopDbContext _context;
        private UserManager<User> _userManager;
        public numberOFUserCartItemComponent(DrinkShopDbContext context, UserManager<User> userManage)
        {
            _context = context;
            _userManager = userManage;
        }
        public async Task<IViewComponentResult> InvokeAsync(string ForWhere)
        {
            string viewAdress = "~/Views/Component/numberOfUserCartItem.cshtml";
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                ViewData["NumberOfCartItem"] = 0;
                ViewData["ForWhere"] = ForWhere;
                return View(viewAdress);
            }
            var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
            if (buyer == null)
            {
                ViewData["NumberOfCartItem"] = 0;
                ViewData["ForWhere"] = ForWhere;
                return View(viewAdress);
            }
            int userCart = _context.orders.Where(o => o.buyerId == buyer.id && (o.Status == OrderStatus.Pending || o.Status == OrderStatus.reffered || o.Status == OrderStatus.doing)).Count();
            ViewData["NumberOfCartItem"] = userCart;
            ViewData["ForWhere"] = ForWhere;
            return View(viewAdress);
        }
    }
}
