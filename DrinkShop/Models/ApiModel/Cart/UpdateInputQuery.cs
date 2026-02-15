using System.ComponentModel.DataAnnotations;

namespace DrinkShop.Models.ApiModel
{
    public class UpdateInputQuery
    {
        [Required(ErrorMessage = "تعداد الزامی است")]
        [Range(1, 200, ErrorMessage = "تعداد باید بین 1 تا 200 باشد")]
        public int number { get; set; }
    }
}
