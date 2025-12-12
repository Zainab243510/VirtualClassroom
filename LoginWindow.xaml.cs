using System.Windows;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        throw new NotImplementedException();
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