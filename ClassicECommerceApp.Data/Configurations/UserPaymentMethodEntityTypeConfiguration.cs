using ClassicECommerceApp.Data.Entities;
using ClassicECommerceApp.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class UserPaymentMethodEntityTypeConfiguration : IEntityTypeConfiguration<UserPaymentMethod>
	{
		public void Configure(EntityTypeBuilder<UserPaymentMethod> builder)
		{
			builder.ToTable("user_payment_method");

			builder.HasKey(upm => upm.Id);

			builder.Property(upm => upm.Id)
					.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
					.HasColumnName("user_id");

			builder.Property<Guid>("PaymentTypeId")
					.HasColumnName("payment_type_id");

			builder.Property(upm => upm.Provider)
					.HasColumnName("provider")
					.HasConversion(
						v => v.ToString(),
						v => (PaymentProviderEnum)Enum.Parse(typeof(PaymentProviderEnum), v)
					);

			builder.Property(upm => upm.AccountNumber)
					.HasColumnName("account_number");

			builder.Property(upm => upm.ExpiryDate)
					.HasColumnName("expiry_date");

			builder.Property(upm => upm.IsDefault)
					.HasColumnName("is_default");

			builder.HasMany(upm => upm.ShopOrders)
					.WithOne(so => so.PaymentMethod)
					.HasForeignKey("UserPaymentMethodId")
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();
		}
	}
}
