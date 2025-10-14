namespace Manga_Rica_P1.UI.Soda
{
    partial class Soda
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Soda));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            labelCarnet = new Label();
            pictureBoxEmpleado = new PictureBox();
            labelTitulo = new Label();
            textBoxCarnet = new TextBox();
            buttonBuscarEmpleado = new Button();
            labelNombreCompleto = new Label();
            textBoxNombre = new TextBox();
            textBoxCantidad = new TextBox();
            labelConcepto = new Label();
            comboBoxArticulos = new ComboBox();
            labelCantidad = new Label();
            labelSubTotal = new Label();
            textBoxSubTotal = new TextBox();
            buttonAgregar = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnBuscar = new Button();
            buttonAnular = new Button();
            buttonGuardar = new Button();
            dataGridView1 = new DataGridView();
            CantidadColumna = new DataGridViewTextBoxColumn();
            CodigoColumna = new DataGridViewTextBoxColumn();
            DescripcionColumna = new DataGridViewTextBoxColumn();
            PrecioColumna = new DataGridViewTextBoxColumn();
            TotalColumna = new DataGridViewTextBoxColumn();
            checkBoxAnulada = new CheckBox();
            labelNumFactura = new Label();
            textBoxNumFactura = new TextBox();
            comboBoxFecha = new ComboBox();
            labelTotal = new Label();
            textBoxTotal = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxEmpleado).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // labelCarnet
            // 
            labelCarnet.AutoSize = true;
            labelCarnet.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCarnet.ForeColor = Color.Green;
            labelCarnet.Location = new Point(161, 108);
            labelCarnet.Name = "labelCarnet";
            labelCarnet.Size = new Size(60, 21);
            labelCarnet.TabIndex = 0;
            labelCarnet.Text = "Carnet";
            // 
            // pictureBoxEmpleado
            // 
            pictureBoxEmpleado.BackColor = SystemColors.ActiveBorder;
            pictureBoxEmpleado.Location = new Point(122, 161);
            pictureBoxEmpleado.Name = "pictureBoxEmpleado";
            pictureBoxEmpleado.Size = new Size(143, 115);
            pictureBoxEmpleado.TabIndex = 1;
            pictureBoxEmpleado.TabStop = false;
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.ForeColor = Color.Green;
            labelTitulo.Location = new Point(292, 41);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(176, 30);
            labelTitulo.TabIndex = 2;
            labelTitulo.Text = "Modulo de Soda";
            // 
            // textBoxCarnet
            // 
            textBoxCarnet.BackColor = Color.DarkGray;
            textBoxCarnet.Location = new Point(122, 132);
            textBoxCarnet.Name = "textBoxCarnet";
            textBoxCarnet.Size = new Size(110, 23);
            textBoxCarnet.TabIndex = 3;
            textBoxCarnet.KeyDown += textBoxCarnet_KeyDown;
            // 
            // buttonBuscarEmpleado
            // 
            buttonBuscarEmpleado.BackColor = Color.FromArgb(0, 122, 204);
            buttonBuscarEmpleado.FlatAppearance.BorderSize = 0;
            buttonBuscarEmpleado.FlatStyle = FlatStyle.Flat;
            buttonBuscarEmpleado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            buttonBuscarEmpleado.ForeColor = Color.White;
            buttonBuscarEmpleado.Location = new Point(237, 132);
            buttonBuscarEmpleado.Name = "buttonBuscarEmpleado";
            buttonBuscarEmpleado.Size = new Size(28, 23);
            buttonBuscarEmpleado.TabIndex = 4;
            buttonBuscarEmpleado.Text = "🔍";
            buttonBuscarEmpleado.UseVisualStyleBackColor = false;
            buttonBuscarEmpleado.Click += buttonBuscarEmpleado_Click;
            // 
            // labelNombreCompleto
            // 
            labelNombreCompleto.AutoSize = true;
            labelNombreCompleto.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelNombreCompleto.ForeColor = Color.Green;
            labelNombreCompleto.Location = new Point(436, 111);
            labelNombreCompleto.Name = "labelNombreCompleto";
            labelNombreCompleto.Size = new Size(122, 17);
            labelNombreCompleto.TabIndex = 4;
            labelNombreCompleto.Text = "Nombre Completo";
            // 
            // textBoxNombre
            // 
            textBoxNombre.BackColor = SystemColors.ActiveBorder;
            textBoxNombre.Location = new Point(305, 131);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(385, 23);
            textBoxNombre.TabIndex = 5;
            // 
            // textBoxCantidad
            // 
            textBoxCantidad.BackColor = SystemColors.ActiveBorder;
            textBoxCantidad.Location = new Point(548, 190);
            textBoxCantidad.Name = "textBoxCantidad";
            textBoxCantidad.Size = new Size(142, 23);
            textBoxCantidad.TabIndex = 7;
            textBoxCantidad.TextChanged += textBoxCantidad_TextChanged;
            textBoxCantidad.KeyDown += textBoxCantidad_KeyDown;
            textBoxCantidad.KeyPress += textBoxCantidad_KeyPress;
            textBoxCantidad.Leave += textBoxCantidad_Leave;
            // 
            // labelConcepto
            // 
            labelConcepto.AutoSize = true;
            labelConcepto.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelConcepto.ForeColor = Color.Green;
            labelConcepto.Location = new Point(333, 170);
            labelConcepto.Name = "labelConcepto";
            labelConcepto.Size = new Size(116, 17);
            labelConcepto.TabIndex = 6;
            labelConcepto.Text = "Por Concepto De ";
            // 
            // comboBoxArticulos
            // 
            comboBoxArticulos.BackColor = SystemColors.ActiveBorder;
            comboBoxArticulos.FormattingEnabled = true;
            comboBoxArticulos.Location = new Point(305, 190);
            comboBoxArticulos.Name = "comboBoxArticulos";
            comboBoxArticulos.Size = new Size(171, 23);
            comboBoxArticulos.TabIndex = 8;
            comboBoxArticulos.SelectedIndexChanged += comboBoxArticulos_SelectedIndexChanged;
            comboBoxArticulos.KeyDown += comboBoxArticulos_KeyDown;
            // 
            // labelCantidad
            // 
            labelCantidad.AutoSize = true;
            labelCantidad.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCantidad.ForeColor = Color.Green;
            labelCantidad.Location = new Point(588, 170);
            labelCantidad.Name = "labelCantidad";
            labelCantidad.Size = new Size(63, 17);
            labelCantidad.TabIndex = 9;
            labelCantidad.Text = "Cantidad";
            // 
            // labelSubTotal
            // 
            labelSubTotal.AutoSize = true;
            labelSubTotal.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSubTotal.ForeColor = Color.Green;
            labelSubTotal.Location = new Point(359, 233);
            labelSubTotal.Name = "labelSubTotal";
            labelSubTotal.Size = new Size(66, 17);
            labelSubTotal.TabIndex = 11;
            labelSubTotal.Text = "Sub Total";
            // 
            // textBoxSubTotal
            // 
            textBoxSubTotal.BackColor = SystemColors.ActiveBorder;
            textBoxSubTotal.Location = new Point(305, 253);
            textBoxSubTotal.Name = "textBoxSubTotal";
            textBoxSubTotal.Size = new Size(171, 23);
            textBoxSubTotal.TabIndex = 10;
            // 
            // buttonAgregar
            // 
            buttonAgregar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonAgregar.Location = new Point(548, 241);
            buttonAgregar.Name = "buttonAgregar";
            buttonAgregar.Size = new Size(142, 35);
            buttonAgregar.TabIndex = 12;
            buttonAgregar.Text = "Agregar Producto";
            buttonAgregar.UseVisualStyleBackColor = true;
            buttonAgregar.Click += buttonAgregar_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.Green;
            flowLayoutPanel1.Controls.Add(btnBuscar);
            flowLayoutPanel1.Controls.Add(buttonGuardar);
            flowLayoutPanel1.Controls.Add(buttonAnular);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.ForeColor = Color.Lime;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(802, 30);
            flowLayoutPanel1.TabIndex = 13;
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
            buttonGuardar.Click += buttonGuardar_Click;
            // 
            // buttonAnular
            // 
            buttonAnular.BackColor = Color.IndianRed;
            buttonAnular.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            buttonAnular.ForeColor = Color.White;
            buttonAnular.Location = new Point(75, 3);
            buttonAnular.Name = "buttonAnular";
            buttonAnular.Size = new Size(70, 25);
            buttonAnular.TabIndex = 16;
            buttonAnular.Text = "ANULAR";
            buttonAnular.UseVisualStyleBackColor = false;
            buttonAnular.Click += buttonAnular_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { CantidadColumna, CodigoColumna, DescripcionColumna, PrecioColumna, TotalColumna });
            dataGridView1.Location = new Point(39, 299);
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
            dataGridView1.TabIndex = 14;
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
            // checkBoxAnulada
            // 
            checkBoxAnulada.AutoSize = true;
            checkBoxAnulada.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBoxAnulada.ForeColor = SystemColors.HotTrack;
            checkBoxAnulada.Location = new Point(598, 41);
            checkBoxAnulada.Name = "checkBoxAnulada";
            checkBoxAnulada.Size = new Size(78, 21);
            checkBoxAnulada.TabIndex = 15;
            checkBoxAnulada.Text = "Anulada";
            checkBoxAnulada.UseVisualStyleBackColor = true;
            // 
            // labelNumFactura
            // 
            labelNumFactura.AutoSize = true;
            labelNumFactura.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            labelNumFactura.ForeColor = Color.Red;
            labelNumFactura.Location = new Point(615, 69);
            labelNumFactura.Name = "labelNumFactura";
            labelNumFactura.Size = new Size(61, 20);
            labelNumFactura.TabIndex = 16;
            labelNumFactura.Text = "Factura";
            // 
            // textBoxNumFactura
            // 
            textBoxNumFactura.BackColor = SystemColors.ActiveBorder;
            textBoxNumFactura.Location = new Point(682, 69);
            textBoxNumFactura.Name = "textBoxNumFactura";
            textBoxNumFactura.Size = new Size(92, 23);
            textBoxNumFactura.TabIndex = 17;
            // 
            // comboBoxFecha
            // 
            comboBoxFecha.BackColor = SystemColors.ActiveBorder;
            comboBoxFecha.FormattingEnabled = true;
            comboBoxFecha.Location = new Point(682, 40);
            comboBoxFecha.Name = "comboBoxFecha";
            comboBoxFecha.Size = new Size(92, 23);
            comboBoxFecha.TabIndex = 18;
            // 
            // labelTotal
            // 
            labelTotal.AutoSize = true;
            labelTotal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelTotal.ForeColor = Color.Green;
            labelTotal.Location = new Point(597, 360);
            labelTotal.Name = "labelTotal";
            labelTotal.Size = new Size(48, 21);
            labelTotal.TabIndex = 20;
            labelTotal.Text = "Total";
            // 
            // textBoxTotal
            // 
            textBoxTotal.BackColor = SystemColors.ActiveBorder;
            textBoxTotal.Location = new Point(651, 358);
            textBoxTotal.Name = "textBoxTotal";
            textBoxTotal.Size = new Size(123, 23);
            textBoxTotal.TabIndex = 19;
            // 
            // Soda
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(802, 472);
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
            Controls.Add(buttonBuscarEmpleado);
            Controls.Add(textBoxCarnet);
            Controls.Add(labelTitulo);
            Controls.Add(pictureBoxEmpleado);
            // Reemplaza 'ClientSize' por 'this.ClientSize' y 'Controls.Add' por 'this.Controls.Add' en el método InitializeComponent

            // ...dentro de InitializeComponent()...

            // Cambia estas líneas:
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(802, 472);
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
            Name = "Soda";

            // Por estas:
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(802, 472);
            this.Controls.Add(labelTotal);
            this.Controls.Add(textBoxTotal);
            this.Controls.Add(comboBoxFecha);
            this.Controls.Add(textBoxNumFactura);
            this.Controls.Add(labelNumFactura);
            this.Controls.Add(checkBoxAnulada);
            this.Controls.Add(dataGridView1);
            this.Controls.Add(flowLayoutPanel1);
            this.Controls.Add(buttonAgregar);
            this.Controls.Add(labelSubTotal);
            this.Controls.Add(textBoxSubTotal);
            this.Controls.Add(labelCantidad);
            this.Controls.Add(comboBoxArticulos);
            this.Controls.Add(textBoxCantidad);
            this.Controls.Add(labelConcepto);
            this.Controls.Add(textBoxNombre);
            this.Controls.Add(labelNombreCompleto);
            this.Controls.Add(textBoxCarnet);
            this.Controls.Add(labelTitulo);
            this.Controls.Add(pictureBoxEmpleado);
            this.Controls.Add(labelCarnet);
            this.Name = "Soda";
            Controls.Add(labelCarnet);
            Name = "Soda";
            ((System.ComponentModel.ISupportInitialize)pictureBoxEmpleado).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelCarnet;
        private PictureBox pictureBoxEmpleado;
        private Label labelTitulo;
        private TextBox textBoxCarnet;
        private Button buttonBuscarEmpleado;
        private Label labelNombreCompleto;
        private TextBox textBoxNombre;
        private TextBox textBoxCantidad;
        private Label labelConcepto;
        private ComboBox comboBoxArticulos;
        private Label labelCantidad;
        private Label labelSubTotal;
        private TextBox textBoxSubTotal;
        private Button buttonAgregar;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnBuscar;
        private Button buttonAnular;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn CantidadColumna;
        private DataGridViewTextBoxColumn CodigoColumna;
        private DataGridViewTextBoxColumn DescripcionColumna;
        private DataGridViewTextBoxColumn PrecioColumna;
        private DataGridViewTextBoxColumn TotalColumna;
        private Button buttonGuardar;
        private CheckBox checkBoxAnulada;
        private Label labelNumFactura;
        private TextBox textBoxNumFactura;
        private ComboBox comboBoxFecha;
        private Label labelTotal;
        private TextBox textBoxTotal;
    }
}
