using DrinkShop.Data;
using DrinkShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DrinkShop.Components
{
    public class FavoritesItems : ViewComponent
    {
        private DrinkShopDbContext _context;
        private UserManager<User> _userManager;
        public FavoritesItems(DrinkShopDbContext context, UserManager<User> userManage)
        {
            _context = context;
            _userManager = userManage;
        }
        public async Task<IViewComponentResult> InvokeAsync(string ForWhere)
        {
            string viewAdress = "~/Views/Component/FavoritesItems.cshtml";
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                ViewData["FavoriteItemsNumber"] = 0;
                ViewData["ForWhere"] = ForWhere;
                return View(viewAdress);
            }
            var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
            if (buyer == null)
            {
                ViewData["FavoriteItemsNumber"] = 0;
                ViewData["ForWhere"] = ForWhere;
                return View(viewAdress);
            }
            int ItemNumbers = _context.favorites.Where(f => f.buyerId == buyer.id).Count();
            ViewData["FavoriteItemsNumber"] = ItemNumbers;
            ViewData["ForWhere"] = ForWhere;
            return View(viewAdress);
        }
    }
}
