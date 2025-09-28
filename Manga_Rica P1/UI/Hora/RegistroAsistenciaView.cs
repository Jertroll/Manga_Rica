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

        // “BD” en memoria para lógica futura
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
                // En modo Empleados y también en Horas abrimos en tab de ENTRADA (puedes cambiarlo)
                AbrirFormularioRegistro(esEntrada: true);
            };

            pagedGrid.EditRequested += (_, __) =>
            {
                if (_mode == GridMode.Empleados) return; // no aplica
                AbrirEdicionHora(); // <--- NUEVO: edición en modo Horas
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
            _tblHoras.Columns.Add("Hora_Salida", typeof(DateTime));   // usar DBNull cuando no haya salida
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

            // Sincroniza lista en memoria
            SyncListFromTable();
        }

        private void SyncListFromTable()
        {
            _registros.Clear();
            foreach (DataRow r in _tblHoras.Rows)
            {
                _registros.Add(new HoraEntity
                {
                    Id = r.Field<int>("Id"),
                    Id_Empleado = r.Field<int>("Id_Empleado"),
                    Carne = r.Field<long>("Carne"),
                    Fecha = r.Field<DateTime>("Fecha"),
                    Hora_Entrada = r.Field<DateTime>("Hora_Entrada"),
                    Hora_Salida = r.IsNull("Hora_Salida") ? (DateTime?)null : r.Field<DateTime>("Hora_Salida"),
                    Total_Horas = r.Field<double>("Total_Horas"),
                    Id_Usuario = r.Field<int>("Id_Usuario"),
                });
            }
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

        private int? GetSelectedHoraId()
        {
            // En modo Horas, la grilla tiene columna "Id"
            return pagedGrid.SelectedId;
        }

        // =========================
        //  Abrir tu formulario (crear entrada / salida)
        // =========================
        private void AbrirFormularioRegistro(bool esEntrada)
        {
            // Si estás en vista de Empleados, necesitas una fila seleccionada
            if (_mode == GridMode.Empleados)
            {
                var id = SelectedEmpleadoId();
                if (id is null)
                {
                    MessageBox.Show("Selecciona un empleado.", "Asistencia",
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

                var dlg = new Manga_Rica_P1.UI.Hora.RegistroEntradaYSalida
                {
                    StartPosition = FormStartPosition.CenterScreen
                };

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

                // === Enganchar botones del diálogo (modo DEMO en memoria) ===
                var btnRegEnt = dlg.Controls.Find("buttonRegistrarEntrada", true).FirstOrDefault() as Button;
                var btnRegSal = dlg.Controls.Find("buttonRegistrarSalida", true).FirstOrDefault() as Button;

                if (btnRegEnt != null && dtFechaEnt != null && dtHoraEnt != null)
                {
                    btnRegEnt.Click += (_, __) =>
                    {
                        var fecha = dtFechaEnt.Value.Date;
                        var hora = dtHoraEnt.Value;
                        AddEntradaDemo(id.Value, carne.Value, fecha, hora);
                        MessageBox.Show("Entrada registrada.", "Asistencia",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dlg.Close();
                        if (_mode == GridMode.Horas) RefreshHoras();
                    };
                }

                if (btnRegSal != null)
                {
                    btnRegSal.Click += (_, __) =>
                    {
                        var horaSalida = DateTime.Now; // o leer de dtHoraSal
                        var ok = CerrarSalidaDemo(id.Value, carne.Value, horaSalida, out string msg);
                        if (!ok)
                        {
                            MessageBox.Show(msg, "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        MessageBox.Show("Salida registrada.", "Asistencia",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dlg.Close();
                        if (_mode == GridMode.Horas) RefreshHoras();
                    };
                }

                dlg.ShowDialog(this);
                return;
            }

            // Si estás en vista de Horas, permitir crear entrada genérica (sin empleado seleccionado)
            // Aquí, como demo, pedimos seleccionar un empleado igualmente (puedes abrir tu mini buscador)
            MessageBox.Show("Para registrar una entrada, cambia a la vista de Empleados y selecciona un empleado.",
                "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =========================
        //  NUEVO: Edición en modo Horas
        // =========================
        private void AbrirEdicionHora()
        {
            var horaId = GetSelectedHoraId();
            if (horaId is null)
            {
                MessageBox.Show("Selecciona un registro de horas.", "Asistencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var h = _registros.FirstOrDefault(x => x.Id == horaId.Value);
            if (h == null)
            {
                MessageBox.Show("No se encontró el registro en memoria.", "Asistencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (h.Hora_Salida != null)
            {
                MessageBox.Show("Este registro ya tiene salida. No es editable en este prototipo.",
                    "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dlg = new Manga_Rica_P1.UI.Hora.RegistroEntradaYSalida
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            // Ir a pestaña de ENTRADA (porque estamos editando la entrada)
            var tab = dlg.Controls.Find("tabControl1", true).FirstOrDefault() as TabControl;
            if (tab != null) tab.SelectedIndex = 0;

            // Prefill entrada
            void Set(string name, string value)
            {
                var tb = dlg.Controls.Find(name, true).FirstOrDefault() as TextBox;
                if (tb != null) tb.Text = value;
            }
            Set("textBoxCarneEntrada", h.Carne.ToString());
            Set("textBoxNombreEntrada", "(demo)"); // TODO: traer nombre con JOIN DAL/BLL
            Set("textBoxApellido1Entrada", "");
            Set("textBoxApellido2Entrada", "");

            var dtFechaEnt = dlg.Controls.Find("dateTimePickerFechaEntrada", true).FirstOrDefault() as DateTimePicker;
            var dtHoraEnt = dlg.Controls.Find("dateTimePickerRegistroEntrada", true).FirstOrDefault() as DateTimePicker;
            if (dtFechaEnt != null) dtFechaEnt.Value = h.Fecha.Date;
            if (dtHoraEnt != null) dtHoraEnt.Value = h.Hora_Entrada;

            // Botón Registrar Entrada actuará como "Guardar cambio"
            var btnRegEnt = dlg.Controls.Find("buttonRegistrarEntrada", true).FirstOrDefault() as Button;
            if (btnRegEnt != null && dtFechaEnt != null && dtHoraEnt != null)
            {
                btnRegEnt.Text = "Guardar";
                btnRegEnt.Click += (_, __) =>
                {
                    // Validación: no permitir que la nueva hora de entrada sea > ahora + reglas si tuvieras
                    h.Fecha = dtFechaEnt.Value.Date;
                    h.Hora_Entrada = dtHoraEnt.Value;
                    h.Total_Horas = 0; // sigue abierta

                    ActualizarTablaDesdeRegistro(h);
                    MessageBox.Show("Entrada actualizada.", "Asistencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dlg.Close();
                    RefreshHoras();
                };
            }

            // Ocultar controles de Salida en esta edición (opcional)
            var btnRegSal = dlg.Controls.Find("buttonRegistrarSalida", true).FirstOrDefault() as Button;
            if (btnRegSal != null) btnRegSal.Enabled = false;

            dlg.ShowDialog(this);
        }

        // =========================
        //  Operaciones DEMO (memoria)
        // =========================
        private void AddEntradaDemo(int idEmpleado, long carne, DateTime fecha, DateTime horaEntrada)
        {
            // Id nuevo
            var newId = (_registros.Count == 0) ? 1 : _registros.Max(x => x.Id) + 1;

            var reg = new HoraEntity
            {
                Id = newId,
                Id_Empleado = idEmpleado,
                Carne = carne,
                Fecha = fecha.Date,
                Hora_Entrada = horaEntrada,
                Hora_Salida = null,
                Total_Horas = 0,
                Id_Usuario = _usuarioActualId
            };
            _registros.Add(reg);

            // También a la tabla
            _tblHoras.Rows.Add(reg.Id, reg.Id_Empleado, reg.Carne, reg.Fecha, reg.Hora_Entrada, DBNull.Value, reg.Total_Horas, reg.Id_Usuario);
        }

        private bool CerrarSalidaDemo(int idEmpleado, long carne, DateTime horaSalida, out string message)
        {
            // Busca abierta de hoy o de ayer (cruce medianoche)
            var hoy = DateTime.Today;
            var abierto = _registros
                .Where(r => r.Id_Empleado == idEmpleado &&
                            (r.Fecha.Date == hoy || r.Fecha.Date == hoy.AddDays(-1)) &&
                            r.Hora_Salida == null)
                .OrderByDescending(r => r.Hora_Entrada)
                .FirstOrDefault();

            if (abierto == null)
            {
                message = "No hay una entrada abierta para cerrar.";
                return false;
            }

            try
            {
                abierto.CerrarJornada(horaSalida, maxHoras: 15);
                ActualizarTablaDesdeRegistro(abierto);
                message = "";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        private void ActualizarTablaDesdeRegistro(HoraEntity h)
        {
            // Actualiza la DataTable para reflejar cambios
            var row = _tblHoras.AsEnumerable().FirstOrDefault(r => r.Field<int>("Id") == h.Id);
            if (row == null)
            {
                _tblHoras.Rows.Add(h.Id, h.Id_Empleado, h.Carne, h.Fecha,
                    h.Hora_Entrada, h.Hora_Salida ?? (object)DBNull.Value, h.Total_Horas, h.Id_Usuario);
                return;
            }

            row["Id_Empleado"] = h.Id_Empleado;
            row["Carne"] = h.Carne;
            row["Fecha"] = h.Fecha;
            row["Hora_Entrada"] = h.Hora_Entrada;
            row["Hora_Salida"] = h.Hora_Salida ?? (object)DBNull.Value;
            row["Total_Horas"] = h.Total_Horas;
            row["Id_Usuario"] = h.Id_Usuario;
        }

        private void RefreshHoras()
        {
            if (_mode == GridMode.Horas)
            {
                pagedGrid.RefreshData();
            }
        }

        // Habilita/oculta los botones del panel derecho del PagedSearchGrid
        private void UpdateCrudButtons()
        {
            bool viendoEmpleados = _mode == GridMode.Empleados;

            // Siempre puedes crear una entrada
            pagedGrid.BtnNuevo.Enabled = true;
            pagedGrid.BtnNuevo.Visible = true;

            if (viendoEmpleados)
            {
                // En la vista de Empleados NO se muestran Editar/Eliminar
                pagedGrid.BtnEditar.Visible = false;
                pagedGrid.BtnEliminar.Visible = false;
            }
            else
            {
                // En la vista de Horas: Editar visible (para ajustar ENTRADA si está abierta),
                // Eliminar normalmente oculto
                pagedGrid.BtnEditar.Visible = true;
                pagedGrid.BtnEditar.Enabled = true;
                pagedGrid.BtnEditar.Text = "Editar entrada";

                pagedGrid.BtnEliminar.Visible = false;
                pagedGrid.BtnEliminar.Enabled = false;
            }
        }

        private HoraEntity? GetAbiertaHoyOAnterior(int idEmpleado, long carne)
        {
            var hoy = DateTime.Today;
            var r = _registros.FirstOrDefault(x => x.Id_Empleado == idEmpleado &&
                                                   x.Fecha.Date == hoy &&
                                                   x.Hora_Salida == null);
            if (r != null) return r;
            var ayer = hoy.AddDays(-1);
            return _registros.FirstOrDefault(x => x.Id_Empleado == idEmpleado &&
                                                  x.Fecha.Date == ayer &&
                                                  x.Hora_Salida == null);
        }

        private bool PuedeEditar(HoraEntity h) => h.Hora_Salida == null;
    }
}
