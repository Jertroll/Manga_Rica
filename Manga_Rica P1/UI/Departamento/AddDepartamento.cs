using Manga_Rica_P1.Entity;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Departamentos
{
    public class AddDepartamento : Form
    {
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtCodigo = new TextBox();
        private readonly Button btnGuardar = new Button();
        private readonly Button btnCancelar = new Button();

        public Departamento Result { get; private set; } = new Departamento();

        public AddDepartamento(Departamento? seed = null)
        {
            Text = seed == null ? "Nuevo Departamento" : "Editar Departamento";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 190);

            var lblNombre = new Label { Text = "Departamento", AutoSize = true, Left = 20, Top = 20 };
            txtNombre.Left = 20; txtNombre.Top = 45; txtNombre.Width = 370;

            var lblCodigo = new Label { Text = "Código", AutoSize = true, Left = 20, Top = 80 };
            txtCodigo.Left = 20; txtCodigo.Top = 105; txtCodigo.Width = 370;

            btnGuardar.Text = "Guardar";
            btnGuardar.Left = 220; btnGuardar.Top = 140; btnGuardar.Width = 80;

            btnCancelar.Text = "Cancelar";
            btnCancelar.Left = 310; btnCancelar.Top = 140; btnCancelar.Width = 80;

            btnGuardar.Click += (_, __) => OnSave();
            btnCancelar.Click += (_, __) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[] { lblNombre, txtNombre, lblCodigo, txtCodigo, btnGuardar, btnCancelar });

            if (seed != null)
            {
                Result = new Departamento { Id = seed.Id, nombre = seed.nombre, codigo = seed.codigo };
                txtNombre.Text = seed.nombre ?? string.Empty;
                txtCodigo.Text = seed.codigo ?? string.Empty;
            }
        }

        private void OnSave()
        {
            var nombre = txtNombre.Text?.Trim();
            var codigo = txtCodigo.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre del departamento es requerido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("El código es requerido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return;
            }

            Result.nombre = nombre;
            Result.codigo = codigo;

            DialogResult = DialogResult.OK;
        }
    }
}
