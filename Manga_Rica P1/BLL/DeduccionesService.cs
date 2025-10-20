using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.ENTITY;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Servicio de lógica de negocio para Deducciones de Uniformes.
    /// Basado en el SodaService pero adaptado para manejar deducciones de uniformes.
    /// Implementa toda la funcionalidad del código legacy para uniformes.
    /// </summary>
    public sealed class DeduccionesService
    {
        private readonly DeduccionesRepository _repo;
        private readonly DeduccionesDetallesRepository _detallesRepo;
        private readonly ArticulosRepository _articulosRepo;
        private readonly EmpleadoRepository _empleadoRepo;

        public DeduccionesService(
            DeduccionesRepository repo, 
            DeduccionesDetallesRepository detallesRepo,
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
        public EmpleadoInfoUniforme? BuscarEmpleadoPorCarne(long carne)
        {
            var empleado = _empleadoRepo.GetByCarne(carne);
            if (empleado == null) return null;

            // Construir nombre completo: Nombre + Apellido1 + Apellido2
            var nombreCompleto = $"{empleado.Nombre} {empleado.Primer_Apellido}";
            if (!string.IsNullOrEmpty(empleado.Segundo_Apellido))
            {
                nombreCompleto += $" {empleado.Segundo_Apellido}";
            }

            return new EmpleadoInfoUniforme
            {
                Id = (int)empleado.Id, // Cast de long a int
                Carne = empleado.Carne,
                NombreCompleto = nombreCompleto,
                Foto = empleado.Foto,
                Activo = empleado.Activo
            };
        }

        /// <summary>
        /// Obtiene los artículos de categoría UNIFORMES para el ComboBox
        /// Basado en: Llenar_Articulos() del código legacy
        /// </summary>
        public List<ArticuloUniforme> GetArticulosUniformes()
        {
            var (items, _) = _articulosRepo.GetPage(1, int.MaxValue / 4, "UNIFORMES");
            return items.Where(a => a.categoria == "UNIFORMES")
                       .Select(a => new ArticuloUniforme
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
        /// Valida un detalle temporal antes de agregarlo a la lista
        /// Basado en las validaciones del código legacy
        /// </summary>
        public (bool IsValid, string ErrorMessage) ValidarDetalleTemp(int articuloId, int cantidad, List<DetalleUniformeTemp> detallesExistentes)
        {
            if (cantidad <= 0)
                return (false, "La cantidad debe ser mayor a cero");

            var articulo = _articulosRepo.GetById(articuloId);
            if (articulo == null)
                return (false, "Artículo no encontrado");

            if (articulo.existencia < cantidad)
                return (false, $"No hay suficiente existencia. Disponible: {articulo.existencia}");

            // Verificar si ya existe en la lista temporal
            if (detallesExistentes.Any(d => d.Codigo == articuloId))
                return (false, "Este artículo ya está agregado a la deducción");

            return (true, "");
        }

        /// <summary>
        /// Crea un detalle temporal para mostrar en el DataGridView
        /// Basado en la lógica de agregado del código legacy
        /// </summary>
        public DetalleUniformeTemp CrearDetalleTemp(int articuloId, int cantidad)
        {
            var articulo = _articulosRepo.GetById(articuloId);
            if (articulo == null)
                throw new InvalidOperationException("Artículo no encontrado");

            var total = articulo.precio * cantidad;

            return new DetalleUniformeTemp
            {
                Cantidad = cantidad,
                Codigo = articuloId,
                Descripcion = articulo.descripcion,
                Precio = articulo.precio,
                Total = total
            };
        }

        /// <summary>
        /// Calcula el total de una lista de detalles temporales
        /// </summary>
        public decimal CalcularTotalDetalles(List<DetalleUniformeTemp> detalles)
        {
            return (decimal)detalles.Sum(d => d.Total);
        }

        // =========================================================
        //  Operaciones CRUD principales
        // =========================================================

        /// <summary>
        /// Guarda una nueva deducción con sus detalles
        /// Basado en: Guardar() del código legacy
        /// </summary>
        public long GuardarDeduccion(DeduccionUniformeCompleta deduccionCompleta, int usuarioId)
        {
            if (deduccionCompleta.Empleado == null)
                throw new ArgumentException("Debe especificar un empleado");

            if (!deduccionCompleta.Detalles.Any())
                throw new ArgumentException("Debe agregar al menos un artículo");

            // Validar existencia de todos los artículos
            foreach (var detalle in deduccionCompleta.Detalles)
            {
                var (isValid, errorMessage) = ValidarDetalleTemp(
                    detalle.Codigo, 
                    detalle.Cantidad, 
                    deduccionCompleta.Detalles.Where(d => d.Codigo != detalle.Codigo).ToList());
                
                if (!isValid)
                    throw new InvalidOperationException($"Error en artículo {detalle.Descripcion}: {errorMessage}");
            }

            // Calcular totales
            var total = CalcularTotalDetalles(deduccionCompleta.Detalles);
            var saldo = total; // El saldo inicial es igual al total

            // Crear la deducción principal
            var deduccion = new Deducciones
            {
                Id_Empleado = deduccionCompleta.Empleado.Id,
                Total = (float)total,
                Saldo = (float)saldo,
                Id_Usuario = usuarioId,
                Anulada = false, // No anulada
                Fecha = deduccionCompleta.Fecha
            };

            // Guardar en transacción (simulada con try-catch)
            var deduccionId = _repo.Insert(deduccion);

            try
            {
                // Crear los detalles
                var detallesEntity = deduccionCompleta.Detalles.Select(d => new Deducciones_Detalles
                {
                    Id_Deduccion = deduccionId,
                    Codigo_Articulo = d.Codigo,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    Total = d.Total
                }).ToList();

                _detallesRepo.InsertBatch(detallesEntity);
                
                return deduccionId;
            }
            catch
            {
                // Si falla la inserción de detalles, eliminar la deducción
                _repo.Delete((int)deduccionId);
                throw;
            }
        }

        /// <summary>
        /// Carga una deducción completa por ID
        /// Basado en la funcionalidad de consulta del código legacy
        /// </summary>
        public DeduccionUniformeCompleta? CargarDeduccionCompleta(int deduccionId)
        {
            var deduccion = _repo.GetById(deduccionId);
            if (deduccion == null) return null;

            var empleado = _empleadoRepo.GetById(deduccion.Id_Empleado);
            if (empleado == null) return null;

            var detalles = _detallesRepo.GetByDeduccionId(deduccionId);

            // Construir el empleado info
            var nombreCompleto = $"{empleado.Nombre} {empleado.Primer_Apellido}";
            if (!string.IsNullOrEmpty(empleado.Segundo_Apellido))
            {
                nombreCompleto += $" {empleado.Segundo_Apellido}";
            }

            var empleadoInfo = new EmpleadoInfoUniforme
            {
                Id = (int)empleado.Id, // Cast de long a int
                Carne = empleado.Carne,
                NombreCompleto = nombreCompleto,
                Foto = empleado.Foto,
                Activo = empleado.Activo
            };

            // Construir los detalles con información de artículos
            var detallesTemp = new List<DetalleUniformeTemp>();
            foreach (var detalle in detalles)
            {
                var articulo = _articulosRepo.GetById(detalle.Codigo_Articulo);
                detallesTemp.Add(new DetalleUniformeTemp
                {
                    Cantidad = detalle.Cantidad,
                    Codigo = detalle.Codigo_Articulo,
                    Descripcion = articulo?.descripcion ?? "Artículo no encontrado",
                    Precio = detalle.Precio,
                    Total = detalle.Total
                });
            }

            return new DeduccionUniformeCompleta
            {
                Id = deduccion.Id,
                Empleado = empleadoInfo,
                Fecha = deduccion.Fecha,
                Total = deduccion.Total,
                Saldo = deduccion.Saldo,
                Anulada = deduccion.Anulada,
                Detalles = detallesTemp
            };
        }

        /// <summary>
        /// Anula una deducción
        /// Basado en: AnularDeduccion() del código legacy
        /// </summary>
        public void AnularDeduccion(int deduccionId, int usuarioId)
        {
            var deduccion = _repo.GetById(deduccionId);
            if (deduccion == null)
                throw new InvalidOperationException("Deducción no encontrada");

            if (deduccion.Anulada)
                throw new InvalidOperationException("La deducción ya está anulada");

            _repo.Anular(deduccionId, usuarioId);
        }

        /// <summary>
        /// Actualiza el saldo de una deducción (para pagos parciales)
        /// </summary>
        public void ActualizarSaldo(int deduccionId, float nuevoSaldo)
        {
            var d = _repo.GetById(deduccionId) ?? throw new InvalidOperationException("Deducción no encontrada");
            if (d.Anulada) throw new InvalidOperationException("No se puede modificar una deducción anulada");
            if (nuevoSaldo < 0) throw new ArgumentException("El saldo no puede ser negativo");
            if (nuevoSaldo > d.Total) throw new ArgumentException("El saldo no puede exceder el total");

            _repo.ActualizarSaldo(deduccionId, nuevoSaldo);
        }

        // =========================================================
        //  Búsquedas y listados
        // =========================================================

        /// <summary>
        /// Busca deducciones con filtro simple (para buscadores de UI)
        /// Basado en la lógica del SodaService
        /// </summary>
        public List<DeduccionResumenUniforme> BuscarDeduccionesSimple(string? filtro = null)
        {
            var (items, _) = BuscarDeducciones(1, 1000, filtro);
            return items.OrderByDescending(d => d.Fecha).ToList();
        }

        /// <summary>
        /// Busca deducciones para el buscador con filtros
        /// Basado en la funcionalidad de búsqueda del código legacy
        /// </summary>
        public (List<DeduccionResumenUniforme> items, int total) BuscarDeducciones(int pageIndex, int pageSize, string? filtro = null)
        {
            var (deducciones, total) = _repo.GetPage(pageIndex, pageSize, filtro);
            
            var resumen = deducciones.Select(d =>
            {
                var empleado = _empleadoRepo.GetById(d.Id_Empleado);
                return new DeduccionResumenUniforme
                {
                    Id = d.Id,
                    Carne = empleado?.Carne.ToString() ?? "N/A",
                    Nombre = empleado != null ? $"{empleado.Nombre} {empleado.Primer_Apellido}" : "Empleado no encontrado",
                    Fecha = d.Fecha,
                    Total = d.Total,
                    Saldo = d.Saldo,
                    Anulada = d.Anulada
                };
            }).ToList();

            return (resumen, total);
        }

        /// <summary>
        /// Obtiene deducciones de un empleado específico
        /// </summary>
        public (List<DeduccionResumenUniforme> items, int total) BuscarDeduccionesPorEmpleado(int empleadoId, int pageIndex = 1, int pageSize = 50)
        {
            var (deducciones, total) = _repo.GetByEmpleado(empleadoId, pageIndex, pageSize);
            var empleado = _empleadoRepo.GetById(empleadoId);
            
            var resumen = deducciones.Select(d => new DeduccionResumenUniforme
            {
                Id = d.Id,
                Carne = empleado?.Carne.ToString() ?? "N/A",
                Nombre = empleado != null ? $"{empleado.Nombre} {empleado.Primer_Apellido}" : "Empleado no encontrado",
                Fecha = d.Fecha,
                Total = d.Total,
                Saldo = d.Saldo,
                Anulada = d.Anulada
            }).ToList();

            return (resumen, total);
        }

        // =========================================================
        //  Reportes y estadísticas
        // =========================================================

        /// <summary>
        /// Obtiene estadísticas de artículos más deducidos
        /// </summary>
        public List<EstadisticaArticuloUniforme> ObtenerEstadisticasArticulos(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var estadisticas = _detallesRepo.GetEstadisticasArticulos(fechaInicio, fechaFin);
            
            return estadisticas.Select(e => new EstadisticaArticuloUniforme
            {
                ArticuloId = e.ArticuloId,
                Descripcion = e.Descripcion,
                TotalCantidad = e.TotalCantidad,
                TotalImporte = e.TotalImporte
            }).ToList();
        }

        /// <summary>
        /// Valida si se puede agregar un detalle (inventario, duplicados, etc.)
        /// </summary>
        public (bool isValid, string errorMessage) ValidarDetalle(int articuloId, int cantidad, int empleadoId)
        {
            try
            {
                // Obtener artículo
                var articulo = _articulosRepo.GetById(articuloId);
                if (articulo == null)
                {
                    return (false, "Artículo no encontrado");
                }

                // Verificar existencia
                if (articulo.existencia < cantidad)
                {
                    return (false, $"No hay suficiente existencia. Disponible: {articulo.existencia}");
                }

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, $"Error al validar detalle: {ex.Message}");
            }
        }

        /// <summary>
        /// Crea un detalle temporal para la UI
        /// </summary>
        public DetalleUniformeTemp CrearDetalleTemporal(int articuloId, int cantidad)
        {
            var articulo = _articulosRepo.GetById(articuloId) ?? throw new ArgumentException("Artículo no encontrado");
            
            return new DetalleUniformeTemp
            {
                Cantidad = cantidad,
                Codigo = articuloId,
                Descripcion = articulo.descripcion,
                Precio = articulo.precio,
                Total = articulo.precio * cantidad
            };
        }

        /// <summary>
        /// Registra una nueva deducción con sus detalles
        /// </summary>
        public long RegistrarDeduccion(int empleadoId, List<DetalleUniformeTemp> detalles, int usuarioId)
        {
            try
            {
                // Crear la deducción principal
                var deduccion = new Deducciones
                {
                    Id_Empleado = empleadoId,
                    Fecha = DateTime.Now,
                    Total = (float)CalcularTotalDetalles(detalles),
                    Saldo = (float)CalcularTotalDetalles(detalles), // Inicialmente el saldo es igual al total
                    Anulada = false, // false = no anulada, true = anulada
                    Id_Usuario = usuarioId
                };

                // Insertar deducción y obtener ID
                var deduccionId = _repo.Insert(deduccion);

                // Insertar detalles
                var detallesEntidad = detalles.Select(d => new Deducciones_Detalles
                {
                    Id_Deduccion = deduccionId,
                    Codigo_Articulo = d.Codigo,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    Total = d.Total
                }).ToList();

                _detallesRepo.InsertBatch(detallesEntidad);

                return deduccionId;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al registrar deducción: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Carga una deducción existente para edición/consulta
        /// Basado en la lógica de Buscar_Click del código legacy
        /// </summary>
        public DeduccionUniformeCompleta? CargarDeduccion(long id)
        {
            try
            {
                var deduccion = _repo.GetById(id);
                if (deduccion == null) return null;

                var empleado = _empleadoRepo.GetById(deduccion.Id_Empleado);
                if (empleado == null) return null;

                var detalles = _detallesRepo.GetByDeduccionId(id);

                return new DeduccionUniformeCompleta
                {
                    Id = deduccion.Id,
                    Empleado = new EmpleadoInfoUniforme
                    {
                        Id = (int)empleado.Id,
                        Carne = empleado.Carne,
                        NombreCompleto = $"{empleado.Nombre} {empleado.Primer_Apellido}",
                        Foto = empleado.Foto,
                        Activo = empleado.Activo
                    },
                    Fecha = deduccion.Fecha,
                    Total = deduccion.Total,
                    Saldo = deduccion.Saldo,
                    Anulada = deduccion.Anulada,
                    Detalles = detalles.Select(d =>
                    {
                        var articulo = _articulosRepo.GetById(d.Codigo_Articulo);
                        return new DetalleUniformeTemp
                        {
                            Cantidad = d.Cantidad,
                            Codigo = d.Codigo_Articulo,
                            Descripcion = articulo?.descripcion ?? "Artículo no encontrado",
                            Precio = d.Precio,
                            Total = d.Total
                        };
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al cargar deducción: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Anula una deducción existente
        /// </summary>
        public void AnularDeduccion(long id, int usuarioId)
        {
            try
            {
                _repo.Anular(id, usuarioId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al anular deducción: {ex.Message}", ex);
            }
        }
    }

    // =========================================================
    //  Clases de apoyo (DTOs)
    // =========================================================

    /// <summary>
    /// Información básica del empleado para la UI
    /// </summary>
    public class EmpleadoInfoUniforme
    {
        public int Id { get; set; }
        public long Carne { get; set; }
        public string NombreCompleto { get; set; } = "";
        public string? Foto { get; set; }
        public int Activo { get; set; }
    }

    /// <summary>
    /// Información de artículo para el ComboBox
    /// </summary>
    public class ArticuloUniforme
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = "";
        public float Precio { get; set; }
        public int Existencia { get; set; }
    }

    /// <summary>
    /// Detalle temporal para manejar en la UI antes de guardar
    /// </summary>
    public class DetalleUniformeTemp
    {
        public int Cantidad { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; } = "";
        public double Precio { get; set; }
        public double Total { get; set; }
    }

    /// <summary>
    /// Deducción completa con empleado y detalles
    /// </summary>
    public class DeduccionUniformeCompleta
    {
        public long Id { get; set; }
        public EmpleadoInfoUniforme Empleado { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public double Total { get; set; }
        public double Saldo { get; set; }
        public bool Anulada { get; set; }
        public List<DetalleUniformeTemp> Detalles { get; set; } = new();
    }

    /// <summary>
    /// Resumen de deducción para listados y búsquedas
    /// </summary>
    public class DeduccionResumenUniforme
    {
        public long Id { get; set; }
        public string Carne { get; set; } = "";
        public string Nombre { get; set; } = "";
        public DateTime Fecha { get; set; }
        public double Total { get; set; }
        public double Saldo { get; set; }
        public bool Anulada { get; set; }
    }

    /// <summary>
    /// Estadísticas de artículos para reportes
    /// </summary>
    public class EstadisticaArticuloUniforme
    {
        public int ArticuloId { get; set; }
        public string Descripcion { get; set; } = "";
        public int TotalCantidad { get; set; }
        public float TotalImporte { get; set; }
    }
}