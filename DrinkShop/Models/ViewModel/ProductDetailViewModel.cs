using DrinkShop.Models.Entities;

namespace DrinkShop.Models.ViewModel
{
    public class ProductDetailViewModel
    {
        public ProductDetailViewModel()
        {
            comments= new List<Comment>();
        }
        public Product product { get; set; }
        public List<Comment> comments { get; set; }
    }
}
