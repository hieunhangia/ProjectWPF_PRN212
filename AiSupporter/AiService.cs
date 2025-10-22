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

#pragma warning disable SKEXP0001
namespace AiSupporter
{
    public partial class AiService(ProductService productService,
        IChatCompletionService chatCompletionService,
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
        VectorStoreCollection<string, VectorDataModel> vectorStoreCollection,
        VectorStoreTextSearch<VectorDataModel> vectorStoreTextSearch)
    {
        private readonly ProductService _productService = productService;
        private readonly IChatCompletionService _chatCompletionService = chatCompletionService;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator = embeddingGenerator;
        private readonly VectorStoreCollection<string, VectorDataModel> _vectorStoreCollection = vectorStoreCollection;
        private readonly VectorStoreTextSearch<VectorDataModel> _vectorStoreTextSearch = vectorStoreTextSearch;

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

            await _vectorStoreCollection.UpsertAsync(new VectorDataModel
            {
                Id = product.Id.ToString(),
                Content = content,
                EmbeddingContent = (await _embeddingGenerator.GenerateAsync(content)).Vector
            });
        }

        public static string GetProductContent(Product product)
        {
            var sb = new StringBuilder();
            string unitName = product.ProductUnit.Name;

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

        public async Task<string> AskQuestion(string query)
        {
            StringBuilder sb = new();
            await foreach (var result in (await _vectorStoreTextSearch.GetTextSearchResultsAsync(query)).Results)
            {
                sb.AppendLine(result.Value);
            }

            string prompt = $"""
            Bạn nhận được thông tin về các sản phẩm trong kho hàng như sau:
            {sb}

            Dựa vào thông tin sản phẩm được cung cấp ở trên, hãy trả lời câu hỏi sau một cách chính xác:
            {query}


            Về vai trò và phong cách trả lời:
            - Bạn là một "Trợ lý AI hỗ trợ Người bán hàng", chuyên về quản lý kho.
            - Nhiệm vụ chính của bạn là trả lời các câu hỏi của Người bán về tình trạng hàng hóa trong kho một cách chính xác, ngắn gọn và hữu ích.


            Nguồn dữ liệu:
            - Toàn bộ thông tin bạn có về sản phẩm được cung cấp ở phần trên.
            - Đây là nguồn thông tin duy nhất và là sự thật tuyệt đối. Tuyệt đối không được bịa đặt, suy diễn hoặc cung cấp thông tin không có trong dữ liệu được cung cấp.


            Nguyên tắc trả lời:
            - Bám sát dữ liệu: Chỉ trả lời dựa trên thông tin sản phẩm đã cho. Khi trả lời về một sản phẩm, hãy bắt đầu bằng cách xác nhận lại tên sản phẩm đó.
            - Trả lời trực tiếp và súc tích: Đi thẳng vào vấn đề người dùng hỏi. Nếu hỏi về số lượng, hãy cung cấp con số. Nếu hỏi về hạn sử dụng, hãy cung cấp ngày tháng.
            - Tổng hợp thông tin: Khi được hỏi một câu chung về sản phẩm, hãy tóm tắt các thông tin quan trọng nhất như: tổng tồn kho (nếu có), số lượng lô hàng còn hạn, và trạng thái kinh doanh.
            - Xử lý các trường hợp đặc biệt:
                - Nếu sản phẩm có trạng thái "Ngừng kinh doanh", hãy nhấn mạnh điều này trong câu trả lời.
                - Nếu sản phẩm "chưa có thông tin nhập kho (chưa có lô hàng nào)", hãy thông báo rõ ràng tình trạng này.
                - Nếu "tất cả các lô hàng của sản phẩm đã hết tồn kho", hãy xác nhận là sản phẩm đã hết hàng.
            - Bạn KHÔNG ĐƯỢC PHÉP dùng Markdown, nếu muốn xuống dòng hãy dùng ký tự xuống dòng thông thường (/n).
            - Tuyệt đối không nhắc đến các cụm từ như "dựa vào thông tin được cung cấp", "theo dữ liệu đã cho",...
            """;

            return (await _chatCompletionService.GetChatMessageContentAsync(prompt)).Content!;
        }
    }

}
