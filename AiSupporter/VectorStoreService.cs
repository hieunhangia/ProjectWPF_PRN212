using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Repository;
using Service.product;
using System.Text;
using System.Text.RegularExpressions;

namespace AiSupporter
{
    public partial class VectorStoreService(ProductService productService,
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
        VectorStoreCollection<string, VectorDataModel> vectorStoreCollection)
    {
        private readonly ProductService _productService = productService;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator = embeddingGenerator;
        private readonly VectorStoreCollection<string, VectorDataModel> _vectorStoreCollection = vectorStoreCollection;

        public async Task SaveAllExistedProductsToVectorStore()
        {
            foreach (var product in _productService.GetAllProducts())
            {
                await SaveProductToVectorStore(product);
            }
        }

        public async Task SaveProductToVectorStore(Product product)
        {

            string content = GetProductContent(product);

            await _vectorStoreCollection.UpsertAsync(new VectorDataModel
            {
                Id = product.Id.ToString(),
                Content = content,
                EmbeddingContent = (await _embeddingGenerator.GenerateAsync(content)).Vector
            });
        }

        private static string GetProductContent(Product product)
        {
            var sb = new StringBuilder();
            string unitName = product.ProductUnit!.Name;

            sb.Append($"Tên sản phẩm: {product.Name}. Mô tả: '{product.Description}'. Đơn vị tính: {unitName}. Trạng thái kinh doanh: {(product.IsActive ? "Đang kinh doanh" : "Ngừng kinh doanh")}. ");

            if (product.ProductBatches != null && product.ProductBatches.Count != 0)
            {
                var validBatches = product.ProductBatches
                    .Where(batch => batch.Quantity > 0)
                    .ToList();

                if (validBatches.Count != 0)
                {
                    int totalStock = validBatches.Sum(batch => batch.Quantity);

                    sb.Append($"Tổng số lô hàng tồn kho của sản phẩm là {validBatches.Count} lô. ");
                    var batchDetails = validBatches.Select(batch =>
                        $"lô có mã {batch.Id} với hạn sử dụng {batch.ExpiryDate:dd/MM/yyyy} còn {batch.Quantity} {unitName}"
                    );
                    sb.Append($"Chi tiết các lô sắp xếp theo thứ tự hạn sử dụng tăng dần: {string.Join("; ", batchDetails)}. ");
                }
                else
                {
                    sb.Append($"Tất cả các lô hàng của sản phẩm đã hết tồn kho. ");
                }
            }
            else
            {
                sb.Append($"Sản phẩm chưa có thông tin nhập kho (chưa có lô hàng nào). ");
            }

            return sb.ToString();
        }

    }
    public class VectorDataModel
    {
        [VectorStoreKey]
        [TextSearchResultName]
        public required string Id { get; set; }

        [VectorStoreData]
        [TextSearchResultValue]
        public required string Content { get; set; }

        [VectorStoreVector(Dimensions: 3072, DistanceFunction = DistanceFunction.CosineSimilarity)]
        public ReadOnlyMemory<float> EmbeddingContent { get; set; }
    }

}
