namespace DrinkShop.Models.View_Models
{
    public class BuyerListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Email { get; set; }
        public int SuccessfullOrders { get; set; }
        public int CanceledOrder { get; set; }

    }
}
