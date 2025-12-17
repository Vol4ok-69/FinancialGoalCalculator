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
                _goalService.ChangeStatus(CurrentGoal.Id, SelectedStatus.Id);
                _originalStatusId = SelectedStatus.Id;
                btnSave.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
    }
}
