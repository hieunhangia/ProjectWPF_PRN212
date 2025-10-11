using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Models.user;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Delegates;

namespace Repository
{
    public class UserChangeListener(IDbContextFactory<DbContext> dbContextFactory)
    {
        private readonly IDbContextFactory<DbContext> _dbContextFactory = dbContextFactory;
        private SqlTableDependency<User>? _dependency;

        public void StartListening(params ChangedEventHandler<User>[] changedEventHandlers)
        {
            _dependency = new(_dbContextFactory.CreateDbContext().Database.GetConnectionString(), "user", includeOldValues: true);
            foreach (var handler in changedEventHandlers)
            {
                _dependency.OnChanged += handler;
            }
            _dependency.Start();
        }

        public void StopListening()
        {
            _dependency!.Stop();
            _dependency.Dispose();
        }
    }
}
