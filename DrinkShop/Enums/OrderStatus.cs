using System.ComponentModel.DataAnnotations;

namespace DrinkShop.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "در حال پردازش")]
        Pending,

        [Display(Name = "در حال آماده سازی")]
        preparation,

        [Display(Name = "در حال ارسال")]
        Sending,

        [Display(Name = "لغو شده")]
        canceled,

        [Display(Name = "تکمیل شده")]
        finished
    }
}
