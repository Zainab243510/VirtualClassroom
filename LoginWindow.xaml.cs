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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VirtualClassroom.DataLayer;

namespace VirtualClassroom
{
    public partial class LoginWindow : Window
    {
        private void TestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isConnected = DatabaseHelper.TestConnection();

                if (isConnected)
                {
                    MessageBox.Show("✅ Database connection successful!\n\nYou can now login with:\nUsername: admin\nPassword: admin123",
                                   "Success",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Connection failed!\n\nError: {ex.Message}\n\nMake sure:\n1. SQL Server is running\n2. Database exists\n3. Connection string is correct",
                               "Connection Error",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            string username = TxtUsername.Text;
            // Use PasswordBox.Password for security
            string password = PwdPassword.Password;

            // Call your existing DatabaseHelper.cs method
            if (DatabaseHelper.ValidateAdmin(username, password))
            {
                MessageBox.Show("Login Successful!");

                // Open the main dashboard window
                DashboardWindow dashboard = new DashboardWindow();
                dashboard.Show();

                this.Close(); // Close the login window
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}