using DrinkShop.Models.Dtos;
using DrinkShop.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrinkShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        public readonly DbSet<Product> products;
        public ProductRepository(DbSet<Product> products)
        {
            this.products = products;
        }
        public async Task<List<Product>> GetAll(PagedListQuery pagedListQuery, CancellationToken cancellationToken)
        {
            int skip = (pagedListQuery.PageNumber - 1) * pagedListQuery.PageSize;

            return await products.AsNoTracking()
                 .Where(p => p.IsDelete == false)
                 .Skip(skip)
                 .Take(pagedListQuery.PageSize)
                 .ToListAsync();
        }
    }
}
