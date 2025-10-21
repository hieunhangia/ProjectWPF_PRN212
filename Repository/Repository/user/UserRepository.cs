using Microsoft.EntityFrameworkCore;
using Repository.Models.user;

namespace Repository.Repository.user
{
    public class UserRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<User, long>(contextFactory)
    {
    }
}
