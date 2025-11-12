using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    public sealed class SodaRepository
    {
        private readonly string _cs;

        public SodaRepository(string connectionString) => _cs = connectionString;

        /// <summary>
        /// Obtiene el próximo ID consecutivo para una nueva deducción de soda
        /// </summary>
        public long GetNextId()
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT ISNULL(MAX(Id), 0) + 1 FROM dbo.Soda";
            cn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt64(result);
        }

        /// <summary>
        /// Página de deducciones de soda con filtro opcional por empleado
        /// </summary>
        public (IEnumerable<Soda> items, int total) GetPage(int pageIndex, int pageSize, string? filtro)
        {
            using var cn = new SqlConnection(_cs);
            var sql = @"
                WITH Filtered AS (
                    SELECT s.*, e.Nombre + ' ' + e.Primer_Apellido as NombreEmpleado, e.Carne
                    FROM dbo.Soda s
                    INNER JOIN dbo.Empleados e ON s.Id_Empleado = e.Id
                    WHERE (@filtro IS NULL OR e.Carne LIKE '%' + @filtro + '%' OR e.Nombre LIKE '%' + @filtro + '%')
                ),
                Paged AS (
                    SELECT *, ROW_NUMBER() OVER (ORDER BY Fecha DESC, Id DESC) as RowNum
                    FROM Filtered
                )
                SELECT Id, Id_Empleado, Total, Id_Usuario, Anulada, Fecha
                FROM Paged
                WHERE RowNum BETWEEN @offset + 1 AND @offset + @pageSize;

                SELECT COUNT(*) FROM (
                    SELECT s.Id
                    FROM dbo.Soda s
                    INNER JOIN dbo.Empleados e ON s.Id_Empleado = e.Id
                    WHERE (@filtro IS NULL OR e.Carne LIKE '%' + @filtro + '%' OR e.Nombre LIKE '%' + @filtro + '%')
                ) T;";

            using var cmd = cn.CreateCommand();
            cmd.CommandText = sql;

            var offset = (pageIndex - 1) * pageSize;
            cmd.Parameters.Add("@filtro", SqlDbType.NVarChar, 100).Value = (object?)filtro ?? DBNull.Value;
            cmd.Parameters.Add("@offset", SqlDbType.Int).Value = offset;
            cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;

            cn.Open();

            var items = new List<Soda>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    items.Add(new Soda
                    {
                        Id = reader.GetInt64("Id"),
                        Id_Empleado = reader.GetInt64("Id_Empleado"),
                        Total = reader.GetDouble("Total"),
                        Id_Usuario = reader.GetInt32("Id_Usuario"),
                        Anulada = reader.GetBoolean("Anulada"),
                        Fecha = reader.GetDateTime("Fecha")
                    });
                }

                reader.NextResult();
                reader.Read();
                var total = reader.GetInt32(0);
                return (items, total);
            }
        }

        public Soda? GetById(long id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Id_Empleado, Total, Id_Usuario, Anulada, Fecha
                FROM dbo.Soda
                WHERE Id = @id";

            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
            cn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Soda
                {
                    Id = reader.GetInt64("Id"),
                    Id_Empleado = reader.GetInt64("Id_Empleado"),
                    Total = reader.GetDouble("Total"),
                    Id_Usuario = reader.GetInt32("Id_Usuario"),
                    Anulada = reader.GetBoolean("Anulada"),
                    Fecha = reader.GetDateTime("Fecha")
                };
            }
            return null;
        }

        /// <summary>
        /// Inserta una nueva deducción de soda y retorna el nuevo Id
        /// </summary>
        public long Insert(Soda soda)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SET DATEFORMAT DMY;
                INSERT INTO dbo.Soda (Id_Empleado, Total, Id_Usuario, Anulada, Fecha)
                VALUES (@Id_Empleado, @Total, @Id_Usuario, @Anulada, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS bigint);";

            cmd.Parameters.Add("@Id_Empleado", SqlDbType.BigInt).Value = soda.Id_Empleado;
            cmd.Parameters.Add("@Total", SqlDbType.Float).Value = soda.Total;
            cmd.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = soda.Id_Usuario;
            cmd.Parameters.Add("@Anulada", SqlDbType.Bit).Value = soda.Anulada;

            cn.Open();
            return (long)cmd.ExecuteScalar()!;
        }

        public void Update(Soda soda)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                UPDATE dbo.Soda 
                SET Id_Empleado = @Id_Empleado,
                    Total = @Total,
                    Id_Usuario = @Id_Usuario,
                    Anulada = @Anulada,
                    Fecha = @Fecha
                WHERE Id = @Id";

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = soda.Id;
            cmd.Parameters.Add("@Id_Empleado", SqlDbType.Int).Value = soda.Id_Empleado;
            cmd.Parameters.Add("@Total", SqlDbType.Float).Value = soda.Total;
            cmd.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = soda.Id_Usuario;
            cmd.Parameters.Add("@Anulada", SqlDbType.Int).Value = soda.Anulada;
            cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = soda.Fecha;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM dbo.Soda WHERE Id = @Id";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Obtiene todas las deducciones de soda para un empleado específico
        /// </summary>
        public List<Soda> GetByEmpleadoId(long empleadoId)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Id_Empleado, Total, Id_Usuario, Anulada, Fecha
                FROM dbo.Soda
                WHERE Id_Empleado = @empleadoId
                ORDER BY Fecha DESC, Id DESC";

            cmd.Parameters.Add("@empleadoId", SqlDbType.BigInt).Value = empleadoId;
            cn.Open();

            var items = new List<Soda>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new Soda
                {
                    Id = reader.GetInt64("Id"),
                    Id_Empleado = reader.GetInt64("Id_Empleado"),
                    Total = reader.GetDouble("Total"),
                    Id_Usuario = reader.GetInt32("Id_Usuario"),
                    Anulada = reader.GetBoolean("Anulada"),
                    Fecha = reader.GetDateTime("Fecha")
                });
            }
            return items;
        }

        /// <summary>
        /// Anula una deducción de soda (actualiza campo Anulada = 1)
        /// </summary>
        public void Anular(long id, int usuarioId)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                UPDATE dbo.Soda 
                SET Anulada = 1, Id_Usuario = @Id_Usuario
                WHERE Id = @Id";

            cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;
            cmd.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = usuarioId;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // Suma el total de soda por empleado en un rango [desde..hasta] (inclusive por día)
        public double SumTotalByEmpleadoYRango(long idEmpleado, DateTime desde, DateTime hasta)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            // Inclusivo por la izquierda y exclusivo por la derecha:
            // Fecha >= desde  AND Fecha < (hasta + 1 día) evita el clásico 23:59:59
            cmd.CommandText = @"
        SELECT ISNULL(SUM(Total), 0)
        FROM dbo.Soda
        WHERE Id_Empleado = @emp
          AND Fecha >= @fini
          AND Fecha < DATEADD(DAY, 1, @ffin);";

            cmd.Parameters.Add("@emp", SqlDbType.BigInt).Value = idEmpleado;
            cmd.Parameters.Add("@fini", SqlDbType.DateTime).Value = desde.Date;
            cmd.Parameters.Add("@ffin", SqlDbType.DateTime).Value = hasta.Date;

            cn.Open();
            return Convert.ToDouble(cmd.ExecuteScalar());
        }

    }
}