using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.User
{
    public partial class UserView : UserControl
    {
        private DataTable _tablaCompleta = new DataTable();
        private int _pageIndex = 0;   // 0-based
        private int _pageSize = 10;   // se setea desde el combo
        private int _totalPages = 1;

        // 🔎 NUEVO: texto de filtro
        private string _filtro = string.Empty;
        public UserView()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        private void UserView_Load(object sender, EventArgs e)
        {
            // ===== Datos DEMO: 100 filas =====
            _tablaCompleta.Columns.Add("Id", typeof(int));
            _tablaCompleta.Columns.Add("Nombre", typeof(string));
            _tablaCompleta.Columns.Add("Usuario", typeof(string));
            _tablaCompleta.Columns.Add("Rol", typeof(string));

            _tablaCompleta.Rows.Add(1, "María Pérez", "mperez", "Admin");
            _tablaCompleta.Rows.Add(2, "Juan Soto", "jsoto", "Empleado");
            _tablaCompleta.Rows.Add(3, "Laura Vargas", "lvargas", "Supervisor");

            for (int i = 4; i <= 50; i++)
                _tablaCompleta.Rows.Add(i, $"Usuario {i}", $"user{i}", i % 2 == 0 ? "Empleado" : "Supervisor");

            // Tamaños de página disponibles
            cboPageSize.Items.Clear();
            cboPageSize.Items.AddRange(new object[] { 5, 10, 20, 50 });
            cboPageSize.SelectedItem = 20;
            _pageSize = (int)cboPageSize.SelectedItem;

            _pageIndex = 0;
            RefrescarPagina();
        }

        // 🔎 NUEVO: función de filtrado (case-insensitive, contiene)
        private static IEnumerable<DataRow> Filtrar(DataTable t, string filtro)
        {
            if (t.Rows.Count == 0) return Enumerable.Empty<DataRow>();
            if (string.IsNullOrWhiteSpace(filtro)) return t.AsEnumerable();

            string f = filtro.Trim().ToLower();

            return t.AsEnumerable().Where(r =>
                r.Field<int>("Id").ToString().Contains(f) ||
                (r.Field<string>("Nombre") ?? string.Empty).ToLower().Contains(f) ||
                (r.Field<string>("Usuario") ?? string.Empty).ToLower().Contains(f) ||
                (r.Field<string>("Rol") ?? string.Empty).ToLower().Contains(f)
            );
        }

        private void RefrescarPagina()
        {
            // 1) Fuente = conjunto filtrado
            var fuente = Filtrar(_tablaCompleta, _filtro);
            int totalFilas = fuente.Count();

            if (totalFilas == 0)
            {
                dataGridUsuarios.DataSource = null;
                _totalPages = 1;
                _pageIndex = 0;
                ActualizarUI(totalFilas);
                return;
            }

            // 2) Paginación sobre el filtrado
            _totalPages = (int)Math.Ceiling(totalFilas / (double)_pageSize);
            if (_pageIndex < 0) _pageIndex = 0;
            if (_pageIndex > _totalPages - 1) _pageIndex = _totalPages - 1;

            var page = _tablaCompleta.Clone();
            var rows = fuente.Skip(_pageIndex * _pageSize)
                             .Take(_pageSize);

            foreach (var r in rows) page.ImportRow(r);

            dataGridUsuarios.DataSource = page;

            // 3) UI
            ActualizarUI(totalFilas);
        }

        private void ActualizarUI(int totalFilasFiltradas)
        {
            lblPageInfo.Text = $"{(_totalPages == 0 ? 0 : _pageIndex + 1)} de {_totalPages} (Coincidencias: {totalFilasFiltradas})";
            btnFirst.Enabled = btnPrev.Enabled = _pageIndex > 0;
            btnNext.Enabled = btnLast.Enabled = _pageIndex < _totalPages - 1;
        }
        // ===== Paginador =====
        private void btnFirst_Click(object sender, EventArgs e) { _pageIndex = 0; RefrescarPagina(); }
        private void btnPrev_Click(object sender, EventArgs e) { _pageIndex--; RefrescarPagina(); }
        private void btnNext_Click(object sender, EventArgs e) { _pageIndex++; RefrescarPagina(); }
        private void btnLast_Click(object sender, EventArgs e) { _pageIndex = _totalPages - 1; RefrescarPagina(); }

        private void cboPageSize_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _pageSize = (int)cboPageSize.SelectedItem;
            _pageIndex = 0;
            RefrescarPagina();
        }

        // 🔎 NUEVO: eventos de búsqueda
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            _filtro = (txtBuscar.Text ?? string.Empty);
            _pageIndex = 0;
            RefrescarPagina();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            _filtro = "";
            _pageIndex = 0;
            RefrescarPagina();
            txtBuscar.Focus();
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBuscar.PerformClick();
                e.SuppressKeyPress = true; // evita 'ding'
            }
        }

        private void dataGridUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Tu lógica si necesitas
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var dlg = new AddUser(); // modo Nuevo
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var r = dlg.Resultado;

                int newId = _tablaCompleta.Rows.Count == 0
                    ? 1
                    : _tablaCompleta.AsEnumerable().Max(x => x.Field<int>("Id")) + 1;

                _tablaCompleta.Rows.Add(newId, r.Nombre,
                    /*Usuario*/ r.Nombre.ToLower().Replace(" ", ""),
                    /*Rol*/     r.Perfil);

                _pageIndex = 0;
                RefrescarPagina();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridUsuarios.CurrentRow is null) return;

            int id = Convert.ToInt32(dataGridUsuarios.CurrentRow.Cells["Id"].Value);
            var fila = _tablaCompleta.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == id);
            if (fila is null) return;

            var inicial = new AddUser.UsuarioResult
            {
                Id = id,
                Nombre = fila.Field<string>("Nombre") ?? "",
                Perfil = fila.Field<string>("Rol") ?? "",
                FechaExpiracion = DateTime.Today // si no la manejas en demo, pon Today
            };

            using var dlg = new AddUser(inicial); // modo Editar
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var r = dlg.Resultado;
                fila.SetField("Nombre", r.Nombre);
                fila.SetField("Rol", r.Perfil);

                // Usuario: decide si lo recalculas o lo mantienes
                // fila.SetField("Usuario", r.Nombre.ToLower().Replace(" ", ""));

                // Clave: si r.Clave viene vacío, NO cambiar.
                // Como ahora estamos en memoria y no guardas claves, lo omitimos.
                // En DB: si r.Clave != "" => actualizas hash.

                RefrescarPagina();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // 1) Validar selección
            if (dataGridUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione al menos un usuario para eliminar.",
                                "Sin selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2) Tomar IDs desde la página mostrada (recuerda que el grid muestra una copia paginada)
            var ids = dataGridUsuarios.SelectedRows
                .Cast<DataGridViewRow>()
                .Where(r => r.Cells["Id"]?.Value != null)
                .Select(r => Convert.ToInt32(r.Cells["Id"].Value))
                .ToList();

            if (ids.Count == 0)
            {
                MessageBox.Show("No se pudo identificar el/los registro(s) a eliminar.",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3) Confirmar
            string detalle = ids.Count == 1 ? $"Id {ids[0]}" : $"{ids.Count} usuarios";
            var dr = MessageBox.Show($"Confirmar acción?\n\nSe eliminará: {detalle}",
                                     "Confirmar acción",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Warning,
                                     MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes) return;

            // 4) Eliminar en la tabla base (_tablaCompleta)
            foreach (var id in ids)
            {
                var fila = _tablaCompleta.AsEnumerable()
                                         .FirstOrDefault(x => x.Field<int>("Id") == id);
                if (fila != null)
                    _tablaCompleta.Rows.Remove(fila);
            }

            // 5) Ajustar la página si quedó “vacía” tras la eliminación
            //    (usamos el mismo filtro actual)
            int totalFiltradas = Filtrar(_tablaCompleta, _filtro).Count();
            if (_pageIndex > 0 && (_pageIndex * _pageSize) >= Math.Max(1, totalFiltradas))
                _pageIndex = Math.Max(0, _pageIndex - 1);

            // 6) Refrescar grilla y paginador
            RefrescarPagina();
        }

    }
}
