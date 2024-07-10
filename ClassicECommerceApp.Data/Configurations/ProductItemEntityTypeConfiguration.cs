using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class ProductItemEntityTypeConfiguration : IEntityTypeConfiguration<ProductItem>
	{
		public void Configure(EntityTypeBuilder<ProductItem> builder)
		{
			builder.ToTable("product_item");

			builder.HasKey(pi => pi.Id);

			builder.Property(pi => pi.Id)
				.HasColumnName("id");

			builder.Property(pi => pi.SKU)
				.HasColumnName("SKU");

			builder.Property(pi => pi.QuantityInStock)
				.HasColumnName("quantity_in_stock");

			builder.Property(pi => pi.Price)
				.HasColumnName("price")
				.HasColumnType("decimal(18,2)");

			builder.Property<Guid>("ProductId")
				.HasColumnName("product_id");

			builder.HasMany(pi => pi.VariationOptions)
				.WithMany(vo => vo.ProductItems)
				.UsingEntity<ProductConfiguration>(
					j => j.ToTable("product_configuration")
							.HasOne(pc => pc.VariationOption)
							.WithMany(vo => vo.ProductConfigurations)
							.HasForeignKey("VariationOptionId")
							.OnDelete(DeleteBehavior.Restrict),
					j => j.HasOne(pc => pc.ProductItem)
							.WithMany(pi => pi.ProductConfigurations)
							.HasForeignKey("ProductItemId")
							.OnDelete(DeleteBehavior.Restrict),
					j =>
					{
						j.Property<Guid>("ProductItemId").HasColumnName("product_item_id");
						j.Property<Guid>("VariationOptionId").HasColumnName("variation_option_id");
						j.HasKey("ProductItemId", "VariationOptionId");
					}
			);

			builder.HasMany(pi => pi.ShoppingCartItems)
					.WithOne(sci => sci.ProductItem)
					.HasForeignKey("ProductItemId")
					.IsRequired();

			builder.HasMany(pi => pi.OrderLines)
					.WithOne(ol => ol.ProductItem)
					.HasForeignKey("ProductItemId")
					.IsRequired();
		}
	}
}
