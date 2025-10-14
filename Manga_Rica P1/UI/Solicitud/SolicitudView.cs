using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Drawing;

using Manga_Rica_P1.BLL;                       // ✅ SolicitudesService
using Manga_Rica_P1.Entity;                    // ✅ Entity.Solicitud
using Manga_Rica_P1.UI.Helpers;                // PagedSearchGrid
using Manga_Rica_P1.UI.Solicitudes.Modales;    // AddSolicitud

// Alias opcional (si quieres dejar claro que es la entidad)
using EntitySolicitud = Manga_Rica_P1.Entity.Solicitudes;

namespace Manga_Rica_P1.UI.Solicitudes
{
    public partial class SolicitudView : UserControl
    {
        // ✅ Servicio BLL inyectado
        private readonly SolicitudesService _svc;
        private PagedSearchGrid pagedGrid;

        // ✅ Recibe el servicio por ctor (como SemanaView/DepartamentoView)
        public SolicitudView(SolicitudesService svc)
        {
            InitializeComponent();
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Solicitudes"
            };

            // ↓↓↓ AJUSTE DE FUENTES (poner ANTES de Add/Refresh) ↓↓↓
            var g = pagedGrid.Grid;

            // Fuente de las celdas (filas)
            g.DefaultCellStyle.Font = new Font(g.Font.FontFamily, 9f);

            // Fuente de encabezados de columna
            g.ColumnHeadersDefaultCellStyle.Font =
                new Font((g.ColumnHeadersDefaultCellStyle.Font ?? g.Font).FontFamily, 9.5f, FontStyle.Bold);

            // (Opcional) encabezados de fila
            g.RowHeadersDefaultCellStyle.Font =
                new Font((g.RowHeadersDefaultCellStyle.Font ?? g.Font).FontFamily, 9f);

            // Alturas acordes al tamaño de fuente
            g.RowTemplate.Height = 24;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // ✅ MODO SERVIDOR: página desde BLL (DataTable + total en el propio control)
            pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                _svc.GetPageAsDataTable(pageIndex, pageSize, filtro);

            // ✅ Acciones CRUD
            pagedGrid.NewRequested += (_, __) => Nuevo();
            pagedGrid.EditRequested += (_, __) => Editar();
            pagedGrid.DeleteRequested += (_, __) => Eliminar();

            // Formateo de columna Laboro como “Sí/No”
            pagedGrid.Grid.CellFormatting += (s, e) =>
            {
                var name = pagedGrid.Grid.Columns[e.ColumnIndex].Name;
                if (!string.Equals(name, "Laboro", StringComparison.OrdinalIgnoreCase)) return;

                if (e.Value is bool b) { e.Value = b ? "Sí" : "No"; e.FormattingApplied = true; }
                else if (e.Value is int n) { e.Value = n == 1 ? "Sí" : "No"; e.FormattingApplied = true; }
                else if (e.Value is byte by) { e.Value = by == 1 ? "Sí" : "No"; e.FormattingApplied = true; }
            };

            Controls.Add(pagedGrid);

            // Primer bind
            pagedGrid.RefreshData();
        }

        // =========================
        // CRUD
        // =========================
        private void Nuevo()
        {
            using var dlg = new AddSolicitud();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _svc.Create(dlg.Result); 
                pagedGrid.RefreshData();
            }
            catch (SqlException sqlEx) { ShowSqlError(sqlEx); }
            catch (ArgumentException valEx)
            {
                MessageBox.Show(this, valEx.Message, "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error inesperado",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            // Carga la entidad real desde BLL
            var s = _svc.Get(id.Value);
            if (s is null) return;

            using var dlg = new AddSolicitud(s);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            // Actualiza campos editables
            s.Cedula = dlg.Result.Cedula;
            s.Primer_Apellido = dlg.Result.Primer_Apellido;
            s.Segundo_Apellido = dlg.Result.Segundo_Apellido;
            s.Nombre = dlg.Result.Nombre;
            s.Fecha_Nacimiento = dlg.Result.Fecha_Nacimiento;
            s.Estado_Civil = dlg.Result.Estado_Civil;
            s.Celular = dlg.Result.Celular;
            s.Nacionalidad = dlg.Result.Nacionalidad;
            s.Laboro = dlg.Result.Laboro;
            s.Direccion = dlg.Result.Direccion;

            try
            {
                _svc.Update(s);
                pagedGrid.RefreshData();
            }
            catch (SqlException sqlEx) { ShowSqlError(sqlEx); }
            catch (ArgumentException valEx)
            {
                MessageBox.Show(this, valEx.Message, "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error inesperado",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Eliminar()
        {
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0 && pagedGrid.SelectedId is int unico)
                ids.Add(unico);
            if (ids.Count == 0) return;

            var dr = MessageBox.Show(
                ids.Count == 1 ? $"¿Eliminar Id {ids[0]}?" : $"¿Eliminar {ids.Count} solicitudes?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes) return;

            int ok = 0, fail = 0;
            foreach (var _id in ids)
            {
                try { _svc.Delete(_id); ok++; }
                catch (SqlException sqlEx) { fail++; ShowSqlError(sqlEx); }
                catch (Exception ex)
                {
                    fail++;
                    MessageBox.Show(this, ex.Message, "Error al eliminar",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (ok > 0) pagedGrid.RefreshData();
        }

        // =========================
        // Utilidad errores SQL
        // =========================
        private void ShowSqlError(SqlException ex)
        {
            string msg = ex.Number switch
            {
                547 => "No se puede eliminar/modificar por tener datos relacionados (llave foránea).",
                515 => "Hay un campo requerido en blanco.",
                2627 => "Violación de clave única (duplicado).",
                2601 => "Violación de índice único (duplicado).",
                2628 => "Texto excede la longitud permitida por la columna.",
                _ => $"Error de base de datos ({ex.Number}): {ex.Message}"
            };
            MessageBox.Show(this, msg, "Error de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Si este control no tiene .Designer.cs, deja este stub.
        private void InitializeComponent()
        {
            // vacío a propósito (evita CS0103 si no usas diseñador)
        }
    }
}