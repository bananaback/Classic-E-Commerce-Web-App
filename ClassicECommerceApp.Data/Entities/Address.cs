namespace ClassicECommerceApp.Data.Entities
{
	public class Address
	{
		public Guid Id { get; set; }
		public string UnitNumber { get; set; } = string.Empty;
		public string StreetNumber { get; set; } = string.Empty;
		public string AddressLine1 { get; set; } = string.Empty;
		public string AddressLine2 { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string Region { get; set; } = string.Empty;
		public string PostalCode { get; set; } = string.Empty;
		public Country Country { get; set; } = null!;
		public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
		public List<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
	}
}
