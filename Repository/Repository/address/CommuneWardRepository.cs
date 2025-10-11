using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.address
{
    public class CommuneWardRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<CommuneWard, string>(contextFactory)
    {
        public new List<CommuneWard> GetAll()
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<CommuneWard>().Include(cw => cw.ProvinceCity)];
        }

        public new CommuneWard? GetById(string id)
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Set<CommuneWard>().Include(cw => cw.ProvinceCity).FirstOrDefault(cw => cw.Code == id);
        }

        public new List<CommuneWard> GetByCondition(Expression<Func<CommuneWard, bool>> condition)
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<CommuneWard>().Include(cw => cw.ProvinceCity).Where(condition)];
        }
    }
}
