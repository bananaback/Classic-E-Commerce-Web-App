using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClassicECommerceApp.Data.Configurations
{
	public class UserReviewEntityTypeConfiguration : IEntityTypeConfiguration<UserReview>
	{
		public void Configure(EntityTypeBuilder<UserReview> builder)
		{
			builder.ToTable("user_review");

			builder.HasKey(ur => ur.Id);

			builder.Property(ur => ur.Id)
					.HasColumnName("id");

			builder.Property<Guid?>("ApplicationUserId")
					.HasColumnName("user_id")
					.IsRequired(false);

			builder.Property<Guid>("OrderLineId")
					.HasColumnName("ordered_product_id");

			builder.Property(ur => ur.RatingValue)
					.HasColumnName("rating_value")
					.HasColumnType("decimal(3,1)")
					.IsRequired()
					.HasPrecision(3, 1);

			builder.Property(ur => ur.Comment)
					.HasColumnName("comment");
		}
	}
}
