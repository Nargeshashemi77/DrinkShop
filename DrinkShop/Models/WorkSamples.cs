using System.ComponentModel.DataAnnotations;

namespace DrinkShop.Models
{
    public class WorkSamples
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? editedAt { get; set; }

        public int? groupId { get; set; }
        public Group group { get; set; }
    }
}
