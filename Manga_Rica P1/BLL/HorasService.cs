// Nueva implementacion
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.BLL
{
    public sealed class HorasService
    {
        // <-- Usamos los repos concretos, igual que en los demás módulos
        private readonly EmpleadoRepository _empRepo;
        private readonly IHorasRepository _horasRepo;

        // Orden que usas en Program: (horaRepo, empleadoRepo)
        public HorasService(IHorasRepository horasRepo, EmpleadoRepository empRepo)
        {
            _horasRepo = horasRepo ?? throw new ArgumentNullException(nameof(horasRepo));
            _empRepo = empRepo ?? throw new ArgumentNullException(nameof(empRepo));
        }

        // === Paginación (empleados activos) ===
        // CORREGIDO: Aplicamos el filtro de activos correctamente y calculamos el total real
        public (DataTable page, int total) GetActiveEmployeesPageAsDataTable(int pageIndex, int pageSize, string filtro)
        {
            // Obtenemos todos los empleados activos que coinciden con el filtro
            var allActiveEmployees = GetAllActiveEmployeesFiltered(filtro);
            
            // Calculamos el total real de empleados activos
            int totalActive = allActiveEmployees.Count;
            
            // Aplicamos paginación manual sobre los empleados activos
            var pagedActiveEmployees = allActiveEmployees
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            // Creamos el DataTable con los empleados de la página actual
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Cedula", typeof(string));
            dt.Columns.Add("Apellido 1", typeof(string));
            dt.Columns.Add("Apellido 2", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Celular", typeof(string));
            dt.Columns.Add("MC_Numero", typeof(long));
            dt.Columns.Add("Carne", typeof(long));

            foreach (var e in pagedActiveEmployees)
            {
                dt.Rows.Add(
                    e.Id,
                    e.Cedula ?? "",
                    e.Primer_Apellido ?? "",
                    e.Segundo_Apellido ?? "",
                    e.Nombre ?? "",
                    e.Celular ?? "",
                    e.MC_Numero,
                    e.Carne
                );
            }

            return (dt, totalActive);
        }

        // Método auxiliar para obtener todos los empleados activos que coinciden con el filtro
        private List<Empleado> GetAllActiveEmployeesFiltered(string filtro)
        {
            var allActiveEmployees = new List<Empleado>();
            int pageIndex = 0;
            const int batchSize = 1000; // Procesar en lotes grandes para eficiencia
            
            while (true)
            {
                var (items, totalFromRepo) = _empRepo.GetPage(pageIndex, batchSize, filtro);
                
                // Si no hay más elementos, salir
                if (!items.Any())
                    break;
                
                // Filtrar solo empleados activos y agregarlos a la lista
                var activeInThisBatch = items.Where(e => e.Activo == 1).ToList();
                allActiveEmployees.AddRange(activeInThisBatch);
                
                // Si hemos procesado todos los registros disponibles, salir
                if (items.Count() < batchSize || totalFromRepo <= (pageIndex + 1) * batchSize)
                    break;
                    
                pageIndex++;
                
                // Límite de seguridad para evitar bucles infinitos
                if (pageIndex > 100)
                    break;
            }
            
            return allActiveEmployees;
        }

        public (DataTable page, int total) GetHorasPageAsDataTable(int pageIndex, int pageSize, string filtro)
            => _horasRepo.GetHorasPage(pageIndex, pageSize, filtro ?? "");

        // ==== Operaciones de negocio (igual que ya tenías) ====
        public void RegistrarEntrada(long carne, DateTime fecha, DateTime horaEntrada, int idUsuario)
        {
            var emp = _empRepo.GetIdentidadBasica(carne);
            if (emp == null)
                throw new InvalidOperationException("Empleado no existe o está inactivo.");

            var abiertaHoy = _horasRepo.GetAbiertaHoy(carne, fecha);
            if (abiertaHoy != null)
                throw new InvalidOperationException("Ya existe una entrada pendiente de salida para este día.");

            _horasRepo.InsertEntrada(emp.Value.Id, carne, fecha.Date, horaEntrada, idUsuario);
        }

        public void RegistrarSalida(long carne, DateTime horaSalida, double maxHoras = 15d)
        {
            if (!_empRepo.ExisteActivoPorCarne(carne))
                throw new InvalidOperationException("Empleado no existe o está inactivo.");

            var abierta = _horasRepo.GetAbiertaHoy(carne, horaSalida.Date)
                       ?? _horasRepo.GetAbiertaAyer(carne, horaSalida.Date.AddDays(-1));

            if (abierta == null)
                throw new InvalidOperationException("No hay una entrada abierta para cerrar.");

            if (horaSalida < abierta.Hora_Entrada)
                throw new InvalidOperationException("La hora de salida no puede ser menor que la de entrada.");

            var horas = (horaSalida - abierta.Hora_Entrada).TotalHours;
            if (maxHoras > 0 && horas > maxHoras)
                throw new InvalidOperationException($"La jornada no puede exceder {maxHoras} horas.");

            var total = Math.Round((decimal)horas, 2);
            var ok = _horasRepo.TryCerrar(abierta.Id, horaSalida, total);
            if (!ok)
                throw new InvalidOperationException("La marca ya fue cerrada por otro proceso.");
        }

        public Hora? GetById(long id) => _horasRepo.GetById(id);

        public void UpdateEntrada(Hora h)
        {
            if (h.Hora_Salida != null)
                throw new InvalidOperationException("No se puede editar la entrada de una marca ya cerrada.");
            _horasRepo.UpdateEntrada(h);
        }
    }
}
