using DrinkShop.Models.Entities;

namespace DrinkShop.Models.ViewModel
{
    public class EditSubGroupViewModel
    {
        public EditSubGroupViewModel()
        {
            groups = new List<Group>();
        }
        public int subGroupId { get; set; }
        public string subgroupName { get; set; }
        public int parentGroupId { get; set; }       
        public List<Group> groups { get; set; }
    }
}
