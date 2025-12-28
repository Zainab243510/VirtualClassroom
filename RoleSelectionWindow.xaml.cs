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

namespace VirtualClassroom
{
    public partial class RoleSelectionWindow : Window
    {
        public RoleSelectionWindow()
        {
            InitializeComponent();
        }
        private void TeacherPortal_Click(object sender, RoutedEventArgs e)
        {
            // Opens the teacher login window you already created
            LoginWindow teacherLogin = new LoginWindow();
            teacherLogin.Show();
            this.Close();
        }

        private void StudentPortal_Click(object sender, RoutedEventArgs e)
        {
            // Opens a student login window 
            StudentLoginWindow studentLogin = new StudentLoginWindow();
            studentLogin.Show();
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}