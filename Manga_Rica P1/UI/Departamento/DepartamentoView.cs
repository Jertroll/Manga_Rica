// Nueva implementacion
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Manga_Rica_P1.UI.Helpers;          // PagedSearchGrid
using Manga_Rica_P1.Entity;              //  Entidad Departamento
using Manga_Rica_P1.UI.Departamentos; 



namespace Manga_Rica_P1.UI.Departamentos
{
    public partial class DepartamentoView : UserControl
    {
        // Nueva implementacion: fuente demo en memoria
        private DataTable _tablaCompleta = new();

        // Nueva implementacion: grid reutilizable con búsqueda + paginación
        private PagedSearchGrid pagedGrid;

        public DepartamentoView()
        {
            // Nueva implementacion: limpiar y montar el control compuesto
            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Departamentos"
            };

            BuildDemoTable();
            pagedGrid.GetAllFilteredDataTable = FiltroLocalComoDataTable;

            pagedGrid.NewRequested += (s, e) => Nuevo();
            pagedGrid.EditRequested += (s, e) => Editar();
            pagedGrid.DeleteRequested += (s, e) => Eliminar();

            Controls.Add(pagedGrid);

            pagedGrid.RefreshData();
        }


        // Nueva implementacion: datos demo
        private void BuildDemoTable()
        {
            _tablaCompleta.Columns.Add("Id", typeof(int));
            _tablaCompleta.Columns.Add("Departamento", typeof(string));
            _tablaCompleta.Columns.Add("Codigo", typeof(string));

            _tablaCompleta.Rows.Add(1, "Recursos Humanos", "RH-01");
            _tablaCompleta.Rows.Add(2, "Contabilidad", "CT-02");
            _tablaCompleta.Rows.Add(3, "Producción", "PR-03");
            _tablaCompleta.Rows.Add(4, "Bodega", "BD-04");
            _tablaCompleta.Rows.Add(5, "Compras", "CP-05");
            _tablaCompleta.Rows.Add(6, "Ventas", "VT-06");
            _tablaCompleta.Rows.Add(7, "Logística", "LG-07");
            _tablaCompleta.Rows.Add(8, "Calidad", "CQ-08");
            _tablaCompleta.Rows.Add(9, "Mantenimiento", "MT-09");
            _tablaCompleta.Rows.Add(10, "Sistemas", "IT-10");
        }

        // Nueva implementacion: filtro local devolviendo DataTable
        private DataTable FiltroLocalComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tablaCompleta.Copy();

            string f = filtro.Trim().ToLowerInvariant();

            var query = _tablaCompleta.AsEnumerable().Where(r =>
                r.Field<int>("Id").ToString().Contains(f) ||
                (r.Field<string>("Departamento") ?? string.Empty).ToLowerInvariant().Contains(f) ||
                (r.Field<string>("Codigo") ?? string.Empty).ToLowerInvariant().Contains(f)
            );

            var tbl = _tablaCompleta.Clone();
            foreach (var row in query) tbl.ImportRow(row);
            return tbl;
        }

        // ====== CRUD ======
        private void Nuevo()
        {
            // Nota: si tu AddDepartamento vive en otro namespace, ajusta el using o usa nombre calificado
            using var dlg = new AddDepartamento(); // modal
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var r = dlg.Result; // DepartamentoResult
                int newId = _tablaCompleta.Rows.Count == 0
                    ? 1
                    : _tablaCompleta.AsEnumerable().Max(x => x.Field<int>("Id")) + 1;

                _tablaCompleta.Rows.Add(newId, r.nombre, r.codigo);

                pagedGrid.RefreshData();
            }
        }

        private void Editar()
        {
            // Requiere que tu PagedSearchGrid exponga SelectedId (como en tu UserView)
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            var fila = _tablaCompleta.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == id.Value);
            if (fila is null) return;

            // Seed de entidad (ENTITY) para el modal
            var seed = new Departamento
            {
                Id = id.Value,
                nombre = fila.Field<string>("Departamento") ?? string.Empty,
                codigo = fila.Field<string>("Codigo") ?? string.Empty
            };

            using var dlg = new AddDepartamento(seed);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var r = dlg.Result; // DepartamentoResult
                fila.SetField("Departamento", r.nombre);
                fila.SetField("Codigo", r.codigo);

                pagedGrid.RefreshData();
            }
        }

        private void Eliminar()
        {
            // Requiere que tu PagedSearchGrid exponga SelectedIds (como en tu UserView)
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0) return;

            string detalle = ids.Count == 1 ? $"Id {ids[0]}" : $"{ids.Count} departamentos";
            var dr = MessageBox.Show($"Confirmar acción?\n\nSe eliminará: {detalle}",
                                     "Confirmar acción",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Warning,
                                     MessageBoxDefaultButton.Button2);
            if (dr != DialogResult.Yes) return;

            foreach (var id in ids)
            {
                var fila = _tablaCompleta.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == id);
                if (fila != null) _tablaCompleta.Rows.Remove(fila);
            }

            pagedGrid.RefreshData();
        }
    }
}
