// Nueva implementacion
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Manga_Rica_P1.UI.Helpers;                 // PagedSearchGrid
using Manga_Rica_P1.UI.Solicitudes.Modales;     // AddSolicitud
using Manga_Rica_P1.ENTITY;
// Alias de la entidad (asegúrate que tu Solicitud esté en este namespace)
using EntitySolicitud = Manga_Rica_P1.Entity.Solicitud;

namespace Manga_Rica_P1.UI.Solicitudes
{
    public partial class SolicitudView : UserControl
    {
        // Nueva implementacion: datos quemados en memoria
        private DataTable _tablaCompleta = new();
        private PagedSearchGrid pagedGrid;

        public SolicitudView()
        {
            // Nueva implementacion: montar grid compuesto
            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Solicitudes"
            };

            BuildDemoTable();

            // Nueva implementacion: modo CLIENTE (DataTable completo + filtro local)
            pagedGrid.GetAllFilteredDataTable = FiltroLocalComoDataTable;

            // Nueva implementacion: eventos CRUD
            pagedGrid.NewRequested += (_, __) => Nuevo();
            pagedGrid.EditRequested += (_, __) => Editar();
            pagedGrid.DeleteRequested += (_, __) => Eliminar();

            Controls.Add(pagedGrid);

            pagedGrid.Grid.CellFormatting += (s, e) =>
            {
                if (pagedGrid.Grid.Columns[e.ColumnIndex].Name == "Laboro" && e.Value is int val)
                {
                    e.Value = val == 1 ? "Sí" : "No";
                    e.FormattingApplied = true;
                }
            };

            // Primer bind
            pagedGrid.RefreshData();
        }

        // Nueva implementacion: columnas con encabezados visibles “como los pusiste”
        private void BuildDemoTable()
        {
            _tablaCompleta.Columns.Add("Id", typeof(int));
            _tablaCompleta.Columns.Add("Cedula", typeof(string));
            _tablaCompleta.Columns.Add("Apellido 1", typeof(string));
            _tablaCompleta.Columns.Add("Apellido 2", typeof(string));
            _tablaCompleta.Columns.Add("Nombre", typeof(string));
            _tablaCompleta.Columns.Add("Fecha Nacimiento", typeof(DateTime));
            _tablaCompleta.Columns.Add("Estado Civil", typeof(string));
            _tablaCompleta.Columns.Add("Celular", typeof(string));
            _tablaCompleta.Columns.Add("Nacionalidad", typeof(string));
            _tablaCompleta.Columns.Add("Laboro", typeof(int));     // 0/1
            _tablaCompleta.Columns.Add("Direccion", typeof(string));

            _tablaCompleta.Rows.Add(1, "1-1234-5678", "Soto", "Vargas", "Juan",
                new DateTime(1995, 5, 10), "Soltero", "8888-1111", "Costarricense", 1, "San José, Centro");

            _tablaCompleta.Rows.Add(2, "1-8765-4321", "Pérez", null, "María",
                new DateTime(1990, 11, 2), "Casado", "7777-2222", "Nicaragüense", 0, "Heredia, San Francisco");
        }

        // Nueva implementacion: filtro local usando exactamente esos encabezados
        private DataTable FiltroLocalComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tablaCompleta.Copy();

            string f = filtro.Trim().ToLowerInvariant();

            var query = _tablaCompleta.AsEnumerable().Where(r =>
                r.Field<int>("Id").ToString().Contains(f) ||
                (r.Field<string>("Cedula") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Apellido 1") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Apellido 2") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Nombre") ?? "").ToLower().Contains(f) ||
                r.Field<DateTime>("Fecha Nacimiento").ToString("yyyy-MM-dd").Contains(f) ||
                (r.Field<string>("Estado Civil") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Celular") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Nacionalidad") ?? "").ToLower().Contains(f) ||
                r.Field<int>("Laboro").ToString().Contains(f) ||
                (r.Field<string>("Direccion") ?? "").ToLower().Contains(f)
            );

            var tbl = _tablaCompleta.Clone();
            foreach (var row in query) tbl.ImportRow(row);
            return tbl;
        }

        // ===== CRUD =====
        private void Nuevo()
        {
            using var dlg = new AddSolicitud();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            var s = dlg.Result;

            int newId = _tablaCompleta.Rows.Count == 0
                ? 1
                : _tablaCompleta.AsEnumerable().Max(x => x.Field<int>("Id")) + 1;

            // Importante: mapear entity -> columnas con los nombres visibles
            _tablaCompleta.Rows.Add(
                newId,
                s.Cedula,
                s.Primer_Apellido,
                s.Segundo_Apellido,
                s.Nombre,
                s.Fecha_Nacimiento,
                s.Estado_Civil,
                s.Celular,
                s.Nacionalidad,
                s.Laboro,
                s.Direccion
            );

            pagedGrid.RefreshData();
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            var fila = _tablaCompleta.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == id.Value);
            if (fila is null) return;

            // Mapear columnas visibles -> entity (seed)
            var seed = new EntitySolicitud
            {
                Id = id.Value,
                Cedula = fila.Field<string>("Cedula") ?? "",
                Primer_Apellido = fila.Field<string>("Apellido 1") ?? "",
                Segundo_Apellido = fila.Field<string>("Apellido 2"),
                Nombre = fila.Field<string>("Nombre") ?? "",
                Fecha_Nacimiento = fila.Field<DateTime>("Fecha Nacimiento"),
                Estado_Civil = fila.Field<string>("Estado Civil") ?? "",
                Celular = fila.Field<string>("Celular") ?? "",
                Nacionalidad = fila.Field<string>("Nacionalidad") ?? "",
                Laboro = fila.Field<int>("Laboro"),
                Direccion = fila.Field<string>("Direccion") ?? ""
            };

            using var dlg = new AddSolicitud(seed);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            var s = dlg.Result;

            // Mapear entity -> columnas visibles
            fila.SetField("Cedula", s.Cedula);
            fila.SetField("Apellido 1", s.Primer_Apellido);
            fila.SetField("Apellido 2", s.Segundo_Apellido);
            fila.SetField("Nombre", s.Nombre);
            fila.SetField("Fecha Nacimiento", s.Fecha_Nacimiento);
            fila.SetField("Estado Civil", s.Estado_Civil);
            fila.SetField("Celular", s.Celular);
            fila.SetField("Nacionalidad", s.Nacionalidad);
            fila.SetField("Laboro", s.Laboro);
            fila.SetField("Direccion", s.Direccion);

            pagedGrid.RefreshData();
        }

        private void Eliminar()
        {
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0) return;

            var dr = MessageBox.Show(
                ids.Count == 1 ? $"¿Eliminar Id {ids[0]}?" : $"¿Eliminar {ids.Count} solicitudes?",
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
