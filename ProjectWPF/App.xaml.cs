using Controller;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.Pinecone;
using Microsoft.SemanticKernel.Data;
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

        protected override void OnStartup(StartupEventArgs e)
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((_, configBuilder) =>
                {
                    configBuilder.AddJsonFile("appsettings.json");
                    configBuilder.AddJsonFile("appsettings_secret.json");
                })
                .ConfigureServices(ConfigureServices).Build();

            _host.Start();

            using (var scope = _host.Services.CreateScope())
            {
                var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<Repository.DbContext>>();
                using var context = contextFactory.CreateDbContext();

                context.Database.EnsureCreated();

            }

            base.OnStartup(e);

            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window),
                new FrameworkPropertyMetadata
                {
                    DefaultValue = FindResource(typeof(Window))
                });

            _host.Services.GetRequiredService<SellerWindows.AiSupporter>().Show();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddDbContextFactory<Repository.DbContext>(options =>
            {
                options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<CommuneWardRepository>();
            services.AddSingleton<ProvinceCityRepository>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<SellerRepository>();
            services.AddSingleton<ProductRepository>();

            services.AddSingleton<AddressService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<SellerService>();
            services.AddSingleton<ProductService>();

            services.AddSingleton<UserController>();
            services.AddSingleton<AdminController>();
            services.AddSingleton<SellerController>();

            services.AddTransient<Login>();
            services.AddTransient<AdminWindows.MainWindow>();
            services.AddTransient<AdminWindows.SellerList>();
            services.AddTransient<AdminWindows.SellerForm>();
            services.AddTransient<AdminWindows.SellerRequest>();
            services.AddTransient<SellerWindows.MainWindow>();

            services.AddSingleton<UserChangeListener>();

            var collection = new PineconeVectorStore(new(context.Configuration["PineconeApiKey"])).GetCollection<string, VectorDataModel>("project-wpf");
            collection.EnsureCollectionExistsAsync().ConfigureAwait(false);
            var textEmbeddingGenerator = new GoogleAIEmbeddingGenerator(
                modelId: "gemini-embedding-001",
                apiKey: context.Configuration["GoogleVertexAiApiKey"]!
            );
            var kernelBuilder = Kernel.CreateBuilder();
            kernelBuilder.Services.AddGoogleAIGeminiChatCompletion(
                modelId: "gemini-2.5-pro",
                apiKey: context.Configuration["GoogleVertexAiApiKey"]!
            );
#pragma warning disable SKEXP0001
            kernelBuilder.Plugins.Add(new VectorStoreTextSearch<VectorDataModel>(collection, textEmbeddingGenerator)
                .CreateWithGetTextSearchResults("SearchPlugin"));
#pragma warning restore SKEXP0001
            var kernel = kernelBuilder.Build();

            services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(textEmbeddingGenerator);
            services.AddSingleton<VectorStoreCollection<string, VectorDataModel>>(collection);
            services.AddSingleton(kernel);
            services.AddSingleton<AiService>();
            services.AddTransient<SellerWindows.AiSupporter>();
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
