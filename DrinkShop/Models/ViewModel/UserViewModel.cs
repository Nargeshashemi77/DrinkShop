using DrinkShop.Models.Entities;

namespace DrinkShop.Models.ViewModel
{
    public class UserViewModel
    {
        public User user { get; set; }
        public IFormFile userImage { get; set; }
    }
}
