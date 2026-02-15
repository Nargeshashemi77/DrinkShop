namespace DrinkShop.Models
{
    public class Group
    {
        public Group()
        {
            subGroups = new List<SubGroup>();
            product = new List<Product>();
        }
        public int id { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }

        //navigation property
        public ICollection<Product> product { get; set; }
        public ICollection<SubGroup> subGroups { get; set; }
    }
}
