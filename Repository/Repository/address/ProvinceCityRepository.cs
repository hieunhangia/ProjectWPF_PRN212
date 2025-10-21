using Microsoft.EntityFrameworkCore;

namespace Repository.Repository.address
{
    public class ProvinceCityRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<ProvinceCity, string>(contextFactory)
    {
    }
}
