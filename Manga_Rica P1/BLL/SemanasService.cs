using System;
using System.Collections.Generic;
using System.Data;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Lógica de negocio para Semanas (sin interfaz).
    /// Reglas portadas del módulo viejo:
    ///  - Semana requerida
    ///  - Fecha_Final >= Fecha_Inicio
    /// </summary>
    public sealed class SemanasService
    {
        private readonly SemanaRepository _repo;
        public SemanasService(SemanaRepository repo) => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        // Listado paginado para PagedSearchGrid
        public (DataTable page, int total) GetPageAsDataTable(int pageIndex, int pageSize, string? filtro)
        {
            var res = _repo.GetPage(pageIndex, pageSize, filtro);
            return (ToDataTable(res.items), res.total);
        }

        // Consultas
        public Semana? Get(int id) => _repo.GetById(id);

        // Comandos
        public int Create(Semana s)
        {
            if (s is null) throw new ArgumentNullException(nameof(s));
            Normalize(s);
            Validate(s);
            return _repo.Insert(s);
        }

        public void Update(Semana s)
        {
            if (s is null) throw new ArgumentNullException(nameof(s));
            Normalize(s);
            Validate(s);
            _repo.Update(s);
        }

        public void Delete(int id) => _repo.Delete(id);

        // Helpers
        private static void Normalize(Semana s)
        {
            // Redondear fecha a día (sin hora) por consistencia, opcional:
            s.fecha_Inicio = s.fecha_Inicio.Date;
            s.fecha_Final = s.fecha_Final.Date;
        }

        private static void Validate(Semana s)
        {
            if (s.semana <= 0)
                throw new ArgumentException("El número de semana es obligatorio y debe ser mayor a cero.");

            if (s.fecha_Final < s.fecha_Inicio)
                throw new ArgumentException("La fecha final no puede ser menor que la fecha inicial.");
        }

        private static DataTable ToDataTable(IEnumerable<Semana> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Semana", typeof(int));
            dt.Columns.Add("Fecha_Inicio", typeof(DateTime));
            dt.Columns.Add("Fecha_Final", typeof(DateTime));

            foreach (var s in items)
                dt.Rows.Add(s.Id, s.semana, s.fecha_Inicio, s.fecha_Final);

            return dt;
        }
    }
}
