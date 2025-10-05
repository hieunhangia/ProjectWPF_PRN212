using Repository;
using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ProjectWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            using var context = new DbContext();
            {
                context.Database.EnsureCreated();
            }
            base.OnStartup(e);
        }
    }

}
