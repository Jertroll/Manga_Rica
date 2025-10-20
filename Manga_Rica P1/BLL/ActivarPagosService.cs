using System;
using System.Collections.Generic;
using Manga_Rica_P1.DAL;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Lógica de negocio para Activar Pagos por semana.
    /// Reglas (portadas del módulo legacy):
    /// - No permite activar una semana que ya tenga registros en Pagos.
    /// - Inserta una fila en Pagos por cada empleado ACTIVO con valores 0 y Registrado=0.
    /// </summary>
    public sealed class ActivarPagosService
    {
        private readonly ActivarPagosRepository _pagosRepo;
        private readonly EmpleadoRepository _empleadoRepo;
        private readonly SemanaRepository _semanaRepo;

        public ActivarPagosService(
            ActivarPagosRepository pagosRepo,
            EmpleadoRepository empleadoRepo,
            SemanaRepository semanaRepo)
        {
            _pagosRepo = pagosRepo ?? throw new ArgumentNullException(nameof(pagosRepo));
            _empleadoRepo = empleadoRepo ?? throw new ArgumentNullException(nameof(empleadoRepo));
            _semanaRepo = semanaRepo ?? throw new ArgumentNullException(nameof(semanaRepo));
        }

        public sealed class SemanaDto
        {
            public int Id { get; set; }
            public string Semana { get; set; } = "";
        }

        /// <summary>
        /// Lista de semanas para el combo (Id + texto Semana).
        /// Reutiliza el paginador del repo (primeras 1000 filas).
        /// </summary>
        public List<SemanaDto> GetSemanas()
        {
            var list = new List<SemanaDto>();
            var (items, _) = _semanaRepo.GetPage(pageIndex: 1, pageSize: 1000, filtro: null);

            foreach (var s in items)
            {
                list.Add(new SemanaDto
                {
                    Id = s.Id,
                    // La entidad tiene "semana" (int), no "Semana".
                    Semana = $"Semana {s.semana}"
                });
            }
            return list;
        }

        /// <summary>
        /// Activa una semana: valida que no exista y crea Pagos=0 para todos los empleados activos.
        /// </summary>
        /// <returns>(insertados, totalEmpleadosActivos)</returns>
        public (int insertados, int totalEmpleados) ActivarSemana(int idSemana, int idUsuario)
        {
            // 1) Validación: esa semana no debe estar activada ya
            if (_pagosRepo.ExisteParaSemana(idSemana))
                throw new InvalidOperationException("Ya fue activada esta semana de pagos.");

            // 2) Obtener empleados activos
            var empleados = new List<long>(_pagosRepo.GetEmpleadosActivosIds());
            var hoy = DateTime.Now;
            var count = 0;

            // Nota: el legacy no usaba transacción. Mantenemos el comportamiento.
            foreach (var empId in empleados)
            {
                _pagosRepo.InsertPagoInicial(empId, idSemana, idUsuario, hoy);
                count++;
            }

            return (count, empleados.Count);
        }
    }
}