// Nueva implementación (modo servidor)
using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

using Manga_Rica_P1.BLL;
using Manga_Rica_P1.UI.Helpers;          // PagedSearchGrid
using Manga_Rica_P1.UI.Articulos.Modales; // AddArticulo
using EntityArticulo = Manga_Rica_P1.Entity.Articulos;

namespace Manga_Rica_P1.UI.Articulos
{
    public partial class ArticulosView : UserControl
    {
        private readonly ArticulosService _svc;
        private PagedSearchGrid pagedGrid;

        // ⬅️ Recibe el servicio por inyección (lo crea Program y lo pasa Principal)
        public ArticulosView(ArticulosService svc)
        {
            InitializeComponent();
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Artículos"
            };

            // Paginación + búsqueda en servidor
            pagedGrid.GetPage = (pageIndex, pageSize, filtro)
                => _svc.GetPageAsDataTable(pageIndex, pageSize, filtro);

            // CRUD
            pagedGrid.NewRequested += (_, __) => Nuevo();
            pagedGrid.EditRequested += (_, __) => Editar();
            pagedGrid.DeleteRequested += (_, __) => Eliminar();

            Controls.Add(pagedGrid);
            pagedGrid.RefreshData();
        }

        private void Nuevo()
        {
            using var dlg = new AddArticulo();            // combo solo SODA/UNIFORMES
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _svc.Create(dlg.Result);                  // valida longitudes/requeridos
                pagedGrid.RefreshData();
            }
            catch (SqlException sqlEx) { ShowSqlError(sqlEx); }
            catch (ArgumentException ve) { MessageBox.Show(this, ve.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            var art = _svc.Get(id.Value);
            if (art is null) return;

            using var dlg = new AddArticulo(art);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                // dlg.Result ya conserva el Id del seed
                _svc.Update(dlg.Result);
                pagedGrid.RefreshData();
            }
            catch (SqlException sqlEx) { ShowSqlError(sqlEx); }
            catch (ArgumentException ve) { MessageBox.Show(this, ve.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (Exception ex) { MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Eliminar()
        {
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0 && pagedGrid.SelectedId is int unico) ids.Add(unico);
            if (ids.Count == 0) return;

            var dr = MessageBox.Show(
                ids.Count == 1 ? $"¿Eliminar Id {ids[0]}?" : $"¿Eliminar {ids.Count} artículos?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes) return;

            int ok = 0, fail = 0;
            foreach (var _id in ids)
            {
                try { _svc.Delete(_id); ok++; }
                catch (SqlException sqlEx) { fail++; ShowSqlError(sqlEx); }
                catch (Exception ex) { fail++; MessageBox.Show(this, ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            if (ok > 0) pagedGrid.RefreshData();
        }

        private void ShowSqlError(SqlException ex)
        {
            string msg = ex.Number switch
            {
                547 => "No se puede eliminar/modificar por tener datos relacionados (FK).",
                515 => "Hay un campo requerido en blanco.",
                2628 => "Texto excede la longitud permitida por la columna.",
                _ => $"Error de base de datos ({ex.Number}): {ex.Message}"
            };
            MessageBox.Show(this, msg, "Error de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Si no usas designer, deja el stub para evitar CS0103
        private void InitializeComponent() { }
    }
}
