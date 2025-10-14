using System;
using System.Windows.Forms;
using Manga_Rica_P1.BLL;
using Manga_Rica_P1.Entity;
using Manga_Rica_P1.UI.Helpers;

namespace Manga_Rica_P1.UI.User
{
    public partial class UserView : UserControl
    {
        private readonly UsuariosService _svc;
        private PagedSearchGrid pagedGrid;

        public UserView(UsuariosService svc)
        {
            InitializeComponent();
            _svc = svc;

            // Limpia y monta el control compuesto
            Controls.Clear();

            pagedGrid = new PagedSearchGrid
            {
                Dock = DockStyle.Fill,
                Title = "Listado de Usuarios"
            };

            // ✅ MODO SERVIDOR: el grid pide página al servicio
            pagedGrid.GetPage = (pageIndex, pageSize, filtro) =>
                _svc.GetPageAsDataTable(pageIndex, pageSize, filtro);

            // CRUD → BLL
            pagedGrid.NewRequested += (s, e) => Nuevo();
            pagedGrid.EditRequested += (s, e) => Editar();
            pagedGrid.DeleteRequested += (s, e) => Eliminar();

            Controls.Add(pagedGrid);
            pagedGrid.RefreshData();
        }

        // ====== CRUD (ahora contra el servicio) ======
        private void Nuevo()
        {
            using var dlg = new AddUser(); // tu modal
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            var r = dlg.Resultado;
            var nombre = (r?.Nombre ?? "").Trim();
            var perfil = r?.Perfil?.Trim();
            var passRaw = (r?.Clave ?? "temp123").Trim(); // Recuerda: Clave en BD es VARCHAR(15)

            var u = new Usuario
            {
                username = nombre,
                password = passRaw,
                perfil = perfil,
                fecha = DateTime.Now
            };

            try
            {
                _svc.Create(u, rawPassword: passRaw);
                pagedGrid.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al crear usuario",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Editar()
        {
            var id = pagedGrid.SelectedId;
            if (id is null) return;

            var u = _svc.Get(id.Value);
            if (u is null) return;

            // Precarga valores actuales en el diálogo
            var inicial = new AddUser.UsuarioResult
            {
                Id = id,
                Nombre = u.username,
                Perfil = u.perfil ?? "",
                // Password opcional en edición
                FechaExpiracion = DateTime.Today // si tu diálogo lo usa para algo visual
            };

            using var dlg = new AddUser(inicial);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            var r = dlg.Resultado;
            var nuevoNombre = (r?.Nombre ?? "").Trim();
            var nuevoPerfil = r?.Perfil?.Trim();
            var nuevaPass = r?.Clave?.Trim(); // si viene vacía, no cambia

            u.username = nuevoNombre;
            u.perfil = nuevoPerfil;

            try
            {
                _svc.Update(u, newRawPassword: string.IsNullOrWhiteSpace(nuevaPass) ? null : nuevaPass);
                pagedGrid.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al actualizar usuario",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Eliminar()
        {
            var ids = pagedGrid.SelectedIds;
            if (ids.Count == 0) return;

            string detalle = ids.Count == 1 ? $"Id {ids[0]}" : $"{ids.Count} usuarios";
            var dr = MessageBox.Show($"Confirmar acción?\n\nSe eliminará: {detalle}",
                                     "Confirmar eliminación",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                     MessageBoxDefaultButton.Button2);
            if (dr != DialogResult.Yes) return;

            try
            {
                foreach (var id in ids) _svc.Delete(id);
                pagedGrid.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al eliminar",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
