using DrinkShop.Models.Entities;

namespace DrinkShop.Models.ViewModel
{
    public class AddProductViewModel
    {
        public AddProductViewModel()
        {
            groupAndSubGroups = new List<GroupAndSubGroupViewModel>();
        }
        public Product product { get; set; }
        public List<GroupAndSubGroupViewModel> groupAndSubGroups { get; set; }
        public IFormFile productImage { get; set; }
    }
}
