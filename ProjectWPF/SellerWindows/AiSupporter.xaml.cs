using AiSupporter;
using System.Windows;

namespace ProjectWPF.SellerWindows
{
    /// <summary>
    /// Interaction logic for AiSupporter.xaml
    /// </summary>
    public partial class AiSupporter : Window
    {
        private readonly AiService _aiService;

        public AiSupporter(AiService aiService)
        {
            _aiService = aiService;

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

            AnswerTextBlock.Text = await _aiService.AskQuestion(question);

            AskAiNotiTextBlock.Text = "";
            this.IsEnabled = true;
        }

        private async void UpdateVectorDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật cơ sở dữ liệu AI? Quá trình này có thể mất vài phút.",
                "Xác Nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            UpdateVectorStoreNotiTextBlock.Text = "Đang cập nhật cơ sở dữ liệu AI, vui lòng chờ...";
            this.IsEnabled = false;

            await _aiService.SaveAllProductsExistedToVectorStore();
            MessageBox.Show("Cập nhật cơ sở dữ liệu AI thành công!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateVectorStoreNotiTextBlock.Text = "";
            this.IsEnabled = true;
        }
    }
}
