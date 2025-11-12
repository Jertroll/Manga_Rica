using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.ENTITY.Clock;

namespace Manga_Rica_P1.DAL.Clock
{
    /// <summary>
    /// Acceso a dbo.GLogs (BD del reloj).
    /// Trae marcas crudas por enrollNo (IdDevice).
    /// </summary>
    public sealed class GLogsRepository
    {
        private readonly string _cs;
        private const int _timeout = 90;

        public GLogsRepository(string connectionString) => _cs = connectionString;

        private SqlConnection Open()
        {
            var cn = new SqlConnection(_cs);
            cn.Open();
            return cn;
        }

        /// <summary>
        /// Marcas por enrollNo en un rango [from, to), ordenadas por fecha.
        /// </summary>
        public List<GLog> GetMarksByEnrollNo(string enrollNo, DateTime from, DateTime to, bool ascending = true)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = $@"
SELECT gl._datetime, gl.enrollNo, gl.state
FROM dbo.GLogs gl
WHERE gl.enrollNo = @enroll
  AND gl._datetime >= @from AND gl._datetime < @to
ORDER BY gl._datetime {(ascending ? "ASC" : "DESC")};";
            cmd.Parameters.Add(new SqlParameter("@enroll", SqlDbType.VarChar, 50) { Value = enrollNo });
            cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.DateTime) { Value = from });
            cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.DateTime) { Value = to });
            cmd.CommandTimeout = _timeout;

            var list = new List<GLog>();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new GLog
                {
                    DateTime = rd.GetDateTime(0),
                    EnrollNo = rd.GetString(1),
                    State = rd.IsDBNull(2) ? 0 : rd.GetInt32(2)
                });
            }
            return list;
        }

        /// <summary>
        /// Rápida verificación de cantidad de marcas (para diagnósticos).
        /// </summary>
        public int CountMarksByEnrollNo(string enrollNo, DateTime from, DateTime to)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT COUNT(1)
FROM dbo.GLogs
WHERE enrollNo = @enroll
  AND _datetime >= @from AND _datetime < @to;";
            cmd.Parameters.Add(new SqlParameter("@enroll", SqlDbType.VarChar, 50) { Value = enrollNo });
            cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.DateTime) { Value = from });
            cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.DateTime) { Value = to });
            cmd.CommandTimeout = _timeout;

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}
