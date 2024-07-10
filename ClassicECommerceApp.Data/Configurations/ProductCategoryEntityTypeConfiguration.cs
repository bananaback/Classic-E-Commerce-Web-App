using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class ProductCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ProductCategory>
	{
		public void Configure(EntityTypeBuilder<ProductCategory> builder)
		{
			builder.ToTable("product_category")
				.HasKey(pc => pc.ProductCategoryId);

			builder.Property(pc => pc.ProductCategoryId)
				.HasColumnName("id");

			builder.Property(pc => pc.ParentCategoryId)
				.HasColumnName("parent_category_id");

			builder.Property(pc => pc.CategoryName)
				.HasColumnName("category_name");

			builder.HasOne(pc => pc.ParentCategory)
				.WithMany(pc => pc.ChildCategories)
				.HasForeignKey(pc => pc.ParentCategoryId)
				.OnDelete(DeleteBehavior.Restrict); // Handle set null in application layer instead

			builder.HasMany(pc => pc.Variations)
					.WithOne(v => v.Category)
					.HasForeignKey("CategoryId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();

			builder.HasMany(pc => pc.Products)
					.WithOne(p => p.Category)
					.HasForeignKey("CategoryId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
		}
	}
}
