using System.ComponentModel.DataAnnotations;

namespace DrinkShop.Models.ApiModel.Cart
{
    public class AddInputQuery
    {
        [Required]
        public int productId { get; set; }

        [Required(ErrorMessage = "تعداد الزامی است")]
        [Range(1, 200, ErrorMessage = "تعداد باید بین 1 تا 200 باشد")]
        public int number { get; set; }
    }
}
