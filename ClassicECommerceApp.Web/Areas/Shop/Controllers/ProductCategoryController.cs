using ClassicECommerceApp.Web.Services.Application.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassicECommerceApp.Web.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/api/shop/productcategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoriesJson()
        {
            return Ok(await _productCategoryService.getCategories());
        }
    }


}
