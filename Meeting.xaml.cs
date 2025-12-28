using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using VirtualClassroom.DataLayer; 

namespace VirtualClassroom
{
    public partial class Meeting : Window
    {
        // Your permanent Zoom link
        private string zoomLink = "https://us05web.zoom.us/j/89924535172?pwd=UMk9AZdImpL8FZLO0LzYRasoPEeKZM.1";

        // This should be set based on your login system
        private bool isTeacher = true;

        public Meeting()
        {
            InitializeComponent();
            RefreshStatus();
        }

        private void RefreshStatus()
        {
            // Calls the new method we added to DatabaseHelper
            bool isLive = DatabaseHelper.IsMeetingLive();

            if (isLive)
            {
                StatusDot.Fill = Brushes.LimeGreen;
                TxtMeetingStatus.Text = "CLASS IS CURRENTLY LIVE";
                BtnAction.Content = "Join Zoom Meeting";
                BtnAction.Background = new SolidColorBrush(Color.FromRgb(34, 197, 94)); // Green
                BtnAction.IsEnabled = true;

                // Show "End" button only for teachers when class is live
                if (BtnEndMeeting != null)
                    BtnEndMeeting.Visibility = isTeacher ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                StatusDot.Fill = Brushes.Red;
                TxtMeetingStatus.Text = "No current meeting occurring.";
                if (BtnEndMeeting != null)
                    BtnEndMeeting.Visibility = Visibility.Collapsed;

                if (isTeacher)
                {
                    BtnAction.Content = "Start Meeting";
                    BtnAction.Background = new SolidColorBrush(Color.FromRgb(59, 130, 246)); // Blue
                    BtnAction.IsEnabled = true;
                }
                else
                {
                    BtnAction.Content = "Waiting for Teacher...";
                    BtnAction.Background = Brushes.Gray;
                    BtnAction.IsEnabled = false;
                }
            }
        }

        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            // If teacher starts a non-live meeting, update database first
            if (isTeacher && !DatabaseHelper.IsMeetingLive())
            {
                DatabaseHelper.SetMeetingStatus(true);
            }

            // Open the Zoom Link
            try
            {
                Process.Start(new ProcessStartInfo(zoomLink) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open Zoom link: " + ex.Message);
            }

            RefreshStatus();
        }

        private void BtnEndMeeting_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to end the session for all students?",
                                        "End Session", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DatabaseHelper.SetMeetingStatus(false);
                RefreshStatus();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshStatus();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}