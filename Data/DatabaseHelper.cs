using Dapper;
using DevicesControlApp.Models;
using Microsoft.Data.SqlClient;

namespace DevicesControlApp.Data
{
    public static class DatabaseHelper
    {
        public static string ConnectionString =
      "Server=.;Database=DevicesControlApp;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection() =>
            new SqlConnection(ConnectionString);

        public static void AddDevice(Device device)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Device (Name, Type, Description, Status, CreatedAt, UpdatedAt) VALUES (@Name, @Type, @Description, @Status, GETDATE(), GETDATE())";
                conn.Execute(sql, device);
            }
        }

        public static List<Device> GetDevices()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT * FROM Device";
                var result = conn.Query<Device>(sql).AsList();

                // gán nút
                foreach (var d in result)
                    d.ButtonLabel = d.Status == "Bật" ? "Tắt" : "Bật";

                return result;
            }
        }

        public static void UpdateStatus(int id, string status)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                string sql = "UPDATE Device SET Status=@Status, UpdatedAt=GETDATE() WHERE ID=@ID";
                conn.Execute(sql, new { ID = id, Status = status });
            }
        }

        public static void DeleteDevice(int id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                string sql = "DELETE FROM Device WHERE Id=@Id";
                conn.Execute(sql, new { Id = id });
            }
        }

        public static void AddSchedule(Schedule schedule)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO Schedule
                               (DeviceID, UserID, Action, Status, Time, CreatedAt, UpdatedAt)
                               VALUES (@DeviceID, @UserID, @Action, @Status, @Time, @CreatedAt, @UpdatedAt)";
                conn.Execute(sql, new
                {
                    schedule.DeviceID,
                    schedule.UserID,
                    schedule.Action,
                    schedule.Status,
                    schedule.Time,
                    schedule.CreatedAt,
                    schedule.UpdatedAt
                });
            }
        }
        public static List<Schedule> GetPendingSchedules()
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            string sql = "SELECT * FROM Schedule WHERE Status='Pending'";
            return conn.Query<Schedule>(sql).ToList();
        }

        public static void UpdateScheduleStatus(int scheduleId, string status)
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            string sql = "UPDATE Schedule SET Status=@Status, UpdatedAt=GETDATE() WHERE ID=@ID";
            conn.Execute(sql, new { ID = scheduleId, Status = status });
        }
    }
}
