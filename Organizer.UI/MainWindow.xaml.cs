using Organizer.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Organizer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
