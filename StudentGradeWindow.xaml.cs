using System.Data;
using System.Windows;
using VirtualClassroom.DataLayer;

namespace VirtualClassroom
{
    public partial class StudentGradesWindow : Window
    {
        private int _studentId;

        public StudentGradesWindow(int studentId)
        {
            InitializeComponent();
            _studentId = studentId;
            LoadMyGrades();
        }

        private void LoadMyGrades()
        {
            // Fetch only the grades for THIS student
            DataTable dt = DatabaseHelper.GetGradesByStudentId(_studentId);
            DgStudentGrades.ItemsSource = dt.DefaultView;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Return to the Dashboard
        }
    }
}