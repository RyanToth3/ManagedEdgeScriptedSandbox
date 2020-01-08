using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WpfWebView2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            this.webViewControl.Close();
        }

        private void go_Click(object sender, RoutedEventArgs e)
        {
            this.webViewControl.Navigate(this.urlInput.Text);
        }
    }
}
