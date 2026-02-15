using DrinkShop.Models;

namespace DrinkShop.Models.View_Models
{
    public class ProductPageViewModel
    {
        public List<Product> Products { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int Limit { get; set; }
    }
}
