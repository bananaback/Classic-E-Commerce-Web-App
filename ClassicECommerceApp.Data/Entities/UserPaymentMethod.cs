using ClassicECommerceApp.Data.Enums;

namespace ClassicECommerceApp.Data.Entities
{
	public class UserPaymentMethod
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public PaymentType PaymentType { get; set; } = null!;
		public PaymentProviderEnum Provider { get; set; }
		public string AccountNumber { get; set; } = string.Empty;
		public DateTime ExpiryDate { get; set; }
		public bool IsDefault { get; set; }
		public List<ShopOrder> ShopOrders { get; set; } = new List<ShopOrder>();
	}
}
