using ClassicECommerceApp.Data.Enums;

namespace ClassicECommerceApp.Data.Entities
{
	public class PaymentType
	{
		public Guid Id { get; set; }
		public PaymentTypeEnum Value { get; set; }
		public List<UserPaymentMethod> PaymentMethods { get; set; } = new List<UserPaymentMethod>();
	}
}
