using System;
using System.Windows;
using VirtualClassroom.DataLayer;

namespace VirtualClassroom
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow addStudentWindow = new AddStudentWindow();
            addStudentWindow.Show();
        }

        private void AddCourse_Click(object sender, RoutedEventArgs e)
        {
            AddCourseWindow addCourseWindow = new AddCourseWindow();
            addCourseWindow.Show();
        }

        private void AssignGrades_Click(object sender, RoutedEventArgs e)
        {
            AssignGradesWindow gradesWindow = new AssignGradesWindow();
            gradesWindow.Show();
        }

        private void ViewData_Click(object sender, RoutedEventArgs e)
        {
            ViewDataWindow viewDataWindow = new ViewDataWindow();
            viewDataWindow.Show();
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            ReportsWindow reportsWindow = new ReportsWindow();
            reportsWindow.Show();
        }

        // UPDATED LOGOUT METHOD
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Create the RoleSelectionWindow instead of LoginWindow
            RoleSelectionWindow roleSelection = new RoleSelectionWindow();

            // Show the selection screen
            roleSelection.Show();

            // Close the current Dashboard
            this.Close();
        }

        private void Meetings_Click(object sender, RoutedEventArgs e)
        {
            Meeting meetingWindow = new Meeting();
            meetingWindow.Owner = this;
            meetingWindow.Show();
        }

        private void MarkAttendance_Click(object sender, RoutedEventArgs e)
        {
            MarkAttendanceWindow attendanceWin = new MarkAttendanceWindow();
            attendanceWin.Show();
        }
    }
}