namespace DrinkShop.Models
{
    public class Favorite
    {
        public int productId { get; set; }
        public int buyerId { get; set; }
        public Product product { get; set; }
        public Buyer buyer { get; set; }
    }
}
