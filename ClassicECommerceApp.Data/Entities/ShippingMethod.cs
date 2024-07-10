namespace ClassicECommerceApp.Data.Entities
{
	public class ShippingMethod
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public List<ShopOrder> ShopOrders { get; set; } = new List<ShopOrder>();
	}
}
