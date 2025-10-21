using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AiSupporter;

namespace ProjectWPF.SellerWindows
{
    /// <summary>
    /// Interaction logic for AiSupporter.xaml
    /// </summary>
    public partial class AiSupporter : Window
    {
        private readonly AiService _aiService;
        private readonly AskAiService _askAiService;

        public AiSupporter(AiService aiService, AskAiService askAiService)
        {
            _aiService = aiService;
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

            AnswerTextBlock.Text = await _askAiService.AskQuestion(question);
            
            AskAiNotiTextBlock.Text = "";
            this.IsEnabled = true;
        }

        private async void UpdateVectorDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateVectorStoreNotiTextBlock.Text = "Đang cập nhật cơ sở dữ liệu AI, vui lòng chờ...";
            this.IsEnabled = false;

            await _aiService.SaveAllProductsExistedToVectorStore();

            this.IsEnabled = true;
            UpdateVectorStoreNotiTextBlock.Text = "";
            MessageBox.Show("Cập nhật cơ sở dữ liệu AI thành công!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
