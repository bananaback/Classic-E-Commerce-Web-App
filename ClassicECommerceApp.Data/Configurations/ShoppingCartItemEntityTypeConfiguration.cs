using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class ShoppingCartItemEntityTypeConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
	{
		public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
		{
			builder.ToTable("shopping_cart_item");

			builder.HasKey(sci => sci.Id);

			builder.Property(sci => sci.Id)
					.HasColumnName("id");

			builder.Property<Guid>("ShoppingCartId")
					.HasColumnName("cart_id");

			builder.Property<Guid>("ProductItemId")
					.HasColumnName("product_item_id");

			builder.Property(sci => sci.Quantity)
					.HasColumnName("qty");

		}
	}
}
