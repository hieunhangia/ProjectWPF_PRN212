using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.product
{
    public class ProductRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<Product, long>(contextFactory)
    {
        public new List<Product> GetAll()
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<Product>().Include(p => p.ProductBatches).Include(p => p.ProductUnit)];
        }

    }
}
