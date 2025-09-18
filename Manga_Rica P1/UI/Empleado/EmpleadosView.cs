
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Manga_Rica_P1.UI.Helpers;              // PagedSearchGrid
using Manga_Rica_P1.UI.Solicitudes.Modales;  // AddSolicitud (si luego quieres crear empleado desde solicitud)
using Manga_Rica_P1.UI.Empleados.Modales;    // AddEmpleado (modal simple de demo)

// ENTITIES
using Manga_Rica_P1.ENTITY;
using EntitySolicitud = Manga_Rica_P1.Entity.Solicitud;
using EntityEmpleado = Manga_Rica_P1.Entity.Empleado;

namespace Manga_Rica_P1.UI.Empleados
{
    public partial class EmpleadosView : UserControl
    {
        // Nueva implementacion: 2 tablas en memoria (modo solicitudes / modo empleados)
        private readonly DataTable _tblSolicitudes = new();
        private readonly DataTable _tblEmpleados = new();

        // Nueva implementacion: control reutilizable listado + búsqueda + paginación
        private PagedSearchGrid pagedGrid;

        // Nueva implementacion: pequeño toggle para alternar vistas
        private CheckBox chkVerEmpleados;

        // Estado actual de la vista
        private enum GridMode { Solicitudes, Empleados }
        private GridMode _mode = GridMode.Solicitudes;

        public EmpleadosView()
        {
            // Si tienes Designer, puedes iniciar con InitializeComponent();
            Controls.Clear();

            // Panel superior para el toggle
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

            // Grid compuesto
            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Solicitudes" // inicia mostrando solicitudes
            };
            Controls.Add(pagedGrid);

            pagedGrid.Grid.DataBindingComplete += (_, __) => AplicarTuningDeColumnas();

            // Construir datos de demo
            BuildSolicitudesDemo();
            BuildEmpleadosDemo();

            // Delegado de datos (modo inicial: solicitudes)
            pagedGrid.GetAllFilteredDataTable = FiltroSolicitudesComoDataTable;

            // Eventos CRUD (se bifurcan según el modo actual)
            pagedGrid.NewRequested += (_, __) => Nuevo();
            pagedGrid.EditRequested += (_, __) => Editar();
            pagedGrid.DeleteRequested += (_, __) => Eliminar();

            // Mostrar
            pagedGrid.RefreshData();
            AplicarTuningDeColumnas();

            // Formateo visual (“Sí/No”) para booleanos guardados como int
            pagedGrid.Grid.CellFormatting += (s, e) =>
            {
                var colName = pagedGrid.Grid.Columns[e.ColumnIndex].Name;

                if (colName == "Laboro" && e.Value is int vLab)
                {
                    e.Value = vLab == 1 ? "Sí" : "No";
                    e.FormattingApplied = true;
                }
                if (colName == "Activo" && e.Value is int vAct)
                {
                    e.Value = vAct == 1 ? "Sí" : "No";
                    e.FormattingApplied = true;
                }
            };

          
        }

        // =========================================
        // Nueva implementacion: DEMO DATA
        // =========================================
        private void BuildSolicitudesDemo()
        {
            _tblSolicitudes.Columns.Add("Id", typeof(int));
            _tblSolicitudes.Columns.Add("Cedula", typeof(string));
            _tblSolicitudes.Columns.Add("Apellido 1", typeof(string));
            _tblSolicitudes.Columns.Add("Apellido 2", typeof(string));
            _tblSolicitudes.Columns.Add("Nombre", typeof(string));
            _tblSolicitudes.Columns.Add("Fecha Nacimiento", typeof(DateTime));
            _tblSolicitudes.Columns.Add("Estado Civil", typeof(string));
            _tblSolicitudes.Columns.Add("Celular", typeof(string));
            _tblSolicitudes.Columns.Add("Nacionalidad", typeof(string));
            _tblSolicitudes.Columns.Add("Laboro", typeof(int)); // 0/1
            _tblSolicitudes.Columns.Add("Direccion", typeof(string));

            _tblSolicitudes.Rows.Add(1, "1-1234-5678", "Soto", "Vargas", "Juan",
                new DateTime(1995, 5, 10), "Soltero", "8888-1111", "Costarricense", 1, "San José, Centro");

            _tblSolicitudes.Rows.Add(2, "1-8765-4321", "Pérez", null, "María",
                new DateTime(1990, 11, 2), "Casado", "7777-2222", "Nicaragüense", 0, "Heredia, San Francisco");
        }

        private void BuildEmpleadosDemo()
        {
            _tblEmpleados.Columns.Add("Id", typeof(int));
            _tblEmpleados.Columns.Add("Cedula", typeof(string));
            _tblEmpleados.Columns.Add("Apellido 1", typeof(string));
            _tblEmpleados.Columns.Add("Apellido 2", typeof(string));
            _tblEmpleados.Columns.Add("Nombre", typeof(string));
            _tblEmpleados.Columns.Add("Fecha Nacimiento", typeof(DateTime));
            _tblEmpleados.Columns.Add("Estado Civil", typeof(string));
            _tblEmpleados.Columns.Add("Celular", typeof(string));
            _tblEmpleados.Columns.Add("Nacionalidad", typeof(string));
            _tblEmpleados.Columns.Add("Laboro", typeof(int)); // 0/1
            _tblEmpleados.Columns.Add("Direccion", typeof(string));

            _tblEmpleados.Columns.Add("Id_Departamento", typeof(int));
            _tblEmpleados.Columns.Add("Salario", typeof(float));
            _tblEmpleados.Columns.Add("Puesto", typeof(string));
            _tblEmpleados.Columns.Add("Fecha Ingreso", typeof(DateTime));
            _tblEmpleados.Columns.Add("Fecha Salida", typeof(DateTime));
            _tblEmpleados.Columns.Add("Activo", typeof(int));  // 0/1

            // Filas demo
            _tblEmpleados.Rows.Add(101, "1-1234-5678", "Soto", "Vargas", "Juan",
                new DateTime(1995, 5, 10), "Soltero", "8888-1111", "Costarricense", 1, "San José, Centro",
                5, 450000f, "Operario", new DateTime(2022, 1, 10), DBNull.Value, 1);

            _tblEmpleados.Rows.Add(102, "1-9999-0000", "Rojas", "Campos", "Ana",
                new DateTime(1993, 8, 2), "Casado", "6000-1111", "Costarricense", 0, "Alajuela, Centro",
                2, 700000f, "Jefe área", new DateTime(2021, 10, 3), DBNull.Value, 1);
        }

        // =========================================
        // Nueva implementacion: Filtros
        // =========================================
        private DataTable FiltroSolicitudesComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tblSolicitudes.Copy();

            string f = filtro.Trim().ToLowerInvariant();
            var q = _tblSolicitudes.AsEnumerable().Where(r =>
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

            var tbl = _tblSolicitudes.Clone();
            foreach (var row in q) tbl.ImportRow(row);
            return tbl;
        }

        private DataTable FiltroEmpleadosComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tblEmpleados.Copy();

            string f = filtro.Trim().ToLowerInvariant();
            var q = _tblEmpleados.AsEnumerable().Where(r =>
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
                (r.Field<string>("Direccion") ?? "").ToLower().Contains(f) ||
                r.Field<int>("Id_Departamento").ToString().Contains(f) ||
                r.Field<float>("Salario").ToString().Contains(f) ||
                (r.Field<string>("Puesto") ?? "").ToLower().Contains(f) ||
                (r.Field<DateTime?>("Fecha Salida")?.ToString("yyyy-MM-dd") ?? "").Contains(f) ||
                r.Field<int>("Activo").ToString().Contains(f)
            );

            var tbl = _tblEmpleados.Clone();
            foreach (var row in q) tbl.ImportRow(row);
            return tbl;
        }

        // =========================================
        // Nueva implementacion: Toggle de modos
        // =========================================
        private void ToggleMode()
        {
            _mode = chkVerEmpleados.Checked ? GridMode.Empleados : GridMode.Solicitudes;

            if (_mode == GridMode.Empleados)
            {
                pagedGrid.Title = "Listado de Empleados";
                pagedGrid.GetAllFilteredDataTable = FiltroEmpleadosComoDataTable;
            }
            else
            {
                pagedGrid.Title = "Listado de Solicitudes";
                pagedGrid.GetAllFilteredDataTable = FiltroSolicitudesComoDataTable;
            }

            pagedGrid.RefreshData();
            AplicarTuningDeColumnas();
        }

        // =========================================
        // Nueva implementacion: CRUD (depende del modo)
        // =========================================
        private void Nuevo()
        {
            if (_mode == GridMode.Solicitudes)
            {
                // 1) Necesitamos una fila seleccionada en la tabla de solicitudes
                var id = pagedGrid.SelectedId;
                if (id is null)
                {
                    MessageBox.Show("Selecciona una solicitud primero para crear el empleado.",
                                    "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var fila = _tblSolicitudes.AsEnumerable()
                                          .FirstOrDefault(x => x.Field<int>("Id") == id.Value);
                if (fila is null) return;

                // 2) Mapear DataRow de solicitud -> entidad Empleado (seed)
                var seed = MapSolicitudToEmpleadoSeed(fila);

      
                // 3) Abrir AddEmpleado con los datos pre-cargados (Empleado)
                using var dlg = new AddEmpleado();
                dlg.PrefillFromEmpleado(seed);   // <- este sí recibe Empleado
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                // 4) Al guardar, insertamos en la tabla de empleados
                var e = dlg.Result;
                int newId = _tblEmpleados.Rows.Count == 0
                    ? 1
                    : _tblEmpleados.AsEnumerable().Max(x => x.Field<int>("Id")) + 1;

                _tblEmpleados.Rows.Add(
                    newId, e.Cedula, e.Primer_Apellido, e.Segundo_Apellido, e.Nombre,
                    e.Fecha_Nacimiento, e.Estado_Civil, e.Celular, e.Nacionalidad, e.Laboro, e.Direccion,
                    e.Id_Departamento, e.Salario, e.Puesto, e.Fecha_Ingreso,
                    e.Fecha_Salida == default ? (object)DBNull.Value : e.Fecha_Salida,
                    e.Activo
                );

                // (Opcional) Cambiar a la vista de Empleados para ver el nuevo registro
                chkVerEmpleados.Checked = true;
            }
            else
            {
                // Modo Empleados: se mantiene igual (crear empleado desde cero)
                using var dlg = new AddEmpleado();
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var e = dlg.Result;
                int newId = _tblEmpleados.Rows.Count == 0
                    ? 1
                    : _tblEmpleados.AsEnumerable().Max(x => x.Field<int>("Id")) + 1;

                _tblEmpleados.Rows.Add(
                    newId, e.Cedula, e.Primer_Apellido, e.Segundo_Apellido, e.Nombre,
                    e.Fecha_Nacimiento, e.Estado_Civil, e.Celular, e.Nacionalidad, e.Laboro, e.Direccion,
                    e.Id_Departamento, e.Salario, e.Puesto, e.Fecha_Ingreso,
                    e.Fecha_Salida == default ? (object)DBNull.Value : e.Fecha_Salida,
                    e.Activo
                );
            }

            pagedGrid.RefreshData();
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            if (_mode == GridMode.Solicitudes)
            {
                var fila = _tblSolicitudes.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == id.Value);
                if (fila is null) return;

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
            }
            else
            {
                var fila = _tblEmpleados.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == id.Value);
                if (fila is null) return;

                var seed = new EntityEmpleado
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
                    Direccion = fila.Field<string>("Direccion") ?? "",
                    Id_Departamento = fila.Field<int>("Id_Departamento"),
                    Salario = fila.Field<float>("Salario"),
                    Puesto = fila.Field<string>("Puesto") ?? "",
                    Fecha_Ingreso = fila.Field<DateTime>("Fecha Ingreso"),
                    Fecha_Salida = fila.Field<DateTime?>("Fecha Salida") ?? default,
                    Activo = fila.Field<int>("Activo"),
                };

                using var dlg = new AddEmpleado();
                dlg.PrefillFromEmpleado(seed);
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                var e = dlg.Result;
                fila.SetField("Cedula", e.Cedula);
                fila.SetField("Apellido 1", e.Primer_Apellido);
                fila.SetField("Apellido 2", e.Segundo_Apellido);
                fila.SetField("Nombre", e.Nombre);
                fila.SetField("Fecha Nacimiento", e.Fecha_Nacimiento);
                fila.SetField("Estado Civil", e.Estado_Civil);
                fila.SetField("Celular", e.Celular);
                fila.SetField("Nacionalidad", e.Nacionalidad);
                fila.SetField("Laboro", e.Laboro);
                fila.SetField("Direccion", e.Direccion);
                fila.SetField("Id_Departamento", e.Id_Departamento);
                fila.SetField("Salario", e.Salario);
                fila.SetField("Puesto", e.Puesto);
                fila.SetField("Fecha Ingreso", e.Fecha_Ingreso);
                fila.SetField("Fecha Salida", e.Fecha_Salida == default ? (object)DBNull.Value : e.Fecha_Salida);
                fila.SetField("Activo", e.Activo);
            }

            pagedGrid.RefreshData();
        }

        private void Eliminar()
        {
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0) return;

            string detalle = ids.Count == 1 ? $"Id {ids[0]}" : $"{ids.Count} registro(s)";
            var dr = MessageBox.Show($"¿Eliminar {detalle} del listado de {(_mode == GridMode.Solicitudes ? "solicitudes" : "empleados")}?",
                                     "Confirmar acción",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Warning,
                                     MessageBoxDefaultButton.Button2);
            if (dr != DialogResult.Yes) return;

            if (_mode == GridMode.Solicitudes)
            {
                foreach (var _id in ids)
                {
                    var fila = _tblSolicitudes.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == _id);
                    if (fila != null) _tblSolicitudes.Rows.Remove(fila);
                }
            }
            else
            {
                foreach (var _id in ids)
                {
                    var fila = _tblEmpleados.AsEnumerable().FirstOrDefault(x => x.Field<int>("Id") == _id);
                    if (fila != null) _tblEmpleados.Rows.Remove(fila);
                }
            }

            pagedGrid.RefreshData();
        }


        private void AplicarTuningDeColumnas()
        {
            var g = pagedGrid.Grid;
            if (g.Columns.Count == 0) return;

            var rowFont = new Font("Segoe UI", 8f);              // evita 2pt (demasiado pequeño y a veces Windows lo clampa)
            var headerFont = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);

            // A nivel control y estilos base
            g.Font = rowFont;                                // herencia para celdas sin override
            g.DefaultCellStyle.Font = rowFont;
            g.RowsDefaultCellStyle.Font = rowFont;
            g.AlternatingRowsDefaultCellStyle.Font = rowFont;
            g.ColumnHeadersDefaultCellStyle.Font = headerFont;

            // Si alguna columna ya trae un DefaultCellStyle.Font propio por el bind, lo forzamos aquí:
            foreach (DataGridViewColumn col in g.Columns)
            {
                col.DefaultCellStyle.Font = rowFont;
            }

            // Altura y padding compactos
            g.RowTemplate.Height = 22;
            g.DefaultCellStyle.Padding = new Padding(2);

            // Ajuste base
            g.AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)DataGridViewAutoSizeColumnMode.Fill;
            g.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            g.RowHeadersVisible = false;
            g.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Formatos
            if (g.Columns.Contains("Fecha Nacimiento")) g.Columns["Fecha Nacimiento"].DefaultCellStyle.Format = "d";
            if (g.Columns.Contains("Fecha Ingreso")) g.Columns["Fecha Ingreso"].DefaultCellStyle.Format = "d";
            if (g.Columns.Contains("Fecha Salida")) g.Columns["Fecha Salida"].DefaultCellStyle.Format = "d";
            if (g.Columns.Contains("Salario")) g.Columns["Salario"].DefaultCellStyle.Format = "N0";

            // Vista Empleados: ocultar columnas
            if (_mode == GridMode.Empleados)
            {
                if (g.Columns.Contains("Estado Civil")) g.Columns["Estado Civil"].Visible = false;
                if (g.Columns.Contains("Laboro")) g.Columns["Laboro"].Visible = false;
                if (g.Columns.Contains("Foto")) g.Columns["Foto"].Visible = false;
            }

            if (g.Columns.Contains("Id"))
                g.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            if (g.Columns.Contains("Id_Departamento"))
                g.Columns["Id_Departamento"].HeaderText = "Depto";

            if (g.Columns.Contains("Fecha Nacimiento"))
                g.Columns["Fecha Nacimiento"].HeaderText = "Nacimiento";

            // Pesos (ajústalos a tu gusto)
            void Peso(string col, float w)
            {
                if (!g.Columns.Contains(col)) return;
                g.Columns[col].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                g.Columns[col].FillWeight = w;
            }

            // Comunes
            Peso("Id", 50);
            Peso("Cedula", 100);
            Peso("Apellido 1", 120);
            Peso("Apellido 2", 120);
            Peso("Nombre", 140);
            Peso("Fecha Nacimiento", 110);
            Peso("Celular", 110);
            Peso("Nacionalidad", 110);
            Peso("Direccion", 240);

            // Sólo empleados
            if (_mode == GridMode.Empleados)
            {
                Peso("Fecha Nacimiento", 150);
                Peso("Cedula", 150);
                Peso("Celular", 130);
                Peso("Nacionalidad", 180);
                Peso("Id_Departamento", 80);
                Peso("Salario", 110);
                Peso("Puesto", 150);
                Peso("Fecha Ingreso", 110);
                Peso("Fecha Salida", 110);
                Peso("Activo", 5);

                // Id / Activo compactos
                if (g.Columns.Contains("Id")) g.Columns["Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                if (g.Columns.Contains("Activo")) g.Columns["Activo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

            // Alineación útil
            if (g.Columns.Contains("Id")) g.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (g.Columns.Contains("Salario")) g.Columns["Salario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            foreach (var c in new[] { "Fecha Nacimiento", "Fecha Ingreso", "Fecha Salida" })
                if (g.Columns.Contains(c)) g.Columns[c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }


        // Nueva implementacion: copia campos equivalentes de la solicitud a un empleado semilla
        private Manga_Rica_P1.Entity.Empleado MapSolicitudToEmpleadoSeed(DataRow filaSolicitud)
        {
            // NOTA: Asegúrate que tu clase ENTITY.Empleado sea public (no internal) para usarla aquí.
            var emp = new Manga_Rica_P1.Entity.Empleado
            {
                // Del solicitante:
                Cedula = filaSolicitud.Field<string>("Cedula") ?? "",
                Primer_Apellido = filaSolicitud.Field<string>("Apellido 1") ?? "",
                Segundo_Apellido = filaSolicitud.Field<string>("Apellido 2"),
                Nombre = filaSolicitud.Field<string>("Nombre") ?? "",
                Fecha_Nacimiento = filaSolicitud.Field<DateTime>("Fecha Nacimiento"),
                Estado_Civil = filaSolicitud.Field<string>("Estado Civil") ?? "",
                Celular = filaSolicitud.Field<string>("Celular") ?? "",
                Nacionalidad = filaSolicitud.Field<string>("Nacionalidad") ?? "",
                Laboro = filaSolicitud.Field<int>("Laboro"),
                Direccion = filaSolicitud.Field<string>("Direccion") ?? "",

                // Defaults que el usuario completará en el modal:
                Id_Departamento = 1,                    // por defecto (ajusta si quieres)
                Salario = 0f,                   // completar en modal
                Puesto = "",                   // completar en modal
                Fecha_Ingreso = DateTime.Today,       // sugerimos hoy
                Fecha_Salida = default,              // vacío
                Activo = 1                     // por defecto activo
            };

            return emp;
        }


    }
}
