namespace DrinkShop.Models.Dtos
{
    public class PagedListResult<T> where T : class
    {
        /// <summary>
        /// تعداد
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// مقادیر لیست صفحه بندی شده
        /// </summary>
        public List<T> Values { get; set; }
    }
}
