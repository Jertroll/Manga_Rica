using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Servicio de lógica de negocio para Deducciones de Soda.
    /// Basado en el código legacy VB.NET FrmDeduccionesSoda.
    /// Maneja la lógica completa de deducciones de soda para empleados.
    /// </summary>
    public sealed class SodaService
    {
        private readonly SodaRepository _repo;
        private readonly SodaDetallesRepository _detallesRepo;
        private readonly ArticulosRepository _articulosRepo;
        private readonly EmpleadoRepository _empleadoRepo;

        public SodaService(
            SodaRepository repo, 
            SodaDetallesRepository detallesRepo,
            ArticulosRepository articulosRepo,
            EmpleadoRepository empleadoRepo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _detallesRepo = detallesRepo ?? throw new ArgumentNullException(nameof(detallesRepo));
            _articulosRepo = articulosRepo ?? throw new ArgumentNullException(nameof(articulosRepo));
            _empleadoRepo = empleadoRepo ?? throw new ArgumentNullException(nameof(empleadoRepo));
        }

        // =========================================================
        //  Consultas y búsquedas
        // =========================================================

        /// <summary>
        /// Obtiene el próximo ID consecutivo para una nueva deducción
        /// Basado en: Verificar_Consecutivo() del código legacy
        /// </summary>
        public long GetNextDeduccionId() => _repo.GetNextId();

        /// <summary>
        /// Busca un empleado por número de carné
        /// Basado en: BuscaEmpleado(ByVal Carne As String) del código legacy
        /// </summary>
        public EmpleadoInfo? BuscarEmpleadoPorCarne(long carne)
        {
            var empleado = _empleadoRepo.GetByCarne(carne);
            if (empleado == null) return null;

            // Construir nombre completo: Nombre + Apellido1 + Apellido2
            var nombreCompleto = $"{empleado.Nombre} {empleado.Primer_Apellido}";
            if (!string.IsNullOrEmpty(empleado.Segundo_Apellido))
            {
                nombreCompleto += $" {empleado.Segundo_Apellido}";
            }

            return new EmpleadoInfo
            {
                Id = empleado.Id,
                Carne = empleado.Carne,
                NombreCompleto = nombreCompleto,
                Foto = empleado.Foto,
                Activo = empleado.Activo
            };
        }

        /// <summary>
        /// Obtiene los artículos de categoría SODA para el ComboBox
        /// Basado en: Llenar_Articulos() del código legacy
        /// </summary>
        public List<ArticuloSoda> GetArticulosSoda()
        {
            var (items, _) = _articulosRepo.GetPage(1, int.MaxValue / 4, "SODA");
            return items.Where(a => a.categoria == "SODA")
                       .Select(a => new ArticuloSoda
                       {
                           Id = a.Id,
                           Descripcion = a.descripcion,
                           Precio = a.precio,
                           Existencia = a.existencia
                       })
                       .OrderBy(a => a.Descripcion)
                       .ToList();
        }

        /// <summary>
        /// Calcula el subtotal de un artículo según cantidad
        /// Basado en la lógica de cálculo del código legacy
        /// </summary>
        public decimal CalcularSubtotal(int articuloId, int cantidad)
        {
            var articulo = _articulosRepo.GetById(articuloId);
            if (articulo == null) throw new InvalidOperationException("Artículo no encontrado");
            
            return (decimal)(articulo.precio * cantidad);
        }

        /// <summary>
        /// Valida un detalle antes de agregarlo
        /// Basado en: Validar_Detalles() del código legacy
        /// </summary>
        public (bool IsValid, string ErrorMessage) ValidarDetalle(int articuloId, int cantidad, long empleadoId)
        {
            if (cantidad <= 0) 
                return (false, "Debe Ingresar una Cantidad para esta Deducción !!!!");

            if (articuloId <= 0) 
                return (false, "Debe Escoger algún Artículo válido para esta Deducción !!!!");

            if (empleadoId <= 0) 
                return (false, "Debe Ingresar un Empleado Válido !!!!");

            var articulo = _articulosRepo.GetById(articuloId);
            if (articulo == null) 
                return (false, "El artículo seleccionado no existe");

            if (articulo.categoria != "SODA") 
                return (false, "El artículo seleccionado no es de categoría SODA");

            return (true, "");
        }

        // =========================================================
        //  Gestión de detalles temporales (para la UI)
        // =========================================================

        /// <summary>
        /// Crea un detalle temporal para mostrar en el DataGridView
        /// Basado en: Agregar() del código legacy
        /// </summary>
        public DetalleTemp CrearDetalleTemporal(int articuloId, int cantidad)
        {
            var articulo = _articulosRepo.GetById(articuloId);
            if (articulo == null) throw new InvalidOperationException("Artículo no encontrado");

            var total = articulo.precio * cantidad;

            return new DetalleTemp
            {
                Cantidad = cantidad,
                Codigo = articulo.Id,
                Descripcion = articulo.descripcion,
                Precio = articulo.precio,
                Total = total
            };
        }

        /// <summary>
        /// Calcula el total de una lista de detalles temporales
        /// Basado en: Calcular_Total() del código legacy
        /// </summary>
        public decimal CalcularTotalDetalles(List<DetalleTemp> detalles)
        {
            return (decimal)detalles.Sum(d => d.Total);
        }

        // =========================================================
        //  Persistencia de deducciones
        // =========================================================

        /// <summary>
        /// Valida una deducción completa antes de guardar
        /// Basado en: Validar() del código legacy
        /// </summary>
        public (bool IsValid, string ErrorMessage) ValidarDeduccion(long empleadoId, List<DetalleTemp> detalles)
        {
            if (empleadoId <= 0) 
                return (false, "Debe Ingresar un Carnet para esta Deducción !!!!");

            var empleado = _empleadoRepo.GetById(empleadoId);
            if (empleado == null) 
                return (false, "Debe Ingresar Carnet válido para esta Deducción !!!!");

            if (detalles == null || detalles.Count == 0) 
                return (false, "Debe Tener Al Menos un Artículo !!!!");

            var total = CalcularTotalDetalles(detalles);
            if (total <= 0) 
                return (false, "Debe Tener Al Menos un Artículo !!!!");

            return (true, "");
        }

        /// <summary>
        /// Registra una nueva deducción de soda con sus detalles
        /// Basado en: Registrar_Click del código legacy
        /// </summary>
        public long RegistrarDeduccion(long empleadoId, List<DetalleTemp> detalles, int usuarioId)
        {
            // Validar
            var (isValid, errorMessage) = ValidarDeduccion(empleadoId, detalles);
            if (!isValid) throw new InvalidOperationException(errorMessage);

            // Crear la deducción principal
            var soda = new Soda
            {
                Id_Empleado = empleadoId,
                Total = (double)CalcularTotalDetalles(detalles),
                Id_Usuario = usuarioId,
                Anulada = false,
                Fecha = DateTime.Now
            };

            // Insertar deducción principal
            var sodaId = _repo.Insert(soda);

            // Insertar detalles
            var sodaDetalles = detalles.Select(d => new Soda_Detalles
            {
                Id_Soda = sodaId,
                Codigo_Articulo = d.Codigo,
                Cantidad = d.Cantidad,
                Precio = d.Precio,
                Total = d.Total
            }).ToList();

            _detallesRepo.InsertBatch(sodaDetalles);

            return sodaId;
        }

        // =========================================================
        //  Consulta de deducciones existentes
        // =========================================================

        /// <summary>
        /// Busca deducciones existentes para el buscador
        /// Basado en el SQL del FrmBuscadorHoras usado en Buscar_Click
        /// </summary>
        public List<DeduccionResumen> BuscarDeducciones(string? filtro = null)
        {
            var (items, _) = _repo.GetPage(1, 1000, filtro);
            
            var result = new List<DeduccionResumen>();
            foreach (var soda in items)
            {
                var empleado = _empleadoRepo.GetById(soda.Id_Empleado);
                if (empleado != null)
                {
                    result.Add(new DeduccionResumen
                    {
                        Id = soda.Id,
                        Carne = empleado.Carne.ToString(),
                        Nombre = $"{empleado.Nombre} {empleado.Primer_Apellido}",
                        Fecha = soda.Fecha,
                        Total = soda.Total,
                        Anulada = soda.Anulada
                    });
                }
            }

            return result.OrderByDescending(d => d.Fecha).ToList();
        }

        /// <summary>
        /// Carga una deducción existente para edición/consulta
        /// Basado en la lógica de Buscar_Click del código legacy
        /// </summary>
        public DeduccionCompleta? CargarDeduccion(long id)
        {
            var soda = _repo.GetById(id);
            if (soda == null) return null;

            var empleado = _empleadoRepo.GetById(soda.Id_Empleado);
            if (empleado == null) return null;

            var detalles = _detallesRepo.GetBySodaId(id);

            return new DeduccionCompleta
            {
                Id = soda.Id,
                EmpleadoInfo = new EmpleadoInfo
                {
                    Id = empleado.Id,
                    Carne = empleado.Carne,
                    NombreCompleto = $"{empleado.Nombre} {empleado.Primer_Apellido}",
                    Foto = empleado.Foto,
                    Activo = empleado.Activo
                },
                Total = soda.Total,
                Fecha = soda.Fecha,
                Anulada = soda.Anulada,
                Id_Usuario = soda.Id_Usuario,
                Detalles = detalles.Select(d => new DetalleTemp
                {
                    Cantidad = d.Cantidad,
                    Codigo = d.Codigo_Articulo,
                    Descripcion = d.DescripcionArticulo,
                    Precio = d.Precio,
                    Total = d.Total
                }).ToList()
            };
        }

        // =========================================================
        //  Anulación de deducciones
        // =========================================================

        /// <summary>
        /// Anula una deducción existente
        /// Basado en: Eliminar_Click del código legacy
        /// </summary>
        public void AnularDeduccion(long id, int usuarioId)
        {
            var soda = _repo.GetById(id);
            if (soda == null) 
                throw new InvalidOperationException("La deducción no existe");

            if (soda.Anulada) 
                throw new InvalidOperationException("No se puede Anular deducción ya está Anulada....");

            _repo.Anular(id, usuarioId);
        }

        // =========================================================
        //  Paginación para la UI (si se necesita)
        // =========================================================
        public (DataTable page, int total) GetPageAsDataTable(int pageIndex, int pageSize, string? filtro)
        {
            var result = _repo.GetPage(pageIndex, pageSize, filtro);
            return (ToDataTable(result.items), result.total);
        }

        private static DataTable ToDataTable(IEnumerable<Soda> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(long));
            dt.Columns.Add("Id_Empleado", typeof(long));
            dt.Columns.Add("Total", typeof(double));
            dt.Columns.Add("Id_Usuario", typeof(int));
            dt.Columns.Add("Anulada", typeof(string));
            dt.Columns.Add("Fecha", typeof(DateTime));

            foreach (var item in items)
            {
                dt.Rows.Add(
                    item.Id,
                    item.Id_Empleado,
                    item.Total,
                    item.Id_Usuario,
                    item.Anulada ? "Sí" : "No",
                    item.Fecha
                );
            }

            return dt;
        }
    }

    // =========================================================
    //  Clases de apoyo para la transferencia de datos
    // =========================================================

    /// <summary>
    /// Información básica del empleado para la UI
    /// </summary>
    public class EmpleadoInfo
    {
        public long Id { get; set; }
        public long Carne { get; set; }
        public string NombreCompleto { get; set; } = "";
        public string? Foto { get; set; }
        public int Activo { get; set; }
    }

    /// <summary>
    /// Información de artículo para el ComboBox
    /// </summary>
    public class ArticuloSoda
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = "";
        public double Precio { get; set; }
        public int Existencia { get; set; }
    }

    /// <summary>
    /// Detalle temporal para manejar en la UI antes de guardar
    /// Equivale a las filas del DataTable en el código legacy
    /// </summary>
    public class DetalleTemp
    {
        public int Cantidad { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; } = "";
        public double Precio { get; set; }
        public double Total { get; set; }
    }

    /// <summary>
    /// Resumen de deducción para listados y búsquedas
    /// </summary>
    public class DeduccionResumen
    {
        public long Id { get; set; }
        public string Carne { get; set; } = "";
        public string Nombre { get; set; } = "";
        public DateTime Fecha { get; set; }
        public double Total { get; set; }
        public bool Anulada { get; set; }
    }

    /// <summary>
    /// Deducción completa con todos sus detalles
    /// </summary>
    public class DeduccionCompleta
    {
        public long Id { get; set; }
        public EmpleadoInfo EmpleadoInfo { get; set; } = null!;
        public double Total { get; set; }
        public DateTime Fecha { get; set; }
        public bool Anulada { get; set; }
        public int Id_Usuario { get; set; }
        public List<DetalleTemp> Detalles { get; set; } = new();
    }
}