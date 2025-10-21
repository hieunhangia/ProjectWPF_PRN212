using Microsoft.EntityFrameworkCore;

namespace Repository.Repository.product
{
    public class ProductUnitRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<ProductUnit, long>(contextFactory)
    {
    }
}
