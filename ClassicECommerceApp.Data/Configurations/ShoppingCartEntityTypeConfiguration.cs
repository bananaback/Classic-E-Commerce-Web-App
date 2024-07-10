using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class ShoppingCartEntityTypeConfiguration : IEntityTypeConfiguration<ShoppingCart>
	{
		public void Configure(EntityTypeBuilder<ShoppingCart> builder)
		{
			builder.ToTable("shopping_cart");

			builder.HasKey(sc => sc.Id);

			builder.Property(sc => sc.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.HasMany(sc => sc.ShoppingCartItems)
					.WithOne(sci => sci.ShoppingCart)
					.HasForeignKey("ShoppingCartId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
		}
	}
}
