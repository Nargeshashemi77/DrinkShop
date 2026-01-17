using DrinkShop.Data;
using DrinkShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrinkShop.Components
{
    public class ShowProductByGroupComponent : ViewComponent
    {
        private DrinkShopDbContext _context;
        public ShowProductByGroupComponent(DrinkShopDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(List<Product> products)
        {
            return View("~/Views/Component/ShowProductByGroup.cshtml", products);
        }
    }
}
