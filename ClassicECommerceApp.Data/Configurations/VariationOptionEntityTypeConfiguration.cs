using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class VariationOptionEntityTypeConfiguration : IEntityTypeConfiguration<VariationOption>
	{
		public void Configure(EntityTypeBuilder<VariationOption> builder)
		{
			builder.ToTable("variation_option");

			builder.HasKey(vo => vo.Id);

			builder.Property(vo => vo.Id)
				.HasColumnName("id");

			builder.Property(vo => vo.Value)
				.HasColumnName("value");

			builder.Property<Guid>("VariationId")
				.HasColumnName("variation_id");
		}
	}
}
