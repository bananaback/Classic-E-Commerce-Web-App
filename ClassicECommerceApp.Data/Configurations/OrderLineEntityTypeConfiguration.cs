using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class OrderLineEntityTypeConfiguration : IEntityTypeConfiguration<OrderLine>
	{
		public void Configure(EntityTypeBuilder<OrderLine> builder)
		{
			builder.ToTable("order_line");

			builder.HasKey(ol => ol.Id);

			builder.Property(ol => ol.Id)
					.HasColumnName("id");

			builder.Property<Guid>("ProductItemId")
					.HasColumnName("product_item_id");

			builder.Property<Guid>("ShopOrderId")
					.HasColumnName("order_id");

			builder.Property(ol => ol.Quantity)
					.HasColumnName("qty");

			builder.Property(ol => ol.Price)
					.HasColumnName("price")
					.HasColumnType("decimal(18,2)");

			builder.HasMany(ol => ol.UserReviews)
					.WithOne(ur => ur.OrderedProduct)
					.HasForeignKey("OrderLineId")
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();
		}
	}
}
