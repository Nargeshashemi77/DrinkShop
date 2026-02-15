using DrinkShop.Models;

namespace DrinkShop.Models.View_Models
{
    public class UserProfileViewModel
    {
        public User user { get; set; }
        public IFormFile userImage { get; set; }
    }
}
