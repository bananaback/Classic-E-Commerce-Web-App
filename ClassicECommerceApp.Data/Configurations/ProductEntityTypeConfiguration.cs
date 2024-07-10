using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.ToTable("product");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Id)
				.HasColumnName("id");

			builder.Property(p => p.Name)
				.HasColumnName("name");

			builder.Property(p => p.Description)
				.HasColumnName("description");

			builder.Property(p => p.AverageRating)
				.HasColumnName("average_rating")
				.HasColumnType("decimal(18,2)");

			builder.Property<Guid>("CategoryId")
				.HasColumnName("category_id");

			builder.HasMany(p => p.ProductItems)
				.WithOne(pi => pi.Product)
				.HasForeignKey("ProductId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
