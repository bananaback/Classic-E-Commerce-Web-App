using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class VariationEntityTypeConfiguration : IEntityTypeConfiguration<Variation>
	{
		public void Configure(EntityTypeBuilder<Variation> builder)
		{
			builder.ToTable("variation");

			builder.HasKey(v => v.Id);

			builder.Property(v => v.Id)
				.HasColumnName("id");

			builder.Property(v => v.Name)
				.HasColumnName("name");

			builder.Property<Guid>("CategoryId")
				.HasColumnName("category_id");

			builder.HasMany(v => v.VariationOptions)
				.WithOne(vo => vo.Variation)
				.HasForeignKey("VariationId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
