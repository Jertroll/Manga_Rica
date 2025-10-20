using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    public interface IAcumuladoDiarioRepository
    {
        bool ExisteCierre(DateTime fecha);
        IEnumerable<long> GetEmpleadosActivosIds();
        double GetHorasTrabajadasEnteras(long idEmpleado, DateTime fecha);
        long Insert(Acumulado_Diario fila);
    }

    public sealed class AcumuladoDiarioRepository : IAcumuladoDiarioRepository
    {
        private readonly string _cs;
        public AcumuladoDiarioRepository(string connectionString) => _cs = connectionString;

        private SqlConnection Open() => new SqlConnection(_cs);

        public bool ExisteCierre(DateTime fecha)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(1) 
                                FROM dbo.Acumulado_Diario 
                                WHERE CAST(Fecha AS date)=@f";
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

        public double GetHorasTrabajadasEnteras(long idEmpleado, DateTime fecha)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SELECT ISNULL(SUM(CAST(Total_Horas AS float)), 0)
                FROM dbo.Horas
                WHERE Id_Empleado=@id 
                  AND CAST(Fecha AS date)=@f
                  AND Hora_Salida IS NOT NULL;";
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = idEmpleado;
            cmd.Parameters.Add("@f", SqlDbType.Date).Value = fecha.Date;
            cn.Open();
            var total = Convert.ToDouble(cmd.ExecuteScalar());
            return Math.Truncate(total); // igual que el sistema viejo (CInt)
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
    }
}
