using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class ShippingMethodEntityTypeConfiguration : IEntityTypeConfiguration<ShippingMethod>
	{
		public void Configure(EntityTypeBuilder<ShippingMethod> builder)
		{
			builder.ToTable("shipping_method");

			builder.HasKey(sm => sm.Id);

			builder.Property(sm => sm.Id)
					.HasColumnName("id");

			builder.Property(sm => sm.Name)
					.HasColumnName("name");

			builder.Property(sm => sm.Price)
					.HasColumnName("price")
					.HasColumnType("decimal(18,2)");

			builder.HasMany(sm => sm.ShopOrders)
					.WithOne(so => so.ShippingMethod)
					.HasForeignKey("ShippingMethodId")
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();
		}
	}
}
