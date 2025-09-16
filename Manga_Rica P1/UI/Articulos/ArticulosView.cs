// Nueva implementacion
using Manga_Rica_P1.Entity;

using Manga_Rica_P1.UI.Articulos.Modales;   // AddArticulo
using Manga_Rica_P1.UI.Helpers;  // PagedSearchGrid
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;


using EntityArticulo = Manga_Rica_P1.Entity.Articulos;

namespace Manga_Rica_P1.UI.Articulos
{
    public partial class ArticulosView : UserControl
    {
        private DataTable _tablaCompleta = new();
        private PagedSearchGrid pagedGrid;

        public ArticulosView()
        {
            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Artículos"
            };

            BuildDemoTable();
            pagedGrid.GetAllFilteredDataTable = FiltroLocalComoDataTable;

            pagedGrid.NewRequested += (_, __) => Nuevo();
            pagedGrid.EditRequested += (_, __) => Editar();
            pagedGrid.DeleteRequested += (_, __) => Eliminar();

            Controls.Add(pagedGrid);
            pagedGrid.RefreshData();
        }

        private void BuildDemoTable()
        {
            _tablaCompleta.Columns.Add("Id", typeof(int));
            _tablaCompleta.Columns.Add("Descripcion", typeof(string));
            _tablaCompleta.Columns.Add("Precio", typeof(float));
            _tablaCompleta.Columns.Add("Existencia", typeof(int));
            _tablaCompleta.Columns.Add("Categoria", typeof(string));

            _tablaCompleta.Rows.Add(1, "Camiseta Manga Rica", 4500, 15, "UNIFORMES");
            _tablaCompleta.Rows.Add(2, "Refresco Natural", 1200, 50, "SODA");
            _tablaCompleta.Rows.Add(3, "Pantalón Corporativo", 9800, 8, "UNIFORMES");
            _tablaCompleta.Rows.Add(4, "Casado con pollo", 3500, 20, "SODA");
        }

        private DataTable FiltroLocalComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tablaCompleta.Copy();

            string f = filtro.Trim().ToLower();
            var query = _tablaCompleta.AsEnumerable().Where(r =>
                r.Field<int>("Id").ToString().Contains(f) ||
                (r.Field<string>("Descripcion") ?? "").ToLower().Contains(f) ||
                r.Field<float>("Precio").ToString().Contains(f) ||
                r.Field<int>("Existencia").ToString().Contains(f) ||
                (r.Field<string>("Categoria") ?? "").ToLower().Contains(f)
            );

            var tbl = _tablaCompleta.Clone();
            foreach (var row in query) tbl.ImportRow(row);
            return tbl;
        }

        private void Nuevo()
        {
            using var dlg = new AddArticulo();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var r = dlg.Result;
                int newId = _tablaCompleta.Rows.Count == 0
                    ? 1
                    : _tablaCompleta.AsEnumerable().Max(x => x.Field<int>("Id")) + 1;

                _tablaCompleta.Rows.Add(newId, r.descripcion, r.precio, r.existencia, r.categoria);
                pagedGrid.RefreshData();
            }
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            var fila = _tablaCompleta.AsEnumerable()
                        .FirstOrDefault(x => x.Field<int>("Id") == id.Value);
            if (fila is null) return;

           
            var seed = new EntityArticulo
            {
                Id = id.Value,
                descripcion = fila.Field<string>("Descripcion") ?? "",
                precio = fila.Field<float>("Precio"),
                existencia = fila.Field<int>("Existencia"),
                categoria = fila.Field<string>("Categoria") ?? ""
            };

            
            using var dlg = new AddArticulo(seed);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var r = dlg.Result; // EntityArticulo
                fila.SetField("Descripcion", r.descripcion);
                fila.SetField("Precio", r.precio);
                fila.SetField("Existencia", r.existencia);
                fila.SetField("Categoria", r.categoria);
                pagedGrid.RefreshData();
            }
        }

        private void Eliminar()
        {
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0) return;

            string detalle = ids.Count == 1 ? $"Id {ids[0]}" : $"{ids.Count} artículos";
            var dr = MessageBox.Show($"¿Eliminar {detalle}?",
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
