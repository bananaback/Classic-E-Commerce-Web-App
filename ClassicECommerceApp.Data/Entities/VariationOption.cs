namespace ClassicECommerceApp.Data.Entities
{
	public class VariationOption
	{
		public Guid Id { get; set; }
		public Variation Variation { get; set; } = null!;
		public string Value { get; set; } = string.Empty;
		public List<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
		public List<ProductConfiguration> ProductConfigurations { get; set; } = new List<ProductConfiguration>();
	}
}
