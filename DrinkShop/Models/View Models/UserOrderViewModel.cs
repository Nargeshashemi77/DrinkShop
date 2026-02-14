

using DrinkShop.Enum;

namespace DrinkShop.Models.View_Models
{
    public class UserOrderViewModel
    {
        public int orderId { get; set; }
        public string orderDescription { get; set; }
        public int productId { get; set; }      
        public string productName { get; set; }
        public string productImage { get; set; }
        public int productPrice { get; set; }

        public int number { get; set; }
        public DateTime registerDateTime { get; set; }
        public OrderStatus OrderStat { get; set; }
    }
}
