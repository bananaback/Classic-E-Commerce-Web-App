using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
	{
		public void Configure(EntityTypeBuilder<Address> builder)
		{
			builder.ToTable("address");

			builder.HasKey(a => a.Id);

			builder.Property(a => a.UnitNumber)
				.HasColumnName("unit_number");

			builder.Property(a => a.StreetNumber)
				.HasColumnName("street_number");

			builder.Property(a => a.AddressLine1)
				.HasColumnName("address_line1");

			builder.Property(a => a.AddressLine2)
				.HasColumnName("address_line2");

			builder.Property(a => a.City)
				.HasColumnName("city");

			builder.Property(a => a.Region)
				.HasColumnName("region");

			builder.Property(a => a.PostalCode)
				.HasColumnName("postal_code");

			builder.Property<Guid>("CountryId")
				.HasColumnName("country_id");

		}
	}
}
