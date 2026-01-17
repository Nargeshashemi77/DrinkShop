namespace DrinkShop.Models.View_Models
{
    public class OrderDetailViewModel
    {
        public int orderId { get; set; }
        public string orderDescription { get; set; }
        public string userPhone { get; set; }
        public string userFirstName { get; set; }
        public string userLastName { get; set; }
        public string userAddress { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }
        public string productImage { get; set; }
        public int productPrice { get; set; }
        public string productUnit { get; set; }
        public string productUnitOfMeasurement { get; set; }
        public string productSize { get; set; }
        public DateTime orderRegisterDateTime { get; set; }
    }
}
