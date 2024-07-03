using ClassicECommerceApp.Data.Entities;
using ClassicECommerceApp.Data.Repositories.ProductCategoryRepositories;
using ClassicECommerceApp.Web.Areas.Shop.Models;

namespace ClassicECommerceApp.Web.Services.Application.CategoryServices
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }
        public async Task<List<ProductCategoryDTO>> getCategories()
        {
            List<ProductCategory> productCategories = await _productCategoryRepository.GetAllAsync();
            List<ProductCategoryDTO> productCategoryDTOs = productCategories.Select(pc => new ProductCategoryDTO
            {
                Id = pc.ProductCategoryId,
                ParentId = pc.ParentCategoryId,
                Name = pc.CategoryName
            }).ToList();

            return productCategoryDTOs;
        }
    }
}
