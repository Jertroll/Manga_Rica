// Nueva implementacion
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Manga_Rica_P1.UI.Helpers;       // PagedSearchGrid
using Manga_Rica_P1.BLL;              // HorasService

namespace Manga_Rica_P1.UI.Asistencia
{
    public class RegistroAsistenciaView : UserControl
    {
        // =========================
        //  Estado / Servicios
        // =========================
        private enum GridMode { Empleados, Horas }
        private GridMode _mode = GridMode.Empleados;

        private readonly HorasService _horasService;
        private readonly int _usuarioActualId; // viene desde sesión

        // =========================
        //  UI
        // =========================
        private readonly PagedSearchGrid pagedGrid;
        private readonly CheckBox chkVerHoras;

        // Nueva implementacion: recibe servicio + id de usuario
        public RegistroAsistenciaView(HorasService horasService, int usuarioActualId)
        {
            _horasService = horasService ?? throw new ArgumentNullException(nameof(horasService));
            _usuarioActualId = usuarioActualId;

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

            // Configurar grid con paginación del servidor
            pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                _horasService.GetActiveEmployeesPageAsDataTable(pageIndex, pageSize, filtro);

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
                AbrirEdicionHora(); // edición en modo Horas
            };

            // Render inicial
            pagedGrid.RefreshData();
            UpdateCrudButtons();
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
                pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                    _horasService.GetHorasPageAsDataTable(pageIndex, pageSize, filtro);
            }
            else
            {
                pagedGrid.Title = "Empleados activos";
                pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                    _horasService.GetActiveEmployeesPageAsDataTable(pageIndex, pageSize, filtro);
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
                Peso("Carne", 100); // Nueva columna Carne
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

        // Nueva implementacion: formato robusto (decimal? y nulls)
        private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_mode != GridMode.Horas) return;

            var g = pagedGrid.Grid;
            var col = g.Columns[e.ColumnIndex].Name;

            // --- Total_Horas: puede ser NULL (abierto) y es decimal(6,2) en BD ---
            if (col == "Total_Horas")
            {
                if (e.Value == null || e.Value == DBNull.Value)
                {
                    e.Value = "";
                    e.FormattingApplied = true;
                    return;
                }

                if (e.Value is decimal dec)
                {
                    e.Value = dec.ToString("n2");
                    e.FormattingApplied = true;
                    return;
                }

                if (e.Value is double dbl)
                {
                    e.Value = dbl.ToString("n2");
                    e.FormattingApplied = true;
                    return;
                }
            }

            // --- Fechas y horas: formato amigable ---
            if (e.Value is DateTime dt)
            {
                if (col == "Fecha")
                {
                    e.Value = dt.ToString("d");      // fecha corta según cultura
                    e.FormattingApplied = true;
                    return;
                }

                if (col == "Hora_Entrada" || col == "Hora_Salida")
                {
                    e.Value = dt.ToString("HH:mm");  // solo hora:minuto
                    e.FormattingApplied = true;
                    return;
                }
            }

            // --- Hora_Salida NULL: mostrar vacío (registro abierto) ---
            if (col == "Hora_Salida" && (e.Value == null || e.Value == DBNull.Value))
            {
                e.Value = "";
                e.FormattingApplied = true;
                return;
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
            if (g.Columns.Contains("Carne"))
            {
                var raw = g.CurrentRow.Cells["Carne"].Value;
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
        //  Abrir formulario de registro (crear entrada / salida)
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
                    MessageBox.Show("El empleado seleccionado no tiene carné.",
                        "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Nueva implementacion: pasar servicio + usuario y seleccionar pestaña
                var dlg = new Manga_Rica_P1.UI.Hora.RegistroEntradaYSalida(_horasService, _usuarioActualId)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };

                // Seleccionar pestaña (Entrada=0 / Salida=1). Contempla nombre actual y antiguo.
                var tab = dlg.Controls.Find("tabControlSalida", true).FirstOrDefault() as TabControl
                          ?? dlg.Controls.Find("tabControl1", true).FirstOrDefault() as TabControl;
                if (tab != null) tab.SelectedIndex = esEntrada ? 0 : 1;

                // Prefill para ambos tabs
                dlg.PrefillEmpleado(carne.Value, nom ?? "", ape1 ?? "", ape2 ?? "");

                // Mostrar modal; si guardó y estás viendo Horas, refresca
                if (dlg.ShowDialog(this) == DialogResult.OK && _mode == GridMode.Horas)
                    pagedGrid.RefreshData();

                return;
            }

            // Si estás en vista de Horas, permitir crear entrada genérica (sin empleado seleccionado)
            MessageBox.Show("Para registrar una entrada, cambia a la vista de Empleados y selecciona un empleado.",
                "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =========================
        //  Edición en modo Horas
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

            try
            {
                // Nueva implementacion: Id BIGINT en BD
                var h = _horasService.GetById(Convert.ToInt64(horaId.Value));
                if (h == null)
                {
                    MessageBox.Show("No se encontró el registro.", "Asistencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (h.Hora_Salida != null)
                {
                    MessageBox.Show("Este registro ya tiene salida. No es editable.",
                        "Asistencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Nueva implementacion: pasar servicio + usuario
                var dlg = new Manga_Rica_P1.UI.Hora.RegistroEntradaYSalida(_horasService, _usuarioActualId)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };

                // Ir a pestaña de ENTRADA (porque estamos editando la entrada)
                var tab = dlg.Controls.Find("tabControlSalida", true).FirstOrDefault() as TabControl
                          ?? dlg.Controls.Find("tabControl1", true).FirstOrDefault() as TabControl;
                if (tab != null) tab.SelectedIndex = 0;

                // Prefill entrada (solo carné; nombres opcionales)
                void Set(string name, string value)
                {
                    var tb = dlg.Controls.Find(name, true).FirstOrDefault() as TextBox;
                    if (tb != null) tb.Text = value;
                }
                Set("textBoxCarneEntrada", h.Carne.ToString());
                Set("textBoxNombreEntrada", "");
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
                        try
                        {
                            // Actualizar solo fecha/hora de entrada (marca abierta)
                            h.Fecha = dtFechaEnt.Value.Date;
                            h.Hora_Entrada = dtHoraEnt.Value;
                            // No tocar Total_Horas: debe permanecer NULL mientras esté abierta

                            _horasService.UpdateEntrada(h);
                            MessageBox.Show("Entrada actualizada.", "Asistencia",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dlg.Close();
                            pagedGrid.RefreshData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error al actualizar entrada",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    };
                }

                // Ocultar/inhabilitar controles de Salida en esta edición (opcional)
                var btnRegSal = dlg.Controls.Find("buttonRegistrarSalida", true).FirstOrDefault() as Button;
                if (btnRegSal != null) btnRegSal.Enabled = false;

                dlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar registro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
