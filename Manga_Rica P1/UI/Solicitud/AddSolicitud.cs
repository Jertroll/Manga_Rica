// Nueva implementacion
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Manga_Rica_P1.ENTITY;

// Alias para evitar choques si creas namespaces UI.Solicitud/ Solicitudes
using EntitySolicitud = Manga_Rica_P1.Entity.Solicitud;

namespace Manga_Rica_P1.UI.Solicitudes.Modales
{
    public class AddSolicitud : Form
    {
        private readonly TextBox txtCedula = new TextBox();
        private readonly TextBox txtPrimerApellido = new TextBox();
        private readonly TextBox txtSegundoApellido = new TextBox();
        private readonly TextBox txtNombre = new TextBox();
        private readonly DateTimePicker dtNacimiento = new DateTimePicker();
        private readonly ComboBox cboEstadoCivil = new ComboBox();
        private readonly TextBox txtCelular = new TextBox();
        private readonly TextBox txtNacionalidad = new TextBox();
        private readonly CheckBox chkLaboro = new CheckBox();
        private readonly TextBox txtDireccion = new TextBox();

        private readonly Button btnGuardar = new Button();
        private readonly Button btnCancelar = new Button();

        // El modal devuelve directamente la entidad
        public EntitySolicitud Result { get; private set; }

        public AddSolicitud(EntitySolicitud seed = null)
        {
            Text = seed == null ? "Nueva Solicitud" : "Editar Solicitud";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(640, 420);


            int L = 20, W = 280, H = 24, G = 14, R = 320;

            // Fila 1
            var lblCedula = new Label { Text = "Cédula", AutoSize = true, Left = L, Top = 20 };
            txtCedula.SetBounds(L, 40, W, H);

            var lblNombre = new Label { Text = "Nombre", AutoSize = true, Left = R, Top = 20 };
            txtNombre.SetBounds(R, 40, W, H);

            // Fila 2
            var lblPrimerApe = new Label { Text = "Primer Apellido", AutoSize = true, Left = L, Top = 40 + H + G };
            txtPrimerApellido.SetBounds(L, 60 + H + G, W, H);

            var lblSegundoApe = new Label { Text = "Segundo Apellido", AutoSize = true, Left = R, Top = 40 + H + G };
            txtSegundoApellido.SetBounds(R, 60 + H + G, W, H);

            // Fila 3
            var lblNacimiento = new Label { Text = "Fecha Nacimiento", AutoSize = true, Left = L, Top = 60 + 2 * (H + G) };
            dtNacimiento.SetBounds(L, 80 + 2 * (H + G), W, H);
            dtNacimiento.Format = DateTimePickerFormat.Short;

            var lblEstadoCivil = new Label { Text = "Estado Civil", AutoSize = true, Left = R, Top = 60 + 2 * (H + G) };
            cboEstadoCivil.SetBounds(R, 80 + 2 * (H + G), W, H);
            cboEstadoCivil.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstadoCivil.Items.AddRange(new object[] { "Soltero", "Casado", "Unión Libre", "Divorciado", "Viudo" });

            // Fila 4
            var lblCelular = new Label { Text = "Celular", AutoSize = true, Left = L, Top = 80 + 3 * (H + G) };
            txtCelular.SetBounds(L, 100 + 3 * (H + G), W, H);

            var lblNacionalidad = new Label { Text = "Nacionalidad", AutoSize = true, Left = R, Top = 80 + 3 * (H + G) };
            txtNacionalidad.SetBounds(R, 100 + 3 * (H + G), W, H);

            // Fila 5
            chkLaboro.Text = "¿Laboró anteriormente?";
            chkLaboro.SetBounds(L, 120 + 4 * (H + G), 200, H);

            // Dirección (multilínea)
            var lblDireccion = new Label { Text = "Dirección", AutoSize = true, Left = L, Top = 160 + 4 * (H + G) };
            txtDireccion.SetBounds(L, 180 + 4 * (H + G), 600, 100);
            txtDireccion.Multiline = true;

            // ===== NUEVA IMPLEMENTACION: layout robusto para botones ===== BOTONES
            int buttonsTop = txtDireccion.Bottom + 12;   // coloca los botones justo debajo del textbox de Dirección

            btnGuardar.Text = "Guardar";
            btnGuardar.SetBounds(440, buttonsTop, 80, 28);

            btnCancelar.Text = "Cancelar";
            btnCancelar.SetBounds(530, buttonsTop, 80, 28);

            // Ajusta el alto del formulario para que los botones queden visibles
            int minHeight = buttonsTop + 60;             // margen inferior
            this.ClientSize = new Size(
                Math.Max(this.ClientSize.Width, 640),
                Math.Max(this.ClientSize.Height, minHeight)
            );

            // Salvavidas por DPI: si algo igual se sale, habilita scroll
            this.AutoScroll = true;

            // Calidad de vida
            this.AcceptButton = btnGuardar;
            this.CancelButton = btnCancelar;
            // ===== FIN NUEVA IMPLEMENTACION =====

            btnGuardar.Click += (_, __) => OnSave();
            btnCancelar.Click += (_, __) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[] {
                lblCedula, txtCedula,
                lblNombre, txtNombre,
                lblPrimerApe, txtPrimerApellido,
                lblSegundoApe, txtSegundoApellido,
                lblNacimiento, dtNacimiento,
                lblEstadoCivil, cboEstadoCivil,
                lblCelular, txtCelular,
                lblNacionalidad, txtNacionalidad,
                chkLaboro,
                lblDireccion, txtDireccion,
                btnGuardar, btnCancelar
            });

            // Inicializar Result
            Result = seed != null
                ? new EntitySolicitud
                {
                    Id = seed.Id,
                    Cedula = seed.Cedula,
                    Primer_Apellido = seed.Primer_Apellido,
                    Segundo_Apellido = seed.Segundo_Apellido,
                    Nombre = seed.Nombre,
                    Fecha_Nacimiento = seed.Fecha_Nacimiento,
                    Estado_Civil = seed.Estado_Civil,
                    Celular = seed.Celular,
                    Nacionalidad = seed.Nacionalidad,
                    Laboro = seed.Laboro,
                    Direccion = seed.Direccion
                }
                : new EntitySolicitud();

            // Pintar valores si edita
            if (seed != null)
            {
                txtCedula.Text = seed.Cedula;
                txtNombre.Text = seed.Nombre;
                txtPrimerApellido.Text = seed.Primer_Apellido;
                txtSegundoApellido.Text = seed.Segundo_Apellido ?? string.Empty;
                dtNacimiento.Value = seed.Fecha_Nacimiento == default ? DateTime.Today : seed.Fecha_Nacimiento;
                cboEstadoCivil.SelectedItem = cboEstadoCivil.Items.Cast<object>().FirstOrDefault(x => x.ToString() == seed.Estado_Civil) ?? cboEstadoCivil.Items.Cast<object>().FirstOrDefault();
                txtCelular.Text = seed.Celular;
                txtNacionalidad.Text = seed.Nacionalidad;
                chkLaboro.Checked = seed.Laboro != 0;
                txtDireccion.Text = seed.Direccion;
            }
            else
            {
                cboEstadoCivil.SelectedIndex = 0;
                dtNacimiento.Value = DateTime.Today.AddYears(-18);
            }
        }

        private void OnSave()
        {
            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(txtCedula.Text))
            {
                MessageBox.Show("La cédula es requerida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCedula.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es requerido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txtPrimerApellido.Text))
            {
                MessageBox.Show("El primer apellido es requerido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrimerApellido.Focus(); return;
            }
            if (cboEstadoCivil.SelectedItem is null)
            {
                MessageBox.Show("Seleccione el estado civil.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboEstadoCivil.DroppedDown = true; return;
            }
            if (string.IsNullOrWhiteSpace(txtCelular.Text))
            {
                MessageBox.Show("El celular es requerido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCelular.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txtNacionalidad.Text))
            {
                MessageBox.Show("La nacionalidad es requerida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNacionalidad.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("La dirección es requerida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDireccion.Focus(); return;
            }

            // Mapear a entidad
            Result.Cedula = txtCedula.Text.Trim();
            Result.Primer_Apellido = txtPrimerApellido.Text.Trim();
            Result.Segundo_Apellido = string.IsNullOrWhiteSpace(txtSegundoApellido.Text) ? null : txtSegundoApellido.Text.Trim();
            Result.Nombre = txtNombre.Text.Trim();
            Result.Fecha_Nacimiento = dtNacimiento.Value.Date;
            Result.Estado_Civil = cboEstadoCivil.SelectedItem!.ToString()!;
            Result.Celular = txtCelular.Text.Trim();
            Result.Nacionalidad = txtNacionalidad.Text.Trim();
            Result.Laboro = chkLaboro.Checked ? 1 : 0; // bit -> int 0/1 en tu entity
            Result.Direccion = txtDireccion.Text.Trim();

            DialogResult = DialogResult.OK;
        }
    }
}
