using Finance.Interfaces;
using Finance.Models;
using Finance.Services;
using Microsoft.EntityFrameworkCore.Storage;
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

namespace Finance.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        IUserService _userService;
        public Autho(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _userService.Authorization(tbLogin.Text, tbPassword.Password);
                ResetForm();
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.ShowMainView();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                ResetForm();
            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                ResetForm();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ResetForm()
        {
            tbLogin.Clear();
            tbPassword.Clear();
        }
    }
}
