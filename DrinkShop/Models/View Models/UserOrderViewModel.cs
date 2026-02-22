using DrinkShop.Enums;
using DrinkShop.Shared;

namespace DrinkShop.Models.View_Models
{
    public class UserOrderViewModel
    {
        public int Id { get; set; }

        public int TotalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string StatusDescription => Status.GetDisplayName();
        public DateTime createdAt { get; set; }
    }
}
