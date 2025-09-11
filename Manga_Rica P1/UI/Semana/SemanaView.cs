// Nueva implementacion
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Manga_Rica_P1.UI.Helpers;               // PagedSearchGrid
using Manga_Rica_P1.UI.Semanas.Modales;       
using Manga_Rica_P1.Entity;                   

// Alias opcional (si te gusta dejar claro que es Entity)
using EntitySemana = Manga_Rica_P1.Entity.Semana;

namespace Manga_Rica_P1.UI.Semanas
{
    public partial class SemanaView : UserControl
    {
        private DataTable _tablaCompleta = new();
        private PagedSearchGrid pagedGrid;

        public SemanaView()
        {
            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Semanas"
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
            _tablaCompleta.Columns.Add("Semana", typeof(int));
            _tablaCompleta.Columns.Add("Fecha_Inicio", typeof(DateTime));
            _tablaCompleta.Columns.Add("Fecha_Final", typeof(DateTime));

            _tablaCompleta.Rows.Add(1, 1, new DateTime(2025, 01, 06), new DateTime(2025, 01, 12));
            _tablaCompleta.Rows.Add(2, 2, new DateTime(2025, 01, 13), new DateTime(2025, 01, 19));
            _tablaCompleta.Rows.Add(3, 3, new DateTime(2025, 01, 20), new DateTime(2025, 01, 26));
            _tablaCompleta.Rows.Add(4, 4, new DateTime(2025, 01, 27), new DateTime(2025, 02, 02));
            _tablaCompleta.Rows.Add(5, 5, new DateTime(2025, 02, 03), new DateTime(2025, 02, 09));
        }

        private DataTable FiltroLocalComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tablaCompleta.Copy();

            string f = filtro.Trim().ToLowerInvariant();

            var query = _tablaCompleta.AsEnumerable().Where(r =>
                r.Field<int>("Id").ToString().Contains(f) ||
                r.Field<int>("Semana").ToString().Contains(f) ||
                r.Field<DateTime>("Fecha_Inicio").ToString("yyyy-MM-dd").Contains(f) ||
                r.Field<DateTime>("Fecha_Final").ToString("yyyy-MM-dd").Contains(f)
            );

            var tbl = _tablaCompleta.Clone();
            foreach (var row in query) tbl.ImportRow(row);
            return tbl;
        }

        private void Nuevo()
        {
            using var dlg = new AddSemana();   // devuelve ENTITY.Semana
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var s = dlg.Result;

                int newId = _tablaCompleta.Rows.Count == 0
                    ? 1
                    : _tablaCompleta.AsEnumerable().Max(x => x.Field<int>("Id")) + 1;

                s.Id = newId; // en BD lo haría el IDENTITY
                _tablaCompleta.Rows.Add(s.Id, s.semana, s.fecha_Inicio, s.fecha_Final);

                pagedGrid.RefreshData();
            }
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            var fila = _tablaCompleta.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == id.Value);
            if (fila is null) return;

            var seed = new EntitySemana
            {
                Id = id.Value,
                semana = fila.Field<int>("Semana"),
                fecha_Inicio = fila.Field<DateTime>("Fecha_Inicio"),
                fecha_Final = fila.Field<DateTime>("Fecha_Final")
            };

            using var dlg = new AddSemana(seed);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var s = dlg.Result;
                fila.SetField("Semana", s.semana);
                fila.SetField("Fecha_Inicio", s.fecha_Inicio);
                fila.SetField("Fecha_Final", s.fecha_Final);

                pagedGrid.RefreshData();
            }
        }

        private void Eliminar()
        {
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0) return;

            var dr = MessageBox.Show(
                ids.Count == 1 ? $"¿Eliminar Id {ids[0]}?" : $"¿Eliminar {ids.Count} semanas?",
                "Confirmar acción",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes) return;

            foreach (var _id in ids)
            {
                var fila = _tablaCompleta.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == _id);
                if (fila != null) _tablaCompleta.Rows.Remove(fila);
            }
            pagedGrid.RefreshData();
        }
    }
}
