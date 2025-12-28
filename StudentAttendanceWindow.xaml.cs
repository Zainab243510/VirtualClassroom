using System.Data;
using System.Windows;
using VirtualClassroom.DataLayer;

namespace VirtualClassroom
{
    public partial class StudentAttendanceWindow : Window
    {
        private int _studentId;

        public StudentAttendanceWindow(int studentId)
        {
            InitializeComponent();
            _studentId = studentId;
            LoadAttendance();
        }

        private void LoadAttendance()
        {
            // Call the database helper to get data for this specific student
            DataTable dt = DatabaseHelper.GetAttendanceByStudentId(_studentId);
            DgAttendance.ItemsSource = dt.DefaultView;
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}