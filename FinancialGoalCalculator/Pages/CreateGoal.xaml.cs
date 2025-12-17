using Finance.Interfaces;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Finance.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateGoal.xaml
    /// </summary>
    public partial class CreateGoal : Page
    {
        private readonly IFinancialGoalService _goalService;
        public CreateGoal(IFinancialGoalService goalService)
        {
            InitializeComponent();
            ArgumentNullException.ThrowIfNull(goalService);
            _goalService = goalService;
        }
        private void BtnSaveGoal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbName.Text) ||
                    string.IsNullOrWhiteSpace(tbTargetAmount.Text) ||
                    string.IsNullOrWhiteSpace(tbInitialAmount.Text) ||
                    string.IsNullOrWhiteSpace(tbPlannedAmount.Text))
                {
                    MessageBox.Show("Заполните обязательные поля");
                    return;
                }

                if (!decimal.TryParse(tbTargetAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var targetAmount) ||
                    !decimal.TryParse(tbInitialAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var initialAmount) ||
                    !decimal.TryParse(tbPlannedAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var plannedAmount))
                {
                    MessageBox.Show("Некорректный формат суммы");
                    return;
                }

                if (initialAmount > targetAmount)
                {
                    MessageBox.Show("Начальная сумма не может превышать целевую");
                    return;
                }

                _goalService.CreateGoal(
                    tbName.Text,
                    tbDescription.Text,
                    targetAmount,
                    initialAmount,
                    dpDeadline.SelectedDate,
                    cbPriority.SelectedIndex + 1,
                    cbCategory.SelectedIndex + 1,
                    cbFrequency.SelectedIndex + 1,
                    plannedAmount
                );

                MessageBox.Show("Финансовая цель успешно создана");
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ResetForm()
        {
            tbName.Clear();
            tbDescription.Clear();
            tbTargetAmount.Clear();
            tbPlannedAmount.Clear();
            dpDeadline.SelectedDate = null;

            cbPriority.SelectedIndex = 0;
            cbCategory.SelectedIndex = 0;
            cbFrequency.SelectedIndex = 0;
        }
    }
}
