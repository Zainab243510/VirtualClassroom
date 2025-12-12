using System.Windows;
using System.Data; // Required for DataTable

public partial class ViewDataWindow : Window
{
    public ViewDataWindow()
    {
        InitializeComponent();
        LoadStudentData();
    }

    private void LoadStudentData()
    {
        try
        {
            // Assuming your DatabaseHelper.cs has a method that returns data as a DataTable
            DataTable dtStudents = DatabaseHelper.GetAllStudents();

            // Assign the DataTable directly to the DataGrid's ItemsSource
            StudentDataGrid.ItemsSource = dtStudents.DefaultView;

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading data: {ex.Message}", "Database Error");
        }
    }

    // NOTE: To implement color coding based on grade, you'd need to loop 
    // through the rows after loading or use WPF's styling/trigger system, 
    // which is more complex than simple code-behind.
}