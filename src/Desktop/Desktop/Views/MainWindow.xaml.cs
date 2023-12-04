using System.Windows;

using Desktop.ViewModels;

namespace Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new AuthorViewModel();
        }
    }
}
