using Service;
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

        private void SendQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            AnswerTextBlock.Text = _aiService.AskQuestion(QuestionTextBox.Text);
        }
    }
}
