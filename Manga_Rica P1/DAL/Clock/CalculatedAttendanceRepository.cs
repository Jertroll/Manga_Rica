using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.ENTITY.Clock;

namespace Manga_Rica_P1.DAL.Clock
{
    /// <summary>
    /// Acceso a dbo.calculatedAttendance (BD del reloj).
    /// Devuelve filas consolidadas por día (CA).
    /// </summary>
    public sealed class CalculatedAttendanceRepository
    {
        private readonly string _cs;
        private const int _timeout = 90;

        public CalculatedAttendanceRepository(string connectionString) => _cs = connectionString;

        private SqlConnection Open()
        {
            var cn = new SqlConnection(_cs);
            cn.Open();
            return cn;
        }

        /// <summary>
        /// CA por idEmployee (clock) en rango de fechas.
        /// Solo incluye filas con startEnroll y endEnroll NO nulos.
        /// </summary>
        public List<CalculatedAttendance> GetByEmployeeId(long idEmployee, DateTime fromDate, DateTime toDate)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT
    id,
    idEmployee,
    _date,
    startEnroll,
    endEnroll,
    durationBreak,
    deductBreak,
    total,
    IsOpen,
    IsInOut,
    IsNocturnal,
    InSchedule,
    outSchedule,
    daySeventh,
    dayBreak,
    dayCompensatory,
    isHoliDay,
    isHolidayPay
FROM dbo.calculatedAttendance
WHERE idEmployee = @emp
  AND _date >= @d1 AND _date <= @d2
  AND startEnroll IS NOT NULL
  AND endEnroll   IS NOT NULL
ORDER BY _date, id;";
            cmd.Parameters.Add(new SqlParameter("@emp", SqlDbType.BigInt) { Value = idEmployee });
            cmd.Parameters.Add(new SqlParameter("@d1", SqlDbType.Date) { Value = fromDate.Date });
            cmd.Parameters.Add(new SqlParameter("@d2", SqlDbType.Date) { Value = toDate.Date });
            cmd.CommandTimeout = _timeout;

            var list = new List<CalculatedAttendance>();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new CalculatedAttendance
                {
                    Id = rd.GetInt64(0),                   // id
                    IdEmployee = rd.GetInt64(1),           // idEmployee
                    Date = rd.GetDateTime(2),              // _date
                    StartEnroll = rd.IsDBNull(3) ? null : rd.GetDateTime(3),
                    EndEnroll = rd.IsDBNull(4) ? null : rd.GetDateTime(4),
                    DurationBreak = rd.IsDBNull(5) ? null : rd.GetInt32(5),
                    DeductBreak = !rd.IsDBNull(6) && rd.GetBoolean(6),
                    Total = rd.IsDBNull(7) ? null : rd.GetInt32(7),
                    IsOpen = !rd.IsDBNull(8) && rd.GetBoolean(8),
                    IsInOut = !rd.IsDBNull(9) && rd.GetBoolean(9),
                    IsNocturnal = !rd.IsDBNull(10) && rd.GetBoolean(10),
                    InSchedule = rd.IsDBNull(11) ? null : rd.GetDateTime(11),
                    OutSchedule = rd.IsDBNull(12) ? null : rd.GetDateTime(12),
                    DaySeventh = !rd.IsDBNull(13) && rd.GetBoolean(13),
                    DayBreak = !rd.IsDBNull(14) && rd.GetBoolean(14),
                    DayCompensatory = !rd.IsDBNull(15) && rd.GetBoolean(15),
                    IsHoliDay = !rd.IsDBNull(16) && rd.GetBoolean(16),
                    IsHolidayPay = !rd.IsDBNull(17) && rd.GetBoolean(17)
                });
            }
            return list;
        }

        /// <summary>
        /// CA por code (empleado visible del reloj). Resuelve idEmployee vía dbo.employees.
        /// Versión para code VARCHAR exacto (compatibilidad).
        /// Solo incluye filas con startEnroll y endEnroll NO nulos.
        /// </summary>
        public List<CalculatedAttendance> GetByEmployeeCode(string code, DateTime fromDate, DateTime toDate)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT
    ca.id,
    ca.idEmployee,
    ca._date,
    ca.startEnroll,
    ca.endEnroll,
    ca.durationBreak,
    ca.deductBreak,
    ca.total,
    ca.IsOpen,
    ca.IsInOut,
    ca.IsNocturnal,
    ca.InSchedule,
    ca.outSchedule,
    ca.daySeventh,
    ca.dayBreak,
    ca.dayCompensatory,
    ca.isHoliDay,
    ca.isHolidayPay
FROM dbo.calculatedAttendance ca
JOIN dbo.employees e
  ON e.id = ca.idEmployee
WHERE ca._date >= @d1 AND ca._date <= @d2
  AND e.code = @code
  AND ca.startEnroll IS NOT NULL
  AND ca.endEnroll   IS NOT NULL
ORDER BY ca._date, ca.id;";
            cmd.Parameters.Add(new SqlParameter("@d1", SqlDbType.Date) { Value = fromDate.Date });
            cmd.Parameters.Add(new SqlParameter("@d2", SqlDbType.Date) { Value = toDate.Date });
            cmd.Parameters.Add(new SqlParameter("@code", SqlDbType.VarChar, 50) { Value = code });
            cmd.CommandTimeout = _timeout;

            var list = new List<CalculatedAttendance>();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new CalculatedAttendance
                {
                    Id = rd.GetInt64(0),
                    IdEmployee = rd.GetInt64(1),
                    Date = rd.GetDateTime(2),
                    StartEnroll = rd.IsDBNull(3) ? null : rd.GetDateTime(3),
                    EndEnroll = rd.IsDBNull(4) ? null : rd.GetDateTime(4),
                    DurationBreak = rd.IsDBNull(5) ? null : rd.GetInt32(5),
                    DeductBreak = !rd.IsDBNull(6) && rd.GetBoolean(6),
                    Total = rd.IsDBNull(7) ? null : rd.GetInt32(7),
                    IsOpen = !rd.IsDBNull(8) && rd.GetBoolean(8),
                    IsInOut = !rd.IsDBNull(9) && rd.GetBoolean(9),
                    IsNocturnal = !rd.IsDBNull(10) && rd.GetBoolean(10),
                    InSchedule = rd.IsDBNull(11) ? null : rd.GetDateTime(11),
                    OutSchedule = rd.IsDBNull(12) ? null : rd.GetDateTime(12),
                    DaySeventh = !rd.IsDBNull(13) && rd.GetBoolean(13),
                    DayBreak = !rd.IsDBNull(14) && rd.GetBoolean(14),
                    DayCompensatory = !rd.IsDBNull(15) && rd.GetBoolean(15),
                    IsHoliDay = !rd.IsDBNull(16) && rd.GetBoolean(16),
                    IsHolidayPay = !rd.IsDBNull(17) && rd.GetBoolean(17)
                });
            }
            return list;
        }

        /// <summary>
        /// CA por code numérico (MC_Numero en tu app) → compara con employees.code (VARCHAR) usando TRY_CONVERT.
        /// Úsalo cuando recibes MC_Numero (BIGINT) desde tu BD principal.
        /// Solo incluye filas con startEnroll y endEnroll NO nulos.
        /// </summary>
        public List<CalculatedAttendance> GetByEmployeeCode(long code, DateTime fromDate, DateTime toDate)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT
    ca.id,
    ca.idEmployee,
    ca._date,
    ca.startEnroll,
    ca.endEnroll,
    ca.durationBreak,
    ca.deductBreak,
    ca.total,
    ca.IsOpen,
    ca.IsInOut,
    ca.IsNocturnal,
    ca.InSchedule,
    ca.outSchedule,
    ca.daySeventh,
    ca.dayBreak,
    ca.dayCompensatory,
    ca.isHoliDay,
    ca.isHolidayPay
FROM dbo.calculatedAttendance ca
JOIN dbo.employees e
  ON e.id = ca.idEmployee
WHERE ca._date >= @d1 AND ca._date <= @d2
  AND TRY_CONVERT(bigint, e.code) = @code
  AND ca.startEnroll IS NOT NULL
  AND ca.endEnroll   IS NOT NULL
ORDER BY ca._date, ca.id;";
            cmd.Parameters.Add(new SqlParameter("@d1", SqlDbType.Date) { Value = fromDate.Date });
            cmd.Parameters.Add(new SqlParameter("@d2", SqlDbType.Date) { Value = toDate.Date });
            cmd.Parameters.Add(new SqlParameter("@code", SqlDbType.BigInt) { Value = code });
            cmd.CommandTimeout = _timeout;

            var list = new List<CalculatedAttendance>();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new CalculatedAttendance
                {
                    Id = rd.GetInt64(0),
                    IdEmployee = rd.GetInt64(1),
                    Date = rd.GetDateTime(2),
                    StartEnroll = rd.IsDBNull(3) ? null : rd.GetDateTime(3),
                    EndEnroll = rd.IsDBNull(4) ? null : rd.GetDateTime(4),
                    DurationBreak = rd.IsDBNull(5) ? null : rd.GetInt32(5),
                    DeductBreak = !rd.IsDBNull(6) && rd.GetBoolean(6),
                    Total = rd.IsDBNull(7) ? null : rd.GetInt32(7),
                    IsOpen = !rd.IsDBNull(8) && rd.GetBoolean(8),
                    IsInOut = !rd.IsDBNull(9) && rd.GetBoolean(9),
                    IsNocturnal = !rd.IsDBNull(10) && rd.GetBoolean(10),
                    InSchedule = rd.IsDBNull(11) ? null : rd.GetDateTime(11),
                    OutSchedule = rd.IsDBNull(12) ? null : rd.GetDateTime(12),
                    DaySeventh = !rd.IsDBNull(13) && rd.GetBoolean(13),
                    DayBreak = !rd.IsDBNull(14) && rd.GetBoolean(14),
                    DayCompensatory = !rd.IsDBNull(15) && rd.GetBoolean(15),
                    IsHoliDay = !rd.IsDBNull(16) && rd.GetBoolean(16),
                    IsHolidayPay = !rd.IsDBNull(17) && rd.GetBoolean(17)
                });
            }
            return list;
        }

        /// <summary>
        /// Suma de total (minutos/unidad de 'total') para un día concreto por MC_Numero (BIGINT)
        /// mapeado contra employees.code (VARCHAR) mediante TRY_CONVERT.
        /// Solo considera filas con startEnroll y endEnroll NO nulos.
        /// </summary>
        public double GetHorasDiaByCode(long code, DateTime fecha)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT COALESCE(SUM(CAST(ca.total AS float)), 0)
FROM dbo.calculatedAttendance ca
JOIN dbo.employees e
  ON e.id = ca.idEmployee
WHERE TRY_CONVERT(bigint, e.code) = @code
  AND CAST(ca._date AS date) = @fecha
  AND ca.startEnroll IS NOT NULL
  AND ca.endEnroll   IS NOT NULL;";
            cmd.Parameters.Add(new SqlParameter("@code", SqlDbType.BigInt) { Value = code });
            cmd.Parameters.Add(new SqlParameter("@fecha", SqlDbType.Date) { Value = fecha.Date });
            cmd.CommandTimeout = _timeout;

            var o = cmd.ExecuteScalar();
            return o == null ? 0d : Convert.ToDouble(o);
        }

        /// <summary>
        /// Totales por día en un rango para un MC_Numero (BIGINT) → útil para cierres o verificación.
        /// Solo considera filas con startEnroll y endEnroll NO nulos.
        /// </summary>
        public IEnumerable<(DateTime Fecha, double Total)> GetHorasPorRangoByCode(long code, DateTime desde, DateTime hasta)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
SELECT CAST(ca._date AS date) AS Fecha,
       SUM(CAST(ca.total AS float)) AS Total
FROM dbo.calculatedAttendance ca
JOIN dbo.employees e
  ON e.id = ca.idEmployee
WHERE TRY_CONVERT(bigint, e.code) = @code
  AND CAST(ca._date AS date) BETWEEN @d1 AND @d2
  AND ca.startEnroll IS NOT NULL
  AND ca.endEnroll   IS NOT NULL
GROUP BY CAST(ca._date AS date)
ORDER BY Fecha;";
            cmd.Parameters.Add(new SqlParameter("@code", SqlDbType.BigInt) { Value = code });
            cmd.Parameters.Add(new SqlParameter("@d1", SqlDbType.Date) { Value = desde.Date });
            cmd.Parameters.Add(new SqlParameter("@d2", SqlDbType.Date) { Value = hasta.Date });
            cmd.CommandTimeout = _timeout;

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                yield return (rd.GetDateTime(0), Convert.ToDouble(rd.GetValue(1)));
        }
    }
}
