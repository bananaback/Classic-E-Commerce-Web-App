using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class CountryEntityTypeConfiguration : IEntityTypeConfiguration<Country>
	{
		public void Configure(EntityTypeBuilder<Country> builder)
		{
			builder.ToTable("country");

			builder
				.HasKey(c => c.Id);

			builder.Property(c => c.Id)
				.HasColumnName("id");

			builder.Property(c => c.CountryName)
				.HasColumnName("country_name");

			builder.HasMany(c => c.Addresses)
				.WithOne(a => a.Country)
				.HasForeignKey("CountryId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
