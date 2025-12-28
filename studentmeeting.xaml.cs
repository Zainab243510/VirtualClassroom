using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Threading;
using VirtualClassroom.DataLayer; 

namespace VirtualClassroom
{
    public partial class StudentMeeting : Window
    {
        // Your Permanent Zoom Link
        private string zoomLink = "https://us05web.zoom.us/j/89924535172?pwd=UMk9AZdImpL8FZLO0LzYRasoPEeKZM.1";

        // Timer to check database every few seconds
        DispatcherTimer refreshTimer;

        public StudentMeeting()
        {
            InitializeComponent();

            // Initialize the timer
            refreshTimer = new DispatcherTimer();
            refreshTimer.Interval = TimeSpan.FromSeconds(5);
            refreshTimer.Tick += (s, e) => RefreshStatus();
            refreshTimer.Start();

            // Initial check
            RefreshStatus();
        }

        private void RefreshStatus()
        {
            // Uses the method from your DatabaseHelper
            bool isLive = DatabaseHelper.IsMeetingLive();

            if (isLive)
            {
                StatusDot.Fill = Brushes.LimeGreen;
                TxtMeetingStatus.Text = "CLASS IS LIVE";
                BtnAction.Content = "Join Zoom Meeting";
                BtnAction.Background = new SolidColorBrush(Color.FromRgb(34, 197, 94)); // Success Green
                BtnAction.IsEnabled = true;
                TxtAutoRefresh.Text = "Teacher has started the meeting!";
            }
            else
            {
                StatusDot.Fill = Brushes.Red;
                TxtMeetingStatus.Text = "OFFLINE";
                BtnAction.Content = "Waiting for Teacher...";
                BtnAction.Background = new SolidColorBrush(Color.FromRgb(71, 85, 105)); // Muted Gray
                BtnAction.IsEnabled = false;
                TxtAutoRefresh.Text = "Checking for updates every 5s...";
            }
        }

        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(zoomLink) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open Zoom: " + ex.Message);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            refreshTimer.Stop(); // Stop timer before closing
            this.Close();
        }
    }
}