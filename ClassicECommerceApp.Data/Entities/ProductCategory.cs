namespace ClassicECommerceApp.Data.Entities
{
	public class ProductCategory
	{
		public Guid ProductCategoryId { get; set; }
		public Guid? ParentCategoryId { get; set; }
		public string CategoryName { get; set; } = string.Empty;

		public ProductCategory? ParentCategory { get; set; }
		public ICollection<ProductCategory> ChildCategories { get; set; } = new List<ProductCategory>();
	}
}
