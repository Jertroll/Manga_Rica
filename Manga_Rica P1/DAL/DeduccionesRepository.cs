using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Manga_Rica_P1.ENTITY;

namespace Manga_Rica_P1.DAL
{
    /// <summary>
    /// Repositorio para manejo de datos de Deducciones (Uniformes).
    /// Basado en el patrón de SodaRepository pero adaptado para la tabla Deducciones.
    /// </summary>
    public sealed class DeduccionesRepository
    {
        private readonly string _connectionString;

        public DeduccionesRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Obtiene el próximo ID consecutivo para una nueva deducción
        /// </summary>
        public long GetNextId()
        {
            const string sql = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Deducciones";
            
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            
            connection.Open();
            var result = command.ExecuteScalar();
            return Convert.ToInt64(result);
        }

        /// <summary>
        /// Crea una nueva deducción y devuelve su ID
        /// </summary>
        public long Insert(Deducciones deduccion)
        {
            const string sql = @"
                INSERT INTO Deducciones (Id_Empleado, Total, Saldo, Id_Usuario, Anulada, Fecha) 
                VALUES (@Id_Empleado, @Total, @Saldo, @Id_Usuario, @Anulada, @Fecha);
                SELECT CAST(SCOPE_IDENTITY() AS bigint);";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            
            command.Parameters.AddWithValue("@Id_Empleado", deduccion.Id_Empleado);
            command.Parameters.AddWithValue("@Total", deduccion.Total);
            command.Parameters.AddWithValue("@Saldo", deduccion.Saldo);
            command.Parameters.AddWithValue("@Id_Usuario", deduccion.Id_Usuario);
            command.Parameters.AddWithValue("@Anulada", deduccion.Anulada);
            command.Parameters.AddWithValue("@Fecha", deduccion.Fecha);

            connection.Open();
            var result = command.ExecuteScalar();
            return Convert.ToInt64(result);
        }

        /// <summary>
        /// Actualiza una deducción existente
        /// </summary>
        public void Update(Deducciones deduccion)
        {
            const string sql = @"
                UPDATE Deducciones 
                SET Id_Empleado = @Id_Empleado, 
                    Total = @Total, 
                    Saldo = @Saldo, 
                    Id_Usuario = @Id_Usuario, 
                    Anulada = @Anulada, 
                    Fecha = @Fecha
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            
            command.Parameters.AddWithValue("@Id", deduccion.Id);
            command.Parameters.AddWithValue("@Id_Empleado", deduccion.Id_Empleado);
            command.Parameters.AddWithValue("@Total", deduccion.Total);
            command.Parameters.AddWithValue("@Saldo", deduccion.Saldo);
            command.Parameters.AddWithValue("@Id_Usuario", deduccion.Id_Usuario);
            command.Parameters.AddWithValue("@Anulada", deduccion.Anulada);
            command.Parameters.AddWithValue("@Fecha", deduccion.Fecha);

            connection.Open();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Obtiene una deducción por su ID
        /// </summary>
        public Deducciones? GetById(long id)
        {
            const string sql = @"
                SELECT Id, Id_Empleado, Total, Saldo, Id_Usuario, Anulada, Fecha
                FROM Deducciones 
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return new Deducciones
                {
                    Id = reader.GetInt64("Id"),
                    Id_Empleado = reader.GetInt64("Id_Empleado"),
                    Total = reader.GetDouble("Total"),
                    Saldo = reader.GetDouble("Saldo"),
                    Id_Usuario = reader.GetInt32("Id_Usuario"),
                    Anulada = reader.GetBoolean("Anulada"),
                    Fecha = reader.GetDateTime("Fecha")
                };
            }
            
            return null;
        }

        /// <summary>
        /// Busca deducciones por empleado con paginación
        /// </summary>
        public (List<Deducciones> items, int total) GetByEmpleado(long empleadoId, int pageIndex = 1, int pageSize = 50)
        {
            // Consulta para obtener el total
            const string sqlCount = @"
                SELECT COUNT(*) 
                FROM Deducciones 
                WHERE Id_Empleado = @EmpleadoId";

            // Consulta paginada
            const string sqlData = @"
                SELECT Id, Id_Empleado, Total, Saldo, Id_Usuario, Anulada, Fecha
                FROM Deducciones 
                WHERE Id_Empleado = @EmpleadoId
                ORDER BY Fecha DESC, Id DESC
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            // Obtener total
            using var countCommand = new SqlCommand(sqlCount, connection);
            countCommand.Parameters.AddWithValue("@EmpleadoId", empleadoId);
            var total = (int)countCommand.ExecuteScalar();

            // Obtener datos paginados
            var items = new List<Deducciones>();
            using var dataCommand = new SqlCommand(sqlData, connection);
            dataCommand.Parameters.AddWithValue("@EmpleadoId", empleadoId);
            dataCommand.Parameters.AddWithValue("@Offset", (pageIndex - 1) * pageSize);
            dataCommand.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = dataCommand.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new Deducciones
                {
                    Id = reader.GetInt64("Id"),
                    Id_Empleado = reader.GetInt64("Id_Empleado"),
                    Total = reader.GetDouble("Total"),
                    Saldo = reader.GetDouble("Saldo"),
                    Id_Usuario = reader.GetInt32("Id_Usuario"),
                    Anulada = reader.GetBoolean("Anulada"),
                    Fecha = reader.GetDateTime("Fecha")
                });
            }

            return (items, total);
        }

        /// <summary>
        /// Busca deducciones con filtros para el buscador
        /// </summary>
        public (List<Deducciones> items, int total) GetPage(int pageIndex, int pageSize, string? filtro = null)
        {
            var whereClause = "";
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                whereClause = @"
                    WHERE CAST(d.Id AS NVARCHAR) LIKE @Filtro 
                       OR CAST(d.Id_Empleado AS NVARCHAR) LIKE @Filtro
                       OR CAST(e.Carne AS NVARCHAR) LIKE @Filtro
                       OR e.Nombre LIKE @Filtro
                       OR e.Primer_Apellido LIKE @Filtro";
            }

            // Consulta para obtener el total
            var sqlCount = $@"
                SELECT COUNT(*) 
                FROM Deducciones d
                INNER JOIN Empleados e ON d.Id_Empleado = e.Id
                {whereClause}";

            // Consulta paginada con información del empleado
            var sqlData = $@"
                SELECT d.Id, d.Id_Empleado, d.Total, d.Saldo, d.Id_Usuario, d.Anulada, d.Fecha,
                       e.Carne, e.Nombre, e.Primer_Apellido, e.Segundo_Apellido
                FROM Deducciones d
                INNER JOIN Empleados e ON d.Id_Empleado = e.Id
                {whereClause}
                ORDER BY d.Fecha DESC, d.Id DESC
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            // Obtener total
            using var countCommand = new SqlCommand(sqlCount, connection);
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                countCommand.Parameters.AddWithValue("@Filtro", $"%{filtro}%");
            }
            var total = (int)countCommand.ExecuteScalar();

            // Obtener datos paginados
            var items = new List<Deducciones>();
            using var dataCommand = new SqlCommand(sqlData, connection);
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                dataCommand.Parameters.AddWithValue("@Filtro", $"%{filtro}%");
            }
            dataCommand.Parameters.AddWithValue("@Offset", (pageIndex - 1) * pageSize);
            dataCommand.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = dataCommand.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new Deducciones
                {
                    Id = reader.GetInt64("Id"),
                    Id_Empleado = reader.GetInt64("Id_Empleado"),
                    Total = reader.GetDouble("Total"),
                    Saldo = reader.GetDouble("Saldo"),
                    Id_Usuario = reader.GetInt32("Id_Usuario"),
                    Anulada = reader.GetBoolean("Anulada"),
                    Fecha = reader.GetDateTime("Fecha")
                });
            }

            return (items, total);
        }

        /// <summary>
        /// Marca una deducción como anulada
        /// </summary>
        public void Anular(long id, int usuarioId)
        {
            const string sql = @"
                UPDATE Deducciones 
                SET Anulada = 1, Id_Usuario = @Id_Usuario
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Id_Usuario", usuarioId);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Anular(int id, int usuarioId)
        {
            Anular((long)id, usuarioId);
        }

        /// <summary>
        /// Actualiza el saldo de una deducción (para pagos parciales)
        /// </summary>
        public void ActualizarSaldo(int id, float nuevoSaldo)
        {
            const string sql = @"
                UPDATE Deducciones 
                SET Saldo = @Saldo
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Saldo", nuevoSaldo);

            connection.Open();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Elimina una deducción (solo si no tiene detalles)
        /// </summary>
        public void Delete(int id)
        {
            const string sql = "DELETE FROM Deducciones WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}