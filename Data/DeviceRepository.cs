
using Dapper;
using Microsoft.Data.SqlClient;
using DevicesControlApp.Models;

namespace DevicesControlApp.Data
{
    internal class DeviceRepository
    {
        public void Add(Device device)
        {
            using var conn = new SqlConnection(DatabaseHelper.ConnectionString);

            string sql = @"
            INSERT INTO Device (Name, Type, Description, Status, CreatedAt, UpdatedAt)
            VALUES (@Name, @Type, @Description, @Status, @CreatedAt, @UpdatedAt);
        ";

            conn.Execute(sql, device);
        }
    }
}
