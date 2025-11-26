using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;
using System.Data;

namespace Manga_Rica_P1.BLL
{
    public sealed class PuestosService
    {
        private readonly PuestoRepository _repo;
        public PuestosService(PuestoRepository repo) =>
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        // ===== Listado paginado para PagedSearchGrid =====
        public (DataTable page, int total) GetPageAsDataTable(int pageIndex, int pageSize, string? filtro)
        {
            var (items, total) = _repo.GetPage(pageIndex, pageSize, filtro);
            return (ToDataTable(items), total);
        }

        // ===== Consultas =====
        public PuestoEntity? Get(int id) => _repo.GetById(id);

        public IEnumerable<PuestoEntity> GetAll() => _repo.GetAll();

        // DTO liviano para combos
        public sealed class PuestoItem
        {
            public int Id { get; set; }
            public string Puesto { get; set; } = "";
        }

        /// <summary>
        /// Devuelve todos los puestos para combos (Id + texto).
        /// </summary>
        public List<PuestoItem> GetAllForCombo()
        {
            var items = _repo.GetAll();
            var list = new List<PuestoItem>();

            foreach (var p in items)
            {
                list.Add(new PuestoItem
                {
                    Id = p.Id,
                    Puesto = p.puesto
                });
            }

            return list;
        }

        // ===== Comandos =====
        public int Create(PuestoEntity p)
        {
            if (p is null) throw new ArgumentNullException(nameof(p));

            Normalize(p);
            Validate(p, isCreate: true);

            return _repo.Insert(p);
        }

        public void Update(PuestoEntity p)
        {
            if (p is null) throw new ArgumentNullException(nameof(p));
            if (p.Id <= 0) throw new ArgumentException("Id inválido para actualizar Puesto.");

            Normalize(p);
            Validate(p, isCreate: false);

            _repo.Update(p);
        }

        public void Delete(int id)
        {
            if (id <= 0) throw new ArgumentException("Id inválido.", nameof(id));

            // Aquí podrías validar si el Puesto está siendo usado por Empleados, etc.
            _repo.Delete(id);
        }

        // ===== Helpers =====
        private static void Normalize(PuestoEntity p)
        {
            p.puesto = (p.puesto ?? string.Empty).Trim();
            p.descripcion = (p.descripcion ?? string.Empty).Trim();
        }

        private void Validate(PuestoEntity p, bool isCreate)
        {
            if (string.IsNullOrWhiteSpace(p.puesto))
                throw new ArgumentException("El nombre del puesto es obligatorio.");

            if (p.puesto.Length > 150)
                throw new ArgumentException("El nombre del puesto excede los 150 caracteres.");

            if (p.descripcion.Length > 500)
                throw new ArgumentException("La descripción excede los 500 caracteres.");

            // Unicidad por nombre de puesto
            if (_repo.ExistsByNombre(p.puesto, isCreate ? null : p.Id))
                throw new InvalidOperationException("Ya existe un puesto con ese nombre.");
        }

        private static DataTable ToDataTable(IEnumerable<PuestoEntity> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Puesto", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));

            foreach (var p in items)
                dt.Rows.Add(p.Id, p.puesto, p.descripcion);

            return dt;
        }
    }
}
