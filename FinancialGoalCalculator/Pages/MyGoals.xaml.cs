using Finance.Interfaces;
using Finance.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;


namespace Finance.Pages
{
    /// <summary>
    /// Логика взаимодействия для MyGoals.xaml
    /// </summary>
    public partial class MyGoals : Page
    {
        public List<FinancialGoal>? GoalList { get; set; }
        public List<GoalStatus>? StatusList { get; set; }

        private readonly IFinancialGoalService _goalService;
        private string _categoryTag = "0";

        private static readonly JsonSerializerOptions _exportJsonOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public MyGoals(IFinancialGoalService goalService)
        {
            InitializeComponent();
            ArgumentNullException.ThrowIfNull(goalService);
            _goalService = goalService;

            StatusList = [.. _goalService
                .LoadAllGoals()
                .Select(g => g.Status)
                .Distinct()
                .Prepend(new GoalStatus { Id = 0, Name = "Все статусы" })];

            cbStatusFilter.SelectedIndex = 0;
            cbSortBy.SelectedIndex = 0;

            DataContext = this;
        }
        private void GoalsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GoalsListBox.SelectedItem is FinancialGoal goal)
            {
                NavigationService?.Navigate(new GoalDetails(goal.Id, _goalService));
                GoalsListBox.SelectedItem = null;
            }
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            var goals = _goalService.LoadAllGoals().AsQueryable();

            if (_categoryTag != "0" && int.TryParse(_categoryTag, out int categoryId))
            {
                goals = goals.Where(g => g.CategoryId == categoryId);
            }

            if (cbStatusFilter.SelectedIndex > 0)
            {
                var statusId = ((GoalStatus)cbStatusFilter.SelectedItem).Id;
                goals = goals.Where(g => g.StatusId == statusId);
            }
            if (cbNearDeadline.IsChecked == true)
            {
                var limitDate = DateTime.Now.AddDays(7);
                goals = goals.Where(g => g.Deadline != null && g.Deadline <= limitDate);
            }

            switch (cbSortBy.SelectedIndex)
            {
                case 0:
                    goals = goals.OrderByDescending(g => g.CreatedAt);
                    break;
                case 1:
                    goals = goals.OrderBy(g => g.Deadline);
                    break;
                case 2:
                    goals = goals.OrderByDescending(g => g.PriorityId);
                    break;
                case 3:
                    goals = goals.OrderByDescending(g => g.CurrentAmount / g.TargetAmount);
                    break;
            }

            GoalList = [.. goals];
            GoalsListBox.ItemsSource = GoalList;
        }

        public void RefreshData(string categoryTag)
        {
            _categoryTag = categoryTag;
            FilterChanged(null, null);
        }

        private void BtnExportJson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "JSON файл (*.json)|*.json",
                    FileName = "financial_goals.json"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    var goals = _goalService.LoadAllGoals()
                    .Select(g => new
                    {
                        g.Id,
                        g.Name,
                        g.Description,
                        g.TargetAmount,
                        g.CurrentAmount,
                        g.CreatedAt,
                        g.Deadline,

                        Priority = new
                        {
                            g.Priority.Id,
                            g.Priority.Name
                        },

                        Status = new
                        {
                            g.Status.Id,
                            g.Status.Name
                        },

                        Category = new
                        {
                            g.Category.Id,
                            g.Category.Name
                        },

                        GoalPlans = g.GoalPlans.Select(p => new
                        {
                            p.PlannedAmount,
                            Frequency = p.Frequency.Name
                        }),

                        Contributions = g.GoalContributions.Select(c => new
                        {
                            c.Amount,
                            c.Date
                        })
                    })
                    .ToList();

                    var json = JsonSerializer.Serialize(goals, _exportJsonOptions);


                    File.WriteAllText(saveDialog.FileName, json);

                    MessageBox.Show("Данные успешно сохранены в JSON");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка экспорта: " + ex.Message);
            }
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            FilterChanged(null, null);
        }
    }
}
