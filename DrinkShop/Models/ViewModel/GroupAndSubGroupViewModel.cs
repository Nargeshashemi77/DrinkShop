using DrinkShop.Models.Entities;

namespace DrinkShop.Models.ViewModel
{
    public class GroupAndSubGroupViewModel
    {
        public GroupAndSubGroupViewModel()
        {
            subGroops = new List<SubGroup>();
        }
        public int groupId { get; set; }
        public string groupName { get; set; }
        public List<SubGroup> subGroops { get; set; }
    }
}
