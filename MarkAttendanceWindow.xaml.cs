using System;
using System.Data;
using System.Windows;
using VirtualClassroom.DataLayer;

namespace VirtualClassroom
{
    public partial class MarkAttendanceWindow : Window
    {
        public MarkAttendanceWindow()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void LoadCourses()
        {
            CboCourses.ItemsSource = DatabaseHelper.GetAllCourses().DefaultView;
        }

        private void CboCourses_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CboCourses.SelectedValue != null)
            {
                int courseId = (int)CboCourses.SelectedValue;
                // Fetch students enrolled in this course to fill the grid
                DataTable dt = DatabaseHelper.GetStudentsByCourse(courseId);
                // Add a temporary 'Status' column for the UI if it doesn't exist
                if (!dt.Columns.Contains("Status")) dt.Columns.Add("Status");
                DgAttendanceList.ItemsSource = dt.DefaultView;
            }
        }

        private void SaveAttendance_Click(object sender, RoutedEventArgs e)
        {
            int courseId = (int)CboCourses.SelectedValue;
            DateTime date = DpDate.SelectedDate ?? DateTime.Now;

            foreach (DataRowView row in DgAttendanceList.ItemsSource)
            {
                int studentId = (int)row["StudentID"];
                string status = row["Status"]?.ToString() ?? "Absent";

                DatabaseHelper.SaveAttendance(studentId, courseId, date, status);
            }

            MessageBox.Show("Attendance saved successfully!");
            this.Close();
        }
    }
}