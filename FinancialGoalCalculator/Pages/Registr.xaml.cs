using BCrypt.Net;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using Finance.Interfaces;
using Finance.Models;

namespace Finance.Pages
{
    /// <summary>
    /// Логика взаимодействия для Registr.xaml
    /// </summary>
    public partial class Registr : Page
    {
        IUserService _userService;
        public Registr(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _userService.Registration(tbLogin.Text, tbPassword.Password, tbVerifyPassword.Password);
                ResetForm();
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ShowMainView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ResetForm();
            }
        }
        public void ResetForm()
        {
            tbLogin.Clear();
            tbPassword.Clear();
            tbVerifyPassword.Clear();
        }
    }
}
