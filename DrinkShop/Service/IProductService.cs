using DrinkShop.Models.Dtos;
using DrinkShop.Models.Entities;

namespace DrinkShop.Service
{
    public interface IProductService
    {
        /// <summary>
        /// دریافت همه محصولات
        /// </summary>
        /// <param name="pagedListQuery"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedListResult<Product>> GetAllProducts(PagedListQuery pagedListQuery, CancellationToken cancellationToken);
    }
}
