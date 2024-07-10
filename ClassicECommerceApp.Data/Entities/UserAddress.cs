namespace ClassicECommerceApp.Data.Entities
{
	public class UserAddress
	{
		public ApplicationUser ApplicationUser { get; set; } = null!;
		public Address Address { get; set; } = null!;
		public bool IsDefault { get; set; }
	}
}
