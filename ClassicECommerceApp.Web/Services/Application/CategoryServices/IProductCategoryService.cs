using ClassicECommerceApp.Web.Areas.Shop.Models;

namespace ClassicECommerceApp.Web.Services.Application.CategoryServices
{
    public interface IProductCategoryService
    {
        Task<List<ProductCategoryDTO>> getCategories();
    }
}
