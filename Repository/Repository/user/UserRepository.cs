using Microsoft.EntityFrameworkCore;
using Repository.Models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.user
{
    public class UserRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<User, long>(contextFactory)
    {
    }
}
