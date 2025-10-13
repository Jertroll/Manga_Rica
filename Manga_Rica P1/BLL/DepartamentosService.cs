using System;
using System.Collections.Generic;
using System.Data;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Lógica de negocio para Departamentos (sin interfaz).
    /// </summary>
    public sealed class DepartamentosService
    {
        private readonly DepartamentoRepository _repo;
        public DepartamentosService(DepartamentoRepository repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        // ===== Listado paginado para PagedSearchGrid =====
        // Retorna (DataTable page, int total)
        public (DataTable page, int total) GetPageAsDataTable(int pageIndex, int pageSize, string? filtro)
        {
            var res = _repo.GetPage(pageIndex, pageSize, filtro);
            return (ToDataTable(res.items), res.total);
        }

        // ===== Consultas =====
        public Departamento? Get(int id) => _repo.GetById(id);

        // ===== Comandos =====
        public int Create(Departamento d)
        {
            if (d is null) throw new ArgumentNullException(nameof(d));
            Normalize(d);
            Validate(d, isCreate: true);
            return _repo.Insert(d);
        }

        public void Update(Departamento d)
        {
            if (d is null) throw new ArgumentNullException(nameof(d));
            Normalize(d);
            Validate(d, isCreate: false);
            _repo.Update(d);
        }

        public void Delete(int id) => _repo.Delete(id);

        // ===== Helpers =====
        private static void Normalize(Departamento d)
        {
            d.nombre = (d.nombre ?? "").Trim();
            d.codigo = (d.codigo ?? "").Trim();
        }

        public IReadOnlyList<Departamento> GetAll() =>
       _repo.GetAllOrdered().ToList();


        private void Validate(Departamento d, bool isCreate)
        {
            // Requeridos
            if (string.IsNullOrWhiteSpace(d.nombre))
                throw new ArgumentException("El nombre del departamento es obligatorio.");
            if (string.IsNullOrWhiteSpace(d.codigo))
                throw new ArgumentException("El código del departamento es obligatorio.");

            // Tamaños (ajusta si tu esquema usa otras longitudes)
            if (d.nombre.Length > 100)
                throw new ArgumentException("El nombre excede el límite permitido (100).");
            if (d.codigo.Length > 50)
                throw new ArgumentException("El código excede el límite permitido (50).");

            // Unicidades
            if (_repo.ExistsByNombre(d.nombre, isCreate ? null : d.Id))
                throw new InvalidOperationException("Ya existe un departamento con ese nombre.");
            if (_repo.ExistsByCodigo(d.codigo, isCreate ? null : d.Id))
                throw new InvalidOperationException("Ya existe un departamento con ese código.");
        }

        private static DataTable ToDataTable(IEnumerable<Departamento> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Codigo", typeof(string));
            dt.Columns.Add("Departamento", typeof(string));

            foreach (var d in items)
                dt.Rows.Add(d.Id, d.codigo, d.nombre);

            return dt;
        }
    }
}
