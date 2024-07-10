using ClassicECommerceApp.Data.Entities;
using ClassicECommerceApp.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class OrderStatusEntityTypeConfiguration : IEntityTypeConfiguration<OrderStatus>
	{
		public void Configure(EntityTypeBuilder<OrderStatus> builder)
		{
			builder.ToTable("order_status");

			builder.HasKey(os => os.Id);

			builder.Property(os => os.Id)
					.HasColumnName("id");

			builder.Property(os => os.Status)
					.HasColumnName("status");

			builder.Property(os => os.Status)
					.HasConversion(
						v => v.ToString(),
						v => (OrderStatusEnum)Enum.Parse(typeof(OrderStatusEnum), v)
					);

			builder.HasMany(os => os.ShopOrders)
					.WithOne(so => so.OrderStatus)
					.HasForeignKey("OrderStatusId")
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();
		}
	}
}
