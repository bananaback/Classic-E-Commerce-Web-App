using ClassicECommerceApp.Data.Configurations;
using ClassicECommerceApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassicECommerceApp.Data.Contexts
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<ProductCategory> ProductCategories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
			{
				entity.HasKey(r => new { r.UserId, r.RoleId });
			});

			new AddressEntityTypeConfiguration().Configure(modelBuilder.Entity<Address>());
			new ApplicationUserEntityTypeConfiguration().Configure(modelBuilder.Entity<ApplicationUser>());
			new CountryEntityTypeConfiguration().Configure(modelBuilder.Entity<Country>());
			new OrderLineEntityTypeConfiguration().Configure(modelBuilder.Entity<OrderLine>());
			new OrderStatusEntityTypeConfiguration().Configure(modelBuilder.Entity<OrderStatus>());
			new PaymentTypeEntityTypeConfiguration().Configure(modelBuilder.Entity<PaymentType>());
			new ProductCategoryEntityTypeConfiguration().Configure(modelBuilder.Entity<ProductCategory>());
			// new ProductConfigurationEntityTypeConfiguration().Configure(modelBuilder.Entity<ProductConfiguration>());
			new ProductEntityTypeConfiguration().Configure(modelBuilder.Entity<Product>());
			new ProductItemEntityTypeConfiguration().Configure(modelBuilder.Entity<ProductItem>());
			new ShippingMethodEntityTypeConfiguration().Configure(modelBuilder.Entity<ShippingMethod>());
			new ShopOrderEntityTypeConfiguration().Configure(modelBuilder.Entity<ShopOrder>());
			new ShoppingCartEntityTypeConfiguration().Configure(modelBuilder.Entity<ShoppingCart>());
			new ShoppingCartItemEntityTypeConfiguration().Configure(modelBuilder.Entity<ShoppingCartItem>());
			// new UserAddressEntityTypeConfiguration().Configure(modelBuilder.Entity<UserAddress>());
			new UserPaymentMethodEntityTypeConfiguration().Configure(modelBuilder.Entity<UserPaymentMethod>());
			new UserReviewEntityTypeConfiguration().Configure(modelBuilder.Entity<UserReview>());
			new VariationEntityTypeConfiguration().Configure(modelBuilder.Entity<Variation>());
			new VariationOptionEntityTypeConfiguration().Configure(modelBuilder.Entity<VariationOption>());
		}
	}
}
