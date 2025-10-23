using AiSupporter;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.VisualBasic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TableDependency.SqlClient.Base.Messages;

namespace ProjectWPF.SellerWindows
{
    /// <summary>
    /// Interaction logic for AiSupporter.xaml
    /// </summary>
    public partial class AiSupporter : Window
    {
        private readonly VectorStoreService _vectorStoreService;
        private readonly AskAiService _askAiService;

        public AiSupporter(VectorStoreService vectorStoreService,
            AskAiService askAiService)
        {
            _vectorStoreService = vectorStoreService;
            _askAiService = askAiService;

            InitializeComponent();
        }

        private async void SendQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            string question = QuestionTextBox.Text;
            if (string.IsNullOrWhiteSpace(question))
            {
                MessageBox.Show("Vui lòng nhập câu hỏi!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AskAiNotiTextBlock.Text = "Đang chờ phản hồi của AI...";
            this.IsEnabled = false;

            AddMessageToConversation(await _askAiService.AskQuestion(question));

            QuestionTextBox.Text = "";
            AskAiNotiTextBlock.Text = "";
            this.IsEnabled = true;
        }

        private void AddMessageToConversation(ChatHistory conversation)
        {
            ConversationStackPanel.Children.Clear();
            foreach (var message in conversation.Skip(1))
            {
                bool isUser = message.Role == AuthorRole.User;
                var border = new Border
                {
                    Style = (Style)FindResource(isUser ? "UserMessageBorder" : "AiMessageBorder")
                };

                var textBlock = new TextBlock
                {
                    Text = message.Content!,
                    Style = (Style)FindResource(isUser ? "UserMessageText" : "AiMessageText")
                };

                border.Child = textBlock;
                ConversationStackPanel.Children.Add(border);
            }

            if (ConversationStackPanel.Parent is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToBottom();
            }
        }

        private async void UpdateVectorDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật cơ sở dữ liệu AI? Quá trình này có thể mất vài phút.",
                "Xác Nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            UpdateVectorStoreNotiTextBlock.Text = "Đang cập nhật cơ sở dữ liệu AI, vui lòng chờ...";
            this.IsEnabled = false;

            await _vectorStoreService.SaveAllExistedProductsToVectorStore();
            MessageBox.Show("Cập nhật cơ sở dữ liệu AI thành công!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateVectorStoreNotiTextBlock.Text = "";
            this.IsEnabled = true;
        }
    }
}
