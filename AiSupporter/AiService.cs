using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Repository;
using Service.product;
using System.Text;
using System.Text.RegularExpressions;

namespace AiSupporter
{
    public partial class AiService(ProductService productService,
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

        public async Task<string> AskQuestion(string query)
        {

            string promptTemplate = """
            {{#with (SearchPlugin-GetTextSearchResults query)}}  
                {{#each this}}  
                ID: {{Name}}
                Giá Trị: {{Value}}
                -----------------
                {{/each}}
            {{/with}}

            Dựa vào thông tin sản phẩm được cung cấp ở trên, hãy trả lời câu hỏi sau một cách chính xác:
            {{query}}


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
            - Định dạng câu trả lời: Bạn CHỈ ĐƯỢC PHÉP dùng các dạng Markdown cơ bản như : danh sách không sắp xếp (* item), và đoạn văn (\\n\\n).
            - Tuyệt đối không nhắc đến các cụm từ như "dựa vào thông tin được cung cấp", "theo dữ liệu đã cho",...
            """;

            return UnorderedList().Replace((await _kernel.InvokePromptAsync(
                promptTemplate,
                new() {
                    { "query", query },
                },
                templateFormat: HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat,
                promptTemplateFactory: new HandlebarsPromptTemplateFactory()
            )).GetValue<string>()!, "• $1");
        }

        [GeneratedRegex(@"^\* (.*)", RegexOptions.Multiline)]
        private static partial Regex UnorderedList();
    }

}
