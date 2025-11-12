using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    public sealed class PagosRepository
    {
        private readonly string _cs;
        public PagosRepository(string connectionString)
            => _cs = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        private SqlConnection Open() => new SqlConnection(_cs);

        // Trae pago por clave (Id_Empleado, Id_Semana)
        public Pagos? GetByEmpleadoSemana(long idEmpleado, int idSemana)
        {
            const string sql = @"
SELECT TOP (1)
    Id, Id_Empleado, Fecha, Id_Semana,
    Horas_Normales, Horas_Extras, Horas_Dobles, Feriadas,
    Deduccion_Soda, Deduccion_Uniforme, Deduccion_Otras,
    Salario_Bruto, Salario_Neto, Id_Usuario, Registrado
FROM dbo.Pagos
WHERE Id_Empleado = @emp AND Id_Semana = @sem;";

            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add("@emp", SqlDbType.BigInt).Value = idEmpleado;
            cmd.Parameters.Add("@sem", SqlDbType.Int).Value = idSemana;

            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return new Pagos
            {
                Id = Convert.ToInt32(rd["Id"]),
                Id_Empleado = Convert.ToInt64(rd["Id_Empleado"]),
                Fecha = Convert.ToDateTime(rd["Fecha"]),
                Id_Semana = Convert.ToInt32(rd["Id_Semana"]),
                Horas_Normales = Convert.ToSingle(rd["Horas_Normales"]),
                Horas_Extras = Convert.ToSingle(rd["Horas_Extras"]),
                Horas_Dobles = Convert.ToSingle(rd["Horas_Dobles"]),
                Feriadas = Convert.ToSingle(rd["Feriadas"]),
                Deduccion_Soda = Convert.ToSingle(rd["Deduccion_Soda"]),
                Deduccion_Uniforme = Convert.ToSingle(rd["Deduccion_Uniforme"]),
                Deduccion_Otras = Convert.ToSingle(rd["Deduccion_Otras"]),
                Salario_Bruto = Convert.ToSingle(rd["Salario_Bruto"]),
                Salario_Neto = Convert.ToSingle(rd["Salario_Neto"]),
                Id_Usuario = Convert.ToInt32(rd["Id_Usuario"]),
                Registrado = Convert.ToBoolean(rd["Registrado"])
            };
        }

        // ¿Ya existe un pago registrado=1 para ese empleado/semana?
        public bool ExistsRegistrado(long idEmpleado, int idSemana)
        {
            const string sql = @"SELECT 1 FROM dbo.Pagos WHERE Id_Empleado=@emp AND Id_Semana=@sem AND Registrado=1;";
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add("@emp", SqlDbType.BigInt).Value = idEmpleado;
            cmd.Parameters.Add("@sem", SqlDbType.Int).Value = idSemana;
            cn.Open();
            using var rd = cmd.ExecuteReader(CommandBehavior.SingleRow);
            return rd.Read();
        }

        // Upsert por (Id_Empleado, Id_Semana)
        public int Upsert(Pagos p)
        {
            const string upd = @"
UPDATE dbo.Pagos
   SET Fecha=@Fecha,
       Horas_Normales=@HN, Horas_Extras=@HE, Horas_Dobles=@HD, Feriadas=@FE,
       Deduccion_Soda=@DS, Deduccion_Uniforme=@DU, Deduccion_Otras=@DO,
       Salario_Bruto=@SB, Salario_Neto=@SN, Id_Usuario=@Usr, Registrado=@Reg
 WHERE Id_Empleado=@Emp AND Id_Semana=@Sem;";

            const string ins = @"
INSERT INTO dbo.Pagos
( Id_Empleado, Fecha, Id_Semana,
  Horas_Normales, Horas_Extras, Horas_Dobles, Feriadas,
  Deduccion_Soda, Deduccion_Uniforme, Deduccion_Otras,
  Salario_Bruto, Salario_Neto, Id_Usuario, Registrado )
VALUES
( @Emp, @Fecha, @Sem,
  @HN, @HE, @HD, @FE,
  @DS, @DU, @DO,
  @SB, @SN, @Usr, @Reg );";

            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cn.Open();

            void Bind(SqlCommand c)
            {
                c.Parameters.Add("@Emp", SqlDbType.BigInt).Value = p.Id_Empleado;
                c.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = p.Fecha;
                c.Parameters.Add("@Sem", SqlDbType.Int).Value = p.Id_Semana;

                // SqlDbType.Float = float(53) en SQL Server → en .NET conviene enviar double
                c.Parameters.Add("@HN", SqlDbType.Float).Value = (double)p.Horas_Normales;
                c.Parameters.Add("@HE", SqlDbType.Float).Value = (double)p.Horas_Extras;
                c.Parameters.Add("@HD", SqlDbType.Float).Value = (double)p.Horas_Dobles;
                c.Parameters.Add("@FE", SqlDbType.Float).Value = (double)p.Feriadas;

                c.Parameters.Add("@DS", SqlDbType.Float).Value = (double)p.Deduccion_Soda;
                c.Parameters.Add("@DU", SqlDbType.Float).Value = (double)p.Deduccion_Uniforme;
                c.Parameters.Add("@DO", SqlDbType.Float).Value = (double)p.Deduccion_Otras;

                c.Parameters.Add("@SB", SqlDbType.Float).Value = (double)p.Salario_Bruto;
                c.Parameters.Add("@SN", SqlDbType.Float).Value = (double)p.Salario_Neto;

                c.Parameters.Add("@Usr", SqlDbType.Int).Value = p.Id_Usuario;
                c.Parameters.Add("@Reg", SqlDbType.Bit).Value = p.Registrado;
            }

            // Primero UPDATE
            cmd.CommandText = upd;
            Bind(cmd);
            var rows = cmd.ExecuteNonQuery();
            if (rows > 0) return rows;

            // Sino, INSERT
            cmd.Parameters.Clear();
            cmd.CommandText = ins;
            Bind(cmd);
            return cmd.ExecuteNonQuery();
        }

        // Útil para llenar la grilla de “pendientes”
        public List<long> GetEmpleadosPendientesBySemana(int idSemana)
        {
            const string sql = @"SELECT Id_Empleado FROM dbo.Pagos WHERE Id_Semana=@sem AND Registrado=0;";
            var list = new List<long>();
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add("@sem", SqlDbType.Int).Value = idSemana;
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                list.Add(Convert.ToInt64(rd.GetValue(0)));
            return list;
        }
    }
}
