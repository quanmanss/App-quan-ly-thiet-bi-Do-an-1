using Dapper;
using DevicesControlApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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
                string sql = "INSERT INTO Device (Name, Type, Description, Status, CreatedAt, UpdatedAt, RoomID) " +
                    "VALUES (@Name, @Type, @Description, @Status, GETDATE(), GETDATE(),@RoomID)";
                conn.Execute(sql, device);
            }
        }

        public static List<Device> GetDevices(int houseId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                string sql = @"
                SELECT d.*, r.Name AS RoomName
                FROM Device d
                INNER JOIN Room r ON d.RoomId = r.Id
                WHERE r.HouseId = @HouseId
                ORDER BY r.Name
            ";
                    var result = conn.Query<Device>(sql, new { HouseId = houseId }).AsList();

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


        public static int CreateAccount(Account account)
        {

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO Account (Username, Password, CreatedAt)
                    VALUES (@Username, @Password, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";


                string hashedPassword = HashPassword(account.Password);

                int newId = conn.QuerySingle<int>(sql, new
                {
                    account.Username,
                    Password = hashedPassword
                });

                return newId;
            }
        }


        public static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }


        public static User? Login(string username, string passwordInput)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionString))
                {
                    db.Open();

                    string sql = @"
                        SELECT u.ID, u.Name
                        FROM [User] u
                        JOIN [Account] a ON u.AccountID = a.ID
                        WHERE a.Username = @Username AND a.Password = @Password";

                    string hashedPassword = HashPassword(passwordInput.Trim());
                    return db.QueryFirstOrDefault<User>(sql,
                                        new { Username = username.Trim(), Password = hashedPassword });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
                return null;
            }
        }


        public static void CreateUser(User user)
        {   
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO [User] (Name, Email, AccountID)
                       VALUES (@Name, @Email, @AccountID)";

                conn.Execute(sql, user);
            }
        }


    public static int CreateHouse(House house)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                string sql = @"
            INSERT INTO House (Name, Location, OwnerUserID)
            VALUES (@Name, @Location, @OwnerUserID);
            SELECT CAST(SCOPE_IDENTITY() as int);
        ";

                return conn.QuerySingle<int>(sql, house);
            }
        }

        public static void AddHouseMember(int houseId, int userId, string role)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                string sql = @"
                    INSERT INTO HouseMember (HouseID, UserID, Role)
                    VALUES (@HouseID, @UserID, @Role);
                ";

                conn.Execute(sql, new
                {
                    HouseID = houseId,
                    UserID = userId,
                    Role = role
                });
            }
        }

        public static void CreateRoom(int houseId, string roomName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = @"
            INSERT INTO Room (HouseID, Name)
            VALUES (@HouseID, @Name);";

                connection.Execute(sql, new { HouseID = houseId, Name = roomName });
            }
        }

        public static int? GetUserHouseId(int userId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                db.Open();
                string sql = @"SELECT TOP 1 HouseID FROM HouseMember WHERE UserID = @UserID";
                return db.QueryFirstOrDefault<int?>(sql, new { UserID = userId });
            }
        }

        public static List<Room> GetRoomsByHouse(int houseId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                db.Open();
                string sql = "SELECT * FROM Room WHERE HouseID = @HouseID";
                return db.Query<Room>(sql, new { HouseID = houseId }).ToList();
            }
        }

    }
}