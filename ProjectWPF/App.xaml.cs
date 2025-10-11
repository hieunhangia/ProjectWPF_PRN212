using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;
using Repository.Repository.address;
using Repository.Repository.product;
using Repository.Repository.user;
using Service;
using Service.product;
using Service.user;
using System.Windows;

namespace ProjectWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private IHost? _host;

        protected override async void OnStartup(StartupEventArgs e)
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((_, configBuilder) =>
                {
                    configBuilder.AddJsonFile("appsettings.json");
                    //configBuilder.AddJsonFile("appsettings_secret.json");
                })
                .ConfigureServices(ConfigureServices).Build();

            await _host.StartAsync();

            using (var scope = _host.Services.CreateScope())
            {
                var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<Repository.DbContext>>();
                using var dbContext = dbContextFactory.CreateDbContext();

                dbContext.Database.EnsureCreated();

            }

            base.OnStartup(e);

            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = FindResource(typeof(Window))
                });

            _host.Services.GetRequiredService<Login>().Show();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var config = context.Configuration;
            services.AddDbContextFactory<Repository.DbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<Login>();
            services.AddTransient<AdminWindows.MainWindow>();
            services.AddTransient<AdminWindows.SellerList>();
            services.AddTransient<AdminWindows.SellerForm>();
            services.AddTransient<AdminWindows.SellerRequest>();
            services.AddTransient<SellerWindows.MainWindow>();

            services.AddSingleton<CommuneWardRepository>();
            services.AddSingleton<ProvinceCityRepository>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<SellerRepository>();
            services.AddSingleton<ProductRepository>();

            services.AddSingleton<UserChangeListener>();

            services.AddSingleton<AddressService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<SellerService>();
            services.AddSingleton<ProductService>();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
                _host.Dispose();
            }

            base.OnExit(e);
        }

    }

}
