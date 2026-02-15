using System.ComponentModel.DataAnnotations;

namespace DrinkShop.Models
{
    public class Product
    {
        public Product()
        {
            orders = new List<Order>();
        }
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Summary { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string Unit { get; set; }
        public int Stock { get; set; }
        public DateTime registerDate { get; set; }
        public bool IsDelete { get; set; }
        public int? groupId { get; set; }
        public int? subGroupId { get; set; }
        public string productImage { get; set; }
        public Group group { get; set; }
        public SubGroup subGroup { get; set; }
        public ICollection<Order> orders { get; set; }
    }
}
