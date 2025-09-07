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
    }
}
