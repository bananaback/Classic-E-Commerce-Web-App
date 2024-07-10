using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class ProductConfigurationEntityTypeConfiguration : IEntityTypeConfiguration<ProductConfiguration>
	{
		public void Configure(EntityTypeBuilder<ProductConfiguration> builder)
		{
			builder.ToTable("product_configuration");
		}
	}
}
