using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Models.user;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Delegates;

namespace Repository
{
    public class UserChangeListener
    {
        private SqlTableDependency<User>? _dependency;

        public void StartListening(ChangedEventHandler<User> changedEventHandler)
        {
            using var context = new DbContext();
            _dependency = new(context.Database.GetConnectionString(), "user", includeOldValues: true);
            _dependency.OnChanged += changedEventHandler;
            _dependency.Start();
        }

        public void StopListening()
        {
            _dependency!.Stop();
            _dependency.Dispose();
        }
    }
}
