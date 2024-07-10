using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class ShopOrderEntityTypeConfiguration : IEntityTypeConfiguration<ShopOrder>
	{
		public void Configure(EntityTypeBuilder<ShopOrder> builder)
		{
			builder.ToTable("shop_order");

			builder.HasKey(so => so.Id);

			builder.Property(so => so.Id)
					.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
					.HasColumnName("user_id");

			builder.Property(so => so.OrderDate)
					.HasColumnName("order_date");

			builder.Property<Guid>("UserPaymentMethodId")
					.HasColumnName("payment_method_id");

			builder.Property<Guid>("AddressId")
					.HasColumnName("shipping_address");

			builder.Property<Guid>("ShippingMethodId")
					.HasColumnName("shipping_method");

			builder.Property(so => so.OrderTotal)
					.HasColumnName("order_total")
					.HasColumnType("decimal(18,2)");

			builder.Property<Guid>("OrderStatusId")
					.HasColumnName("order_status");

			builder.HasMany(so => so.OrderLines)
					.WithOne(ol => ol.ShopOrder)
					.HasForeignKey("ShopOrderId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
		}
	}
}
