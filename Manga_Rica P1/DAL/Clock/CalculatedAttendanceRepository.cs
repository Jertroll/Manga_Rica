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
        /// Nota: usamos subset de columnas clave; si tu tabla tiene más columnas relevantes,
        /// puedes ampliar el SELECT y el mapeo.
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
                    Id = rd.GetInt64(0),             // id
                    IdEmployee = rd.GetInt64(1),             // idEmployee
                    Date = rd.GetDateTime(2),          // _date
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
WHERE ca._date >= @d1 AND ca._date <= @d2
  AND ca.idEmployee IN (SELECT e.id FROM dbo.employees e WHERE e.code = @code)
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
    }
}
