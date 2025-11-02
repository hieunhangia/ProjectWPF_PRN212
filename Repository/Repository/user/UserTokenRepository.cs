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
    public class UserTokenRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<UserToken, long>(contextFactory)
    {
        public new List<UserToken> GetByCondition(Expression<Func<UserToken, bool>> condition)
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<UserToken>().Include(ut => ut.User).Where(condition)];
        }
    }
}
