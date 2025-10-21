using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Repository;
using Service.product;
using System.Text;

namespace AiSupporter
{
    public class AiService(ProductService productService,
        Kernel kernel)
    {
        private readonly ProductService _productService = productService;
        private readonly Kernel _kernel = kernel;

        public async Task SaveAllProductsExistedToVectorStore()
        {
            foreach (var product in _productService.GetAllProducts())
            {
                await SaveProductToVectorStore(product);
            }
        }

        public async Task SaveProductToVectorStore(Product product)
        {

            string content = GetProductContent(product);

            await _kernel.GetRequiredService<VectorStoreCollection<string, VectorDataModel>>().UpsertAsync(new VectorDataModel
            {
                Id = product.Id.ToString(),
                Content = content,
                EmbeddingContent = (await _kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>().GenerateAsync(content)).Vector
            });
        }

        public static string GetProductContent(Product product)
        {
            var sb = new StringBuilder();
            string unitName = product.ProductUnit.Name;

            sb.Append($"Thông tin sản phẩm cho quản lý kho. ");
            sb.Append($"Mã sản phẩm: {product.Id}. ");
            sb.Append($"Tên: {product.Name}. ");
            sb.Append($"Mô tả: \"{product.Description}.\" ");
            sb.Append($"Đơn vị tính: {unitName}. ");
            sb.Append($"Trạng thái kinh doanh: {(product.IsActive ? "Đang kinh doanh" : "Ngừng kinh doanh")}. ");

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

}
