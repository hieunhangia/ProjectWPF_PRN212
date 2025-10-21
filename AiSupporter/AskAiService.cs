using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiSupporter
{
    public class AskAiService
    {
        private readonly Kernel _kernel;

        private readonly ChatHistory _chatHistory = [];

        public AskAiService(Kernel kernel)
        {
            _kernel = kernel;

            _chatHistory.AddSystemMessage("""
            - Bạn là một "Trợ lý AI hỗ trợ Người bán hàng", chuyên về quản lý kho.
            - Nhiệm vụ chính của bạn là trả lời các câu hỏi của Người bán về tình trạng hàng hóa trong kho một cách chính xác, ngắn gọn và hữu ích.
            - Luôn luôn trả lời bằng tiếng Việt với giọng văn chuyên nghiệp, rõ ràng.
            - Định dạng câu trả lời: Bạn CHỈ ĐƯỢC PHÉP dùng các dạng Markdown cơ bản như in đậm (**text**), in nghiêng (*text*), danh sách không sắp xếp (* item), liên kết ([text](url)), và đoạn văn (\\n\\n).
            - Tuyệt đối không nhắc đến các cụm từ như "dựa vào thông tin được cung cấp", "theo dữ liệu đã cho", hoặc bất kỳ cụm từ nào tương tự.
            - Hãy sử dụng kiến thức chuyên môn của bạn để hỗ trợ Người bán hàng một cách tốt nhất!
            """);
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
            """;

            var answer = (await _kernel.InvokePromptAsync(
                promptTemplate,
                new() { { "query", query } },
                templateFormat: HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat,
                promptTemplateFactory: new HandlebarsPromptTemplateFactory()
            )).GetValue<string>()!;

            _chatHistory.AddUserMessage(query);
            _chatHistory.AddAssistantMessage(answer);
            return answer;
        }
    }
}
