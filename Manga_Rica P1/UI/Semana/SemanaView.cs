using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Manga_Rica_P1.BLL;
using Manga_Rica_P1.Entity;
using Manga_Rica_P1.UI.Helpers;
using Manga_Rica_P1.UI.Semanas.Modales;

namespace Manga_Rica_P1.UI.Semanas
{
    public partial class SemanaView : UserControl
    {
        private readonly SemanasService _svc;
        private PagedSearchGrid pagedGrid;

        public SemanaView(SemanasService svc)
        {
            InitializeComponent();
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));

            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Semanas"
            };

            // Modo servidor: paginación desde BLL
            pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                _svc.GetPageAsDataTable(pageIndex, pageSize, filtro);

            pagedGrid.NewRequested += (_, __) => Nuevo();
            pagedGrid.EditRequested += (_, __) => Editar();
            pagedGrid.DeleteRequested += (_, __) => Eliminar();

            Controls.Add(pagedGrid);
            pagedGrid.RefreshData();
        }

        private void Nuevo()
        {
            using var dlg = new AddSemana();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                _svc.Create(dlg.Result);
                pagedGrid.RefreshData();
            }
            catch (SqlException sqlEx) { ShowSqlError(sqlEx); }
            catch (ArgumentException valEx)
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

            var s = _svc.Get(id.Value);
            if (s is null) return;

            using var dlg = new AddSemana(s);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            s.semana = dlg.Result.semana;
            s.fecha_Inicio = dlg.Result.fecha_Inicio;
            s.fecha_Final = dlg.Result.fecha_Final;

            try
            {
                _svc.Update(s);
                pagedGrid.RefreshData();
            }
            catch (SqlException sqlEx) { ShowSqlError(sqlEx); }
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
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0 && pagedGrid.SelectedId is int unico)
                ids.Add(unico);
            if (ids.Count == 0) return;

            var dr = MessageBox.Show(
                ids.Count == 1 ? $"¿Eliminar Id {ids[0]}?" : $"¿Eliminar {ids.Count} semanas?",
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
                547 => "No se puede eliminar/modificar por tener datos relacionados (restricción de llave foránea).",
                515 => "Hay un campo requerido en blanco.",
                2628 => "Texto excede la longitud permitida por la columna.",
                _ => $"Error de base de datos ({ex.Number}): {ex.Message}"
            };
            MessageBox.Show(this, msg, "Error de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        // Agrega este método parcial para resolver CS0103 si el archivo .Designer.cs no existe
        // o si SemanaView no es un control de Windows Forms generado por diseñador.
        // Si el archivo .Designer.cs existe, asegúrate de que esté correctamente vinculado al archivo principal.

        private void InitializeComponent()
        {
            // Si usas controles agregados manualmente, este método puede estar vacío.
            // Si usas el diseñador de Visual Studio, asegúrate de que el archivo .Designer.cs esté presente y correcto.
        }
    }
}
