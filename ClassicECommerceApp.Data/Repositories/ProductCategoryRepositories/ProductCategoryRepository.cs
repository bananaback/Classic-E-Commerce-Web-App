using ClassicECommerceApp.Data.Contexts;
using ClassicECommerceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassicECommerceApp.Data.Repositories.ProductCategoryRepositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductCategory>> GetAllAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }
    }
}
