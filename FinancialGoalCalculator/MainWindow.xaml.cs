using Finance.Pages;
using Finance.UserControls;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Finance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AuthAndRegistrView _authAndRegistrView;
        private readonly MainView _mainView;
        public MainWindow(AuthAndRegistrView authAndRegistrView, MainView mainView)
        {
            InitializeComponent();
            ArgumentNullException.ThrowIfNull(authAndRegistrView);
            ArgumentNullException.ThrowIfNull(mainView);
            _authAndRegistrView = authAndRegistrView;
            _mainView = mainView;
            MainContentHost.Content = _authAndRegistrView;
        }
        public void ShowMainView()
        {
            MainContentHost.Content = _mainView;
        }
        public void ShowAuthAndRegistrView()
        {
            MainContentHost.Content = _authAndRegistrView;
        }
    }
}