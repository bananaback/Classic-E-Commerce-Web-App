namespace ClassicECommerceApp.Data.Entities
{
	public class UserReview
	{
		public Guid Id { get; set; }
		public ApplicationUser? User { get; set; }
		public OrderLine OrderedProduct { get; set; } = null!;
		public decimal RatingValue { get; set; }
		public string Comment { get; set; } = string.Empty;
	}
}
