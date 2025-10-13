using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

using Manga_Rica_P1.BLL;                       // EmpleadosService + SolicitudesService
using Manga_Rica_P1.Entity;                    // Entities
using Manga_Rica_P1.UI.Helpers;                // PagedSearchGrid
using Manga_Rica_P1.UI.Solicitudes.Modales;    // AddSolicitud
using Manga_Rica_P1.UI.Empleados.Modales;      

using EntitySolicitud = Manga_Rica_P1.Entity.Solicitudes;
using EntityEmpleado = Manga_Rica_P1.Entity.Empleado;

namespace Manga_Rica_P1.UI.Empleados
{
    public partial class EmpleadosView : UserControl
    {
        private readonly EmpleadosService _empSvc;
        private readonly SolicitudesService _solSvc;
        private readonly DepartamentosService _depSvc;


        private PagedSearchGrid pagedGrid;
        private CheckBox chkVerEmpleados;

        private enum GridMode { Solicitudes, Empleados }
        private GridMode _mode = GridMode.Solicitudes;

        // ⬇️ Modo Solicitudes: ahora incluye "Telefono"
        private static readonly string[] COLS_SOLICITUDES =
        {
            "Id","Cedula","Apellido 1","Apellido 2","Nombre",
            "Fecha Nacimiento","Estado Civil","Telefono","Celular","Nacionalidad","Laboro","Direccion"
        };

        private static readonly string[] COLS_EMPLEADOS =
        {
            "Id","Cedula","Apellido 1","Apellido 2","Nombre",
            "Fecha Nacimiento","Estado Civil","Celular","Nacionalidad","Laboro","Direccion",
            "Id_Departamento","Salario","Puesto","Fecha Ingreso","Fecha Salida","Activo"
        };

        public EmpleadosView(EmpleadosService empSvc, SolicitudesService solSvc, DepartamentosService depSvc)
        {
            InitializeComponent();

            _empSvc = empSvc ?? throw new ArgumentNullException(nameof(empSvc));
            _solSvc = solSvc ?? throw new ArgumentNullException(nameof(solSvc));
            _depSvc = depSvc ?? throw new ArgumentNullException(nameof(depSvc));

            Controls.Clear();

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
            chkVerEmpleados = new CheckBox
            {
                Text = "Ver empleados (alternar solicitudes/empleados)",
                AutoSize = true,
                Left = 10,
                Top = 10
            };
            chkVerEmpleados.CheckedChanged += (_, __) => ToggleMode();
            topPanel.Controls.Add(chkVerEmpleados);
            Controls.Add(topPanel);

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Solicitudes"
            };
            Controls.Add(pagedGrid);

            pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                _solSvc.GetPageAsDataTable(pageIndex, pageSize, filtro);

            pagedGrid.NewRequested += (_, __) => Nuevo();
            pagedGrid.EditRequested += (_, __) => Editar();
            pagedGrid.DeleteRequested += (_, __) => Eliminar();

            pagedGrid.Grid.CellFormatting += (s, e) =>
            {
                var name = pagedGrid.Grid.Columns[e.ColumnIndex].Name;
                if ((name == "Laboro" || name == "Activo") && e.Value is int v)
                {
                    e.Value = v == 1 ? "Sí" : "No";
                    e.FormattingApplied = true;
                }
            };

            pagedGrid.Grid.DataBindingComplete += (_, __) => AplicarTuningDeColumnas();
            pagedGrid.RefreshData();
        }

        private void ToggleMode()
        {
            _mode = chkVerEmpleados.Checked ? GridMode.Empleados : GridMode.Solicitudes;

            if (_mode == GridMode.Empleados)
            {
                pagedGrid.Title = "Listado de Empleados";
                pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                    _empSvc.GetPageAsDataTable(pageIndex, pageSize, filtro);
            }
            else
            {
                pagedGrid.Title = "Listado de Solicitudes";
                pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                    _solSvc.GetPageAsDataTable(pageIndex, pageSize, filtro);
            }

            pagedGrid.RefreshData();
            AplicarTuningDeColumnas();
        }

        private void Nuevo()
        {
            try
            {
                if (_mode == GridMode.Solicitudes)
                {
                    var id = pagedGrid.SelectedId;
                    if (id is null)
                    {
                        MessageBox.Show("Seleccione una solicitud primero.", "Atención",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    var s = _solSvc.Get(id.Value);
                    if (s is null) return;

                    // ⬇️ Prefill incluye Telefono si viene en la solicitud
                    var seed = MapSolicitudToEmpleadoSeed(s);

                    using var dlg = new AddEmpleado(_depSvc);
                    dlg.PrefillFromEmpleado(seed);
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    _empSvc.Create(dlg.Result);
                    chkVerEmpleados.Checked = true; // opcional
                }
                else
                {
                    using var dlg = new AddEmpleado(_depSvc);
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                    _empSvc.Create(dlg.Result);
                }

                pagedGrid.RefreshData();
            }
            catch (SqlException ex) { ShowSqlError(ex); }
            catch (ArgumentException valEx)
            {
                MessageBox.Show(valEx.Message, "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error inesperado",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            try
            {
                if (_mode == GridMode.Solicitudes)
                {
                    var s = _solSvc.Get(id.Value);
                    if (s is null) return;

                    using var dlg = new AddSolicitud(s);
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    _solSvc.Update(dlg.Result);
                }
                else
                {
                    var e = _empSvc.GetById(id.Value);
                    if (e is null) return;

                    using var dlg = new AddEmpleado(_depSvc);
                    dlg.PrefillFromEmpleado(e);
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;

                    _empSvc.Update(dlg.Result);
                }

                pagedGrid.RefreshData();
            }
            catch (SqlException ex) { ShowSqlError(ex); }
            catch (ArgumentException valEx)
            {
                MessageBox.Show(valEx.Message, "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error inesperado",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Eliminar()
        {
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0 && pagedGrid.SelectedId is int unico) ids.Add(unico);
            if (ids.Count == 0) return;

            var dr = MessageBox.Show(
                ids.Count == 1 ? $"¿Eliminar Id {ids[0]}?" : $"¿Eliminar {ids.Count} registro(s)?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes) return;

            int ok = 0, fail = 0;

            foreach (var _id in ids)
            {
                try
                {
                    if (_mode == GridMode.Solicitudes) _solSvc.Delete(_id);
                    else _empSvc.Delete(_id);
                    ok++;
                }
                catch (SqlException ex) { fail++; ShowSqlError(ex); }
                catch (Exception ex)
                {
                    fail++;
                    MessageBox.Show(ex.Message, "Error al eliminar",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (ok > 0) pagedGrid.RefreshData();
        }

        private void AplicarTuningDeColumnas()
        {
            var g = pagedGrid.Grid;
            if (g.Columns.Count == 0) return;

            var allow = _mode == GridMode.Solicitudes ? COLS_SOLICITUDES : COLS_EMPLEADOS;
            foreach (DataGridViewColumn c in g.Columns) c.Visible = allow.Contains(c.Name);

            var rowFont = new Font("Segoe UI", 8f);
            var headerFont = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);

            g.Font = rowFont;
            g.DefaultCellStyle.Font = rowFont;
            g.RowsDefaultCellStyle.Font = rowFont;
            g.AlternatingRowsDefaultCellStyle.Font = rowFont;
            g.ColumnHeadersDefaultCellStyle.Font = headerFont;

            foreach (DataGridViewColumn col in g.Columns) col.DefaultCellStyle.Font = rowFont;

            g.RowTemplate.Height = 22;
            g.DefaultCellStyle.Padding = new Padding(2);
            g.RowHeadersVisible = false;
            g.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            if (g.Columns.Contains("Fecha Nacimiento")) g.Columns["Fecha Nacimiento"].DefaultCellStyle.Format = "d";
            if (g.Columns.Contains("Fecha Ingreso")) g.Columns["Fecha Ingreso"].DefaultCellStyle.Format = "d";
            if (g.Columns.Contains("Fecha Salida")) g.Columns["Fecha Salida"].DefaultCellStyle.Format = "d";
            if (g.Columns.Contains("Salario")) g.Columns["Salario"].DefaultCellStyle.Format = "N0";

            if (g.Columns.Contains("Id_Departamento")) g.Columns["Id_Departamento"].HeaderText = "Depto";
            if (g.Columns.Contains("Fecha Nacimiento")) g.Columns["Fecha Nacimiento"].HeaderText = "Nacimiento";

            void Peso(string col, float w)
            {
                if (!g.Columns.Contains(col) || !g.Columns[col].Visible) return;
                g.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                g.Columns[col].FillWeight = w;
            }

            // Comunes (Solicitudes y Empleados)
            Peso("Id", 55);
            Peso("Cedula", 110);
            Peso("Apellido 1", 120);
            Peso("Apellido 2", 120);
            Peso("Nombre", 150);
            Peso("Fecha Nacimiento", 110);
            Peso("Telefono", 110);  // ⬅️ nuevo peso
            Peso("Celular", 110);
            Peso("Nacionalidad", 120);
            Peso("Direccion", 260);

            if (_mode == GridMode.Empleados)
            {
                Peso("Id_Departamento", 80);
                Peso("Salario", 110);
                Peso("Puesto", 150);
                Peso("Fecha Ingreso", 110);
                Peso("Fecha Salida", 110);
                if (g.Columns.Contains("Activo")) g.Columns["Activo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

            if (g.Columns.Contains("Id")) g.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (g.Columns.Contains("Salario")) g.Columns["Salario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            foreach (var c in new[] { "Fecha Nacimiento", "Fecha Ingreso", "Fecha Salida" })
                if (g.Columns.Contains(c))
                    g.Columns[c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // Prefill desde Solicitud → Empleado (incluye Telefono si viene)
        private static EntityEmpleado MapSolicitudToEmpleadoSeed(EntitySolicitud s) =>
            new EntityEmpleado
            {
                Cedula = s.Cedula ?? "",
                Primer_Apellido = s.Primer_Apellido ?? "",
                Segundo_Apellido = s.Segundo_Apellido ?? "",
                Nombre = s.Nombre ?? "",
                Fecha_Nacimiento = s.Fecha_Nacimiento,
                Estado_Civil = s.Estado_Civil ?? "",
                Telefono = s.Telefono ?? "",   // ⬅️ ahora se pasa al empleado
                Celular = s.Celular ?? "",
                Nacionalidad = s.Nacionalidad ?? "",
                Laboro = s.Laboro,
                Direccion = s.Direccion ?? "",

                Id_Departamento = 1,
                Salario = 0f,
                Puesto = "",
                Fecha_Ingreso = DateTime.Today,
                // si tu tabla Empleados exige NOT NULL en Fecha_Salida, el modal debe permitir ajustarla
                Fecha_Salida = DateTime.Today,
                Activo = 1,

                // campos no visibles en la grilla pero presentes en la entidad/BD
                Carne = 0,
                MC_Numero = 0,
                Foto = ""
            };

        private void ShowSqlError(SqlException ex)
        {
            string msg = ex.Number switch
            {
                547 => "No se puede eliminar/modificar por tener datos relacionados.",
                515 => "Hay un campo requerido en blanco.",
                2627 => "Violación de clave única (duplicado).",
                2601 => "Violación de índice único (duplicado).",
                2628 => "Texto excede la longitud permitida por la columna.",
                _ => $"Error de base de datos ({ex.Number}): {ex.Message}"
            };
            MessageBox.Show(this, msg, "Error de base de datos",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void InitializeComponent() { }
    }
}
