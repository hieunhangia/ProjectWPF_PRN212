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
            try
            {
                using var context = new DbContext();
                context.Database.EnsureCreated();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            base.OnStartup(e);

            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = FindResource(typeof(Window))
                });
        }
    }

}
