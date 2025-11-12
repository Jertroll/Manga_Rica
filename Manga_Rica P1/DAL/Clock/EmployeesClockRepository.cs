using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.ENTITY.Clock;

namespace Manga_Rica_P1.DAL.Clock
{
    /// <summary>
    /// Acceso a dbo.employees (BD del reloj).
    /// Mapea: id (bigint), code (varchar), IdDevice (varchar).
    /// </summary>
    public sealed class EmployeesClockRepository
    {
        private readonly string _cs;
        private const int _timeout = 60;

        public EmployeesClockRepository(string connectionString) => _cs = connectionString;

        private SqlConnection Open()
        {
            var cn = new SqlConnection(_cs);
            cn.Open();
            return cn;
        }

        public bool TestConnection()
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT 1";
            cmd.CommandTimeout = _timeout;
            return (int)cmd.ExecuteScalar() == 1;
        }

        public EmployeesClock? GetByCode(string code)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP(1) id, code, IdDevice
FROM dbo.employees
WHERE code = @code;";
            cmd.Parameters.Add(new SqlParameter("@code", SqlDbType.VarChar, 50) { Value = code });
            cmd.CommandTimeout = _timeout;

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            return new EmployeesClock
            {
                Id = rd.GetInt64(0),
                Code = rd.GetString(1),
                IdDevice = rd.IsDBNull(2) ? "" : rd.GetString(2)
            };
        }

        public EmployeesClock? GetById(long id)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP(1) id, code, IdDevice
FROM dbo.employees
WHERE id = @id;";
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt) { Value = id });
            cmd.CommandTimeout = _timeout;

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            return new EmployeesClock
            {
                Id = rd.GetInt64(0),
                Code = rd.GetString(1),
                IdDevice = rd.IsDBNull(2) ? "" : rd.GetString(2)
            };
        }

        public EmployeesClock? GetByIdDevice(string idDevice)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP(1) id, code, IdDevice
FROM dbo.employees
WHERE IdDevice = @idDevice;";
            cmd.Parameters.Add(new SqlParameter("@idDevice", SqlDbType.VarChar, 50) { Value = idDevice });
            cmd.CommandTimeout = _timeout;

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            return new EmployeesClock
            {
                Id = rd.GetInt64(0),
                Code = rd.GetString(1),
                IdDevice = rd.IsDBNull(2) ? "" : rd.GetString(2)
            };
        }
    }
}
