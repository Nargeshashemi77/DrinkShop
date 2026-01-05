using DrinkShop.Models.Entities;

namespace DrinkShop.Models.ViewModel
{
    public class GroupWithSubGroupsWithProductsViewModel
    {
        public GroupWithSubGroupsWithProductsViewModel()
        {
            subGroups= new List<SubGroup>();
        }
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int productNumber { get; set; }
        public List<SubGroup> subGroups { get; set; }
    }
}
