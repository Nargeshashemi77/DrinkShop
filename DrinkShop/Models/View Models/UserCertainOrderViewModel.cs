namespace DrinkShop.Models.View_Models
{
    public class UserCertainOrderViewModel
    {
        public int orderId { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }

        public string productImage { get; set; }
        public int productPrice { get; set; }
        public DateTime orderDatetime { get; set; }

    }
}
