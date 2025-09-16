using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Manga_Rica_P1.Entity;

// ✅ Alias para evitar choque entre el namespace UI.Articulos y el tipo Entity.Articulos
using EntityArticulo = Manga_Rica_P1.Entity.Articulos;

namespace Manga_Rica_P1.UI.Articulos.Modales
{
    public class AddArticulo : Form
    {
        private readonly TextBox txtDescripcion = new TextBox();
        private readonly TextBox txtPrecio = new TextBox();
        private readonly TextBox txtExistencia = new TextBox();
        private readonly ComboBox cboCategoria = new ComboBox();
        private readonly Button btnGuardar = new Button();
        private readonly Button btnCancelar = new Button();

        // ✅ El modal devuelve la entidad real
        public EntityArticulo Result { get; private set; }

        // ✅ Usa parámetro opcional (sin '?') y el alias de la entidad
        public AddArticulo(EntityArticulo seed = null)
        {
            Text = seed == null ? "Nuevo Artículo" : "Editar Artículo";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 310);

            var lblDescripcion = new Label { Text = "Descripción", AutoSize = true, Left = 20, Top = 20 };
            txtDescripcion.Left = 20; txtDescripcion.Top = 45; txtDescripcion.Width = 370;

            var lblPrecio = new Label { Text = "Precio", AutoSize = true, Left = 20, Top = 80 };
            txtPrecio.Left = 20; txtPrecio.Top = 105; txtPrecio.Width = 370;

            var lblExistencia = new Label { Text = "Existencia", AutoSize = true, Left = 20, Top = 140 };
            txtExistencia.Left = 20; txtExistencia.Top = 165; txtExistencia.Width = 370;

            var lblCategoria = new Label { Text = "Categoría", AutoSize = true, Left = 20, Top = 200 };
            cboCategoria.Left = 20; cboCategoria.Top = 225; cboCategoria.Width = 370;
            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoria.Items.AddRange(new object[] { "SODA", "UNIFORMES" });

            btnGuardar.Text = "Guardar";
            btnGuardar.Left = 220; btnGuardar.Top = 265; btnGuardar.Width = 80;

            btnCancelar.Text = "Cancelar";
            btnCancelar.Left = 310; btnCancelar.Top = 265; btnCancelar.Width = 80;

            btnGuardar.Click += (_, __) => OnSave();
            btnCancelar.Click += (_, __) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[] {
                lblDescripcion, txtDescripcion,
                lblPrecio, txtPrecio,
                lblExistencia, txtExistencia,
                lblCategoria, cboCategoria,
                btnGuardar, btnCancelar
            });

            // Inicializar Result desde seed o vacío
            Result = seed != null
                ? new EntityArticulo
                {
                    Id = seed.Id,
                    descripcion = seed.descripcion,
                    precio = seed.precio,
                    existencia = seed.existencia,
                    categoria = seed.categoria
                }
                : new EntityArticulo();

            if (seed != null)
            {
                txtDescripcion.Text = seed.descripcion;
                txtPrecio.Text = seed.precio.ToString(CultureInfo.InvariantCulture);
                txtExistencia.Text = seed.existencia.ToString();
                cboCategoria.SelectedItem = seed.categoria;
            }
            else
            {
                // valores por defecto opcionales
                cboCategoria.SelectedIndex = 0;
            }
        }

        private void OnSave()
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción es requerida.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!float.TryParse(txtPrecio.Text, NumberStyles.Float,
                                CultureInfo.InvariantCulture, out float precio))
            {
                MessageBox.Show("Precio inválido.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtExistencia.Text, out int existencia))
            {
                MessageBox.Show("Existencia inválida.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboCategoria.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una categoría.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Asignar sobre la entidad
            Result.descripcion = txtDescripcion.Text.Trim();
            Result.precio = precio;
            Result.existencia = existencia;
            Result.categoria = cboCategoria.SelectedItem.ToString()!;

            DialogResult = DialogResult.OK;
        }
    }
}
