using System.Windows;
using System;
// Make sure to include the namespace where your DatabaseHelper class is located
// using YourProjectNamespace.DataLayer; 

public partial class AddStudentWindow : Window
{
    public AddStudentWindow()
    {
        InitializeComponent();
        // Optionally set the default date to today
        DtEnrollmentDate.SelectedDate = DateTime.Today;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // 1. Get Data from UI Controls
            if (!int.TryParse(TxtStudentID.Text, out int studentId))
            {
                MessageBox.Show("Please enter a valid Student ID (number).", "Input Error");
                return;
            }

            string name = TxtStudentName.Text;
            string major = TxtMajor.Text;
            DateTime? enrollmentDate = DtEnrollmentDate.SelectedDate; // DateTime? allows null

            if (string.IsNullOrWhiteSpace(name) || !enrollmentDate.HasValue)
            {
                MessageBox.Show("Please fill in all required fields (Name and Date).", "Input Error");
                return;
            }

            // 2. Call the Database Helper (You'll need a method like this)
            bool success = DatabaseHelper.InsertStudent(
                studentId,
                name,
                major,
                enrollmentDate.Value
            );

            // 3. Provide Feedback
            if (success)
            {
                MessageBox.Show($"Student {name} added successfully!", "Success");
                // Clear fields for the next entry
                TxtStudentID.Clear();
                TxtStudentName.Clear();
                TxtMajor.Clear();
                DtEnrollmentDate.SelectedDate = DateTime.Today;
            }
            else
            {
                MessageBox.Show("Failed to add student. Check database connection or constraints.", "DB Error");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "System Error");
        }
    }
}