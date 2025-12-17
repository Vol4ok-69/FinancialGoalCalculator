using Finance.Pages;
using Finance.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Finance.UserControls
{
    public partial class MainView : UserControl
    {
        private readonly MyGoals _myGoals;
        private readonly CreateGoal _createGoal;

        public MainView(MyGoals myGoals, CreateGoal createGoal)
        {
            InitializeComponent();

            ArgumentNullException.ThrowIfNull(myGoals);
            ArgumentNullException.ThrowIfNull(createGoal);

            _myGoals = myGoals;
            _createGoal = createGoal;

            MainContentFrame.Content = _myGoals;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                SessionManager.StopSession();
                MainContentFrame.Content = null;
                mainWindow.ShowAuthAndRegistrView();
            }
        }

        private void btnCreateGoal_Click(object sender, RoutedEventArgs e)
        {
            _createGoal.ResetForm();
            MainContentFrame.Content = _createGoal;
        }

        private void btnGoals_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag != null)
            {
                string categoryTag = button.Tag.ToString();
                _myGoals.RefreshData(categoryTag);
            }

            MainContentFrame.Content = _myGoals;
        }
    }
}
