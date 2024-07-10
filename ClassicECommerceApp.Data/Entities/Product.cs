namespace ClassicECommerceApp.Data.Entities
{
	public class Product
	{
		public Guid Id { get; set; }
		public ProductCategory Category { get; set; } = null!;
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal AverageRating { get; set; }
		public List<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
	}
}
