using System;
using System.Windows;
using System.Data;
using System.IO;
using Microsoft.Win32;
using VirtualClassroom.DataLayer;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace VirtualClassroom
{
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
            QuestPDF.Settings.License = LicenseType.Community;

            DateFrom.SelectedDate = DateTime.Today.AddMonths(-1);
            DateTo.SelectedDate = DateTime.Today;
           
        }

        private void ChkFilterByDate_Checked(object sender, RoutedEventArgs e)
        {
            PanelDateRange.Visibility = ChkFilterByDate.IsChecked == true
                ? Visibility.Visible
                : Visibility.Collapsed;
        }


        private void PreviewReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable reportData = GetReportData();

                if (reportData.Rows.Count == 0)
                {
                    MessageBox.Show("No data found for the selected report.", "No Data",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // 1. Create a path for a temporary PDF file
                string tempPath = Path.Combine(Path.GetTempPath(), $"{reportData.TableName}_Preview.pdf");

                // 2. Generate the PDF (Using the same logic as ExportToPDF)
                GenerateDocument(reportData, tempPath);

                // 3. Open the file in the default PDF viewer
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempPath,
                    UseShellExecute = true // This is the key to opening the file
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating preview: {ex.Message}", "Error",
                                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // To avoid writing the PDF code twice, create this helper method:
        private void GenerateDocument(DataTable data, string filePath)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header().Text($"{data.TableName}").FontSize(24).SemiBold().FontColor("#1E3A8A");

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (DataColumn col in data.Columns) columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            foreach (DataColumn col in data.Columns)
                                header.Cell().Background("#3B82F6").Padding(5).Text(col.ColumnName).FontColor("#FFFFFF").SemiBold();
                        });

                        foreach (DataRow row in data.Rows)
                        {
                            foreach (var item in row.ItemArray)
                                table.Cell().BorderBottom(1).BorderColor("#E2E8F0").Padding(5).Text(item?.ToString() ?? string.Empty);
                        }
                    });

                    page.Footer().AlignCenter().Text(x => {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
                });
            }).GeneratePdf(filePath);
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable reportData = GetReportData();

                if (reportData.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "No Data",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Check which export options are selected
                bool exportPDF = ChkExportPDF.IsChecked == true;
              
                bool printReport = ChkPrint.IsChecked == true;

                if (!exportPDF  && !printReport)
                {
                    MessageBox.Show("Please select at least one export option.", "Select Option",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Generate reports based on selections
              
                if (exportPDF)
                {
                    ExportToPDF(reportData);
                }

                if (printReport)
                {
                    PrintReport(reportData);
                }

                MessageBox.Show("Report generation completed successfully!", "Success",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private DataTable GetReportData()
        {
            DataTable data = new DataTable();

            if (RdoStudentList.IsChecked == true)
            {
                data = DatabaseHelper.GetAllStudents();
                data.TableName = "StudentList";
            }
            else if (RdoCourseList.IsChecked == true)
            {
                // You'll need to add this method to DatabaseHelper
                data = DatabaseHelper.GetAllCoursesTable();
                data.TableName = "CourseList";
            }
            else if (RdoEnrollment.IsChecked == true)
            {
                data = DatabaseHelper.GetEnrollmentData();
                data.TableName = "EnrollmentReport";
            }
            else if (RdoGradeSummary.IsChecked == true)
            {
                // You'll need to add this method to DatabaseHelper
                data = DatabaseHelper.GetGradeSummary();
                data.TableName = "GradeSummary";
            }

           

            return data;
        }

       
       
        private void ExportToPDF(DataTable data)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"{data.TableName}_{DateTime.Now:yyyyMMdd}"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Generate the PDF using QuestPDF
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(50);
                        page.Header().Text($"{data.TableName}").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                        page.Content().PaddingVertical(10).Table(table =>
                        {
                            // Define columns based on DataTable
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (DataColumn col in data.Columns)
                                    columns.RelativeColumn();
                            });

                            // Add Header Row
                            table.Header(header =>
                            {
                                foreach (DataColumn col in data.Columns)
                                    header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text(col.ColumnName).SemiBold();
                            });

                            // Add Data Rows
                            foreach (DataRow row in data.Rows)
                            {
                                foreach (var item in row.ItemArray)
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).Padding(5).Text(item?.ToString() ?? string.Empty);
                            }
                        });
                    });
                }).GeneratePdf(saveFileDialog.FileName);

                MessageBox.Show("PDF saved successfully!", "Success");
            }
        }

        private void PrintReport(DataTable data)
        {
            MessageBox.Show("Print functionality would open print dialog here.",
                          "Print Report", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}