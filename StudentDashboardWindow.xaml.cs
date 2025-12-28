using System;
using System.Windows;
using System.Windows.Threading; 
using VirtualClassroom.DataLayer; 

namespace VirtualClassroom
{
    public partial class StudentDashboardWindow : Window
    {
        // This variable stores the ID of the student who just logged in
        private int _currentStudentId;
        private DispatcherTimer _statusTimer; // Added for live status checking
        public StudentDashboardWindow(int studentId)
        {
            InitializeComponent();
            _currentStudentId = studentId;

            // Start checking for live status
            StartLiveStatusCheck();
        }
        private void StartLiveStatusCheck()
        {
            _statusTimer = new DispatcherTimer();
            _statusTimer.Interval = TimeSpan.FromSeconds(5); // Checks every 5 seconds
            _statusTimer.Tick += (s, e) => UpdateBadge();
            _statusTimer.Start();
            UpdateBadge(); // Initial check
        }
        private void UpdateBadge()
        {
            // Note: This requires x:Name="LiveBadge" in your XAML
            bool isLive = DatabaseHelper.IsMeetingLive();
            if (LiveBadge != null)
            {
                LiveBadge.Visibility = isLive ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        private void JoinMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            // Only allows joining if the meeting is actually live
            if (DatabaseHelper.IsMeetingLive())
            {
                StudentMeeting meetingWindow = new StudentMeeting();
                meetingWindow.Show();
            }
            else
            {
                MessageBox.Show("The teacher has not started the meeting yet.", "Meeting Offline");
            }
        }
        private void ViewAttendance_Click(object sender, RoutedEventArgs e)
        {
            // Open the Attendance window and pass the student ID
            StudentAttendanceWindow attendanceWin = new StudentAttendanceWindow(_currentStudentId);
            attendanceWin.Show();
        }
        private void ViewGrades_Click(object sender, RoutedEventArgs e)
        {
            // Open the Grades window and pass the student ID
            StudentGradesWindow gradesWin = new StudentGradesWindow(_currentStudentId);
            gradesWin.Show();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _statusTimer?.Stop(); // Stop the timer before closing
            // Return to the Role Selection screen
            RoleSelectionWindow roleWin = new RoleSelectionWindow();
            roleWin.Show();
            this.Close();
        }
    }
}