using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Manga_Rica_P1.UI.Helpers;       // PagedSearchGrid
using Manga_Rica_P1.Entity;           // Hora (Entity)
using HoraEntity = Manga_Rica_P1.Entity.Hora;

namespace Manga_Rica_P1.UI.Asistencia
{
    public class RegistroAsistenciaView : UserControl
    {
        // =========================
        //  Estado / Datos en memoria
        // =========================
        private enum GridMode { Empleados, Horas }
        private GridMode _mode = GridMode.Empleados;

        // Empleados activos mostrados en el grid
        private readonly DataTable _tblEmpleadosActivos = new();
        // Registros de horas (puedes reemplazar por EF/DAL después)
        private readonly DataTable _tblHoras = new();

        // “BD” en memoria para lógica futura (si quisieras)
        private readonly List<HoraEntity> _registros = new();
        private readonly int _usuarioActualId = 1;

        // =========================
        //  UI
        // =========================
        private readonly PagedSearchGrid pagedGrid;
        private readonly CheckBox chkVerHoras;

        public RegistroAsistenciaView()
        {
            // ---------- Barra superior: Toggle ----------
            var top = new Panel { Dock = DockStyle.Top, Height = 40 };
            chkVerHoras = new CheckBox
            {
                AutoSize = true,
                Left = 10,
                Top = 10,
                Text = "Ver horas (alternar empleados/horas)"
            };
            chkVerHoras.CheckedChanged += (_, __) => ToggleMode();
            top.Controls.Add(chkVerHoras);
            Controls.Add(top);

            // ---------- Grid reutilizable ----------
            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Empleados activos"
            };
            Controls.Add(pagedGrid);

            // Column / seed
            BuildEmpleadosTableSchema();
            BuildHorasTableSchema();
            SeedEmpleadosDemo();
            SeedHorasDemo();

            // Fuente inicial: empleados
            pagedGrid.GetAllFilteredDataTable = FiltroEmpleadosComoDataTable;

            // Tuning de columnas al ligar datos
            pagedGrid.Grid.DataBindingComplete += (_, __) => TunarColumnas();

            // Formato de fechas/horas al pintar celdas
            pagedGrid.Grid.CellFormatting += Grid_CellFormatting;

            // Desde los botones del grid:
            pagedGrid.NewRequested += (_, __) =>
            {
                // En modo Empleados: abrir tu formulario en tab de ENTRADA
                // En modo Horas: (opcional) podrías abrir también ENTRADA; por ahora lo dejamos igual
                if (_mode == GridMode.Empleados)
                    AbrirFormularioRegistro(esEntrada: true);
                else
                    AbrirFormularioRegistro(esEntrada: true);
            };
            pagedGrid.EditRequested += (_, __) =>
            {
                if (_mode == GridMode.Empleados) return; // deshabilitado en este modo
                AbrirFormularioRegistro(esEntrada: false);
            };

            // Render inicial
            pagedGrid.RefreshData();
            UpdateCrudButtons();
        }

        // =========================
        //  Schemas
        // =========================
        private void BuildEmpleadosTableSchema()
        {
            _tblEmpleadosActivos.Columns.Add("Id", typeof(int));
            _tblEmpleadosActivos.Columns.Add("Cedula", typeof(string));
            _tblEmpleadosActivos.Columns.Add("Apellido 1", typeof(string));
            _tblEmpleadosActivos.Columns.Add("Apellido 2", typeof(string));
            _tblEmpleadosActivos.Columns.Add("Nombre", typeof(string));
            _tblEmpleadosActivos.Columns.Add("Celular", typeof(string));
            _tblEmpleadosActivos.Columns.Add("MC_Numero", typeof(long));
        }

        private void BuildHorasTableSchema()
        {
            _tblHoras.Columns.Add("Id", typeof(int));
            _tblHoras.Columns.Add("Id_Empleado", typeof(int));
            _tblHoras.Columns.Add("Carne", typeof(long));
            _tblHoras.Columns.Add("Fecha", typeof(DateTime));
            _tblHoras.Columns.Add("Hora_Entrada", typeof(DateTime));
            _tblHoras.Columns.Add("Hora_Salida", typeof(DateTime));   // usaremos DBNull cuando no haya salida
            _tblHoras.Columns.Add("Total_Horas", typeof(double));
            _tblHoras.Columns.Add("Id_Usuario", typeof(int));
        }

        // =========================
        //  Seed demo
        // =========================
        private void SeedEmpleadosDemo()
        {
            _tblEmpleadosActivos.Rows.Add(101, "1-1234-5678", "Soto", "Vargas", "Juan", "8888-1111", 1001L);
            _tblEmpleadosActivos.Rows.Add(102, "1-9999-0000", "Rojas", "Campos", "Ana", "6000-1111", 1002L);
        }

        private void SeedHorasDemo()
        {
            var hoy = DateTime.Today;

            // Abierta (sin salida)
            _tblHoras.Rows.Add(1, 101, 1001L, hoy, hoy.AddHours(7).AddMinutes(45), DBNull.Value, 0.0, _usuarioActualId);

            // Cerrada
            var entrada = hoy.AddHours(8);
            var salida = hoy.AddHours(16).AddMinutes(30);
            var total = (salida - entrada).TotalHours;
            _tblHoras.Rows.Add(2, 102, 1002L, hoy, entrada, salida, Math.Round(total, 2), _usuarioActualId);
        }

        // =========================
        //  Filtros
        // =========================
        private DataTable FiltroEmpleadosComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tblEmpleadosActivos.Copy();

            var f = filtro.Trim().ToLowerInvariant();
            var q = _tblEmpleadosActivos.AsEnumerable().Where(r =>
                r.Field<int>("Id").ToString().Contains(f) ||
                (r.Field<string>("Cedula") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Apellido 1") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Apellido 2") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Nombre") ?? "").ToLower().Contains(f) ||
                (r.Field<string>("Celular") ?? "").ToLower().Contains(f) ||
                r.Field<long>("MC_Numero").ToString().Contains(f)
            );
            var tbl = _tblEmpleadosActivos.Clone();
            foreach (var row in q) tbl.ImportRow(row);
            return tbl;
        }

        private DataTable FiltroHorasComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tblHoras.Copy();

            var f = filtro.Trim().ToLowerInvariant();
            var q = _tblHoras.AsEnumerable().Where(r =>
                r.Field<int>("Id").ToString().Contains(f) ||
                r.Field<int>("Id_Empleado").ToString().Contains(f) ||
                r.Field<long>("Carne").ToString().Contains(f) ||
                r.Field<DateTime>("Fecha").ToString("yyyy-MM-dd").Contains(f) ||
                r.Field<DateTime>("Hora_Entrada").ToString("HH:mm").Contains(f) ||
                ((r.IsNull("Hora_Salida") ? "" : r.Field<DateTime>("Hora_Salida").ToString("HH:mm")).Contains(f)) ||
                r.Field<double>("Total_Horas").ToString("n2").Contains(f) ||
                r.Field<int>("Id_Usuario").ToString().Contains(f)
            );
            var tbl = _tblHoras.Clone();
            foreach (var row in q) tbl.ImportRow(row);
            return tbl;
        }

        // =========================
        //  Toggle modo
        // =========================
        private void ToggleMode()
        {
            _mode = chkVerHoras.Checked ? GridMode.Horas : GridMode.Empleados;

            if (_mode == GridMode.Horas)
            {
                pagedGrid.Title = "Registros de Horas";
                pagedGrid.GetAllFilteredDataTable = FiltroHorasComoDataTable;
            }
            else
            {
                pagedGrid.Title = "Empleados activos";
                pagedGrid.GetAllFilteredDataTable = FiltroEmpleadosComoDataTable;
            }

            pagedGrid.RefreshData();
            UpdateCrudButtons();
        }

        // =========================
        //  Tuning columnas + formato
        // =========================
        private void TunarColumnas()
        {
            var g = pagedGrid.Grid;
            if (g.Columns.Count == 0) return;

            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowHeadersVisible = false;
            g.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            if (_mode == GridMode.Empleados)
            {
                Peso("Id", 60);
                Peso("Cedula", 120);
                Peso("Apellido 1", 120);
                Peso("Apellido 2", 120);
                Peso("Nombre", 160);
                Peso("Celular", 110);
                Peso("MC_Numero", 100);
                if (g.Columns.Contains("Id")) g.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            else
            {
                // Horas
                Peso("Id", 60);
                Peso("Id_Empleado", 90);
                Peso("Carne", 100);
                Peso("Fecha", 100);
                Peso("Hora_Entrada", 110);
                Peso("Hora_Salida", 110);
                Peso("Total_Horas", 90);
                Peso("Id_Usuario", 90);
                if (g.Columns.Contains("Id")) g.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

                // Encabezados más amigables (opcional)
                if (g.Columns.Contains("Id_Empleado")) g.Columns["Id_Empleado"].HeaderText = "Empleado";
                if (g.Columns.Contains("Hora_Entrada")) g.Columns["Hora_Entrada"].HeaderText = "Entrada";
                if (g.Columns.Contains("Hora_Salida")) g.Columns["Hora_Salida"].HeaderText = "Salida";
                if (g.Columns.Contains("Total_Horas")) g.Columns["Total_Horas"].HeaderText = "Total (h)";
                if (g.Columns.Contains("Id_Usuario")) g.Columns["Id_Usuario"].HeaderText = "Usuario";
            }

            void Peso(string col, float w)
            {
                if (!g.Columns.Contains(col)) return;
                var c = g.Columns[col];
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                c.FillWeight = w;
            }
        }

        private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_mode != GridMode.Horas) return;

            var g = pagedGrid.Grid;
            var col = g.Columns[e.ColumnIndex].Name;

            if (e.Value is DateTime dt)
            {
                if (col == "Fecha")
                {
                    e.Value = dt.ToString("d"); // fecha corta
                    e.FormattingApplied = true;
                }
                else if (col == "Hora_Entrada" || col == "Hora_Salida")
                {
                    e.Value = dt.ToString("HH:mm");
                    e.FormattingApplied = true;
                }
            }

            if (col == "Hora_Salida" && (e.Value == null || e.Value == DBNull.Value))
            {
                e.Value = ""; // abierto
                e.FormattingApplied = true;
            }

            if (col == "Total_Horas" && e.Value is double d)
            {
                e.Value = d.ToString("n2");
                e.FormattingApplied = true;
            }
        }

        // =========================
        //  Helpers selección actual
        // =========================
        private int? SelectedEmpleadoId() => pagedGrid.SelectedId;

        private (string? Cedula, string? Ape1, string? Ape2, string? Nombre, long? Carne) SelectedEmpleadoDatos()
        {
            var g = pagedGrid.Grid;
            if (g.CurrentRow == null) return (null, null, null, null, null);

            string? val(string col) => g.Columns.Contains(col) ? g.CurrentRow.Cells[col].Value?.ToString() : null;

            long? carne = null;
            if (g.Columns.Contains("MC_Numero"))
            {
                var raw = g.CurrentRow.Cells["MC_Numero"].Value;
                if (raw is long l) carne = l;
                else if (long.TryParse(raw?.ToString(), out var parsed)) carne = parsed;
            }

            return (val("Cedula"), val("Apellido 1"), val("Apellido 2"), val("Nombre"), carne);
        }

        // =========================
        //  Abrir tu formulario (reutilizable)
        // =========================
        private void AbrirFormularioRegistro(bool esEntrada)
        {
            var id = SelectedEmpleadoId();
            if (id is null)
            {
                MessageBox.Show("Selecciona un registro.", "Asistencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var (_, ape1, ape2, nom, carne) = SelectedEmpleadoDatos();
            if (carne is null)
            {
                MessageBox.Show("El empleado seleccionado no tiene carné (MC_Numero).",
                    "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dlg = new Manga_Rica_P1.UI.Hora.RegistroEntradaYSalida();

            // Seleccionar pestaña
            var tab = dlg.Controls.Find("tabControl1", true).FirstOrDefault() as TabControl;
            if (tab != null) tab.SelectedIndex = esEntrada ? 0 : 1;

            // Helper para setear TextBox por nombre
            void SetText(string name, string? value)
            {
                var tb = dlg.Controls.Find(name, true).FirstOrDefault() as TextBox;
                if (tb != null) tb.Text = value ?? string.Empty;
            }

            // Prefill ENTRADA
            SetText("textBoxCarneEntrada", carne?.ToString());
            SetText("textBoxNombreEntrada", nom);
            SetText("textBoxApellido1Entrada", ape1);
            SetText("textBoxApellido2Entrada", ape2);

            var dtFechaEnt = dlg.Controls.Find("dateTimePickerFechaEntrada", true).FirstOrDefault() as DateTimePicker;
            var dtHoraEnt = dlg.Controls.Find("dateTimePickerRegistroEntrada", true).FirstOrDefault() as DateTimePicker;
            if (dtFechaEnt != null) dtFechaEnt.Value = DateTime.Today;
            if (dtHoraEnt != null) dtHoraEnt.Value = DateTime.Now;

            // Prefill SALIDA
            SetText("textBoxCarneSalida", carne?.ToString());
            SetText("textBoxNombreSalida", nom);
            SetText("textBoxApellidoSalida", ape1);
            SetText("textBoxApellido2Salida", ape2);

            var dtFechaSal = dlg.Controls.Find("dateTimePickerSalida", true).FirstOrDefault() as DateTimePicker;
            var dtHoraSal = dlg.Controls.Find("dateTimePickerRegistroSalida", true).FirstOrDefault() as DateTimePicker;
            if (dtFechaSal != null) dtFechaSal.Value = DateTime.Today;
            if (dtHoraSal != null) dtHoraSal.Value = DateTime.Now;

            dlg.ShowDialog(this);
        }

        // Habilita/oculta los botones del panel derecho del PagedSearchGrid
        private void UpdateCrudButtons()
        {
            bool viendoEmpleados = _mode == GridMode.Empleados;

            // En empleados: New = habilitado, Edit/Delete = ocultos o deshabilitados
            pagedGrid.BtnNuevo.Enabled = true;
            pagedGrid.BtnNuevo.Visible = true;

            // Opción 1: deshabilitar pero seguir mostrando
            // pagedGrid.BtnEditar.Enabled = !viendoEmpleados;
            // pagedGrid.BtnEliminar.Enabled = !viendoEmpleados;

            // Opción 2: ocultar completamente
            pagedGrid.BtnEditar.Visible = !viendoEmpleados;
            pagedGrid.BtnEliminar.Visible = !viendoEmpleados;
        }


        // helper: intenta usar propiedades públicas si existen; si no, busca por nombre
        private void SetPgButton(string name, bool enabled, bool visible)
        {
            // 1) si tu PagedSearchGrid expone las referencias, úsalas:
            try
            {
                var pi = pagedGrid.GetType().GetProperty(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (pi != null)
                {
                    var btn = pi.GetValue(pagedGrid) as Control;
                    if (btn != null) { btn.Enabled = enabled; btn.Visible = visible; return; }
                }
            }
            catch { /* fallback abajo */ }

            // 2) fallback: buscar por nombre dentro de los hijos del control
            var ctrl = pagedGrid.Controls.Find(name, true).FirstOrDefault() as Control;
            if (ctrl != null)
            {
                ctrl.Enabled = enabled;
                ctrl.Visible = visible;
            }
        }

    }
}
