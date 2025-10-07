using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;
using System.Globalization;

namespace Manga_Rica_P1.DAL
{
    public sealed class SolicitudRepository
    {
        private readonly string _cs;
        public SolicitudRepository(string connectionString) => _cs = connectionString;

        public (IEnumerable<Solicitudes> items, int total) GetPage(int pageIndex, int pageSize, string? filtro)
        {
            int offset = Math.Max(0, (pageIndex - 1) * pageSize);
            var items = new List<Solicitudes>();
            int total = 0;

            long? fLong = null;
            DateTime? fDate = null;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim();
                if (long.TryParse(f, out var n)) fLong = n;
                if (DateTime.TryParseExact(f, new[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) || DateTime.TryParse(f, out dt))
                    fDate = dt.Date;
            }

            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
SELECT s.Id, s.Cedula, s.Primer_Apellido, s.Segundo_Apellido, s.Nombre,
       s.Fecha_Nacimiento, s.Estado_Civil, s.Telefono, s.Celular, s.Nacionalidad, s.Laboro, s.Direccion
FROM dbo.Solicitudes s
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), s.Id) LIKE '%' + @f + '%'
    OR s.Cedula           LIKE '%' + @f + '%'
    OR s.Primer_Apellido  LIKE '%' + @f + '%'
    OR s.Segundo_Apellido LIKE '%' + @f + '%'
    OR s.Nombre           LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), s.Fecha_Nacimiento, 23) LIKE '%' + @f + '%'
    OR s.Estado_Civil     LIKE '%' + @f + '%'
    OR s.Telefono         LIKE '%' + @f + '%'
    OR s.Celular          LIKE '%' + @f + '%'
    OR s.Nacionalidad     LIKE '%' + @f + '%'
    OR s.Direccion        LIKE '%' + @f + '%'
    OR (@fLong IS NOT NULL AND s.Id = @fLong)
    OR (@fDate IS NOT NULL AND CAST(s.Fecha_Nacimiento AS date) = @fDate)
)
ORDER BY s.Id DESC
OFFSET @off ROWS FETCH NEXT @ps ROWS ONLY;

SELECT COUNT(*)
FROM dbo.Solicitudes s
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), s.Id) LIKE '%' + @f + '%'
    OR s.Cedula           LIKE '%' + @f + '%'
    OR s.Primer_Apellido  LIKE '%' + @f + '%'
    OR s.Segundo_Apellido LIKE '%' + @f + '%'
    OR s.Nombre           LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), s.Fecha_Nacimiento, 23) LIKE '%' + @f + '%'
    OR s.Estado_Civil     LIKE '%' + @f + '%'
    OR s.Telefono         LIKE '%' + @f + '%'
    OR s.Celular          LIKE '%' + @f + '%'
    OR s.Nacionalidad     LIKE '%' + @f + '%'
    OR s.Direccion        LIKE '%' + @f + '%'
    OR (@fLong IS NOT NULL AND s.Id = @fLong)
    OR (@fDate IS NOT NULL AND CAST(s.Fecha_Nacimiento AS date) = @fDate)
);";

            cmd.Parameters.Add("@f", SqlDbType.VarChar, 100).Value =
                string.IsNullOrWhiteSpace(filtro) ? DBNull.Value : filtro!.Trim();
            cmd.Parameters.Add("@fLong", SqlDbType.BigInt).Value = fLong.HasValue ? fLong.Value : DBNull.Value;
            cmd.Parameters.Add("@fDate", SqlDbType.Date).Value = fDate.HasValue ? fDate.Value : DBNull.Value;
            cmd.Parameters.Add("@off", SqlDbType.Int).Value = offset;
            cmd.Parameters.Add("@ps", SqlDbType.Int).Value = pageSize;

            cn.Open();
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                int id = (int)Convert.ToInt64(rd.GetValue(0));      // Id BIGINT → int
                object laboroRaw = rd.GetValue(10);                 // Laboro BIT
                int laboro = laboroRaw is bool b ? (b ? 1 : 0) : Convert.ToInt32(laboroRaw);

                items.Add(new Solicitudes
                {
                    Id = id,
                    Cedula = rd.GetString(1),
                    Primer_Apellido = rd.GetString(2),
                    Segundo_Apellido = rd.IsDBNull(3) ? "" : rd.GetString(3),
                    Nombre = rd.GetString(4),
                    Fecha_Nacimiento = rd.GetDateTime(5),
                    Estado_Civil = rd.GetString(6),
                    Telefono = rd.IsDBNull(7) ? null : rd.GetString(7),  // NULLABLE
                    Celular = rd.GetString(8),
                    Nacionalidad = rd.GetString(9),
                    Laboro = laboro,
                    Direccion = rd.GetString(11)
                });
            }

            if (rd.NextResult() && rd.Read())
                total = (int)Convert.ToInt64(rd.GetValue(0));

            return (items, total);
        }

        public Solicitudes? GetById(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT Id, Cedula, Primer_Apellido, Segundo_Apellido, Nombre, Fecha_Nacimiento,
       Estado_Civil, Telefono, Celular, Nacionalidad, Laboro, Direccion
FROM dbo.Solicitudes WHERE Id=@id;";
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            int entityId = (int)Convert.ToInt64(rd.GetValue(0));
            object laboroRaw = rd.GetValue(10);
            int laboro = laboroRaw is bool b ? (b ? 1 : 0) : Convert.ToInt32(laboroRaw);

            return new Solicitudes
            {
                Id = entityId,
                Cedula = rd.GetString(1),
                Primer_Apellido = rd.GetString(2),
                Segundo_Apellido = rd.IsDBNull(3) ? "" : rd.GetString(3),
                Nombre = rd.GetString(4),
                Fecha_Nacimiento = rd.GetDateTime(5),
                Estado_Civil = rd.GetString(6),
                Telefono = rd.IsDBNull(7) ? null : rd.GetString(7),
                Celular = rd.GetString(8),
                Nacionalidad = rd.GetString(9),
                Laboro = laboro,
                Direccion = rd.GetString(11)
            };
        }

        public int Insert(Solicitudes s)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO dbo.Solicitudes
(Cedula, Primer_Apellido, Segundo_Apellido, Nombre, Fecha_Nacimiento,
 Estado_Civil, Telefono, Celular, Nacionalidad, Laboro, Direccion)
VALUES (@Cedula, @PA, @SA, @Nombre, @FechaNac, @Estado, @Tel, @Cel, @Nac, @Laboro, @Dir);
SELECT CAST(SCOPE_IDENTITY() AS int);";

            cmd.Parameters.Add("@Cedula", SqlDbType.VarChar, 50).Value = s.Cedula;
            cmd.Parameters.Add("@PA", SqlDbType.VarChar, 50).Value = s.Primer_Apellido;
            cmd.Parameters.Add("@SA", SqlDbType.VarChar, 50).Value = string.IsNullOrWhiteSpace(s.Segundo_Apellido) ? "" : s.Segundo_Apellido!;
            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = s.Nombre;
            cmd.Parameters.Add("@FechaNac", SqlDbType.DateTime).Value = s.Fecha_Nacimiento;
            cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = s.Estado_Civil;

            // ▼ Telefono NULLABLE
            cmd.Parameters.Add("@Tel", SqlDbType.VarChar, 50).Value =
                string.IsNullOrWhiteSpace(s.Telefono) ? DBNull.Value : s.Telefono!;

            cmd.Parameters.Add("@Cel", SqlDbType.VarChar, 50).Value = s.Celular;
            cmd.Parameters.Add("@Nac", SqlDbType.VarChar, 50).Value = s.Nacionalidad;
            cmd.Parameters.Add("@Laboro", SqlDbType.Bit).Value = (s.Laboro == 1);
            cmd.Parameters.Add("@Dir", SqlDbType.VarChar, 200).Value = s.Direccion;

            cn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public void Update(Solicitudes s)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
UPDATE dbo.Solicitudes
SET Cedula=@Cedula, Primer_Apellido=@PA, Segundo_Apellido=@SA, Nombre=@Nombre,
    Fecha_Nacimiento=@FechaNac, Estado_Civil=@Estado, Telefono=@Tel, Celular=@Cel, Nacionalidad=@Nac,
    Laboro=@Laboro, Direccion=@Dir
WHERE Id=@Id;";

            cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = s.Id;
            cmd.Parameters.Add("@Cedula", SqlDbType.VarChar, 50).Value = s.Cedula;
            cmd.Parameters.Add("@PA", SqlDbType.VarChar, 50).Value = s.Primer_Apellido;
            cmd.Parameters.Add("@SA", SqlDbType.VarChar, 50).Value = string.IsNullOrWhiteSpace(s.Segundo_Apellido) ? "" : s.Segundo_Apellido!;
            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = s.Nombre;
            cmd.Parameters.Add("@FechaNac", SqlDbType.DateTime).Value = s.Fecha_Nacimiento;
            cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 50).Value = s.Estado_Civil;

            cmd.Parameters.Add("@Tel", SqlDbType.VarChar, 50).Value =
                string.IsNullOrWhiteSpace(s.Telefono) ? DBNull.Value : s.Telefono!;

            cmd.Parameters.Add("@Cel", SqlDbType.VarChar, 50).Value = s.Celular;
            cmd.Parameters.Add("@Nac", SqlDbType.VarChar, 50).Value = s.Nacionalidad;
            cmd.Parameters.Add("@Laboro", SqlDbType.Bit).Value = (s.Laboro == 1);
            cmd.Parameters.Add("@Dir", SqlDbType.VarChar, 200).Value = s.Direccion;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM dbo.Solicitudes WHERE Id=@Id;";
            cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public bool ExistsByCedula(string cedula, int? exceptId = null)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP(1) 1
FROM dbo.Solicitudes
WHERE Cedula=@ced AND (@id IS NULL OR Id<>@id);";
            cmd.Parameters.Add("@ced", SqlDbType.VarChar, 50).Value = cedula;
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = (object?)exceptId ?? DBNull.Value;

            cn.Open();
            var x = cmd.ExecuteScalar();
            return x != null;
        }
    }
}
