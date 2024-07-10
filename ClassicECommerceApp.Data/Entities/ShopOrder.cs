namespace ClassicECommerceApp.Data.Entities
{
	public class ShopOrder
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public DateTime OrderDate { get; set; }
		public UserPaymentMethod PaymentMethod { get; set; } = null!;
		public Address ShippingAddress { get; set; } = null!;
		public ShippingMethod ShippingMethod { get; set; } = null!;
		public decimal OrderTotal { get; set; }
		public OrderStatus OrderStatus { get; set; } = null!;
		public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
	}
}
