using ClassicECommerceApp.Web.Models.ViewModels;

namespace ClassicECommerceApp.Web.Services.Application.ProductServices
{
	public class MockProductService : IProductService
	{
		public Task<List<ProductViewModel>> GetBestSoldProductsAsync()
		{
			var result = new List<ProductViewModel>
			{
				new ProductViewModel
				{
					Name = "Green Apple",
					Description = "An apple a day keep the doctor away.",
					Price = 1,
					ImageUrls = new List<string> { "https://res.cloudinary.com/john-mantas/image/upload/v1537291846/codepen/delicious-apples/green-apple-with-slice.png" }
				},
				new ProductViewModel
				{
					Name = "Green Apple",
					Description = "An apple a day keep the doctor away.",
					Price = 1,
					ImageUrls = new List<string> { "https://res.cloudinary.com/john-mantas/image/upload/v1537291846/codepen/delicious-apples/green-apple-with-slice.png" }
				},
				new ProductViewModel
				{
					Name = "Green Apple",
					Description = "An apple a day keep the doctor away.",
					Price = 1,
					ImageUrls = new List<string> { "https://res.cloudinary.com/john-mantas/image/upload/v1537291846/codepen/delicious-apples/green-apple-with-slice.png" }
				},

			};
			return Task.FromResult(result);
		}

		public Task<List<ProductViewModel>> GetFeaturedProductsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<List<ProductViewModel>> GetNewProductsAsync()
		{
			throw new NotImplementedException();
		}
	}
}
