using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Manga_Rica_P1.Entity;

// Alias para evitar choques entre UI y Entity
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

        // Catálogo fijo (según el sistema viejo)
        private static readonly string[] CategoriasPermitidas = { "UNIFORMES", "SODA" };

        // El modal devuelve la entidad real
        public EntityArticulo Result { get; private set; }

        public AddArticulo(EntityArticulo seed = null)
        {
            Text = seed == null ? "Nuevo Artículo" : "Editar Artículo";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 310);

            // ===== Controles =====
            var lblDescripcion = new Label { Text = "Descripción", AutoSize = true, Left = 20, Top = 20 };
            txtDescripcion.Left = 20; txtDescripcion.Top = 45; txtDescripcion.Width = 370;

            var lblPrecio = new Label { Text = "Precio", AutoSize = true, Left = 20, Top = 80 };
            txtPrecio.Left = 20; txtPrecio.Top = 105; txtPrecio.Width = 370;

            var lblExistencia = new Label { Text = "Existencia", AutoSize = true, Left = 20, Top = 140 };
            txtExistencia.Left = 20; txtExistencia.Top = 165; txtExistencia.Width = 370;

            var lblCategoria = new Label { Text = "Categoría", AutoSize = true, Left = 20, Top = 200 };
            cboCategoria.Left = 20; cboCategoria.Top = 225; cboCategoria.Width = 370;

            // 🔒 Solo categorías permitidas
            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoria.Items.Clear();
            cboCategoria.Items.AddRange(CategoriasPermitidas);

            btnGuardar.Text = "Guardar";
            btnGuardar.Left = 220; btnGuardar.Top = 265; btnGuardar.Width = 80;

            btnCancelar.Text = "Cancelar";
            btnCancelar.Left = 310; btnCancelar.Top = 265; btnCancelar.Width = 80;

            AcceptButton = btnGuardar;   // Enter = Guardar
            CancelButton = btnCancelar;  // Esc = Cancelar

            btnGuardar.Click += (_, __) => OnSave();
            btnCancelar.Click += (_, __) => DialogResult = DialogResult.Cancel;

            // Validación de teclado (como el sistema viejo hacía con Precio)
            txtPrecio.KeyPress += Precio_KeyPressSoloNumerosConDecimal;
            txtExistencia.KeyPress += Existencia_KeyPressSoloEnteros;

            Controls.AddRange(new Control[] {
                lblDescripcion, txtDescripcion,
                lblPrecio, txtPrecio,
                lblExistencia, txtExistencia,
                lblCategoria, cboCategoria,
                btnGuardar, btnCancelar
            });

            // ===== Inicializar Result y precargar si es edición =====
            Result = seed != null
                ? new EntityArticulo
                {
                    Id = seed.Id,
                    descripcion = seed.descripcion,
                    precio = seed.precio,
                    existencia = seed.existencia,
                    categoria = seed.categoria
                }
                : new EntityArticulo { existencia = 0 }; // en el sistema viejo la existencia inicia en 0

            if (seed != null)
            {
                txtDescripcion.Text = seed.descripcion ?? string.Empty;
                txtPrecio.Text = seed.precio.ToString(CultureInfo.InvariantCulture);
                txtExistencia.Text = seed.existencia.ToString(CultureInfo.InvariantCulture);

                // Selección segura por catálogo fijo
                var cat = (seed.categoria ?? "").ToUpperInvariant();
                cboCategoria.SelectedItem = CategoriasPermitidas.FirstOrDefault(c => c.Equals(cat, StringComparison.OrdinalIgnoreCase))
                                            ?? CategoriasPermitidas[0];
            }
            else
            {
                cboCategoria.SelectedIndex = 0;
                txtExistencia.Text = "0";
            }
        }

        private void OnSave()
        {
            // ===== Validaciones (mismo espíritu que el módulo viejo) =====
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("La descripción es requerida.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescripcion.Focus();
                return;
            }

            // Precio numérico válido (> 0)
            if (!float.TryParse(txtPrecio.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float precio) || precio <= 0)
            {
                MessageBox.Show("Precio inválido. Debe ser un número mayor que 0.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return;
            }

            // Existencia entera y no negativa
            if (!int.TryParse(txtExistencia.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int existencia) || existencia < 0)
            {
                MessageBox.Show("Existencia inválida. Debe ser un entero mayor o igual a 0.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtExistencia.Focus();
                return;
            }

            if (cboCategoria.SelectedItem is null)
            {
                MessageBox.Show("Seleccione una categoría.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCategoria.DroppedDown = true;
                return;
            }

            // Normalizamos categoría y validamos contra catálogo fijo
            var categoria = cboCategoria.SelectedItem!.ToString()!.ToUpperInvariant();
            if (!CategoriasPermitidas.Contains(categoria))
            {
                MessageBox.Show("La categoría seleccionada no es válida.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===== Mapear a la entidad =====
            Result.descripcion = txtDescripcion.Text.Trim();
            Result.precio = precio;
            Result.existencia = existencia;   // si quieres replicar 100% lo viejo, fija 0 aquí en 'Nuevo'
            Result.categoria = categoria;     // guardamos en mayúsculas

            DialogResult = DialogResult.OK;
        }

        // ====== Handlers de validación de teclado ======
        private void Precio_KeyPressSoloNumerosConDecimal(object? sender, KeyPressEventArgs e)
        {
            // Permitir dígitos, Backspace y separador decimal (.) o (,) según cultura
            var sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            bool esSeparador = e.KeyChar.ToString() == "." || e.KeyChar.ToString() == ",";

            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
                return;

            if (esSeparador)
            {
                // solo un separador y no como primer carácter
                if (txtPrecio.Text.Contains(".") || txtPrecio.Text.Contains(",") || txtPrecio.SelectionStart == 0)
                    e.Handled = true;
                return;
            }

            e.Handled = true; // bloquear cualquier otro carácter
        }

        private void Existencia_KeyPressSoloEnteros(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
                return;

            e.Handled = true; // solo enteros positivos
        }
    }
}
