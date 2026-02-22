using DrinkShop.Enums;
using DrinkShop.Shared;

namespace DrinkShop.Models
{
    public class Order
    {

        public int Id { get; set; }

        public int TotalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime createdAt { get; set; }

        //Navigation Property
        public List<OrderItems> orderItems { get; set; }
        public int buyerId { get; set; }
        public Buyer buyer { get; set; }
    }
}
