// Nueva implementacion
using Manga_Rica_P1.BLL;
using Manga_Rica_P1.Entity;              //  Entidad Departamento
using Manga_Rica_P1.UI.Helpers;          // PagedSearchGrid
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;



namespace Manga_Rica_P1.UI.Departamentos
{
    public partial class DepartamentoView : UserControl
    {
        // Nueva implementacion: fuente demo en memoria
        private DataTable _tablaCompleta = new();

        // Nueva implementacion: grid reutilizable con búsqueda + paginación
        private readonly DepartamentosService _svc;
        private PagedSearchGrid pagedGrid;


        public DepartamentoView(DepartamentosService svc)
        {
            _svc = svc;
            // Nueva implementacion: limpiar y montar el control compuesto
            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Departamentos"
            };

            // ✅ Modo SERVIDOR: pide página al servicio
            pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                _svc.GetPageAsDataTable(pageIndex, pageSize, filtro);

            // CRUD → BLL
            pagedGrid.NewRequested += (s, e) => Nuevo();
            pagedGrid.EditRequested += (s, e) => Editar();
            pagedGrid.DeleteRequested += (s, e) => Eliminar();


            Controls.Add(pagedGrid);

            pagedGrid.RefreshData();
        }


        // Nueva implementacion: datos demo
        private void BuildDemoTable()
        {
            _tablaCompleta.Columns.Add("Id", typeof(int));
            _tablaCompleta.Columns.Add("Departamento", typeof(string));
            _tablaCompleta.Columns.Add("Codigo", typeof(string));

            _tablaCompleta.Rows.Add(1, "Recursos Humanos", "RH-01");
            _tablaCompleta.Rows.Add(2, "Contabilidad", "CT-02");
            _tablaCompleta.Rows.Add(3, "Producción", "PR-03");
            _tablaCompleta.Rows.Add(4, "Bodega", "BD-04");
            _tablaCompleta.Rows.Add(5, "Compras", "CP-05");
            _tablaCompleta.Rows.Add(6, "Ventas", "VT-06");
            _tablaCompleta.Rows.Add(7, "Logística", "LG-07");
            _tablaCompleta.Rows.Add(8, "Calidad", "CQ-08");
            _tablaCompleta.Rows.Add(9, "Mantenimiento", "MT-09");
            _tablaCompleta.Rows.Add(10, "Sistemas", "IT-10");
        }

        // Nueva implementacion: filtro local devolviendo DataTable
        private DataTable FiltroLocalComoDataTable(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return _tablaCompleta.Copy();

            string f = filtro.Trim().ToLowerInvariant();

            var query = _tablaCompleta.AsEnumerable().Where(r =>
                r.Field<int>("Id").ToString().Contains(f) ||
                (r.Field<string>("Departamento") ?? string.Empty).ToLowerInvariant().Contains(f) ||
                (r.Field<string>("Codigo") ?? string.Empty).ToLowerInvariant().Contains(f)
            );

            var tbl = _tablaCompleta.Clone();
            foreach (var row in query) tbl.ImportRow(row);
            return tbl;
        }

        // ====== CRUD ======
        private void Nuevo()
        {
            using var dlg = new AddDepartamento();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            var d = dlg.Result;
            try
            {
                _svc.Create(d);
                pagedGrid.RefreshData();
            }
            catch (InvalidOperationException dupEx) // de BLL: duplicados
            {
                MessageBox.Show(this, dupEx.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException valEx) // de BLL: requeridos/longitud
            {
                MessageBox.Show(this, valEx.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            var d = _svc.Get(id.Value);
            if (d is null) return;

            using var dlg = new AddDepartamento(d); // semilla = entidad actual
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            var edited = dlg.Result;
            d.nombre = edited.nombre;
            d.codigo = edited.codigo;

            try
            {
                _svc.Update(d);
                pagedGrid.RefreshData();
            }
   
            catch (InvalidOperationException dupEx)
            {
                MessageBox.Show(this, dupEx.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException valEx)
            {
                MessageBox.Show(this, valEx.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Eliminar()
        {
            // 1) Reunir IDs seleccionados (múltiple o simple)
            var ids = pagedGrid.SelectedIds ?? new System.Collections.Generic.List<int>();
            if (ids.Count == 0 && pagedGrid.SelectedId is int unico)
                ids = new System.Collections.Generic.List<int> { unico };

            if (ids.Count == 0)
            {
                MessageBox.Show(this, "Seleccione al menos un departamento.", "Eliminar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2) Confirmar
            var detalle = ids.Count == 1 ? $"Id {ids[0]}" : $"{ids.Count} departamentos";
            var dr = MessageBox.Show(this,
                $"¿Confirma eliminar {detalle}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes) return;

            // 3) Ejecutar borrados uno por uno (robusto ante errores parciales)
            int ok = 0, fail = 0;
            foreach (var id in ids)
            {
                try
                {
                    _svc.Delete(id);  // BLL → DAL
                    ok++;
                }
                catch (Exception ex)
                {
                    fail++;
                    MessageBox.Show(this, ex.Message, "Error al eliminar",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 4) Feedback y refresh
            if (ok > 0) pagedGrid.RefreshData();

            if (fail > 0 && ok == 0)
            {
                // todos fallaron
                MessageBox.Show(this, "No se pudo eliminar ningún registro.",
                    "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
