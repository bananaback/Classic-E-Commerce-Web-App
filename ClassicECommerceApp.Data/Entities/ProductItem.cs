namespace ClassicECommerceApp.Data.Entities
{
	public class ProductItem
	{
		public Guid Id { get; set; }
		public Product Product { get; set; } = null!;
		public string SKU { get; set; } = string.Empty;
		public int QuantityInStock { get; set; }
		public decimal Price { get; set; }
		public List<VariationOption> VariationOptions { get; set; } = new List<VariationOption>();
		public List<ProductConfiguration> ProductConfigurations { get; set; } = new List<ProductConfiguration>();
		public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
		public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
	}
}
