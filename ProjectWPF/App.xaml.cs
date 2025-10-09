using Repository;
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
            context.Database.EnsureCreated();

            base.OnStartup(e);

            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = FindResource(typeof(Window))
                });
        }

    }

}
