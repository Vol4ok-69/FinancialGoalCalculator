using Finance.Interfaces;
using Finance.Models;
using Finance.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Finance.Pages
{
    /// <summary>
    /// Логика взаимодействия для GoalDetails.xaml
    /// </summary>
    public partial class GoalDetails : Page
    {
        public FinancialGoal CurrentGoal { get; set; }
        public List<GoalStatus> AvailableStatuses { get; set; }
        public GoalStatus SelectedStatus { get; set; }

        private readonly IFinancialGoalService _goalService;
        private int _originalStatusId;
        private bool _isEditMode = false;
        public GoalDetails(int goalId, IFinancialGoalService goalService)
        {
            InitializeComponent();
            ArgumentNullException.ThrowIfNull(goalService);
            _goalService = goalService;
            InitializeGoal(goalId);

            DataContext = this;
            btnSave.Visibility = Visibility.Collapsed;
        }

        private void InitializeGoal(int goalId)
        {
            CurrentGoal = _goalService.GetGoalWithDetails(goalId);
            ArgumentNullException.ThrowIfNull(CurrentGoal);

            AvailableStatuses = [.. _goalService
                .LoadAllGoals()
                .Select(g => g.Status)
                .Distinct()];
            ArgumentNullException.ThrowIfNull(AvailableStatuses);

            SelectedStatus = AvailableStatuses
                .FirstOrDefault(s => s.Id == CurrentGoal.StatusId);
            ArgumentNullException.ThrowIfNull(SelectedStatus);

            _originalStatusId = SelectedStatus.Id;

            tbPriorityName.Foreground = CurrentGoal.Priority.Name switch
            {
                "Низкий" => Brushes.Green,
                "Средний" => Brushes.Orange,
                "Высокий" => Brushes.Red,
                _ => Brushes.Gray
            };
        }

        private void cbEditStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedStatus == null)
            {
                btnSave.Visibility = Visibility.Collapsed;
                return;
            }

            btnSave.Visibility =
                SelectedStatus.Id != _originalStatusId
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal target = CurrentGoal.TargetAmount;
                decimal current = CurrentGoal.CurrentAmount;

                if (_isEditMode)
                {
                    if (!decimal.TryParse(tbEditTargetAmount.Text.Replace('.', ','), out target))
                        throw new Exception("Некорректная целевая сумма");

                    if (!decimal.TryParse(tbEditCurrentAmount.Text.Replace('.', ','), out current))
                        throw new Exception("Некорректная текущая сумма");

                    if (current < 0 || target <= 0)
                        throw new Exception("Суммы должны быть положительными");

                    _goalService.UpdateGoal(
                        CurrentGoal.Id,
                        tbGoalName.Text,
                        tbGoalDescription.Text,
                        target,
                        current,
                        dpEditDeadline.SelectedDate
                    );

                    ExitEditMode();
                }

                if (SelectedStatus.Id != _originalStatusId)
                {
                    _goalService.ChangeStatus(CurrentGoal.Id, SelectedStatus.Id);
                    _originalStatusId = SelectedStatus.Id;
                }

                btnSave.Visibility = Visibility.Collapsed;

                ReloadGoal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ReloadGoal()
        {
            CurrentGoal = _goalService.GetGoalWithDetails(CurrentGoal.Id);
            DataContext = null;
            DataContext = this;
        }
        private void ExitEditMode()
        {
            _isEditMode = false;

            tbGoalName.IsReadOnly = true;
            tbGoalDescription.IsReadOnly = true;
            tbEditTargetAmount.IsReadOnly = true;
            tbEditCurrentAmount.IsReadOnly = true;
            dpEditDeadline.IsEnabled = false;

            tbGoalName.Background = Brushes.Transparent;
            tbGoalDescription.Background = Brushes.Transparent;
            tbEditTargetAmount.Background = Brushes.Transparent;
            tbEditCurrentAmount.Background = Brushes.Transparent;

            tbGoalName.BorderThickness = new Thickness(0);
            tbGoalDescription.BorderThickness = new Thickness(0);
        }
        private void btnAddContribution_Click(object sender, RoutedEventArgs e)
        {
            var input = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите сумму пополнения:",
                "Пополнение цели",
                "1000");

            if (decimal.TryParse(input, out var amount) && amount > 0)
            {
                _goalService.AddContribution(CurrentGoal.Id, amount);
                CurrentGoal = _goalService.GetGoalWithDetails(CurrentGoal.Id);
                DataContext = null;
                DataContext = this;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _goalService.DeleteGoal(CurrentGoal.Id);
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            _isEditMode = true;

            tbGoalName.IsReadOnly = false;
            tbGoalDescription.IsReadOnly = false;
            tbEditTargetAmount.IsReadOnly = false;
            tbEditCurrentAmount.IsReadOnly = false;
            dpEditDeadline.IsEnabled = true;

            tbGoalName.Background = Brushes.White;
            tbGoalDescription.Background = Brushes.White;
            tbEditTargetAmount.Background = Brushes.White;
            tbEditCurrentAmount.Background = Brushes.White;

            tbGoalName.BorderThickness = new Thickness(1);
            tbGoalDescription.BorderThickness = new Thickness(1);

            btnSave.Visibility = Visibility.Visible;
        }
    }
}
