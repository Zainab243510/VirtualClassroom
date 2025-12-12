using System.Collections.Generic; // For List<T>
using System.Windows;

public partial class AssignGradesWindow : Window
{
    public AssignGradesWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            // Assuming these methods return List<Student> and List<Course>
            List<Student> students = DatabaseHelper.GetAllStudentsList();
            List<Course> courses = DatabaseHelper.GetAllCoursesList();

            // Populate the ComboBoxes
            CboStudents.ItemsSource = students;
            CboCourses.ItemsSource = courses;

            // Optional: Select the first item by default
            if (students.Count > 0) CboStudents.SelectedIndex = 0;
            if (courses.Count > 0) CboCourses.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading data: {ex.Message}", "Database Error");
        }
    }
    // ... (rest of the class)
}