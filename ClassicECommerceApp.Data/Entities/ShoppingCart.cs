namespace ClassicECommerceApp.Data.Entities
{
	public class ShoppingCart
	{
		public Guid Id { get; set; }
		public ApplicationUser? User { get; set; }
		public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
	}
}
