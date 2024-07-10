using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class UserAddressEntityTypeConfiguration : IEntityTypeConfiguration<UserAddress>
	{
		public void Configure(EntityTypeBuilder<UserAddress> builder)
		{
			builder.ToTable("user_address");
		}
	}
}
