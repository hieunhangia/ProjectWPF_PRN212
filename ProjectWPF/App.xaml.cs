using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.Pinecone;
using Microsoft.SemanticKernel.Data;
using Pinecone;
using ProjectWPF.Models;
using Repository;
using Repository.Models.user;
using Repository.Repository.address;
using Repository.Repository.product;
using Repository.Repository.seller_request;
using Repository.Repository.user;
using Service;
using Service.product;
using Service.seller_request;
using Service.user;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using AiSupporter;

#pragma warning disable SKEXP0001
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

            _host.Services.GetRequiredService<SellerWindows.AiSupporter>().Show();
        }

        
        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddDbContextFactory<Repository.DbContext>(options =>
            {
                options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"));
            });

            var gemini_2dot5_flash = new VertexAIGeminiChatCompletionService(
                projectId: context.Configuration["GoogleVertexAiProjectId"]!,
                bearerKey: context.Configuration["GoogleVertexAiBearerKey"]!,
                modelId: context.Configuration["GoogleVertexAi2Dot5FlashModel"]!,
                location: context.Configuration["GoogleVertexAi2Dot5FlashLocation"]!
            );
            services.AddKeyedSingleton<IChatCompletionService>("gemini-2.5-flash", gemini_2dot5_flash);

            services.AddVertexAIGeminiChatCompletion(
                projectId: context.Configuration["GoogleVertexAiProjectId"]!,
                bearerKey: context.Configuration["GoogleVertexAiBearerKey"]!,
                modelId: context.Configuration["GoogleVertexAi2Dot5ProModel"]!,
                location: context.Configuration["GoogleVertexAi2Dot5ProLocation"]!
            );
            services.AddVertexAIEmbeddingGenerator(
                projectId: context.Configuration["GoogleVertexAiProjectId"]!,
                bearerKey: context.Configuration["GoogleVertexAiBearerKey"]!,
                modelId: context.Configuration["GoogleVertexAiEmbeddingModel"]!,
                location: context.Configuration["GoogleVertexAiEmbeddingLocation"]!
            );
            services.AddPineconeCollection<VectorDataModel>(
                context.Configuration["PineconeIndexName"]!,
                context.Configuration["PineconeApiKey"]!
            );
            services.AddSingleton<ITextSearch>(provider =>
            {
                var collection = provider.GetRequiredService<VectorStoreCollection<string, VectorDataModel>>();
                collection.EnsureCollectionExistsAsync().Wait();
                return new VectorStoreTextSearch<VectorDataModel>(collection, provider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>());
            });
            services.AddSingleton<VectorStoreService>();
            services.AddTransient<AskAiService>();

            services.AddSingleton<CommuneWardRepository>();
            services.AddSingleton<ProvinceCityRepository>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<SellerRepository>();
            services.AddSingleton<ProductRepository>();
            services.AddSingleton<ProductUnitRepository>();
            services.AddSingleton<SellerRequestRepository>();
            services.AddSingleton<SellerRequestTypeRepository>();
            services.AddSingleton<SellerRequestStatusRepository>();


            services.AddSingleton<AddressService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<SellerService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<ProductUnitService>();
            services.AddSingleton<SellerRequestService>();
            services.AddSingleton<SellerRequestStatusService>();
            services.AddSingleton<SellerRequestTypeService>();


            services.AddSingleton<NavigationWindow>();
            services.AddTransient<Login>();
            services.AddTransient<AdminWindows.MainWindow>();
            services.AddTransient<AdminWindows.SellerList>();
            services.AddTransient<AdminWindows.SellerForm>();
            services.AddTransient<AdminWindows.SellerRequestList>();
            services.AddTransient<SellerWindows.MainWindow>();
            services.AddTransient<SellerWindows.CreateProductRequest>();
            services.AddTransient<SellerWindows.ProductList>();

            services.AddTransient(provider =>
            {
                Func<long, AdminWindows.SellerRequestDetail> value = id =>
                {
                    var sellerRequestService = provider.GetRequiredService<SellerRequestService>();
                    var productService = provider.GetRequiredService<ProductService>();
                    var productUnitService = provider.GetRequiredService<ProductUnitService>();
                    return new AdminWindows.SellerRequestDetail(id, sellerRequestService, productService,productUnitService);
                };
                return value;
            });

            services.AddTransient(provider =>
            {
                Func<long, Seller, SellerWindows.EditProductRequest> value = (id, seller) =>
                {
                    var sellerRequestService = provider.GetRequiredService<SellerRequestService>();
                    var productUnitService = provider.GetRequiredService<ProductUnitService>();
                    var productService = provider.GetRequiredService<ProductService>();

                    return new SellerWindows.EditProductRequest(id,sellerRequestService,productUnitService,productService,seller);
                };
                return value;
            });

            services.AddTransient<SellerWindows.AiSupporter>();

            services.AddSingleton<UserChangeListener>();
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
