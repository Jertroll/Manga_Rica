using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Reglas de negocio para Artículos.
    /// </summary>
    public sealed class ArticulosService
    {
        private readonly ArticulosRepository _repo;

        // catálogo fijo (según sistema viejo)
        public static readonly string[] CategoriasPermitidas = new[] { "UNIFORMES", "SODA" };

        public ArticulosService(ArticulosRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // =========================================================
        //  Paginación para la UI (retorna DataTable + total)
        // =========================================================
        public (DataTable page, int total) GetPageAsDataTable(int pageIndex, int pageSize, string? filtro)
        {
            var (items, total) = _repo.GetPage(pageIndex, pageSize, filtro);
            return (ToDataTable(items), total);
        }

        // =========================================================
        //  Lecturas puntuales
        // =========================================================
        public Articulos? Get(int id) => _repo.GetById(id);

        // =========================================================
        //  Comandos (CRUD)
        // =========================================================
        public int Create(Articulos a)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));

            // En el sistema viejo, Existencia al crear se inicializaba en 0.
            if (a.existencia < 0) a.existencia = 0;

            Normalize(a);
            Validate(a, isCreate: true);

            // (Opcional) si quieres evitar duplicados por descripción:
            // if (_repo.ExistsByDescripcion(a.descripcion, null))
            //     throw new InvalidOperationException("Ya existe un artículo con esa descripción.");

            return _repo.Insert(a);
        }

        public void Update(Articulos a)
        {
            if (a is null) throw new ArgumentNullException(nameof(a));

            Normalize(a);
            Validate(a, isCreate: false);

            // (Opcional duplicados):
            // if (_repo.ExistsByDescripcion(a.descripcion, a.Id))
            //     throw new InvalidOperationException("Ya existe un artículo con esa descripción.");

            _repo.Update(a);
        }

        public void Delete(int id) => _repo.Delete(id);

        // =========================================================
        //  Validaciones y normalización
        // =========================================================
        private static void Normalize(Articulos a)
        {
            a.descripcion = (a.descripcion ?? string.Empty).Trim();
            a.categoria = (a.categoria ?? string.Empty).Trim().ToUpperInvariant();
        }

        private static void Validate(Articulos a, bool isCreate)
        {
            if (string.IsNullOrWhiteSpace(a.descripcion))
                throw new ArgumentException("La descripción es obligatoria.");

            if (a.precio <= 0)
                throw new ArgumentException("El precio debe ser mayor que 0.");

            if (a.existencia < 0)
                throw new ArgumentException("La existencia no puede ser negativa.");

            if (string.IsNullOrWhiteSpace(a.categoria))
                throw new ArgumentException("La categoría es obligatoria.");

            if (!CategoriasPermitidas.Contains(a.categoria, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException("Categoría no válida. Solo se permite UNIFORMES o SODA.");
        }

        // =========================================================
        //  Utilidad: mapeo a DataTable para PagedSearchGrid
        // =========================================================
        private static DataTable ToDataTable(IEnumerable<Articulos> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            dt.Columns.Add("Precio", typeof(float));
            dt.Columns.Add("Existencia", typeof(int));
            dt.Columns.Add("Categoria", typeof(string));

            foreach (var a in items)
                dt.Rows.Add(a.Id, a.descripcion, a.precio, a.existencia, a.categoria);

            return dt;
        }
    }
}
