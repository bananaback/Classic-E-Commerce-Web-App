using Microsoft.AspNetCore.Identity;

namespace ClassicECommerceApp.Data.Entities
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public List<Address> Addresses { get; set; } = new List<Address>();
		public List<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
		public ShoppingCart? ShoppingCart { get; set; }
		public List<UserPaymentMethod> PaymentMethods { get; set; } = new List<UserPaymentMethod>();
		public List<UserReview> UserReviews { get; set; } = new List<UserReview>();
	}
}
