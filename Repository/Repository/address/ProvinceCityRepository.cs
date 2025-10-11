using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.address
{
    public class ProvinceCityRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<ProvinceCity, string>(contextFactory)
    {
    }
}
