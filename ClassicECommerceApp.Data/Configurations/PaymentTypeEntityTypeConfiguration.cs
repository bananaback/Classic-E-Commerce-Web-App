using ClassicECommerceApp.Data.Entities;
using ClassicECommerceApp.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class PaymentTypeEntityTypeConfiguration : IEntityTypeConfiguration<PaymentType>
	{
		public void Configure(EntityTypeBuilder<PaymentType> builder)
		{
			builder.ToTable("payment_type");

			builder.HasKey(pt => pt.Id);

			builder.Property(pt => pt.Id)
				.HasColumnName("id");

			builder.Property(pt => pt.Value)
				.HasColumnName("value")
				.HasConversion(
					v => v.ToString(),
					v => (PaymentTypeEnum)Enum.Parse(typeof(PaymentTypeEnum), v)
				);

			builder.HasMany(pt => pt.PaymentMethods)
					.WithOne(pm => pm.PaymentType)
					.HasForeignKey("PaymentTypeId")
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();

		}
	}
}
