using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class SellerRepository : BasicRepository<Seller, long>
    {
        public new List<Seller> GetAll()
        {
            using var context = new DbContext();
            return [.. context.Set<Seller>().Include(s => s.CommuneWard).ThenInclude(c => c!.ProvinceCity)];
        }

        public new Seller? GetById(long id)
        {
            using var context = new DbContext();
            return context.Set<Seller>().Include(s => s.CommuneWard).ThenInclude(c => c!.ProvinceCity).FirstOrDefault(s => s.Id == id);
        }

        public new List<Seller> GetByCondition(Func<Seller, bool> predicate)
        {
            using var context = new DbContext();
            return [.. context.Set<Seller>().Include(s => s.CommuneWard).ThenInclude(c => c.ProvinceCity).Where(predicate)];
        }
    }
}
