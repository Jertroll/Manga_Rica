// Nueva implementacion
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Lógica de negocio para Semanas.
    /// Reglas:
    ///  - Semana requerida (> 0)
    ///  - Fecha_Final >= Fecha_Inicio
    /// </summary>
    public sealed class SemanasService
    {
        private readonly SemanaRepository _repo;
        public SemanasService(SemanaRepository repo) =>
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        // ===== Listado paginado para PagedSearchGrid =====
        public (DataTable page, int total) GetPageAsDataTable(int pageIndex, int pageSize, string? filtro)
        {
            var (items, total) = _repo.GetPage(pageIndex, pageSize, filtro);
            return (ToDataTable(items), total);
        }

        // ===== Consultas =====
        public Semana? Get(int id) => _repo.GetById(id);

        // DTO liviano para combos
        public sealed class SemanaItem
        {
            public int Id { get; set; }
            public string Semana { get; set; } = "";
        }

        /// <summary>
        /// Devuelve todas las semanas para combos (Id + texto).
        /// No hace SQL aquí: reutiliza el repositorio.
        /// </summary>
        public List<SemanaItem> GetAllForCombo()
        {
            // Si tu repo no tiene un "GetAll", usamos paginación con un pageSize grande.
            var (items, _) = _repo.GetPage(pageIndex: 1, pageSize: int.MaxValue / 4, filtro: null);

            var list = new List<SemanaItem>();
            foreach (var s in items)
            {
                // s.semana parece ser int en tu ENTITY; lo convertimos a string para el combo
                list.Add(new SemanaItem
                {
                    Id = s.Id,
                    Semana = s.semana.ToString() // o formatea como necesites: $"Semana {s.semana}"
                });
            }
            return list;
        }

        // ===== Comandos =====
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

        // ===== Helpers =====
        private static void Normalize(Semana s)
        {
            // Redondea la hora (opcional) para consistencia
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

        public IEnumerable<Semana> GetAll() => _repo.GetAll();

        public Semana? GetById(int id) => _repo.GetById(id);
    }
}
