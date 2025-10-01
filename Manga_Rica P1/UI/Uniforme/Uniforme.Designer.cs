namespace Manga_Rica_P1.UI.Uniforme
{
    partial class Uniforme
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Uniforme));
            labelTotal = new Label();
            textBoxTotal = new TextBox();
            comboBoxFecha = new ComboBox();
            textBoxNumFactura = new TextBox();
            labelNumFactura = new Label();
            checkBoxAnulada = new CheckBox();
            dataGridView1 = new DataGridView();
            CantidadColumna = new DataGridViewTextBoxColumn();
            CodigoColumna = new DataGridViewTextBoxColumn();
            DescripcionColumna = new DataGridViewTextBoxColumn();
            PrecioColumna = new DataGridViewTextBoxColumn();
            TotalColumna = new DataGridViewTextBoxColumn();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnBuscar = new Button();
            buttonGuardar = new Button();
            buttonAgregar = new Button();
            labelSubTotal = new Label();
            textBoxSubTotal = new TextBox();
            labelCantidad = new Label();
            comboBoxArticulos = new ComboBox();
            textBoxCantidad = new TextBox();
            labelConcepto = new Label();
            textBoxNombre = new TextBox();
            labelNombreCompleto = new Label();
            textBoxCarnet = new TextBox();
            labelTitulo = new Label();
            pictureBoxEmpleado = new PictureBox();
            labelCarnet = new Label();
            labelSaldo = new Label();
            textBoxSaldo = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxEmpleado).BeginInit();
            SuspendLayout();
            // 
            // labelTotal
            // 
            labelTotal.AutoSize = true;
            labelTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelTotal.ForeColor = Color.Green;
            labelTotal.Location = new Point(606, 397);
            labelTotal.Name = "labelTotal";
            labelTotal.Size = new Size(48, 21);
            labelTotal.TabIndex = 41;
            labelTotal.Text = "Total";
            // 
            // textBoxTotal
            // 
            textBoxTotal.BackColor = SystemColors.ActiveBorder;
            textBoxTotal.Location = new Point(660, 395);
            textBoxTotal.Name = "textBoxTotal";
            textBoxTotal.Size = new Size(123, 23);
            textBoxTotal.TabIndex = 40;
            // 
            // comboBoxFecha
            // 
            comboBoxFecha.BackColor = SystemColors.ActiveBorder;
            comboBoxFecha.FormattingEnabled = true;
            comboBoxFecha.Location = new Point(682, 59);
            comboBoxFecha.Name = "comboBoxFecha";
            comboBoxFecha.Size = new Size(92, 23);
            comboBoxFecha.TabIndex = 39;
            // 
            // textBoxNumFactura
            // 
            textBoxNumFactura.BackColor = SystemColors.ActiveBorder;
            textBoxNumFactura.Location = new Point(682, 88);
            textBoxNumFactura.Name = "textBoxNumFactura";
            textBoxNumFactura.Size = new Size(92, 23);
            textBoxNumFactura.TabIndex = 38;
            // 
            // labelNumFactura
            // 
            labelNumFactura.AutoSize = true;
            labelNumFactura.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            labelNumFactura.ForeColor = Color.Red;
            labelNumFactura.Location = new Point(615, 88);
            labelNumFactura.Name = "labelNumFactura";
            labelNumFactura.Size = new Size(61, 20);
            labelNumFactura.TabIndex = 37;
            labelNumFactura.Text = "Factura";
            // 
            // checkBoxAnulada
            // 
            checkBoxAnulada.AutoSize = true;
            checkBoxAnulada.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBoxAnulada.ForeColor = SystemColors.HotTrack;
            checkBoxAnulada.Location = new Point(598, 60);
            checkBoxAnulada.Name = "checkBoxAnulada";
            checkBoxAnulada.Size = new Size(78, 21);
            checkBoxAnulada.TabIndex = 36;
            checkBoxAnulada.Text = "Anulada";
            checkBoxAnulada.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { CantidadColumna, CodigoColumna, DescripcionColumna, PrecioColumna, TotalColumna });
            dataGridView1.Location = new Point(39, 318);
            dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.Size = new Size(543, 155);
            dataGridView1.TabIndex = 35;
            // 
            // CantidadColumna
            // 
            CantidadColumna.HeaderText = "Cantidad";
            CantidadColumna.Name = "CantidadColumna";
            // 
            // CodigoColumna
            // 
            CodigoColumna.HeaderText = "Codigo";
            CodigoColumna.Name = "CodigoColumna";
            // 
            // DescripcionColumna
            // 
            DescripcionColumna.HeaderText = "Descripcion";
            DescripcionColumna.Name = "DescripcionColumna";
            // 
            // PrecioColumna
            // 
            PrecioColumna.HeaderText = "Precio";
            PrecioColumna.Name = "PrecioColumna";
            // 
            // TotalColumna
            // 
            TotalColumna.HeaderText = "Total";
            TotalColumna.Name = "TotalColumna";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.Green;
            flowLayoutPanel1.Controls.Add(btnBuscar);
            flowLayoutPanel1.Controls.Add(buttonGuardar);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.ForeColor = Color.Lime;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(818, 30);
            flowLayoutPanel1.TabIndex = 34;
            // 
            // btnBuscar
            // 
            btnBuscar.Image = (Image)resources.GetObject("btnBuscar.Image");
            btnBuscar.Location = new Point(3, 3);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(30, 25);
            btnBuscar.TabIndex = 14;
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // buttonGuardar
            // 
            buttonGuardar.Image = (Image)resources.GetObject("buttonGuardar.Image");
            buttonGuardar.Location = new Point(39, 3);
            buttonGuardar.Name = "buttonGuardar";
            buttonGuardar.Size = new Size(30, 25);
            buttonGuardar.TabIndex = 15;
            buttonGuardar.UseVisualStyleBackColor = true;
            // 
            // buttonAgregar
            // 
            buttonAgregar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonAgregar.Location = new Point(548, 260);
            buttonAgregar.Name = "buttonAgregar";
            buttonAgregar.Size = new Size(142, 35);
            buttonAgregar.TabIndex = 33;
            buttonAgregar.Text = "Agregar Producto";
            buttonAgregar.UseVisualStyleBackColor = true;
            // 
            // labelSubTotal
            // 
            labelSubTotal.AutoSize = true;
            labelSubTotal.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSubTotal.ForeColor = Color.Green;
            labelSubTotal.Location = new Point(359, 252);
            labelSubTotal.Name = "labelSubTotal";
            labelSubTotal.Size = new Size(66, 17);
            labelSubTotal.TabIndex = 32;
            labelSubTotal.Text = "Sub Total";
            // 
            // textBoxSubTotal
            // 
            textBoxSubTotal.BackColor = SystemColors.ActiveBorder;
            textBoxSubTotal.Location = new Point(305, 272);
            textBoxSubTotal.Name = "textBoxSubTotal";
            textBoxSubTotal.Size = new Size(171, 23);
            textBoxSubTotal.TabIndex = 31;
            // 
            // labelCantidad
            // 
            labelCantidad.AutoSize = true;
            labelCantidad.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCantidad.ForeColor = Color.Green;
            labelCantidad.Location = new Point(588, 189);
            labelCantidad.Name = "labelCantidad";
            labelCantidad.Size = new Size(63, 17);
            labelCantidad.TabIndex = 30;
            labelCantidad.Text = "Cantidad";
            // 
            // comboBoxArticulos
            // 
            comboBoxArticulos.BackColor = SystemColors.ActiveBorder;
            comboBoxArticulos.FormattingEnabled = true;
            comboBoxArticulos.Location = new Point(305, 209);
            comboBoxArticulos.Name = "comboBoxArticulos";
            comboBoxArticulos.Size = new Size(171, 23);
            comboBoxArticulos.TabIndex = 29;
            // 
            // textBoxCantidad
            // 
            textBoxCantidad.BackColor = SystemColors.ActiveBorder;
            textBoxCantidad.Location = new Point(548, 209);
            textBoxCantidad.Name = "textBoxCantidad";
            textBoxCantidad.Size = new Size(142, 23);
            textBoxCantidad.TabIndex = 28;
            // 
            // labelConcepto
            // 
            labelConcepto.AutoSize = true;
            labelConcepto.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelConcepto.ForeColor = Color.Green;
            labelConcepto.Location = new Point(333, 189);
            labelConcepto.Name = "labelConcepto";
            labelConcepto.Size = new Size(116, 17);
            labelConcepto.TabIndex = 27;
            labelConcepto.Text = "Por Concepto De ";
            // 
            // textBoxNombre
            // 
            textBoxNombre.BackColor = SystemColors.ActiveBorder;
            textBoxNombre.Location = new Point(305, 150);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(385, 23);
            textBoxNombre.TabIndex = 26;
            // 
            // labelNombreCompleto
            // 
            labelNombreCompleto.AutoSize = true;
            labelNombreCompleto.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNombreCompleto.ForeColor = Color.Green;
            labelNombreCompleto.Location = new Point(436, 130);
            labelNombreCompleto.Name = "labelNombreCompleto";
            labelNombreCompleto.Size = new Size(122, 17);
            labelNombreCompleto.TabIndex = 25;
            labelNombreCompleto.Text = "Nombre Completo";
            // 
            // textBoxCarnet
            // 
            textBoxCarnet.BackColor = Color.DarkGray;
            textBoxCarnet.Location = new Point(122, 151);
            textBoxCarnet.Name = "textBoxCarnet";
            textBoxCarnet.Size = new Size(143, 23);
            textBoxCarnet.TabIndex = 24;
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.ForeColor = Color.Green;
            labelTitulo.Location = new Point(292, 60);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(220, 30);
            labelTitulo.TabIndex = 23;
            labelTitulo.Text = "Modulo de Uniforme";
            // 
            // pictureBoxEmpleado
            // 
            pictureBoxEmpleado.BackColor = SystemColors.ActiveBorder;
            pictureBoxEmpleado.Location = new Point(122, 180);
            pictureBoxEmpleado.Name = "pictureBoxEmpleado";
            pictureBoxEmpleado.Size = new Size(143, 115);
            pictureBoxEmpleado.TabIndex = 22;
            pictureBoxEmpleado.TabStop = false;
            // 
            // labelCarnet
            // 
            labelCarnet.AutoSize = true;
            labelCarnet.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCarnet.ForeColor = Color.Green;
            labelCarnet.Location = new Point(161, 127);
            labelCarnet.Name = "labelCarnet";
            labelCarnet.Size = new Size(60, 21);
            labelCarnet.TabIndex = 21;
            labelCarnet.Text = "Carnet";
            // 
            // labelSaldo
            // 
            labelSaldo.AutoSize = true;
            labelSaldo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelSaldo.ForeColor = Color.Green;
            labelSaldo.Location = new Point(606, 355);
            labelSaldo.Name = "labelSaldo";
            labelSaldo.Size = new Size(53, 21);
            labelSaldo.TabIndex = 43;
            labelSaldo.Text = "Saldo";
            // 
            // textBoxSaldo
            // 
            textBoxSaldo.BackColor = SystemColors.ActiveBorder;
            textBoxSaldo.Location = new Point(660, 353);
            textBoxSaldo.Name = "textBoxSaldo";
            textBoxSaldo.Size = new Size(123, 23);
            textBoxSaldo.TabIndex = 42;
            // 
            // Uniforme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(818, 492);
            Controls.Add(labelSaldo);
            Controls.Add(textBoxSaldo);
            Controls.Add(labelTotal);
            Controls.Add(textBoxTotal);
            Controls.Add(comboBoxFecha);
            Controls.Add(textBoxNumFactura);
            Controls.Add(labelNumFactura);
            Controls.Add(checkBoxAnulada);
            Controls.Add(dataGridView1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(buttonAgregar);
            Controls.Add(labelSubTotal);
            Controls.Add(textBoxSubTotal);
            Controls.Add(labelCantidad);
            Controls.Add(comboBoxArticulos);
            Controls.Add(textBoxCantidad);
            Controls.Add(labelConcepto);
            Controls.Add(textBoxNombre);
            Controls.Add(labelNombreCompleto);
            Controls.Add(textBoxCarnet);
            Controls.Add(labelTitulo);
            Controls.Add(pictureBoxEmpleado);
            Controls.Add(labelCarnet);
            Name = "Uniforme";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Uniforme";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxEmpleado).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTotal;
        private TextBox textBoxTotal;
        private ComboBox comboBoxFecha;
        private TextBox textBoxNumFactura;
        private Label labelNumFactura;
        private CheckBox checkBoxAnulada;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn CantidadColumna;
        private DataGridViewTextBoxColumn CodigoColumna;
        private DataGridViewTextBoxColumn DescripcionColumna;
        private DataGridViewTextBoxColumn PrecioColumna;
        private DataGridViewTextBoxColumn TotalColumna;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnBuscar;
        private Button buttonGuardar;
        private Button buttonAgregar;
        private Label labelSubTotal;
        private TextBox textBoxSubTotal;
        private Label labelCantidad;
        private ComboBox comboBoxArticulos;
        private TextBox textBoxCantidad;
        private Label labelConcepto;
        private TextBox textBoxNombre;
        private Label labelNombreCompleto;
        private TextBox textBoxCarnet;
        private Label labelTitulo;
        private PictureBox pictureBoxEmpleado;
        private Label labelCarnet;
        private Label labelSaldo;
        private TextBox textBoxSaldo;
    }
}