using System.Windows;

public partial class DashboardWindow : Window
{
    public DashboardWindow()
    {
        InitializeComponent();
    }

    private void AddStudent_Click(object sender, RoutedEventArgs e)
    {
        // Opens the window you just created
        AddStudentWindow addStudentWindow = new AddStudentWindow();
        addStudentWindow.Show();
    }

    private void AddCourse_Click(object sender, RoutedEventArgs e)
    {
        // You will implement the AddCourseWindow next
        // AddCourseWindow addCourseWindow = new AddCourseWindow();
        // addCourseWindow.Show(); 
        MessageBox.Show("Add Course feature coming soon!");
    }

    private void AssignGrades_Click(object sender, RoutedEventArgs e)
    {
        // You will implement the AssignGradesWindow next
        // AssignGradesWindow gradesWindow = new AssignGradesWindow();
        // gradesWindow.Show();
        MessageBox.Show("Assign Grades feature coming soon!");
    }

    private void ViewData_Click(object sender, RoutedEventArgs e)
    {
        // Opens the window we discussed for simple data display
        ViewDataWindow viewDataWindow = new ViewDataWindow();
        viewDataWindow.Show();
    }

    private void Reports_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Report generation is under development!");
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        // Optional: Open the Login Window again and close the Dashboard
        LoginWindow loginWindow = new LoginWindow();
        loginWindow.Show();
        this.Close();
    }
}