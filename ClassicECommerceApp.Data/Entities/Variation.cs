namespace ClassicECommerceApp.Data.Entities
{
	public class Variation
	{
		public Guid Id { get; set; }
		public ProductCategory Category { get; set; } = null!;
		public string Name { get; set; } = string.Empty;
		public List<VariationOption> VariationOptions { get; set; } = new List<VariationOption>();
	}
}
