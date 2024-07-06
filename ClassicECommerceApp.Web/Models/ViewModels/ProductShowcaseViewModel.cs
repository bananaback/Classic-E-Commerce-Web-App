namespace ClassicECommerceApp.Web.Models.ViewModels
{
    public class ProductShowcaseViewModel
    {
        public string Title { get; set; } = string.Empty;
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    }
}
