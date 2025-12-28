using DrinkShop.Models.Dtos;
using DrinkShop.Models.Entities;

namespace DrinkShop.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll(PagedListQuery pagedListQuery, CancellationToken cancellationToken);
    }
}
