
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Manga_Rica_P1.DAL
{
    /// <summary>
    /// DAL específico para la activación de pagos.
    /// Mantiene el mismo comportamiento del módulo legacy.
    /// </summary>
    public sealed class ActivarPagosRepository
    {
        private readonly string _cs;
        public ActivarPagosRepository(string connectionString) => _cs = connectionString;

        private SqlConnection Open() => new SqlConnection(_cs);

        /// <summary>
        /// Devuelve los Id de todos los empleados ACTIVOS.
        /// </summary>
        public List<long> GetEmpleadosActivosIds()
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
        /// Verifica si ya existen pagos para la semana indicada.
        /// Regla legacy: si existe CUALQUIER fila, se considera "ya activada".
        /// </summary>
        public bool ExisteParaSemana(int idSemana)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(1) FROM dbo.Pagos WHERE Id_Semana = @sem;";
            cmd.Parameters.Add("@sem", SqlDbType.Int).Value = idSemana;
            cn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Inserta un registro "inicial" en Pagos para un empleado y una semana.
        /// Todos los valores de horas/deducciones/salarios se insertan en 0 y Registrado = 0 (legacy).
        /// </summary>
        public void InsertPagoInicial(long idEmpleado, int idSemana, int idUsuario, DateTime fecha)
        {
            using var cn = Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
INSERT INTO dbo.Pagos
    (Id_Empleado, Fecha, Id_Semana, 
     Horas_Normales, Horas_Extras, Horas_Dobles, Feriadas,
     Deduccion_Soda, Deduccion_Uniforme, Deduccion_Otras,
     Salario_Bruto, Salario_Neto, Id_Usuario, Registrado)
VALUES
    (@emp, @fecha, @sem,
     0, 0, 0, 0,
     0, 0, 0,
     0, 0, @user, 0);";

            cmd.Parameters.Add("@emp", SqlDbType.BigInt).Value = idEmpleado;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
            cmd.Parameters.Add("@sem", SqlDbType.Int).Value = idSemana;
            cmd.Parameters.Add("@user", SqlDbType.Int).Value = idUsuario;

            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
