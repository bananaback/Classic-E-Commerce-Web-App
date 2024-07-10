namespace ClassicECommerceApp.Data.Entities
{
	public class ShoppingCartItem
	{
		public Guid Id { get; set; }
		public ShoppingCart ShoppingCart { get; set; } = null!;
		public ProductItem ProductItem { get; set; } = null!;
		public int Quantity { get; set; }
	}
}
