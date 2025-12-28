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
    public partial class AddCourseWindow : Window
    {
        public AddCourseWindow()
        {
            InitializeComponent();
        }
        private void SaveCourse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Get and Validate Data from UI Controls
                if (!int.TryParse(TxtCourseID.Text, out int courseId))
                {
                    MessageBox.Show("Please enter a valid Course ID (number).", "Input Error");
                    return;
                }

                string name = TxtCourseName.Text.Trim();
                string description = TxtDescription.Text.Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Please enter a course name.", "Input Error");
                    return;
                }

                // 2. Call the Database Helper 
                bool success = DatabaseHelper.InsertCourse(courseId, name, description);

                // 3. Provide Feedback
                if (success)
                {
                    MessageBox.Show($"Course '{name}' added successfully!", "Success");
                    // Clear fields for the next entry
                    TxtCourseID.Clear();
                    TxtCourseName.Clear();
                    TxtDescription.Clear();
                }
                else
                {
                    MessageBox.Show("Failed to add course. Check database connection or constraints.", "DB Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "System Error");
            }
        }
    }
}
