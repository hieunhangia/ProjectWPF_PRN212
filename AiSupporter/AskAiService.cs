using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SKEXP0001
namespace AiSupporter
{
    public class AskAiService(
        [FromKeyedServices("gemini-2.5-flash")] IChatCompletionService gemini_2dot5_flash,
        [FromKeyedServices("gemini-2.5-pro")] IChatCompletionService gemini_2dot5_pro,
        VectorStoreTextSearch<VectorDataModel> vectorStoreTextSearch)
    {
        private readonly ChatHistory _chatHistory = new("""
            Bạn là một "Trợ lý AI hỗ trợ Người bán hàng quản lý kho hàng về hoa quả", chuyên về hỗ trợ Người bán hàng về việc quản lý kho hàng về hoa quả.
            Nhiệm vụ của bạn là hỗ trợ Người bán trong việc quản lý kho hàng về hoa quả để tăng cường hiệu suất cho Người bán hàng.
            Các khả năng của bạn chỉ giới hạn ở việc:
                - Trả lời các câu hỏi về tồn kho trong kho hàng hoa quả.
                - Cung cấp thông tin về hạn sử dụng của các loại hoa quả trong kho hàng.
                - Hỗ trợ Người bán hàng thực hiện cac hành động quản lý kho hàng như thêm loại hoa quả mới, cập nhật số lượng tồn kho,...
            TUYỆT ĐỐI không được trả lời hoặc đề cập đến các câu hỏi ngoài phạm vi quản lý kho hàng về hoa quả.
            Bạn KHÔNG ĐƯỢC PHÉP dùng Markdown, nếu muốn xuống dòng hãy dùng ký tự xuống dòng thông thường (/n), nếu muốn liệt kê danh sách hãy dùng dấu gạch ngang (-) cho mỗi mục.
            """);

        public async Task<ChatHistory> AskQuestion(string query)
        {
            string prompt = $"""
                Bạn là một AI điều phối thông minh. Dựa vào yêu cầu của người dùng và các tin nhắn trong quá khứ, hãy phân loại ý định của họ vào một trong các loại sau đây.
                Chỉ trả lời một trong 3 loại ý định, không giải thích gì thêm. Nếu ý định không thuộc 3 loại dưới đây, hãy phân loại nó vào "CannotAnswer".

                CÁC LOẠI Ý ĐỊNH:
                - "General": Khi người dùng chỉ muốn trò chuyện thông thường. Ví dụ: "Chào bạn", "Hôm nay bạn thế nào?", "Bạn là ai?".
                - "FunctionCalling": Khi người dùng ra lệnh trực tiếp hoặc yêu cầu thực hiện một hành động cụ thể. Dưới đây là các hành động được hỗ trợ:
                    - Tạo một yêu cầu thêm một loại hoa quả mới: "Thêm loại hoa quả X vào cơ sở dữ liệu với các thông tin như sau ...".
                    - Tạo một yêu cầu cập nhật thông tin của một loại hoa quả: "Cập nhật số lượng tồn kho của loại hoa quả Y thành Z".
                    * Nếu yêu cầu của người dùng vẫn thuộc loại ra lệnh hoặc thực hiện hành động nhưng không rõ ràng hoặc không thuộc các hành động trên, hãy phân loại nó vào "CannotAnswer".
                - "RAG": Khi người dùng hỏi một câu hỏi cần tìm kiếm thông tin trong tài liệu, cơ sở kiến thức. Dưới đây là các ví dụ:
                    - "Loại hoa quả X còn bao nhiêu trong kho?"
                    - "Khi nào loại hoa quả Y hết hạn sử dụng?"
                    - "Tình trạng kinh doanh của loại hoa quả Z hiện tại như thế nào?"
                    * Nếu yêu cầu của người dùng vẫn thuộc loại tìm kiếm thông tin nhưng không rõ ràng hoặc không liên quan đến việc quản lý kho hàng hoa quả, hãy phân loại nó vào "CannotAnswer".

                Yêu cầu của người dùng:
                {query}

                Ý định của người dùng là:
                """;
            
            var tempChatHistory = new ChatHistory(_chatHistory);
            tempChatHistory.AddUserMessage(prompt);
            string response = (await gemini_2dot5_flash.GetChatMessageContentAsync(tempChatHistory)).Content switch
            {
                "General" => await AskGeneralQuestion(query),
                "FunctionCalling" => await AskFunctionCallingQuestion(query),
                "RAG" => await AskRAGQuestion(query),
                _ => "Xin lỗi, khả năng của tôi là hỗ trợ Người bán hàng về việc quản lí kho hàng hoa quả nên không thể trả lời câu hỏi của bạn. Xin vui lòng thử lại với một câu hỏi khác."
            };

            _chatHistory.AddUserMessage(query);
            _chatHistory.AddAssistantMessage(response);

            if (_chatHistory.Count > 20)
            {
                _chatHistory.RemoveAt(1);
            }

            return _chatHistory;
        }

        private async Task<string> AskGeneralQuestion(string query)
        {
            string prompt = $"""
            Hãy trả lời câu hỏi sau:
            {query}


            Trong câu hỏi này, bạn chỉ trả lời giống như các câu giao tiếp chung chung, nhưng vẫn phải nhớ bạn là ai để trả lời phù hợp với vai trò "Trợ lý AI hỗ trợ Người bán hàng" chuyên về việc quản lý kho hàng hoa quả.
            """;

            var tempChatHistory = new ChatHistory(_chatHistory);
            tempChatHistory.AddUserMessage(prompt);
            return (await gemini_2dot5_flash.GetChatMessageContentAsync(tempChatHistory)).Content!;
        }

        private async Task<string> AskFunctionCallingQuestion(string query)
        {
            var tempChatHistory = new ChatHistory(_chatHistory);
            tempChatHistory.AddUserMessage(query);
            return (await gemini_2dot5_pro.GetChatMessageContentAsync(tempChatHistory)).Content!;
        }

        private async Task<string> AskRAGQuestion(string query)
        {
            StringBuilder searchResult = new();
            await foreach (var result in (await vectorStoreTextSearch.GetTextSearchResultsAsync(query)).Results)
            {
                searchResult.AppendLine(result.Value);
            }

            string prompt = $"""
            Bạn nhận được thông tin về các sản phẩm trong kho hàng hoa quả như sau:
            {searchResult}

            Dựa vào thông tin sản phẩm được cung cấp ở trên, hãy trả lời câu hỏi sau một cách chính xác:
            {query}


            Vai trò và phong cách trả lời:
            - Nhiệm vụ chính của bạn là trả lời các câu hỏi của Người bán về tình trạng hàng hóa trong kho một cách chính xác, ngắn gọn và hữu ích.

            Nguồn dữ liệu:
            - Toàn bộ thông tin bạn có về sản phẩm được cung cấp ở phần trên.
            - Đây là nguồn thông tin duy nhất và là sự thật tuyệt đối. Tuyệt đối không được bịa đặt, suy diễn hoặc cung cấp thông tin không có trong dữ liệu được cung cấp.

            Nguyên tắc trả lời:
            - Trả lời trực tiếp và súc tích: Đi thẳng vào vấn đề người dùng hỏi. Nếu hỏi về số lượng, hãy cung cấp con số. Nếu hỏi về hạn sử dụng, hãy cung cấp ngày tháng.
            - Tổng hợp thông tin: Khi được hỏi một câu chung về sản phẩm, hãy tóm tắt các thông tin quan trọng nhất như: tổng tồn kho (nếu có), số lượng lô hàng còn hạn, và trạng thái kinh doanh.
            - Xử lý các trường hợp đặc biệt:
                - Nếu sản phẩm có trạng thái "Ngừng kinh doanh", hãy nhấn mạnh điều này trong câu trả lời.
                - Nếu sản phẩm "chưa có thông tin nhập kho (chưa có lô hàng nào)", hãy thông báo rõ ràng tình trạng này.
                - Nếu "tất cả các lô hàng của sản phẩm đã hết tồn kho", hãy xác nhận là sản phẩm đã hết hàng.
            - Tuyệt đối không nhắc đến các cụm từ như "dựa vào thông tin được cung cấp", "theo dữ liệu đã cho",...
            """;

            var tempChatHistory = new ChatHistory(_chatHistory);
            tempChatHistory.AddUserMessage(prompt);
            return (await gemini_2dot5_pro.GetChatMessageContentAsync(tempChatHistory)).Content!;
        }
    }
}
