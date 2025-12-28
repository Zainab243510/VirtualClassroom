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
using System.Collections.Generic;

namespace VirtualClassroom
{
    public partial class AssignGradesWindow : Window
    {
        public AssignGradesWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                // Assuming these methods return List<Student> and List<Course>
                List<Student> students = DatabaseHelper.GetAllStudentsList();
                List<Course> courses = DatabaseHelper.GetAllCoursesList();

                // Populate the ComboBoxes
                CboStudents.ItemsSource = students;
                CboCourses.ItemsSource = courses;

                // Optional: Select the first item by default
                if (students.Count > 0) CboStudents.SelectedIndex = 0;
                if (courses.Count > 0) CboCourses.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Database Error");
            }
        }
        private void SaveGrade_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validate Selections
            if (CboStudents.SelectedItem == null || CboCourses.SelectedItem == null)
            {
                MessageBox.Show("Please select both a student and a course.", "Input Error");
                return;
            }

            // 2. Extract Data (Casting the selected items back to their original object types)
            Student selectedStudent = (Student)CboStudents.SelectedItem;
            Course selectedCourse = (Course)CboCourses.SelectedItem;
            string grade = TxtGrade.Text.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(grade))
            {
                MessageBox.Show("Please enter a grade.", "Input Error");
                return;
            }

            // Basic grade validation (optional)
            if (grade.Length > 1)
            {
                MessageBox.Show("Grade must be a single letter (A, B, C, D, F).", "Input Error");
                return;
            }

            try
            {
                // 3. Call the Database Helper (You must ensure this method exists in DatabaseHelper.cs)
                bool success = DatabaseHelper.UpdateOrInsertEnrollment(
                    selectedStudent.StudentID,
                    selectedCourse.CourseID,
                    grade
                );

                // 4. Provide Feedback
                if (success)
                {
                    MessageBox.Show($"Grade '{grade}' assigned to {selectedStudent.StudentName} for {selectedCourse.CourseName}.", "Success");
                    TxtGrade.Clear(); // Clear the grade field
                }
                else
                {
                    MessageBox.Show("Failed to save grade. Check database constraints or connection.", "DB Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving: {ex.Message}", "System Error");
            }
        }
    }
}