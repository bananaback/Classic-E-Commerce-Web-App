namespace ClassicECommerceApp.Data.Entities
{
	public class Country
	{
		public Guid Id { get; set; }
		public string CountryName { get; set; } = string.Empty;

		public List<Address> Addresses { get; set; } = new List<Address>();
	}
}
