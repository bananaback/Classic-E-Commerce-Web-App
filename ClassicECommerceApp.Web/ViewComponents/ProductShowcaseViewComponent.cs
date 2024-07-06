using ClassicECommerceApp.Web.Models.ViewModels;
using ClassicECommerceApp.Web.Services.Application.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace ClassicECommerceApp.Web.ViewComponents
{
    public class ProductShowcaseViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductShowcaseViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string title)
        {
            if (title == "Best sold products")
            {
                var products = await _productService.GetBestSoldProductsAsync();
                var viewModel = new ProductShowcaseViewModel
                {
                    Title = title,
                    Products = products
                };
                return View(viewModel);
            }
            else
            {
                var products = await _productService.GetBestSoldProductsAsync();
                var viewModel = new ProductShowcaseViewModel
                {
                    Title = title,
                    Products = products
                };
                return View(viewModel);
            }
        }
    }
}
