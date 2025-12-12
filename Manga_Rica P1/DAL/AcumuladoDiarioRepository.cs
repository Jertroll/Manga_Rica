using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    // ========= INTERFAZ ÚNICA =========
    public interface IAcumuladoDiarioRepository
    {
        bool ExisteCierre(DateTime fecha);
        IEnumerable<long> GetEmpleadosActivosIds();
        double GetHorasTrabajadasEnteras(long idEmpleado, DateTime fecha);
        long Insert(Acumulado_Diario fila);

        // Nuevos métodos
        List<Acumulado_Diario> ListByEmpleadoYRango(long idEmpleado, DateTime desde, DateTime hasta);
        bool Exists(long idEmpleado, DateTime fecha);
        void Update(Acumulado_Diario fila);
        void Upsert(Acumulado_Diario fila);

        // (Opcional) sumarización directa
        (float Normales, float Extras, float Dobles, float Feriado)
            SumByEmpleadoYRango(long idEmpleado, DateTime desde, DateTime hasta);
    }

    // ========= IMPLEMENTACIÓN =========
    public sealed class AcumuladoDiarioRepository : IAcumuladoDiarioRepository
    {
        private readonly string _cs;

        // ▼ Dependencias opcionales para integrar con el reloj marcador
        private readonly EmpleadoRepository? _empRepo; // BD principal (para leer MC_Numero)
        private readonly Clock.CalculatedAttendanceRepository? _clockRepo; // BD Clock

        public AcumuladoDiarioRepository(string connectionString)
            => _cs = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        /// <summary>
        /// Ctor extendido: inyecta repos de Empleado (main DB) y Clock (calculatedAttendance).
        /// Permite que GetHorasTrabajadasEnteras use el reloj marcador con el mapeo MC_Numero → employees.code.
        /// </summary>
        public AcumuladoDiarioRepository(
            string connectionString,
            EmpleadoRepository empRepo,
            Clock.CalculatedAttendanceRepository clockRepo) : this(connectionString)
        {
            _empRepo = empRepo ?? throw new ArgumentNullException(nameof(empRepo));
            _clockRepo = clockRepo ?? throw new ArgumentNullException(nameof(clockRepo));
        }

        private SqlConnection Open() => new SqlConnection(_cs);

        public bool ExisteCierre(DateTime fecha)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SELECT COUNT(1) 
                FROM dbo.Acumulado_Diario 
                WHERE CAST(Fecha AS date) = @f;";
            cmd.Parameters.Add("@f", SqlDbType.Date).Value = fecha.Date;
            cn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public IEnumerable<long> GetEmpleadosActivosIds()
        {
            var list = new List<long>();
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT Id FROM dbo.Empleados WHERE Activo = 1;";
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                list.Add(Convert.ToInt64(rd.GetValue(0)));
            return list;
        }

        /// <summary>
        /// Devuelve horas TRUNCADAS del día. Preferencia:
        ///  1) Si hay repos de Empleado + Clock inyectados: usa Clock (calculatedAttendance.total por employees.code).
        ///  2) Si no, usa la tabla Horas del sistema nuevo (plan B).
        /// </summary>
        public double GetHorasTrabajadasEnteras(long idEmpleado, DateTime fecha)
        {
            // ===== 1) Preferencia: reloj marcador (Clock) =====
            if (_empRepo is not null && _clockRepo is not null)
            {
                var emp = _empRepo.GetById(idEmpleado);
                if (emp is not null && emp.MC_Numero > 0)
                {
                    // minutos totales del día según Clock
                    var totalMinutos = _clockRepo.GetHorasDiaByCode(emp.MC_Numero, fecha);

                    // Parte entera de horas y minutos restantes
                    var horasEnteras = (int)(totalMinutos / 60.0);
                    var minutosRestantes = totalMinutos - (horasEnteras * 60.0);

                    // Regla: <= 30 baja, > 30 sube
                    if (minutosRestantes > 29.9)
                    {
                        horasEnteras += 1;
                    }

                    return horasEnteras;
                }
            }

            // ===== 2) Plan B: tabla Horas (Total_Horas en horas decimales) =====
            using (var cn = Open())
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
            SELECT ISNULL(SUM(CAST(Total_Horas AS float)), 0)
            FROM dbo.Horas
            WHERE Id_Empleado = @id 
              AND CAST(Fecha AS date) = @f
              AND Hora_Salida IS NOT NULL;";
                cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = idEmpleado;
                cmd.Parameters.Add("@f", SqlDbType.Date).Value = fecha.Date;
                cn.Open();
                var totalHoras = Convert.ToDouble(cmd.ExecuteScalar());

                // Separamos parte entera y fracción
                var horasEnteras = Math.Truncate(totalHoras);
                var fraccion = totalHoras - horasEnteras;
                var minutos = fraccion * 60.0;

                // Regla: <= 30 baja, > 30 sube
                if (minutos > 30.0)
                {
                    horasEnteras += 1.0;
                }

                return horasEnteras;
            }
        }


        public long Insert(Acumulado_Diario fila)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO dbo.Acumulado_Diario
                    (Id_Empleado, Fecha, Normales, Extras, Dobles, Feriado)
                VALUES
                    (@idEmp, @fecha, @n, @e, @d, @f);
                SELECT CAST(SCOPE_IDENTITY() AS bigint);";
            cmd.Parameters.Add("@idEmp", SqlDbType.BigInt).Value = fila.Id_Empleado;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fila.Fecha;
            cmd.Parameters.Add("@n", SqlDbType.Float).Value = fila.Normales;
            cmd.Parameters.Add("@e", SqlDbType.Float).Value = fila.Extras;
            cmd.Parameters.Add("@d", SqlDbType.Float).Value = fila.Dobles;
            cmd.Parameters.Add("@f", SqlDbType.Float).Value = fila.Feriado;

            cn.Open();
            var obj = cmd.ExecuteScalar();
            return Convert.ToInt64(obj);
        }

        public List<Acumulado_Diario> ListByEmpleadoYRango(long idEmpleado, DateTime desde, DateTime hasta)
        {
            const string sql = @"
                SELECT Id_Empleado, Fecha, Normales, Extras, Dobles, Feriado
                FROM dbo.Acumulado_Diario
                WHERE Id_Empleado = @emp
                  AND Fecha >= @desde AND Fecha <= @hasta
                ORDER BY Fecha;";

            var list = new List<Acumulado_Diario>();
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add("@emp", SqlDbType.BigInt).Value = idEmpleado;
            cmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde.Date;
            cmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta.Date;

            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new Acumulado_Diario
                {
                    Id_Empleado = rd.GetInt64(rd.GetOrdinal("Id_Empleado")),
                    Fecha = rd.GetDateTime(rd.GetOrdinal("Fecha")),
                    Normales = Convert.ToSingle(rd["Normales"]),
                    Extras = Convert.ToSingle(rd["Extras"]),
                    Dobles = Convert.ToSingle(rd["Dobles"]),
                    Feriado = Convert.ToSingle(rd["Feriado"])
                });
            }
            return list;
        }

        public bool Exists(long idEmpleado, DateTime fecha)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SELECT 1
                FROM dbo.Acumulado_Diario
                WHERE Id_Empleado = @emp AND CAST(Fecha AS date) = @f;";
            cmd.Parameters.Add("@emp", SqlDbType.BigInt).Value = idEmpleado;
            cmd.Parameters.Add("@f", SqlDbType.Date).Value = fecha.Date;
            cn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read();
        }

        public void Update(Acumulado_Diario fila)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                UPDATE dbo.Acumulado_Diario
                SET Normales = @n, Extras = @e, Dobles = @d, Feriado = @f
                WHERE Id_Empleado = @idEmp AND CAST(Fecha AS date) = @fecha;";
            cmd.Parameters.Add("@idEmp", SqlDbType.BigInt).Value = fila.Id_Empleado;
            cmd.Parameters.Add("@fecha", SqlDbType.Date).Value = fila.Fecha.Date;
            cmd.Parameters.Add("@n", SqlDbType.Float).Value = fila.Normales;
            cmd.Parameters.Add("@e", SqlDbType.Float).Value = fila.Extras;
            cmd.Parameters.Add("@d", SqlDbType.Float).Value = fila.Dobles;
            cmd.Parameters.Add("@f", SqlDbType.Float).Value = fila.Feriado;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Upsert(Acumulado_Diario fila)
        {
            if (Exists(fila.Id_Empleado, fila.Fecha))
                Update(fila);
            else
                Insert(fila);
        }

        public (float Normales, float Extras, float Dobles, float Feriado)
            SumByEmpleadoYRango(long idEmpleado, DateTime desde, DateTime hasta)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    ISNULL(SUM(CAST(Normales AS float)), 0),
                    ISNULL(SUM(CAST(Extras   AS float)), 0),
                    ISNULL(SUM(CAST(Dobles   AS float)), 0),
                    ISNULL(SUM(CAST(Feriado  AS float)), 0)
                FROM dbo.Acumulado_Diario
                WHERE Id_Empleado = @emp
                  AND Fecha >= @desde AND Fecha <= @hasta;";
            cmd.Parameters.Add("@emp", SqlDbType.BigInt).Value = idEmpleado;
            cmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde.Date;
            cmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta.Date;

            cn.Open();
            using var rd = cmd.ExecuteReader();
            rd.Read();
            return (
                Convert.ToSingle(rd.GetValue(0)),
                Convert.ToSingle(rd.GetValue(1)),
                Convert.ToSingle(rd.GetValue(2)),
                Convert.ToSingle(rd.GetValue(3))
            );
        }
    }
}
