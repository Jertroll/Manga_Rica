
namespace Manga_Rica_P1.UI.User
{
    partial class AddUser
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            labelNombre = new Label();
            labelTitulo = new Label();
            pictureBox1 = new PictureBox();
            labelClave = new Label();
            labelPerfil = new Label();
            labelFecha = new Label();
            btnAgregar = new Button();
            btnCancelar = new Button();
            textBoxNombre = new TextBox();
            textBoxClave = new TextBox();

            dateTimePicker1 = new DateTimePicker();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // labelNombre
            // 
            labelNombre.AutoSize = true;
            labelNombre.Font = new Font("Segoe UI", 10F);
            labelNombre.Location = new Point(283, 73);
            labelNombre.Name = "labelNombre";
            labelNombre.Size = new Size(59, 19);
            labelNombre.TabIndex = 0;
            labelNombre.Text = "Nombre";
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            labelTitulo.ForeColor = SystemColors.ButtonHighlight;
            labelTitulo.Location = new Point(10, 9);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(187, 25);
            labelTitulo.TabIndex = 1;
            labelTitulo.Text = "Registro de Usuario";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.user_small3;
            pictureBox1.Location = new Point(57, 95);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(172, 183);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // labelClave
            // 
            labelClave.AutoSize = true;
            labelClave.Font = new Font("Segoe UI", 10F);
            labelClave.Location = new Point(572, 73);
            labelClave.Name = "labelClave";
            labelClave.Size = new Size(79, 19);
            labelClave.TabIndex = 3;
            labelClave.Text = "Contraseña";
            labelClave.Click += labelClave_Click;
            // 
            // labelPerfil
            // 
            labelPerfil.AutoSize = true;
            labelPerfil.Font = new Font("Segoe UI", 10F);
            labelPerfil.Location = new Point(420, 73);
            labelPerfil.Name = "labelPerfil";
            labelPerfil.Size = new Size(108, 19);
            labelPerfil.TabIndex = 4;
            labelPerfil.Text = "Perfil de Usuario";
            labelPerfil.Click += labelPerfil_Click;
            // 
            // labelFecha
            // 
            labelFecha.AutoSize = true;
            labelFecha.Font = new Font("Segoe UI", 10F);
            labelFecha.Location = new Point(419, 152);
            labelFecha.Name = "labelFecha";
            labelFecha.Size = new Size(109, 19);
            labelFecha.TabIndex = 5;
            labelFecha.Text = "Fecha Expiracion";
            // 
            // btnAgregar
            // 
            btnAgregar.BackColor = Color.LimeGreen;
            btnAgregar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAgregar.ForeColor = SystemColors.ControlLightLight;
            btnAgregar.Location = new Point(367, 244);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(100, 35);
            btnAgregar.TabIndex = 7;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Red;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(477, 244);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 35);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // textBoxNombre
            // 
            textBoxNombre.Location = new Point(283, 95);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(104, 23);
            textBoxNombre.TabIndex = 9;
            // 
            // textBoxClave
            // 
            textBoxClave.Location = new Point(572, 95);
            textBoxClave.Name = "textBoxClave";
            textBoxClave.Size = new Size(112, 23);
            textBoxClave.TabIndex = 10;
            // 
            // comboPerfil
            comboPerfil = new ComboBox();
            comboPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            comboPerfil.Location = new Point(420, 95);
            comboPerfil.Name = "comboPerfil";
            comboPerfil.Size = new Size(111, 23);
            comboPerfil.TabIndex = 11;

            // Cargar opciones fijas
            comboPerfil.Items.AddRange(new object[] { "Admin", "Empleado", "Supervisor" });
            comboPerfil.SelectedIndex = 0; 
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(367, 186);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(210, 23);
            dateTimePicker1.TabIndex = 12;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Lime;
            panel1.Controls.Add(labelTitulo);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(729, 41);
            panel1.TabIndex = 13;
            // 
            // AddUser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(729, 388);
            Controls.Add(panel1);
            Controls.Add(dateTimePicker1);
            Controls.Add(comboPerfil);
            Controls.Add(textBoxClave);
            Controls.Add(textBoxNombre);
            Controls.Add(btnCancelar);
            Controls.Add(btnAgregar);
            Controls.Add(labelFecha);
            Controls.Add(labelPerfil);
            Controls.Add(labelClave);
            Controls.Add(labelNombre);
            Controls.Add(pictureBox1);
            Name = "AddUser";
            Text = "Agregar Usuario";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        // ===== Botones =====
        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            // Validaciones mínimas (puedes migrar a ErrorProvider)
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                MessageBox.Show("El nombre es requerido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNombre.Focus();
                return;
            }
            if (comboPerfil.SelectedItem is null)
            {
                MessageBox.Show("Seleccione un perfil.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboPerfil.DroppedDown = true;
                return;
            }
            if (_mode == EditMode.Nuevo && string.IsNullOrWhiteSpace(textBoxClave.Text))
            {
                MessageBox.Show("La contraseña es requerida.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxClave.Focus();
                return;
            }

            // Mapear al resultado
            Resultado.Nombre = textBoxNombre.Text.Trim();
            Resultado.Perfil = comboPerfil.SelectedItem?.ToString() ?? "";
            Resultado.FechaExpiracion = dateTimePicker1.Value.Date;

            // En Editar: solo cambiar clave si el usuario digitó una nueva
            if (!string.IsNullOrWhiteSpace(textBoxClave.Text))
                Resultado.Clave = textBoxClave.Text;

            this.DialogResult = DialogResult.OK; // cierra modal devolviendo OK
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        private Label labelNombre;
        private Label labelTitulo;
        private Label labelClave;
        private Label labelPerfil;
        private Label labelFecha;
        private PictureBox pictureBox1;
        private Button btnAgregar;
        private Button btnCancelar;
        private TextBox textBoxNombre;
        private TextBox textBoxClave;
        private ComboBox comboPerfil;
        private DateTimePicker dateTimePicker1;
        private Panel panel1;
    }
}
