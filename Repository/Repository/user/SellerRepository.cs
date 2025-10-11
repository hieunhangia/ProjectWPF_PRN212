using Microsoft.EntityFrameworkCore;
using Repository.Models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.user
{
    public class SellerRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<Seller, long>(contextFactory)
    {
        public new List<Seller> GetAll()
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<Seller>().Include(s => s.CommuneWard).ThenInclude(c => c!.ProvinceCity)];
        }

        public new Seller? GetById(long id)
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Set<Seller>().Include(s => s.CommuneWard).ThenInclude(c => c!.ProvinceCity).FirstOrDefault(s => s.Id == id);
        }

        public new List<Seller> GetByCondition(Expression<Func<Seller, bool>> condition)
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<Seller>().Include(s => s.CommuneWard).ThenInclude(c => c!.ProvinceCity).Where(condition)];
        }
    }
}
