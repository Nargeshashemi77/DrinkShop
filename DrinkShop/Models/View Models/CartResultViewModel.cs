using DrinkShop.Enum;

namespace DrinkShop.Models.View_Models
{
    public class CartResultViewModel
    {
        public int CartId { get; set; }   // 👈 مهم

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
