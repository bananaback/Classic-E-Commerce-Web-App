using ClassicECommerceApp.Data.Entities;

namespace ClassicECommerceApp.Data.Repositories.ProductCategoryRepositories
{
    public interface IProductCategoryRepository
    {
        Task<List<ProductCategory>> GetAllAsync();
    }
}
