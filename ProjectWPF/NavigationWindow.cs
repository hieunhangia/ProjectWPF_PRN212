using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ProjectWPF
{
    public class NavigationWindow(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public T GetWindow<T>() where T : Window => _serviceProvider.GetRequiredService<T>();

        public void ShowWindow<T>() where T : Window => GetWindow<T>().Show();

        public void ShowDialog<T>() where T : Window => GetWindow<T>().ShowDialog();

        public void ShowWindowAndCloseCurrent<TNewWindow>(Window currentWindow) where TNewWindow : Window
        {
            GetWindow<TNewWindow>().Show();
            currentWindow.Close();
        }
    }
}
