using Microsoft.EntityFrameworkCore;

namespace Repository.Repository.product
{
    public class ProductRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<Product, long>(contextFactory)
    {
        public new List<Product> GetAll()
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<Product>().Include(p => p.ProductBatches).Include(p => p.ProductUnit)];
        }

        public Product? GetProductById(long id)
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Set<Product>()
                .Include(p => p.ProductBatches)
                .Include(p => p.ProductUnit)
                .FirstOrDefault(p => p.Id == id);
        }
    }
}
