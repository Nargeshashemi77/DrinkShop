using DrinkShop.Models.Dtos;
using DrinkShop.Models.Entities;
using DrinkShop.Repository;

namespace DrinkShop.Service
{
    public class ProductService : IProductService
    {
        public readonly IProductRepository productRepository;
        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<PagedListResult<Product>> GetAllProducts(PagedListQuery pagedListQuery, CancellationToken cancellationToken)
        {
            var dbResult = await this.productRepository.GetAll(pagedListQuery, cancellationToken);

            PagedListResult<Product> result = new()
            {
                Count = dbResult.Count,
                Values = dbResult
            };

            return result;
        }
    }
}
