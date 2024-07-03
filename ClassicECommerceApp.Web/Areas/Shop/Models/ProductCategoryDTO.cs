namespace ClassicECommerceApp.Web.Areas.Shop.Models
{
    public class ProductCategoryDTO
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
