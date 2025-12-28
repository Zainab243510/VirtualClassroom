using System;
using System.Windows;
using VirtualClassroom.DataLayer;

namespace VirtualClassroom
{
    public partial class AddStudentWindow : Window
    {
        public AddStudentWindow()
        {
            InitializeComponent();
            DtEnrollmentDate.SelectedDate = DateTime.Today;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Capture the inputs
                string studentId = TxtStudentID.Text;
                string name = TxtStudentName.Text;
                string major = TxtMajor.Text;
                string user = TxtUsername.Text;
                string pass = TxtPassword.Password;
                DateTime enrollDate = DtEnrollmentDate.SelectedDate ?? DateTime.Today;

                // 2. Validation
                if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                {
                    MessageBox.Show("Please fill in all fields (ID, Name, Username, and Password).", "Validation Error");
                    return;
                }

                // 3. CALL THE DATABASE 
                bool success = DatabaseHelper.InsertStudent(studentId, name, major, user, pass, enrollDate);

                // 4. Provide Feedback
                if (success)
                {
                    MessageBox.Show($"Student '{name}' added successfully!", "Success");
                    this.Close(); // Close the window on success
                }
                else
                {
                    // If DatabaseHelper returns false, it likely means a Unique Constraint error
                    MessageBox.Show("Failed to add student. The Student ID or Username might already exist in the database.", "Database Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "System Error");
            }
        }
    }
}