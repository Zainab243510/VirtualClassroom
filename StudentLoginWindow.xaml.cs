using System;
using System.Collections.Generic;
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
    public partial class StudentLoginWindow : Window
    {
        public StudentLoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string user = TxtUsername.Text;
            string pass = PwdPassword.Password;

            // Use a method that checks the Students table
            int studentId = DatabaseHelper.ValidateStudent(user, pass);

            if (studentId != -1)
            {
                // Pass the studentId to the dashboard so they see their own data
                StudentDashboardWindow dash = new StudentDashboardWindow(studentId);
                dash.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Student Credentials");
            }
        }

    }
}