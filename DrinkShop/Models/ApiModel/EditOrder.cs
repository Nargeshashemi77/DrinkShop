using System.ComponentModel.DataAnnotations;

namespace DrinkShop.Models.ApiModel
{
    public class EditOrder
    {

        public int OrderId { get; set; }

        [Required(ErrorMessage = "تعداد الزامی است")]
        [Range(2, int.MaxValue, ErrorMessage = "تعداد باید بیشتر از 1 باشد")]
        public int number { get; set; }
    }
}
