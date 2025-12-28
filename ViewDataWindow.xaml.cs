using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using VirtualClassroom.DataLayer;


namespace VirtualClassroom
{
    public partial class ViewDataWindow : Window
    {
        private DataTable? _fullData;
        public ViewDataWindow()
        {
            InitializeComponent();
            LoadFullData();
        }
        private void LoadFullData()
        {
            // Use the combined data table method
            _fullData = DatabaseHelper.GetEnrollmentData();
            DataGridEnrollments.ItemsSource = _fullData.DefaultView;
        }

        // --- Filtering and Searching Logic ---

        private void ApplyFilters()
        {
            if (_fullData == null) return;

            // Get the DataView to apply filtering
            DataView dv = _fullData.DefaultView;
            string filterExpression = "";

            // 1. Search by Student Name
            string searchText = TxtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                // Use LIKE syntax for DataView RowFilter
                filterExpression += $"[Student Name] LIKE '%{searchText}%'";
            }

            // 2. Filter by Grade
            ComboBoxItem? selectedGradeItem = CboGradeFilter.SelectedItem as ComboBoxItem;
            string? selectedGrade = selectedGradeItem?.Content?.ToString();

            if (selectedGrade != null && selectedGrade != "All Grades")
            {
                // If there's already a search filter, combine them with 'AND'
                if (!string.IsNullOrEmpty(filterExpression))
                {
                    filterExpression += " AND ";
                }
                filterExpression += $"Grade = '{selectedGrade}'";
            }

            // Apply the final filter string
            dv.RowFilter = filterExpression;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CboGradeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            TxtSearch.Clear();
            CboGradeFilter.SelectedIndex = 0; // Reset to "All Grades"
            ApplyFilters(); // This will apply an empty filter, resetting the view
        }
    }
}
