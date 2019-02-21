using MahApps.Metro.Controls;
using Organizer.UI.ViewModels;
using System.Windows;

namespace Organizer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel _mainWindowVM;

        public MainWindow(MainWindowViewModel mainWindowVM)
        {
            InitializeComponent();
            _mainWindowVM = mainWindowVM;
            
            DataContext = _mainWindowVM;

            // wystarczy wpisać Loaded (event odpalajacy sie po wyrenderowaniu kompinentu) a następnie += i tab - wygeneryje się hendler MainWindow_Loaded
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _mainWindowVM.LoadDataAsync();
        }
    }
}
