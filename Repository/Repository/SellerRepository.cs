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
            return [.. context.Set<Seller>().Include(s => s.CommuneWard).ThenInclude(c => c.ProvinceCity)];
        }
    }
}
