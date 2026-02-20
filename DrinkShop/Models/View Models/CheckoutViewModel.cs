namespace DrinkShop.Models.View_Models
{
    public class CheckoutViewModel
    {
        public User UserInfo { get; set; }

        public List<CartResultViewModel> CartItems { get; set; }
    }
}
