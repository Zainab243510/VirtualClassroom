using System.Configuration; // Optional, for reading connection string from App.config
using System.Data;
using System.Data.SqlClient; // Crucial namespace for SQL Server connections

namespace VirtualClassroom.DataLayer
{
    public static class DatabaseHelper
    {
        // 1. Connection String: Replace this with your actual SQL Server connection details
        private static readonly string ConnectionString =
            "Data Source=YOUR_SERVER_NAME;Initial Catalog=YOUR_DB_NAME;User ID=YOUR_USER;Password=YOUR_PASSWORD;";
        // OR use Windows Authentication: "Data Source=.;Initial Catalog=YOUR_DB_NAME;Integrated Security=True;";


        // 2. Method Example: Validate Admin Login (used by LoginForm)
        public static bool ValidateAdmin(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                // ALWAYS use parameterized queries for safety!
                string sql = "SELECT COUNT(1) FROM Admins WHERE Username = @User AND Password = @Pass";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@User", username);
                    command.Parameters.AddWithValue("@Pass", password);

                    try
                    {
                        connection.Open();
                        // ExecuteScalar returns the first column of the first row (our COUNT)
                        int count = (int)command.ExecuteScalar();
                        return count == 1;
                    }
                    catch (Exception ex)
                    {
                        // Log error here
                        MessageBox.Show("Database error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // 3. Method Example: Insert Student (used by AddStudentWindow)
        // ... (Similar structure with INSERT command)

    }
}