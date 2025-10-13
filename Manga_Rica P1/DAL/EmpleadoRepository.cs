using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    /// <summary>
    /// Acceso a datos para Empleados (tabla dbo.Empleados).
    /// </summary>
    public sealed class EmpleadoRepository
    {
        private readonly string _cs;
        public EmpleadoRepository(string connectionString) => _cs = connectionString;

        // =========================================================
        //  Paginación con filtro
        // =========================================================
        public (IEnumerable<Empleado> items, int total) GetPage(int pageIndex, int pageSize, string? filtro)
        {
            int offset = Math.Max(0, (pageIndex - 1) * pageSize);
            var items = new List<Empleado>();
            int total = 0;

            int? fInt = null;
            long? fLong = null;
            decimal? fDec = null;
            DateTime? fDate = null;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim();

                if (int.TryParse(f, out var n)) fInt = n;
                if (long.TryParse(f, out var lg)) fLong = lg;

                if (decimal.TryParse(f, NumberStyles.Any, CultureInfo.InvariantCulture, out var dec))
                    fDec = dec;

                if (DateTime.TryParseExact(f, new[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" },
                                           CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
                    || DateTime.TryParse(f, out dt))
                {
                    fDate = dt.Date;
                }
            }

            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();

            cmd.CommandText = @"
SELECT  e.Id, e.Carne, e.Cedula, e.Primer_Apellido, e.Segundo_Apellido, e.Nombre,
        e.Fecha_Nacimiento, e.Estado_Civil, e.Telefono, e.Celular, e.Nacionalidad,
        e.Laboro, e.Direccion, e.Id_Departamento, e.Salario, e.Puesto,
        e.Fecha_Ingreso, e.Fecha_Salida, e.Foto, e.Activo, e.MC_Numero
FROM dbo.Empleados e
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), e.Id) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), e.Carne) LIKE '%' + @f + '%'
    OR e.Cedula           LIKE '%' + @f + '%'
    OR e.Primer_Apellido  LIKE '%' + @f + '%'
    OR e.Segundo_Apellido LIKE '%' + @f + '%'
    OR e.Nombre           LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), e.Fecha_Nacimiento, 23) LIKE '%' + @f + '%'
    OR e.Estado_Civil     LIKE '%' + @f + '%'
    OR e.Telefono         LIKE '%' + @f + '%'
    OR e.Celular          LIKE '%' + @f + '%'
    OR e.Nacionalidad     LIKE '%' + @f + '%'
    OR e.Direccion        LIKE '%' + @f + '%'
    OR e.Puesto           LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), e.Fecha_Ingreso, 23) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), e.Fecha_Salida, 23) LIKE '%' + @f + '%'
    OR (@fInt  IS NOT NULL AND (e.Id = @fInt OR e.Id_Departamento = @fInt OR e.Activo = @fInt OR e.Laboro = @fInt))
    OR (@fLong IS NOT NULL AND (e.MC_Numero = @fLong OR e.Carne = @fLong))
    OR (@fDec  IS NOT NULL AND e.Salario = @fDec)
    OR (@fDate IS NOT NULL AND (
            CAST(e.Fecha_Nacimiento AS date) = @fDate
        OR  CAST(e.Fecha_Ingreso AS date)    = @fDate
        OR  CAST(e.Fecha_Salida AS date)     = @fDate
    ))
)
ORDER BY e.Id DESC
OFFSET @off ROWS FETCH NEXT @ps ROWS ONLY;

SELECT COUNT(*)
FROM dbo.Empleados e
WHERE (
    @f IS NULL
    OR CONVERT(nvarchar(20), e.Id) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(20), e.Carne) LIKE '%' + @f + '%'
    OR e.Cedula           LIKE '%' + @f + '%'
    OR e.Primer_Apellido  LIKE '%' + @f + '%'
    OR e.Segundo_Apellido LIKE '%' + @f + '%'
    OR e.Nombre           LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), e.Fecha_Nacimiento, 23) LIKE '%' + @f + '%'
    OR e.Estado_Civil     LIKE '%' + @f + '%'
    OR e.Telefono         LIKE '%' + @f + '%'
    OR e.Celular          LIKE '%' + @f + '%'
    OR e.Nacionalidad     LIKE '%' + @f + '%'
    OR e.Direccion        LIKE '%' + @f + '%'
    OR e.Puesto           LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), e.Fecha_Ingreso, 23) LIKE '%' + @f + '%'
    OR CONVERT(nvarchar(10), e.Fecha_Salida, 23) LIKE '%' + @f + '%'
    OR (@fInt  IS NOT NULL AND (e.Id = @fInt OR e.Id_Departamento = @fInt OR e.Activo = @fInt OR e.Laboro = @fInt))
    OR (@fLong IS NOT NULL AND (e.MC_Numero = @fLong OR e.Carne = @fLong))
    OR (@fDec  IS NOT NULL AND e.Salario = @fDec)
    OR (@fDate IS NOT NULL AND (
            CAST(e.Fecha_Nacimiento AS date) = @fDate
        OR  CAST(e.Fecha_Ingreso AS date)    = @fDate
        OR  CAST(e.Fecha_Salida AS date)     = @fDate
    ))
);";

            cmd.Parameters.Add("@f", SqlDbType.NVarChar, 120).Value =
                string.IsNullOrWhiteSpace(filtro) ? DBNull.Value : filtro!.Trim();

            var pInt = cmd.Parameters.Add("@fInt", SqlDbType.Int);
            pInt.Value = fInt.HasValue ? fInt.Value : DBNull.Value;

            var pLong = cmd.Parameters.Add("@fLong", SqlDbType.BigInt);
            pLong.Value = fLong.HasValue ? fLong.Value : DBNull.Value;

            var pDec = cmd.Parameters.Add("@fDec", SqlDbType.Decimal);
            pDec.Precision = 18; pDec.Scale = 2;
            pDec.Value = fDec.HasValue ? fDec.Value : DBNull.Value;

            var pDate = cmd.Parameters.Add("@fDate", SqlDbType.Date);
            pDate.Value = fDate.HasValue ? fDate.Value : DBNull.Value;

            cmd.Parameters.Add("@off", SqlDbType.Int).Value = offset;
            cmd.Parameters.Add("@ps", SqlDbType.Int).Value = pageSize;

            cn.Open();
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                items.Add(MapEmpleado(rd));
            }

            if (rd.NextResult() && rd.Read())
                total = Convert.ToInt32(rd.GetValue(0));

            return (items, total);
        }

        // =========================================================
        //  Getters
        // =========================================================
        public Empleado? GetById(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT Id, Carne, Cedula, Primer_Apellido, Segundo_Apellido, Nombre, Fecha_Nacimiento,
       Estado_Civil, Telefono, Celular, Nacionalidad, Laboro, Direccion, Id_Departamento,
       Salario, Puesto, Fecha_Ingreso, Fecha_Salida, Foto, Activo, MC_Numero
FROM dbo.Empleados WHERE Id=@id;";
            // La columna es BIGINT en BD
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return MapEmpleado(rd);
        }

        public Empleado? GetByCedula(string cedula)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP(1) Id, Carne, Cedula, Primer_Apellido, Segundo_Apellido, Nombre, Fecha_Nacimiento,
       Estado_Civil, Telefono, Celular, Nacionalidad, Laboro, Direccion, Id_Departamento,
       Salario, Puesto, Fecha_Ingreso, Fecha_Salida, Foto, Activo, MC_Numero
FROM dbo.Empleados WHERE Cedula=@ced;";
            cmd.Parameters.Add("@ced", SqlDbType.NVarChar, 50).Value = cedula;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return MapEmpleado(rd);
        }

        // =========================================================
        //  Insert / Update / Delete
        // =========================================================
        public int Insert(Empleado e)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO dbo.Empleados
(Carne, Cedula, Primer_Apellido, Segundo_Apellido, Nombre, Fecha_Nacimiento,
 Estado_Civil, Telefono, Celular, Nacionalidad, Laboro, Direccion, Id_Departamento,
 Salario, Puesto, Fecha_Ingreso, Fecha_Salida, Foto, Activo, MC_Numero)
VALUES
(@Carne,@Cedula,@PA,@SA,@Nombre,@FNac,@Estado,@Tel,@Cel,@Nac,@Laboro,@Dir,@IdDep,
 @Salario,@Puesto,@FIng,@FSal,@Foto,@Activo,@MC);
SELECT CAST(SCOPE_IDENTITY() AS bigint);"; // la PK es BIGINT

            cmd.Parameters.Add("@Carne", SqlDbType.BigInt).Value = e.Carne;
            cmd.Parameters.Add("@Cedula", SqlDbType.NVarChar, 50).Value = e.Cedula;
            // Columnas NOT NULL: nunca enviar DBNull
            cmd.Parameters.Add("@PA", SqlDbType.NVarChar, 50).Value = e.Primer_Apellido ?? "";
            cmd.Parameters.Add("@SA", SqlDbType.NVarChar, 50).Value = e.Segundo_Apellido ?? "";
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = e.Nombre ?? "";
            cmd.Parameters.Add("@FNac", SqlDbType.DateTime).Value = e.Fecha_Nacimiento;
            cmd.Parameters.Add("@Estado", SqlDbType.NVarChar, 50).Value = e.Estado_Civil ?? "";
            cmd.Parameters.Add("@Tel", SqlDbType.NVarChar, 50).Value = e.Telefono ?? "";
            cmd.Parameters.Add("@Cel", SqlDbType.NVarChar, 50).Value = e.Celular ?? "";
            cmd.Parameters.Add("@Nac", SqlDbType.NVarChar, 50).Value = e.Nacionalidad ?? "";
            cmd.Parameters.Add("@Laboro", SqlDbType.Bit).Value = e.Laboro != 0;
            cmd.Parameters.Add("@Dir", SqlDbType.NVarChar, 300).Value = e.Direccion ?? "";
            cmd.Parameters.Add("@IdDep", SqlDbType.Int).Value = e.Id_Departamento;

            var pSal = cmd.Parameters.Add("@Salario", SqlDbType.Float); // BD: float
            pSal.Value = e.Salario;

            cmd.Parameters.Add("@Puesto", SqlDbType.NVarChar, 50).Value = e.Puesto ?? "";
            cmd.Parameters.Add("@FIng", SqlDbType.DateTime).Value = e.Fecha_Ingreso;
            cmd.Parameters.Add("@FSal", SqlDbType.DateTime).Value = e.Fecha_Salida; // NOT NULL en BD
            cmd.Parameters.Add("@Foto", SqlDbType.NVarChar, 300).Value = e.Foto ?? "";
            cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = e.Activo != 0;
            cmd.Parameters.Add("@MC", SqlDbType.BigInt).Value = e.MC_Numero;

            cn.Open();
            var newIdObj = cmd.ExecuteScalar();
            return Convert.ToInt32(newIdObj);
        }

        public void Update(Empleado e)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
UPDATE dbo.Empleados SET
    Carne=@Carne, Cedula=@Cedula, Primer_Apellido=@PA, Segundo_Apellido=@SA, Nombre=@Nombre,
    Fecha_Nacimiento=@FNac, Estado_Civil=@Estado, Telefono=@Tel, Celular=@Cel, Nacionalidad=@Nac,
    Laboro=@Laboro, Direccion=@Dir, Id_Departamento=@IdDep, Salario=@Salario,
    Puesto=@Puesto, Fecha_Ingreso=@FIng, Fecha_Salida=@FSal, Foto=@Foto,
    Activo=@Activo, MC_Numero=@MC
WHERE Id=@Id;";

            cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = e.Id;
            cmd.Parameters.Add("@Carne", SqlDbType.BigInt).Value = e.Carne;
            cmd.Parameters.Add("@Cedula", SqlDbType.NVarChar, 50).Value = e.Cedula;
            cmd.Parameters.Add("@PA", SqlDbType.NVarChar, 50).Value = e.Primer_Apellido ?? "";
            cmd.Parameters.Add("@SA", SqlDbType.NVarChar, 50).Value = e.Segundo_Apellido ?? "";
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = e.Nombre ?? "";
            cmd.Parameters.Add("@FNac", SqlDbType.DateTime).Value = e.Fecha_Nacimiento;
            cmd.Parameters.Add("@Estado", SqlDbType.NVarChar, 50).Value = e.Estado_Civil ?? "";
            cmd.Parameters.Add("@Tel", SqlDbType.NVarChar, 50).Value = e.Telefono ?? "";
            cmd.Parameters.Add("@Cel", SqlDbType.NVarChar, 50).Value = e.Celular ?? "";
            cmd.Parameters.Add("@Nac", SqlDbType.NVarChar, 50).Value = e.Nacionalidad ?? "";
            cmd.Parameters.Add("@Laboro", SqlDbType.Bit).Value = e.Laboro != 0;
            cmd.Parameters.Add("@Dir", SqlDbType.NVarChar, 300).Value = e.Direccion ?? "";
            cmd.Parameters.Add("@IdDep", SqlDbType.Int).Value = e.Id_Departamento;

            var pSal = cmd.Parameters.Add("@Salario", SqlDbType.Float);
            pSal.Value = e.Salario;

            cmd.Parameters.Add("@Puesto", SqlDbType.NVarChar, 50).Value = e.Puesto ?? "";
            cmd.Parameters.Add("@FIng", SqlDbType.DateTime).Value = e.Fecha_Ingreso;
            cmd.Parameters.Add("@FSal", SqlDbType.DateTime).Value = e.Fecha_Salida;
            cmd.Parameters.Add("@Foto", SqlDbType.NVarChar, 300).Value = e.Foto ?? "";
            cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = e.Activo != 0;
            cmd.Parameters.Add("@MC", SqlDbType.BigInt).Value = e.MC_Numero;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM dbo.Empleados WHERE Id=@Id;";
            cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // =========================================================
        //  Unicidad por cédula
        // =========================================================
        public bool ExistsByCedula(string cedula, int? exceptId = null)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP(1) 1
FROM dbo.Empleados
WHERE Cedula=@ced AND (@id IS NULL OR Id<>@id);";
            cmd.Parameters.Add("@ced", SqlDbType.NVarChar, 50).Value = cedula;
            var pId = cmd.Parameters.Add("@id", SqlDbType.BigInt);
            pId.Value = exceptId.HasValue ? exceptId.Value : DBNull.Value;

            cn.Open();
            var x = cmd.ExecuteScalar();
            return x != null;
        }

        // =========================================================
        //  Utilidades
        // =========================================================
        private static Empleado MapEmpleado(SqlDataReader rd)
        {
            //  0: Id (BIGINT)
            //  1: Carne (BIGINT)
            // 14: Salario (float en SQL -> double en .NET)
            var emp = new Empleado
            {
                Id = Convert.ToInt32(rd.GetValue(0)),                     // BIGINT -> int (app)
                Carne = Convert.ToInt64(rd.GetValue(1)),
                Cedula = rd.GetString(2),
                Primer_Apellido = rd.GetString(3),
                Segundo_Apellido = rd.IsDBNull(4) ? "" : rd.GetString(4), // NOT NULL por esquema
                Nombre = rd.GetString(5),
                Fecha_Nacimiento = rd.GetDateTime(6),
                Estado_Civil = rd.GetString(7),
                Telefono = rd.IsDBNull(8) ? "" : rd.GetString(8),         // NOT NULL por esquema
                Celular = rd.GetString(9),
                Nacionalidad = rd.GetString(10),
                Laboro = rd.GetBoolean(11) ? 1 : 0,
                Direccion = rd.GetString(12),
                Id_Departamento = rd.GetInt32(13),
                Salario = Convert.ToSingle(rd.GetValue(14)),              // float -> double -> float
                Puesto = rd.GetString(15),
                Fecha_Ingreso = rd.GetDateTime(16),
                Fecha_Salida = rd.GetDateTime(17),                         // NOT NULL
                Foto = rd.IsDBNull(18) ? "" : rd.GetString(18),
                Activo = rd.GetBoolean(19) ? 1 : 0,
                MC_Numero = Convert.ToInt64(rd.GetValue(20))
            };

            return emp;
        }

        public bool ExisteActivoPorCarne(long carne)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT TOP (1) 1 
                        FROM dbo.Empleados 
                        WHERE Activo = 1 AND Carne = @carne;";
            cmd.Parameters.Add("@carne", SqlDbType.BigInt).Value = carne;

            cn.Open();
            var x = cmd.ExecuteScalar();
            return x != null;
        }

        // Nueva implementacion
        /// <summary>
        /// Devuelve Id (BIGINT) y el NombreCompleto de un empleado ACTIVO por carné.
        /// Retorna null si no existe o está inactivo.
        /// </summary>
        public (long Id, string NombreCompleto)? GetIdentidadBasica(long carne)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP (1) 
       CAST(Id AS bigint) AS Id,
       (Nombre + ' ' + Primer_Apellido + ' ' + ISNULL(Segundo_Apellido,'')) AS NombreCompleto
FROM dbo.Empleados
WHERE Activo = 1 AND Carne = @carne;";
            cmd.Parameters.Add("@carne", SqlDbType.BigInt).Value = carne;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            var id = rd.GetInt64(0);                 // BIGINT → long
            var nom = rd.GetString(1);
            return (id, nom);
        }

        // Nueva implementacion
        /// <summary>
        /// (Opcional) Obtiene el empleado ACTIVO por carné (objeto completo).
        /// Útil si más adelante necesitas más campos en BLL/UI.
        /// </summary>
        public Empleado? GetActivoByCarne(long carne)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT TOP(1) Id, Carne, Cedula, Primer_Apellido, Segundo_Apellido, Nombre, 
       Fecha_Nacimiento, Estado_Civil, Telefono, Celular, Nacionalidad, Laboro, 
       Direccion, Id_Departamento, Salario, Puesto, Fecha_Ingreso, Fecha_Salida, 
       Foto, Activo, MC_Numero
FROM dbo.Empleados
WHERE Activo = 1 AND Carne = @carne;";
            cmd.Parameters.Add("@carne", SqlDbType.BigInt).Value = carne;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return MapEmpleado(rd);  // reutiliza tu mapeador existente
        }
    }
}
