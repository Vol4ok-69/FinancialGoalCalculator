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
using Finance.Pages;

namespace Finance.UserControls
{
    /// <summary>
    /// Логика взаимодействия для AuthAndRegistrView.xaml
    /// </summary>
    public partial class AuthAndRegistrView : UserControl
    {
        private readonly Registr _registr;
        private readonly Autho _autho;
        public AuthAndRegistrView(Registr registr, Autho autho)
        {
            InitializeComponent();
            _registr = registr;
            _autho = autho;
            frmAuthAndRegistr.Navigate(_autho);
        }
        private void btnSwapRegistrationAndAuthorization_Click(object sender, RoutedEventArgs e)
        {
            if (frmAuthAndRegistr.Content is Registr)
            {
                _autho.ResetForm();
                frmAuthAndRegistr.Content = _autho;
                btnSwapRegistrationAndAuthorization.Content = "Регистрация";
            }
            else
            {
                _autho.ResetForm();
                frmAuthAndRegistr.Content = _registr;
                btnSwapRegistrationAndAuthorization.Content = "Вход";
            }
        }

        private void frmAuthAndRegistr_ContentRendered(object sender, EventArgs e)
        {

        }
    }
}
