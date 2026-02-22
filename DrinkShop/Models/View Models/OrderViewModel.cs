



using DrinkShop.Enums;

namespace DrinkShop.Models.View_Models
{
    public class OrderViewModel
    {
        public int orderId { get; set; }
        public string userName { get; set; }
        public DateTime orderDate { get; set; }
        public int orderPrice { get; set; }

        public OrderStatus orderStatus { get; set; }
    }
}
