using ClassicECommerceApp.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassicECommerceApp.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("product_category")
                    .HasKey(pc => pc.ProductCategoryId);

                entity.Property(pc => pc.ProductCategoryId)
                    .HasColumnName("id");
                entity.Property(pc => pc.ParentCategoryId)
                    .HasColumnName("parent_category_id");
                entity.Property(pc => pc.CategoryName)
                    .HasColumnName("category_name");

                entity.HasOne(pc => pc.ParentCategory)
                    .WithMany(pc => pc.ChildCategories)
                    .HasForeignKey(pc => pc.ParentCategoryId)
                    .HasConstraintName("FK_ProductCategory_ParentCategory");
            });


        }
    }
}
