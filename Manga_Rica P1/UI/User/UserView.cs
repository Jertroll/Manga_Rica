// UserView.cs
using Manga_Rica_P1.UI.Helpers;
using Manga_Rica_P1.UI.Helpers;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.User
{
    public partial class UserView : UserControl
    {
        private DataTable _tablaCompleta = new();
        private PagedSearchGrid pagedGrid;

        public UserView()
        {
            InitializeComponent();

            // Limpia y usa el control compuesto
            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Usuarios"
            };

            // DEMO: datos quemados
            BuildDemoTable();
            // Conectar el modo CLIENTE (in-memory)
            pagedGrid.GetAllFilteredDataTable = FiltroLocalComoDataTable;

            // Eventos CRUD
            pagedGrid.NewRequested += (s, e) => Nuevo();
            pagedGrid.EditRequested += (s, e) => Editar();
            pagedGrid.DeleteRequested += (s, e) => Eliminar();

            Controls.Add(pagedGrid);

            // Primer bind
            pagedGrid.RefreshData();
        }

        private void BuildDemoTable()
        {
            _tablaCompleta.Columns.Add("Id", typeof(int));
            _tablaCompleta.Columns.Add("Nombre", typeof(string));
            _tablaCompleta.Columns.Add("Usuario", typeof(string));
            _tablaCompleta.Columns.Add("Rol", typeof(string));

            _tablaCompleta.Rows.Add(1, "María Pérez", "mperez", "Admin");
            _tablaCompleta.Rows.Add(2, "Juan Soto", "jsoto", "Empleado");
            _tablaCompleta.Rows.Add(3, "Laura Vargas", "lvargas", "Supervisor");
            for (int i = 4; i <= 50; i++)
                _tablaCompleta.Rows.Add(i, $"Usuario {i}", $"user{i}", i % 2 == 0 ? "Empleado" : "Supervisor");
        }

        // ====== Filtro local: devuelve un DataTable con todas las coincidencias ======
        private DataTable FiltroLocalComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tablaCompleta.Copy();

            string f = filtro.Trim().ToLower();

            var query = _tablaCompleta.AsEnumerable().Where(r =>
                r.Field<int>("Id").ToString().Contains(f) ||
                (r.Field<string>("Nombre") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Usuario") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Rol") ?? "").ToLower().Contains(f)
            );

            var tbl = _tablaCompleta.Clone();
            foreach (var row in query) tbl.ImportRow(row);
            return tbl;
        }

        // ====== CRUD ======
        private void Nuevo()
        {
            using var dlg = new AddUser();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var r = dlg.Resultado;
                int newId = _tablaCompleta.Rows.Count == 0
                    ? 1
                    : _tablaCompleta.AsEnumerable().Max(x => x.Field<int>("Id")) + 1;

                // Usuario DEMO: si no tienes campo usuario en el form, lo genero del nombre
                string usuario = r.Nombre.ToLower().Replace(" ", "");
                _tablaCompleta.Rows.Add(newId, r.Nombre, usuario, r.Perfil);

                pagedGrid.RefreshData(); // mantiene filtro y página (recalcula si hace falta)
            }
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            var fila = _tablaCompleta.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == id.Value);
            if (fila is null) return;

            var inicial = new AddUser.UsuarioResult
            {
                Id = id,
                Nombre = fila.Field<string>("Nombre") ?? "",
                Perfil = fila.Field<string>("Rol") ?? "",
                FechaExpiracion = DateTime.Today
            };

            using var dlg = new AddUser(inicial);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var r = dlg.Resultado;
                fila.SetField("Nombre", r.Nombre);
                fila.SetField("Rol", r.Perfil);
                // usuario: decide si recalculas o mantienes
                // fila.SetField("Usuario", r.Nombre.ToLower().Replace(" ", ""));

                pagedGrid.RefreshData();
            }
        }

        private void Eliminar()
        {
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0) return;

            string detalle = ids.Count == 1 ? $"Id {ids[0]}" : $"{ids.Count} usuarios";
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
