namespace ClassicECommerceApp.Data.Entities
{
	public class ProductCategory
	{
		public Guid ProductCategoryId { get; set; }
		public Guid? ParentCategoryId { get; set; }
		public string CategoryName { get; set; } = string.Empty;

		public ProductCategory? ParentCategory { get; set; }
		public List<ProductCategory> ChildCategories { get; set; } = new List<ProductCategory>();
		public List<Variation> Variations { get; set; } = new List<Variation>();
		public List<Product> Products { get; set; } = new List<Product>();
	}
}
