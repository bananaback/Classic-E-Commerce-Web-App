namespace ClassicECommerceApp.Data.Entities
{
	public class OrderLine
	{
		public Guid Id { get; set; }
		public ProductItem ProductItem { get; set; } = null!;
		public ShopOrder ShopOrder { get; set; } = null!;
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public List<UserReview> UserReviews { get; set; } = new List<UserReview>();
	}
}
