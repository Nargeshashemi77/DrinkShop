namespace DrinkShop.Models.InputQuery
{
    public class GetProductInputQuery
    {
        public int page { get; set; } = 1;
        public int limit { get; set; } = 10;
        public string? sort { get; set; }
        public string? search { get; set; }
        public string? filter { get; set; }
    }
}
