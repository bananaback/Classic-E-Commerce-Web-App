using ClassicECommerceApp.Web.Models.ViewModels;

namespace ClassicECommerceApp.Web.Services.Application.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductViewModel>> GetFeaturedProductsAsync();
        Task<List<ProductViewModel>> GetNewProductsAsync();
        Task<List<ProductViewModel>> GetBestSoldProductsAsync();
    }
}
