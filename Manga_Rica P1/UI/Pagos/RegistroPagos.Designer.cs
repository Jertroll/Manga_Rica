namespace Manga_Rica_P1.UI.Pagos
{
    partial class RegistroPagos
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistroPagos));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            labelTitulo = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            buttonGuardar = new Button();
            labelHorasNormales = new Label();
            textBoxHorasNormales = new TextBox();
            comboBoxSemana = new ComboBox();
            labelSemana = new Label();
            textBoxNombre = new TextBox();
            labelNombreCompleto = new Label();
            textBoxCarnet = new TextBox();
            pictureBoxEmpleado = new PictureBox();
            labelCarnet = new Label();
            labelHorasExtra = new Label();
            textBoxHorasExtras = new TextBox();
            labelFeriados = new Label();
            textBoxFeriados = new TextBox();
            labelHorasDobles = new Label();
            textBoxHorasDobles = new TextBox();
            labelSalario = new Label();
            textBoxSalario = new TextBox();
            labelPagoNeto = new Label();
            textBoxPagoNeto = new TextBox();
            labelPagoBruto = new Label();
            textBoxPagoBruto = new TextBox();
            labelOtras = new Label();
            textBoxOtras = new TextBox();
            labelUniforme = new Label();
            textBoxUniforme = new TextBox();
            labelSoda = new Label();
            textBoxSoda = new TextBox();
            dataGridViewEmpleados = new DataGridView();
            CarneColumna = new DataGridViewTextBoxColumn();
            NombreColumna = new DataGridViewTextBoxColumn();
            Apellido1Columna = new DataGridViewTextBoxColumn();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxEmpleado).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewEmpleados).BeginInit();
            SuspendLayout();
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.ForeColor = Color.Green;
            labelTitulo.Location = new Point(431, 56);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(269, 25);
            labelTitulo.TabIndex = 1;
            labelTitulo.Text = "Modulo de Registro De pago";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.Green;
            flowLayoutPanel1.Controls.Add(buttonGuardar);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1153, 33);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // buttonGuardar
            // 
            buttonGuardar.Image = (Image)resources.GetObject("buttonGuardar.Image");
            buttonGuardar.Location = new Point(3, 3);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(30, 25);
            buttonGuardar.TabIndex = 16;
            buttonGuardar.UseVisualStyleBackColor = true;
            buttonGuardar.Click += buttonGuardar_Click;
            // 
            // labelHorasNormales
            // 
            labelHorasNormales.AutoSize = true;
            labelHorasNormales.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelHorasNormales.ForeColor = Color.Green;
            labelHorasNormales.Location = new Point(250, 199);
            labelHorasNormales.Name = "labelHorasNormales";
            labelHorasNormales.Size = new Size(85, 17);
            labelHorasNormales.TabIndex = 20;
            labelHorasNormales.Text = "H. Normales";
            // 
            // textBoxHorasNormales
            // 
            textBoxHorasNormales.BackColor = SystemColors.ActiveBorder;
            textBoxHorasNormales.Location = new Point(343, 198);
            textBoxHorasNormales.Name = "textBoxHorasNormales";
            textBoxHorasNormales.Size = new Size(130, 23);
            textBoxHorasNormales.TabIndex = 19;
            // 
            // comboBoxSemana
            // 
            comboBoxSemana.BackColor = SystemColors.ActiveBorder;
            comboBoxSemana.FormattingEnabled = true;
            comboBoxSemana.Location = new Point(917, 76);
            comboBoxSemana.Name = "comboBoxSemana";
            comboBoxSemana.Size = new Size(120, 23);
            comboBoxSemana.TabIndex = 18;
            // 
            // labelSemana
            // 
            labelSemana.AutoSize = true;
            labelSemana.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSemana.ForeColor = Color.Green;
            labelSemana.Location = new Point(950, 56);
            labelSemana.Name = "labelSemana";
            labelSemana.Size = new Size(56, 17);
            labelSemana.TabIndex = 17;
            labelSemana.Text = "Semana";
            // 
            // textBoxNombre
            // 
            textBoxNombre.BackColor = SystemColors.ActiveBorder;
            textBoxNombre.Location = new Point(423, 134);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(277, 23);
            textBoxNombre.TabIndex = 16;
            // 
            // labelNombreCompleto
            // 
            labelNombreCompleto.AutoSize = true;
            labelNombreCompleto.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNombreCompleto.ForeColor = Color.Green;
            labelNombreCompleto.Location = new Point(295, 134);
            labelNombreCompleto.Name = "labelNombreCompleto";
            labelNombreCompleto.Size = new Size(122, 17);
            labelNombreCompleto.TabIndex = 15;
            labelNombreCompleto.Text = "Nombre Completo";
            // 
            // textBoxCarnet
            // 
            textBoxCarnet.BackColor = Color.DarkGray;
            textBoxCarnet.Location = new Point(101, 156);
            textBoxCarnet.Name = "textBoxCarnet";
            textBoxCarnet.Size = new Size(143, 23);
            textBoxCarnet.TabIndex = 14;
            // 
            // pictureBoxEmpleado
            // 
            pictureBoxEmpleado.BackColor = SystemColors.ActiveBorder;
            pictureBoxEmpleado.Location = new Point(101, 185);
            pictureBoxEmpleado.Name = "pictureBoxEmpleado";
            pictureBoxEmpleado.Size = new Size(143, 115);
            pictureBoxEmpleado.TabIndex = 13;
            pictureBoxEmpleado.TabStop = false;
            // 
            // labelCarnet
            // 
            labelCarnet.AutoSize = true;
            labelCarnet.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCarnet.ForeColor = Color.Green;
            labelCarnet.Location = new Point(140, 132);
            labelCarnet.Name = "labelCarnet";
            labelCarnet.Size = new Size(60, 21);
            labelCarnet.TabIndex = 12;
            labelCarnet.Text = "Carnet";
            // 
            // labelHorasExtra
            // 
            labelHorasExtra.AutoSize = true;
            labelHorasExtra.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelHorasExtra.ForeColor = Color.Green;
            labelHorasExtra.Location = new Point(271, 244);
            labelHorasExtra.Name = "labelHorasExtra";
            labelHorasExtra.Size = new Size(63, 17);
            labelHorasExtra.TabIndex = 22;
            labelHorasExtra.Text = "H. Extras";
            // 
            // textBoxHorasExtras
            // 
            textBoxHorasExtras.BackColor = SystemColors.ActiveBorder;
            textBoxHorasExtras.Location = new Point(343, 243);
            textBoxHorasExtras.Name = "textBoxHorasExtras";
            textBoxHorasExtras.Size = new Size(130, 23);
            textBoxHorasExtras.TabIndex = 21;
            // 
            // labelFeriados
            // 
            labelFeriados.AutoSize = true;
            labelFeriados.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelFeriados.ForeColor = Color.Green;
            labelFeriados.Location = new Point(271, 337);
            labelFeriados.Name = "labelFeriados";
            labelFeriados.Size = new Size(60, 17);
            labelFeriados.TabIndex = 26;
            labelFeriados.Text = "Feriados";
            // 
            // textBoxFeriados
            // 
            textBoxFeriados.BackColor = SystemColors.ActiveBorder;
            textBoxFeriados.Location = new Point(343, 336);
            textBoxFeriados.Name = "textBoxFeriados";
            textBoxFeriados.Size = new Size(130, 23);
            textBoxFeriados.TabIndex = 25;
            // 
            // labelHorasDobles
            // 
            labelHorasDobles.AutoSize = true;
            labelHorasDobles.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelHorasDobles.ForeColor = Color.Green;
            labelHorasDobles.Location = new Point(262, 292);
            labelHorasDobles.Name = "labelHorasDobles";
            labelHorasDobles.Size = new Size(69, 17);
            labelHorasDobles.TabIndex = 24;
            labelHorasDobles.Text = "H. Dobles";
            // 
            // textBoxHorasDobles
            // 
            textBoxHorasDobles.BackColor = SystemColors.ActiveBorder;
            textBoxHorasDobles.Location = new Point(343, 291);
            textBoxHorasDobles.Name = "textBoxHorasDobles";
            textBoxHorasDobles.Size = new Size(130, 23);
            textBoxHorasDobles.TabIndex = 23;
            // 
            // labelSalario
            // 
            labelSalario.AutoSize = true;
            labelSalario.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSalario.ForeColor = Color.Green;
            labelSalario.Location = new Point(281, 381);
            labelSalario.Name = "labelSalario";
            labelSalario.Size = new Size(50, 17);
            labelSalario.TabIndex = 28;
            labelSalario.Text = "Salario";
            // 
            // textBoxSalario
            // 
            textBoxSalario.BackColor = SystemColors.ActiveBorder;
            textBoxSalario.Location = new Point(343, 380);
            textBoxSalario.Name = "textBoxSalario";
            textBoxSalario.Size = new Size(130, 23);
            textBoxSalario.TabIndex = 27;
            // 
            // labelPagoNeto
            // 
            labelPagoNeto.AutoSize = true;
            labelPagoNeto.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelPagoNeto.ForeColor = Color.Green;
            labelPagoNeto.Location = new Point(491, 381);
            labelPagoNeto.Name = "labelPagoNeto";
            labelPagoNeto.Size = new Size(73, 17);
            labelPagoNeto.TabIndex = 38;
            labelPagoNeto.Text = "Pago Neto";
            // 
            // textBoxPagoNeto
            // 
            textBoxPagoNeto.BackColor = SystemColors.ActiveBorder;
            textBoxPagoNeto.Location = new Point(570, 380);
            textBoxPagoNeto.Name = "textBoxPagoNeto";
            textBoxPagoNeto.Size = new Size(130, 23);
            textBoxPagoNeto.TabIndex = 37;
            // 
            // labelPagoBruto
            // 
            labelPagoBruto.AutoSize = true;
            labelPagoBruto.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelPagoBruto.ForeColor = Color.Green;
            labelPagoBruto.Location = new Point(487, 337);
            labelPagoBruto.Name = "labelPagoBruto";
            labelPagoBruto.Size = new Size(77, 17);
            labelPagoBruto.TabIndex = 36;
            labelPagoBruto.Text = "Pago Bruto";
            // 
            // textBoxPagoBruto
            // 
            textBoxPagoBruto.BackColor = SystemColors.ActiveBorder;
            textBoxPagoBruto.Location = new Point(570, 336);
            textBoxPagoBruto.Name = "textBoxPagoBruto";
            textBoxPagoBruto.Size = new Size(130, 23);
            textBoxPagoBruto.TabIndex = 35;
            // 
            // labelOtras
            // 
            labelOtras.AutoSize = true;
            labelOtras.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelOtras.ForeColor = Color.Green;
            labelOtras.Location = new Point(514, 292);
            labelOtras.Name = "labelOtras";
            labelOtras.Size = new Size(41, 17);
            labelOtras.TabIndex = 34;
            labelOtras.Text = "Otras";
            // 
            // textBoxOtras
            // 
            textBoxOtras.BackColor = SystemColors.ActiveBorder;
            textBoxOtras.Location = new Point(570, 291);
            textBoxOtras.Name = "textBoxOtras";
            textBoxOtras.Size = new Size(130, 23);
            textBoxOtras.TabIndex = 33;
            // 
            // labelUniforme
            // 
            labelUniforme.AutoSize = true;
            labelUniforme.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelUniforme.ForeColor = Color.Green;
            labelUniforme.Location = new Point(498, 244);
            labelUniforme.Name = "labelUniforme";
            labelUniforme.Size = new Size(66, 17);
            labelUniforme.TabIndex = 32;
            labelUniforme.Text = "Uniforme";
            // 
            // textBoxUniforme
            // 
            textBoxUniforme.BackColor = SystemColors.ActiveBorder;
            textBoxUniforme.Location = new Point(570, 243);
            textBoxUniforme.Name = "textBoxUniforme";
            textBoxUniforme.Size = new Size(130, 23);
            textBoxUniforme.TabIndex = 31;
            // 
            // labelSoda
            // 
            labelSoda.AutoSize = true;
            labelSoda.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSoda.ForeColor = Color.Green;
            labelSoda.Location = new Point(517, 198);
            labelSoda.Name = "labelSoda";
            labelSoda.Size = new Size(38, 17);
            labelSoda.TabIndex = 30;
            labelSoda.Text = "Soda";
            // 
            // textBoxSoda
            // 
            textBoxSoda.BackColor = SystemColors.ActiveBorder;
            textBoxSoda.Location = new Point(570, 198);
            textBoxSoda.Name = "textBoxSoda";
            textBoxSoda.Size = new Size(130, 23);
            textBoxSoda.TabIndex = 29;
            // 
            // dataGridViewEmpleados
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridViewEmpleados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewEmpleados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewEmpleados.Columns.AddRange(new DataGridViewColumn[] { CarneColumna, NombreColumna, Apellido1Columna });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridViewEmpleados.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewEmpleados.Location = new Point(735, 134);
            dataGridViewEmpleados.Name = "dataGridViewEmpleados";
            dataGridViewEmpleados.Size = new Size(339, 269);
            dataGridViewEmpleados.TabIndex = 39;
            // 
            // CarneColumna
            // 
            CarneColumna.HeaderText = "Carne";
            CarneColumna.Name = "CarneColumna";
            // 
            // NombreColumna
            // 
            NombreColumna.HeaderText = "Nombre";
            NombreColumna.Name = "NombreColumna";
            // 
            // Apellido1Columna
            // 
            Apellido1Columna.HeaderText = "Apellido 1";
            Apellido1Columna.Name = "Apellido1Columna";
            // 
            // RegistroPagos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dataGridViewEmpleados);
            Controls.Add(labelPagoNeto);
            Controls.Add(textBoxPagoNeto);
            Controls.Add(labelPagoBruto);
            Controls.Add(textBoxPagoBruto);
            Controls.Add(labelOtras);
            Controls.Add(textBoxOtras);
            Controls.Add(labelUniforme);
            Controls.Add(textBoxUniforme);
            Controls.Add(labelSoda);
            Controls.Add(textBoxSoda);
            Controls.Add(labelSalario);
            Controls.Add(textBoxSalario);
            Controls.Add(labelFeriados);
            Controls.Add(textBoxFeriados);
            Controls.Add(labelHorasDobles);
            Controls.Add(textBoxHorasDobles);
            Controls.Add(labelHorasExtra);
            Controls.Add(textBoxHorasExtras);
            Controls.Add(labelTitulo);
            Controls.Add(labelHorasNormales);
            Controls.Add(textBoxHorasNormales);
            Controls.Add(comboBoxSemana);
            Controls.Add(labelSemana);
            Controls.Add(textBoxNombre);
            Controls.Add(labelNombreCompleto);
            Controls.Add(textBoxCarnet);
            Controls.Add(pictureBoxEmpleado);
            Controls.Add(labelCarnet);
            Controls.Add(flowLayoutPanel1);
            Name = "RegistroPagos";
            Size = new Size(1153, 462);
            Load += RegistroPagos_Load;
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxEmpleado).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewEmpleados).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label labelTitulo;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button buttonGuardar;
        private Label labelHorasNormales;
        private TextBox textBoxHorasNormales;
        private ComboBox comboBoxSemana;
        private Label labelSemana;
        private TextBox textBoxNombre;
        private Label labelNombreCompleto;
        private TextBox textBoxCarnet;
        private PictureBox pictureBoxEmpleado;
        private Label labelCarnet;
        private Label labelHorasExtra;
        private TextBox textBoxHorasExtras;
        private Label labelFeriados;
        private TextBox textBoxFeriados;
        private Label labelHorasDobles;
        private TextBox textBoxHorasDobles;
        private Label labelSalario;
        private TextBox textBoxSalario;
        private Label labelPagoNeto;
        private TextBox textBoxPagoNeto;
        private Label labelPagoBruto;
        private TextBox textBoxPagoBruto;
        private Label labelOtras;
        private TextBox textBoxOtras;
        private Label labelUniforme;
        private TextBox textBoxUniforme;
        private Label labelSoda;
        private TextBox textBoxSoda;
        private DataGridView dataGridViewEmpleados;
        private DataGridViewTextBoxColumn CarneColumna;
        private DataGridViewTextBoxColumn NombreColumna;
        private DataGridViewTextBoxColumn Apellido1Columna;
    }
}
