using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace VirtualClassroom.DataLayer
{
    public static class DatabaseHelper
    {

        private static readonly string ConnectionString =
            "Data Source=localhost\\SQLEXPRESS;Initial Catalog=VirtualClassroomDB;Integrated Security=True;TrustServerCertificate=True";

        // Test connection method
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed!\n\nError: {ex.Message}\n\nCurrent Connection String:\n{ConnectionString}",
                               "Database Error",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                return false;
            }
        }

        // Validate Admin Login
        public static bool ValidateAdmin(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(1) FROM Admins WHERE Username = @User AND Password = @Pass";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@User", username);
                    command.Parameters.AddWithValue("@Pass", password);

                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count == 1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Database error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Insert Student
        public static bool InsertStudent(string name, string major, string user, string pass)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Students (StudentName, Major, Username, Password) VALUES (@name, @major, @user, @pass)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@major", major);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", pass);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Database Error: " + ex.Message);
                    return false;
                }
            }
        }

        // Get All Students as List
        public static List<Student> GetAllStudentsList()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT StudentID, StudentName, Major FROM Students ORDER BY StudentName";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                students.Add(new Student
                                {
                                    StudentID = reader.GetInt32(0),
                                    StudentName = reader.GetString(1),
                                    Major = reader.GetString(2)
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database Error fetching student list: {ex.Message}");
                    }
                }
            }
            return students;
        }

        // Get All Courses as List
        public static List<Course> GetAllCoursesList()
        {
            List<Course> courses = new List<Course>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT CourseID, CourseName, Description FROM Courses ORDER BY CourseName";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                courses.Add(new Course
                                {
                                    CourseID = reader.GetInt32(0),
                                    CourseName = reader.GetString(1),
                                    Description = reader.GetString(2)
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database Error fetching course list: {ex.Message}");
                    }
                }
            }
            return courses;
        }

        // Get All Students as DataTable
        public static DataTable GetAllStudents()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT * FROM Students";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database Error fetching students: {ex.Message}");
                    }
                }
            }
            return dt;
        }

        // Insert Course
        public static bool InsertCourse(int id, string name, string description)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Courses (CourseID, CourseName, Description) VALUES (@Id, @Name, @Desc)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Desc", description);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected == 1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database error during course insertion: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        // Get Enrollment Data
        public static DataTable GetEnrollmentData()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = @"
                    SELECT 
                        S.StudentName AS [Student Name],
                        C.CourseName AS [Course Title],
                        E.Grade AS Grade,
                        S.StudentID,
                        C.CourseID
                    FROM Enrollments E
                    JOIN Students S ON E.StudentID = S.StudentID
                    JOIN Courses C ON E.CourseID = C.CourseID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database Error fetching enrollment data: {ex.Message}");
                    }
                }
            }
            return dt;
        }

        public static bool UpdateOrInsertEnrollment(int studentId, int courseId, string grade)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Check if enrollment exists
                    string checkSql = "SELECT COUNT(1) FROM Enrollments WHERE StudentID = @StudentId AND CourseID = @CourseId";

                    using (SqlCommand checkCommand = new SqlCommand(checkSql, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@StudentId", studentId);
                        checkCommand.Parameters.AddWithValue("@CourseId", courseId);

                        int count = (int)checkCommand.ExecuteScalar();
                        string sql;

                        if (count > 0)
                        {
                            // Update existing grade
                            sql = "UPDATE Enrollments SET Grade = @Grade WHERE StudentID = @StudentId AND CourseID = @CourseId";
                        }
                        else
                        {
                            // Insert new enrollment
                            sql = "INSERT INTO Enrollments (StudentID, CourseID, Grade) VALUES (@StudentId, @CourseId, @Grade)";
                        }

                        // Execute the update or insert
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@StudentId", studentId);
                            command.Parameters.AddWithValue("@CourseId", courseId);
                            command.Parameters.AddWithValue("@Grade", grade);

                            int rowsAffected = command.ExecuteNonQuery();
                            return rowsAffected == 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving grade: {ex.Message}", "Database Error");
                return false;
            }
        }
        // Add these methods to your existing DatabaseHelper class
        public static DataTable GetAllCoursesTable()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT CourseID, CourseName, Description, CreatedDate FROM Courses ORDER BY CourseName";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error fetching courses: {ex.Message}");
                    }
                }
            }
            return dt;
        }
        public static DataTable GetGradeSummary()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = @"
            SELECT 
                Grade,
                COUNT(*) as StudentCount,
                AVG(CASE WHEN Grade = 'A' THEN 4
                         WHEN Grade = 'B' THEN 3
                         WHEN Grade = 'C' THEN 2
                         WHEN Grade = 'D' THEN 1
                         WHEN Grade = 'F' THEN 0
                         ELSE NULL END) as AverageGPA
            FROM Enrollments 
            WHERE Grade IS NOT NULL
            GROUP BY Grade
            ORDER BY Grade";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error fetching grade summary: {ex.Message}");
                    }
                }
            }
            return dt;
        }
        public static bool IsMeetingLive()
        {
            bool isLive = false;
            // Uses the same ConnectionString your other methods use
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT IsLive FROM MeetingStatus WHERE ID = 1";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            isLive = Convert.ToBoolean(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error checking meeting status: {ex.Message}");
                    }
                }
            }
            return isLive;
        }
        public static void SetMeetingStatus(bool status)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sql = "UPDATE MeetingStatus SET IsLive = @status WHERE ID = 1";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating meeting status: {ex.Message}");
                    }
                }
            }
        }
        // For Student Login
        public static int ValidateStudent(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT StudentID FROM Students WHERE Username = @user AND Password = @pass";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Login Error: " + ex.Message);
                        return -1;
                    }
                }
            }
        }

        // For Adding New Students
        public static bool InsertStudent(string name, string major)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Students (StudentName, Major) VALUES (@name, @major)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@major", major);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
     

        public static DataTable GetGradesByStudentId(int studentId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // This query joins Enrollments and Courses to show the Name instead of just an ID
                string sql = @"SELECT C.CourseName, E.Grade 
                       FROM Enrollments E 
                       JOIN Courses C ON E.CourseID = C.CourseID 
                       WHERE E.StudentID = @id";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@id", studentId);

                try
                {
                    conn.Open();
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading grades: " + ex.Message);
                }
            }
            return dt;
        }
        public static bool InsertStudent(string studentId, string name, string major, string user, string pass, DateTime enrollDate)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // Updated SQL to include EnrollmentDate and StudentID
                string sql = @"INSERT INTO Students (StudentID, StudentName, Major, Username, Password, EnrollmentDate) 
                               VALUES (@id, @name, @major, @user, @pass, @date)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", studentId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@major", major);
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.Parameters.AddWithValue("@date", enrollDate); // Fixes NULL EnrollmentDate error

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SqlException ex)
                    {
                        // Specifically catches primary key/unique constraint violations
                        if (ex.Number == 2627 || ex.Number == 2601)
                        {
                            MessageBox.Show("Error: Student ID or Username already exists. Please use unique values.", "Duplicate Entry");
                        }
                        else
                        {
                            MessageBox.Show("Database Error: " + ex.Message);
                        }
                        return false;
                    }
                }
            }
        }
        public static DataTable GetAttendanceByStudentId(int studentId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                // Query assumes an Attendance table with StudentID, CourseID, AttendanceDate, and Status
                string sql = @"SELECT A.AttendanceDate, C.CourseName, A.Status 
                       FROM Attendance A
                       JOIN Courses C ON A.CourseID = C.CourseID 
                       WHERE A.StudentID = @id
                       ORDER BY A.AttendanceDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@id", studentId);

                try
                {
                    conn.Open();
                    da.Fill(dt);
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show("Attendance Error: " + ex.Message);
                }
            }
            return dt;
        }
    public static DataTable GetStudentsByCourse(int courseId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT S.StudentID, S.StudentName 
                       FROM Students S
                       JOIN Enrollments E ON S.StudentID = E.StudentID
                       WHERE E.CourseID = @courseId";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@courseId", courseId);
                da.Fill(dt);
            }
            return dt;
        }

        public static void SaveAttendance(int studentId, int courseId, DateTime date, string status)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"IF EXISTS (SELECT 1 FROM Attendance WHERE StudentID=@s AND CourseID=@c AND AttendanceDate=@d)
                               UPDATE Attendance SET Status=@st WHERE StudentID=@s AND CourseID=@c AND AttendanceDate=@d
                               ELSE
                               INSERT INTO Attendance (StudentID, CourseID, AttendanceDate, Status) VALUES (@s, @c, @d, @st)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@s", studentId);
                cmd.Parameters.AddWithValue("@c", courseId);
                cmd.Parameters.AddWithValue("@d", date.Date);
                cmd.Parameters.AddWithValue("@st", status);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public static DataTable GetAllCourses()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT CourseID, CourseName FROM Courses";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                try { conn.Open(); da.Fill(dt); }
                catch (Exception ex) { MessageBox.Show("Error loading courses: " + ex.Message); }
            }
            return dt;
        }
    } 
}