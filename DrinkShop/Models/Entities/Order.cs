using DrinkShop.Enum;

namespace DrinkShop.Models
{
    public class Order
    {

        public int Id { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime createdAt { get; set; }

        public int Number { get; set; }

        //Navigation Property
        public int productId { get; set; }
        public Product product { get; set; }
        public int buyerId { get; set; }
        public Buyer buyer { get; set; }
    }
}
