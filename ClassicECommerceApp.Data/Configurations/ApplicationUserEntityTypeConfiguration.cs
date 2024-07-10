using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder
				.HasIndex(u => u.Email)
				.IsUnique();

			builder.HasMany(au => au.Addresses)
				.WithMany(a => a.Users)
				.UsingEntity<UserAddress>(
					j => j.ToTable("user_address")
						  .HasOne(ua => ua.Address)
						  .WithMany(a => a.UserAddresses)
						  .HasForeignKey("AddressId")
						  .OnDelete(DeleteBehavior.Cascade),
					j => j.HasOne(ua => ua.ApplicationUser)
						  .WithMany(au => au.UserAddresses)
						  .HasForeignKey("ApplicationUserId")
						  .OnDelete(DeleteBehavior.Cascade),
					j =>
					{
						j.ToTable("user_address");
						j.Property(ua => ua.IsDefault).HasColumnName("is_default");
						j.Property<Guid>("ApplicationUserId").HasColumnName("user_id");
						j.Property<Guid>("AddressId").HasColumnName("address_id");
						j.HasKey("ApplicationUserId", "AddressId");
					}
				);

			builder.HasOne(au => au.ShoppingCart)
					.WithOne(sc => sc.User)
					.HasForeignKey<ShoppingCart>("ApplicationUserId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired(false);

			builder.HasMany(au => au.PaymentMethods)
					.WithOne(pm => pm.User)
					.HasForeignKey("ApplicationUserId")
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();

			builder.HasMany(au => au.UserReviews)
					.WithOne(ur => ur.User)
					.HasForeignKey("ApplicationUserId")
					.OnDelete(DeleteBehavior.SetNull)
					.IsRequired(false);
		}
	}
}
