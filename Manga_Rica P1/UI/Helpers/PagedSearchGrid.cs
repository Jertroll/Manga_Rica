using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Helpers
{
    public class PagedSearchGrid : UserControl
    {
        // ==== Delegados de datos ====
        public Func<string, DataTable>? GetAllFilteredDataTable { get; set; }
        public Func<int /*pageIndex*/, int /*pageSize*/, string /*filtro*/, (DataTable page, int total)>? GetPage { get; set; }

        // ==== Eventos CRUD + extras ====
        public event EventHandler? NewRequested;
        public event EventHandler? EditRequested;
        public event EventHandler? DeleteRequested;
        public event EventHandler? ViewNewRequested;
        public event EventHandler? SecondaryRequested; // 🆕 segundo botón extra

        // ==== Campos UI ====
        private Panel panelHeader = new();
        private Label lblTitle = new();

        private Panel panelSearch = new();
        private Label lblBuscar = new();
        private TextBox txtBuscar = new();
        private Button btnBuscar = new();
        private Button btnLimpiar = new();

        private Panel panelToolbar = new();
        private Button btnNuevo = new();
        private Button btnEditar = new();
        private Button btnEliminar = new();
        private Button btnVerNuevas = new();   // botón extra 1
        private Button btnSecondary = new();   // 🆕 botón extra 2

        private DataGridView grid = new();

        private Panel panelPager = new();
        private Button btnFirst = new(), btnPrev = new(), btnNext = new(), btnLast = new();
        private Label lblPageInfo = new();
        private ComboBox cboPageSize = new();
        private Label lblTam = new();

        // ==== Estado ====
        public string Title { get => lblTitle.Text; set => lblTitle.Text = value; }
        public int PageIndex { get; private set; } = 0;
        public int PageSize { get; private set; } = 20;
        private int _totalPages = 1;
        private int _totalFiltrado = 0;

        public int[] PageSizeOptions { get; set; } = new[] { 5, 10, 20, 50 };
        public string FilterText => txtBuscar.Text.Trim();
        public DataGridView Grid => grid;

        public int? SelectedId =>
            grid.CurrentRow?.Cells["Id"]?.Value is object v ? Convert.ToInt32(v) : (int?)null;

        public List<int> SelectedIds =>
            grid.SelectedRows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["Id"]?.Value != null)
                .Select(r => Convert.ToInt32(r.Cells["Id"]!.Value))
                .ToList();

        // ==== Accesores CRUD ====
        public Button BtnNuevo => btnNuevo;
        public Button BtnEditar => btnEditar;
        public Button BtnEliminar => btnEliminar;

        // ==== Botón extra 1 (existente) ====
        public void SetViewNewButtonText(string text)
        {
            btnVerNuevas.Text = text;
        }

        public bool ViewNewButtonVisible
        {
            get => btnVerNuevas.Visible;
            set => btnVerNuevas.Visible = value;
        }

        // ==== Botón extra 2 (nuevo) ====
        public void SetSecondaryButtonText(string text)
        {
            btnSecondary.Text = text;
        }

        public bool SecondaryButtonVisible
        {
            get => btnSecondary.Visible;
            set => btnSecondary.Visible = value;
        }

        public PagedSearchGrid()
        {
            DoubleBuffered = true;
            BuildUi();
            WireEvents();
        }

        private void BuildUi()
        {
            // Header
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(230, 135, 45)
            };
            lblTitle = new Label
            {
                Text = "Listado",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12.75f, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelHeader.Controls.Add(lblTitle);

            // Search
            panelSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(8, 5, 8, 5)
            };
            lblBuscar = new Label { Text = "Buscar:", AutoSize = true, Location = new Point(10, 11) };
            txtBuscar = new TextBox { Location = new Point(65, 8), Width = 220 };
            btnBuscar = new Button { Text = "Buscar", Location = new Point(295, 7), Size = new Size(65, 25) };
            btnLimpiar = new Button { Text = "Limpiar", Location = new Point(365, 7), Size = new Size(65, 25) };
            panelSearch.Controls.AddRange(new Control[] { lblBuscar, txtBuscar, btnBuscar, btnLimpiar });

            // Toolbar
            panelToolbar = new Panel
            {
                Dock = DockStyle.Right,
                Width = 92,
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle
            };

            btnNuevo = new Button
            {
                Text = "Nuevo",
                BackColor = Color.FromArgb(230, 135, 45),
                ForeColor = Color.White,
                Size = new Size(75, 30),
                Location = new Point(6, 22)
            };

            btnEditar = new Button
            {
                Text = "Editar",
                BackColor = Color.FromArgb(124, 179, 66),
                ForeColor = Color.White,
                Size = new Size(75, 30),
                Location = new Point(6, 73)
            };

            btnEliminar = new Button
            {
                Text = "Eliminar",
                BackColor = Color.FromArgb(211, 47, 47),
                ForeColor = Color.White,
                Size = new Size(75, 30),
                Location = new Point(6, 124)
            };

            btnVerNuevas = new Button
            {
                Text = "Ver Nuevas",
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Size = new Size(80, 30),
                Location = new Point(6, 175),
                Font = new Font("Segoe UI", 8.25f),
                Visible = false
            };

            btnSecondary = new Button
            {
                Text = "Acción",
                BackColor = Color.FromArgb(156, 39, 176),
                ForeColor = Color.White,
                Size = new Size(80, 30),
                Location = new Point(6, 216),
                Font = new Font("Segoe UI", 8.25f),
                Visible = false
            };

            panelToolbar.Controls.AddRange(new Control[]
            {
                btnNuevo, btnEditar, btnEliminar, btnVerNuevas, btnSecondary
            });

            // Grid
            grid = new DataGridView { Dock = DockStyle.Fill };
            GridStyler.ApplyDefault(grid);

            // Pager
            panelPager = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(8, 5, 8, 5),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnFirst = new Button { Text = "⏮", Size = new Size(40, 28), Location = new Point(10, 6) };
            btnPrev = new Button { Text = "◀", Size = new Size(40, 28), Location = new Point(55, 6) };
            btnNext = new Button { Text = "▶", Size = new Size(40, 28), Location = new Point(100, 6) };
            btnLast = new Button { Text = "⏭", Size = new Size(40, 28), Location = new Point(145, 6) };

            lblPageInfo = new Label { AutoSize = true, Location = new Point(200, 11) };
            lblTam = new Label { AutoSize = true, Location = new Point(290, 11), Text = "Tamaño:" };

            cboPageSize = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(350, 8),
                Size = new Size(60, 23)
            };
            cboPageSize.Items.AddRange(PageSizeOptions.Cast<object>().ToArray());
            cboPageSize.SelectedItem = PageSize;

            panelPager.Controls.AddRange(new Control[]
            {
                btnFirst, btnPrev, btnNext, btnLast, lblPageInfo, lblTam, cboPageSize
            });

            Controls.Add(grid);
            Controls.Add(panelPager);
            Controls.Add(panelToolbar);
            Controls.Add(panelSearch);
            Controls.Add(panelHeader);
        }

        private void WireEvents()
        {
            btnBuscar.Click += (s, e) => { PageIndex = 0; RefreshData(); };
            btnLimpiar.Click += (s, e) => { txtBuscar.Text = ""; PageIndex = 0; RefreshData(); };

            btnNuevo.Click += (s, e) => NewRequested?.Invoke(this, EventArgs.Empty);
            btnEditar.Click += (s, e) => EditRequested?.Invoke(this, EventArgs.Empty);
            btnEliminar.Click += (s, e) => DeleteRequested?.Invoke(this, EventArgs.Empty);

            btnVerNuevas.Click += (s, e) => ViewNewRequested?.Invoke(this, EventArgs.Empty);
            btnSecondary.Click += (s, e) => SecondaryRequested?.Invoke(this, EventArgs.Empty);

            btnFirst.Click += (s, e) => { PageIndex = 0; RefreshData(); };
            btnPrev.Click += (s, e) => { PageIndex--; RefreshData(); };
            btnNext.Click += (s, e) => { PageIndex++; RefreshData(); };
            btnLast.Click += (s, e) => { PageIndex = _totalPages - 1; RefreshData(); };

            cboPageSize.SelectionChangeCommitted += (s, e) =>
            {
                PageSize = (int)cboPageSize.SelectedItem!;
                PageIndex = 0;
                RefreshData();
            };
        }

        public void RefreshData()
        {
            if (GetPage != null)
            {
                var (pageResult, total) = GetPage(PageIndex, PageSize, FilterText);
                _totalFiltrado = total;
                _totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
                grid.DataSource = pageResult;
                UpdatePager(total);
                return;
            }

            if (GetAllFilteredDataTable == null)
            {
                grid.DataSource = null;
                UpdatePager(0);
                return;
            }

            var filtered = GetAllFilteredDataTable(FilterText);
            var totalFiltradas = filtered?.Rows.Count ?? 0;

            _totalFiltrado = totalFiltradas;
            _totalPages = Math.Max(1, (int)Math.Ceiling(totalFiltradas / (double)PageSize));

            var pageSegment = filtered!.Clone();
            foreach (var r in filtered.AsEnumerable().Skip(PageIndex * PageSize).Take(PageSize))
                pageSegment.ImportRow(r);

            grid.DataSource = pageSegment;
            UpdatePager(totalFiltradas);
        }

        private void UpdatePager(int totalFiltradas)
        {
            lblPageInfo.Text = $"{(PageIndex + 1)} de {_totalPages} (Coincidencias: {totalFiltradas})";
            btnFirst.Enabled = btnPrev.Enabled = PageIndex > 0;
            btnNext.Enabled = btnLast.Enabled = PageIndex < _totalPages - 1;
        }
    }
}
