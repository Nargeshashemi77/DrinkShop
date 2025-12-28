namespace DrinkShop.Models.Dtos
{
    public class PagedListQuery
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
