using DrinkShop.Models;

namespace DrinkShop.Shared
{
    public static class Filter
    {
        public static IQueryable<Product> ApplySort(IQueryable<Product> products, string sort)
        {
            if (string.IsNullOrEmpty(sort))
                return products;

            return sort switch
            {
                "Newest" => products.OrderByDescending(p => p.registerDate),
                "Oldest" => products.OrderBy(p => p.registerDate),
                "ExpensiveToCheap" => products.OrderByDescending(p => p.Price),
                "CheapToExpensive" => products.OrderBy(p => p.Price),
                "AlphabetAscending" => products.OrderBy(p => p.Name),
                "AlphabetDescending" => products.OrderByDescending(p => p.Name),
                _ => products
            };
        }
    }
}
