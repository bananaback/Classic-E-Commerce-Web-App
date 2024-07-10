using ClassicECommerceApp.Data.Enums;

namespace ClassicECommerceApp.Data.Entities
{
	public class OrderStatus
	{
		public Guid Id { get; set; }
		public OrderStatusEnum Status { get; set; }
		public List<ShopOrder> ShopOrders { get; set; } = new List<ShopOrder>();
	}
}
