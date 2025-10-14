using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Manga_Rica_P1.ENTITY;

namespace Manga_Rica_P1.DAL
{
    /// <summary>
    /// Repositorio para manejo de datos de Deducciones_Detalles (Uniformes).
    /// Basado en el patrón de SodaDetallesRepository pero adaptado para la tabla Deducciones_Detalles.
    /// </summary>
    public sealed class DeduccionesDetallesRepository
    {
        private readonly string _connectionString;

        public DeduccionesDetallesRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Inserta un detalle de deducción
        /// </summary>
        public int Insert(Deducciones_Detalles detalle)
        {
            const string sql = @"
                INSERT INTO Deducciones_Detalles (Id_Deduccion, Codigo_Articulo, Cantidad, Precio, Total) 
                VALUES (@Id_Deduccion, @Codigo_Articulo, @Cantidad, @Precio, @Total);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            
            command.Parameters.AddWithValue("@Id_Deduccion", detalle.Id_Deduccion);
            command.Parameters.AddWithValue("@Codigo_Articulo", detalle.Codigo_Articulo);
            command.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
            command.Parameters.AddWithValue("@Precio", detalle.Precio);
            command.Parameters.AddWithValue("@Total", detalle.Total);

            connection.Open();
            var result = command.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Inserta múltiples detalles en una transacción
        /// </summary>
        public void InsertBatch(List<Deducciones_Detalles> detalles)
        {
            if (detalles == null || !detalles.Any()) return;

            const string sql = @"
                INSERT INTO Deducciones_Detalles (Id_Deduccion, Codigo_Articulo, Cantidad, Precio, Total) 
                VALUES (@Id_Deduccion, @Codigo_Articulo, @Cantidad, @Precio, @Total)";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var detalle in detalles)
                {
                    using var command = new SqlCommand(sql, connection, transaction);
                    command.Parameters.AddWithValue("@Id_Deduccion", detalle.Id_Deduccion);
                    command.Parameters.AddWithValue("@Codigo_Articulo", detalle.Codigo_Articulo);
                    command.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    command.Parameters.AddWithValue("@Precio", detalle.Precio);
                    command.Parameters.AddWithValue("@Total", detalle.Total);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los detalles de una deducción específica
        /// </summary>
        public List<Deducciones_Detalles> GetByDeduccionId(long deduccionId)
        {
            const string sql = @"
                SELECT dd.Id, dd.Id_Deduccion, dd.Codigo_Articulo, dd.Cantidad, dd.Precio, dd.Total,
                       a.descripcion as Descripcion_Articulo
                FROM Deducciones_Detalles dd
                INNER JOIN Articulos a ON dd.Codigo_Articulo = a.Id
                WHERE dd.Id_Deduccion = @DeduccionId
                ORDER BY dd.Id";

            var detalles = new List<Deducciones_Detalles>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DeduccionId", deduccionId);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                detalles.Add(new Deducciones_Detalles
                {
                    Id = reader.GetInt64("Id"),
                    Id_Deduccion = reader.GetInt64("Id_Deduccion"),
                    Codigo_Articulo = reader.GetInt32("Codigo_Articulo"),
                    Cantidad = reader.GetInt32("Cantidad"),
                    Precio = reader.GetDouble("Precio"),
                    Total = reader.GetDouble("Total")
                });
            }

            return detalles;
        }

        /// <summary>
        /// Obtiene un detalle por su ID
        /// </summary>
        public Deducciones_Detalles? GetById(int id)
        {
            const string sql = @"
                SELECT Id, Id_Deduccion, Codigo_Articulo, Cantidad, Precio, Total
                FROM Deducciones_Detalles 
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return new Deducciones_Detalles
                {
                    Id = reader.GetInt64("Id"),
                    Id_Deduccion = reader.GetInt64("Id_Deduccion"),
                    Codigo_Articulo = reader.GetInt32("Codigo_Articulo"),
                    Cantidad = reader.GetInt32("Cantidad"),
                    Precio = reader.GetDouble("Precio"),
                    Total = reader.GetDouble("Total")
                };
            }
            
            return null;
        }

        /// <summary>
        /// Actualiza un detalle existente
        /// </summary>
        public void Update(Deducciones_Detalles detalle)
        {
            const string sql = @"
                UPDATE Deducciones_Detalles 
                SET Id_Deduccion = @Id_Deduccion,
                    Codigo_Articulo = @Codigo_Articulo,
                    Cantidad = @Cantidad,
                    Precio = @Precio,
                    Total = @Total
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            
            command.Parameters.AddWithValue("@Id", detalle.Id);
            command.Parameters.AddWithValue("@Id_Deduccion", detalle.Id_Deduccion);
            command.Parameters.AddWithValue("@Codigo_Articulo", detalle.Codigo_Articulo);
            command.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
            command.Parameters.AddWithValue("@Precio", detalle.Precio);
            command.Parameters.AddWithValue("@Total", detalle.Total);

            connection.Open();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Elimina un detalle específico
        /// </summary>
        public void Delete(int id)
        {
            const string sql = "DELETE FROM Deducciones_Detalles WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Elimina todos los detalles de una deducción específica
        /// </summary>
        public void DeleteByDeduccionId(int deduccionId)
        {
            const string sql = "DELETE FROM Deducciones_Detalles WHERE Id_Deduccion = @DeduccionId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DeduccionId", deduccionId);

            connection.Open();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Obtiene el total de detalles de una deducción
        /// </summary>
        public float GetTotalByDeduccionId(int deduccionId)
        {
            const string sql = @"
                SELECT ISNULL(SUM(Total), 0) 
                FROM Deducciones_Detalles 
                WHERE Id_Deduccion = @DeduccionId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DeduccionId", deduccionId);

            connection.Open();
            var result = command.ExecuteScalar();
            return Convert.ToSingle(result);
        }

        /// <summary>
        /// Verifica si existe un artículo específico en los detalles de una deducción
        /// </summary>
        public bool ExistsArticuloInDeduccion(int deduccionId, int articuloId)
        {
            const string sql = @"
                SELECT COUNT(*) 
                FROM Deducciones_Detalles 
                WHERE Id_Deduccion = @DeduccionId AND Codigo_Articulo = @ArticuloId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DeduccionId", deduccionId);
            command.Parameters.AddWithValue("@ArticuloId", articuloId);

            connection.Open();
            var result = (int)command.ExecuteScalar();
            return result > 0;
        }

        /// <summary>
        /// Obtiene estadísticas de artículos más deducidos (para reportes)
        /// </summary>
        public List<(int ArticuloId, string Descripcion, int TotalCantidad, float TotalImporte)> GetEstadisticasArticulos(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var whereClause = "";
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                whereClause = "WHERE d.Fecha BETWEEN @FechaInicio AND @FechaFin";
            }

            var sql = $@"
                SELECT dd.Codigo_Articulo, a.descripcion,
                       SUM(dd.Cantidad) as TotalCantidad,
                       SUM(dd.Total) as TotalImporte
                FROM Deducciones_Detalles dd
                INNER JOIN Deducciones d ON dd.Id_Deduccion = d.Id
                INNER JOIN Articulos a ON dd.Codigo_Articulo = a.Id
                {whereClause}
                GROUP BY dd.Codigo_Articulo, a.descripcion
                ORDER BY SUM(dd.Total) DESC";

            var estadisticas = new List<(int, string, int, float)>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                command.Parameters.AddWithValue("@FechaInicio", fechaInicio.Value);
                command.Parameters.AddWithValue("@FechaFin", fechaFin.Value);
            }

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                estadisticas.Add((
                    reader.GetInt32("Codigo_Articulo"),
                    reader.GetString("descripcion"),
                    reader.GetInt32("TotalCantidad"),
                    reader.GetFloat("TotalImporte")
                ));
            }

            return estadisticas;
        }
    }
}