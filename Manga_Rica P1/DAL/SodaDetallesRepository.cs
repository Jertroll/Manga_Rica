using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.DAL
{
    public sealed class SodaDetallesRepository
    {
        private readonly string _cs;

        public SodaDetallesRepository(string connectionString) => _cs = connectionString;

        /// <summary>
        /// Obtiene todos los detalles de una deducción de soda específica con información del artículo
        /// </summary>
        public List<SodaDetalleCompleto> GetBySodaId(long sodaId)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    sd.Id, 
                    sd.Id_Soda, 
                    sd.Codigo_Articulo, 
                    sd.Cantidad, 
                    sd.Precio, 
                    sd.Total,
                    a.Descripcion as DescripcionArticulo
                FROM dbo.Soda_Detalles sd
                INNER JOIN dbo.Articulos a ON sd.Codigo_Articulo = a.Id
                WHERE sd.Id_Soda = @sodaId
                ORDER BY sd.Id";

            cmd.Parameters.Add("@sodaId", SqlDbType.BigInt).Value = sodaId;
            cn.Open();

            var detalles = new List<SodaDetalleCompleto>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                detalles.Add(new SodaDetalleCompleto
                {
                    Id = reader.GetInt64("Id"),
                    Id_Soda = reader.GetInt64("Id_Soda"),
                    Codigo_Articulo = reader.GetInt32("Codigo_Articulo"),
                    Cantidad = reader.GetInt32("Cantidad"),
                    Precio = reader.GetDouble("Precio"),
                    Total = reader.GetDouble("Total"),
                    DescripcionArticulo = reader.GetString("DescripcionArticulo")
                });
            }
            return detalles;
        }

        public Soda_Detalles? GetById(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Id_Soda, Codigo_Articulo, Cantidad, Precio, Total
                FROM dbo.Soda_Detalles
                WHERE Id = @id";

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Soda_Detalles
                {
                    Id = reader.GetInt32("Id"),
                    Id_Soda = reader.GetInt32("Id_Soda"),
                    Codigo_Articulo = reader.GetInt32("Codigo_Articulo"),
                    Cantidad = reader.GetInt32("Cantidad"),
                    Precio = reader.GetFloat("Precio"),
                    Total = reader.GetFloat("Total")
                };
            }
            return null;
        }

        /// <summary>
        /// Inserta un nuevo detalle de soda
        /// Basado en el código legacy: Id_Deduccion se refiere a Id_Soda
        /// </summary>
        public int Insert(Soda_Detalles detalle)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                SET DATEFORMAT DMY;
                INSERT INTO dbo.Soda_Detalles (Id_Soda, Codigo_Articulo, Cantidad, Precio, Total)
                VALUES (@Id_Soda, @Codigo_Articulo, @Cantidad, @Precio, @Total);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            cmd.Parameters.Add("@Id_Soda", SqlDbType.Int).Value = detalle.Id_Soda;
            cmd.Parameters.Add("@Codigo_Articulo", SqlDbType.Int).Value = detalle.Codigo_Articulo;
            cmd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = detalle.Cantidad;
            cmd.Parameters.Add("@Precio", SqlDbType.Float).Value = detalle.Precio;
            cmd.Parameters.Add("@Total", SqlDbType.Float).Value = detalle.Total;

            cn.Open();
            return (int)cmd.ExecuteScalar()!;
        }

        public void Update(Soda_Detalles detalle)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = @"
                UPDATE dbo.Soda_Detalles 
                SET Id_Soda = @Id_Soda,
                    Codigo_Articulo = @Codigo_Articulo,
                    Cantidad = @Cantidad,
                    Precio = @Precio,
                    Total = @Total
                WHERE Id = @Id";

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = detalle.Id;
            cmd.Parameters.Add("@Id_Soda", SqlDbType.Int).Value = detalle.Id_Soda;
            cmd.Parameters.Add("@Codigo_Articulo", SqlDbType.Int).Value = detalle.Codigo_Articulo;
            cmd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = detalle.Cantidad;
            cmd.Parameters.Add("@Precio", SqlDbType.Float).Value = detalle.Precio;
            cmd.Parameters.Add("@Total", SqlDbType.Float).Value = detalle.Total;

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM dbo.Soda_Detalles WHERE Id = @Id";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Elimina todos los detalles de una deducción de soda específica
        /// </summary>
        public void DeleteBySodaId(int sodaId)
        {
            using var cn = new SqlConnection(_cs);
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "DELETE FROM dbo.Soda_Detalles WHERE Id_Soda = @sodaId";
            cmd.Parameters.Add("@sodaId", SqlDbType.Int).Value = sodaId;
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Inserta múltiples detalles en una sola transacción
        /// </summary>
        public void InsertBatch(IEnumerable<Soda_Detalles> detalles)
        {
            using var cn = new SqlConnection(_cs);
            cn.Open();
            using var transaction = cn.BeginTransaction();

            try
            {
                foreach (var detalle in detalles)
                {
                    using var cmd = cn.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = @"
                        SET DATEFORMAT DMY;
                        INSERT INTO dbo.Soda_Detalles (Id_Soda, Codigo_Articulo, Cantidad, Precio, Total)
                        VALUES (@Id_Soda, @Codigo_Articulo, @Cantidad, @Precio, @Total)";

                    cmd.Parameters.Add("@Id_Soda", SqlDbType.Int).Value = detalle.Id_Soda;
                    cmd.Parameters.Add("@Codigo_Articulo", SqlDbType.Int).Value = detalle.Codigo_Articulo;
                    cmd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = detalle.Cantidad;
                    cmd.Parameters.Add("@Precio", SqlDbType.Float).Value = detalle.Precio;
                    cmd.Parameters.Add("@Total", SqlDbType.Float).Value = detalle.Total;

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    /// <summary>
    /// Clase para manejar detalles con información completa del artículo
    /// </summary>
    public class SodaDetalleCompleto : Soda_Detalles
    {
        public string DescripcionArticulo { get; set; } = "";
    }
}