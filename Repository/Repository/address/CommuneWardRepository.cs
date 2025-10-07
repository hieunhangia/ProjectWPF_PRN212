using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.address
{
    public class CommuneWardRepository : BasicRepository<CommuneWard, string>
    {
        public new List<CommuneWard> GetAll()
        {
            using var context = new DbContext();
            return [.. context.Set<CommuneWard>().Include(cw => cw.ProvinceCity)];
        }

        public new CommuneWard? GetById(string id)
        {
            using var context = new DbContext();
            return context.Set<CommuneWard>().Include(cw => cw.ProvinceCity).FirstOrDefault(cw => cw.Code == id);
        }

        public new List<CommuneWard> GetByCondition(Func<CommuneWard, bool> condition)
        {
            using var context = new DbContext();
            return [.. context.Set<CommuneWard>().Include(cw => cw.ProvinceCity).Where(condition)];
        }
    }
}
