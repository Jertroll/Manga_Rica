namespace Manga_Rica_P1.UI.Soda
{
    partial class BuscadorSoda
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            labelTitulo = new Label();
            dataGridView1 = new DataGridView();
            ColumnaCarnet = new DataGridViewTextBoxColumn();
            ColumnaNombre = new DataGridViewTextBoxColumn();
            ColumnaIdSoda = new DataGridViewTextBoxColumn();
            ColumnaFecha = new DataGridViewTextBoxColumn();
            textBox1 = new TextBox();
            buttonBuscar = new Button();
            buttonAceptar = new Button();
            buttonCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.ForeColor = Color.Green;
            labelTitulo.Location = new Point(154, 25);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(188, 30);
            labelTitulo.TabIndex = 3;
            labelTitulo.Text = "Buscador de Soda";
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColumnaCarnet, ColumnaNombre, ColumnaIdSoda, ColumnaFecha });
            dataGridView1.Location = new Point(28, 123);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(438, 150);
            dataGridView1.TabIndex = 4;
            // 
            // ColumnaCarnet
            // 
            ColumnaCarnet.HeaderText = "Carne";
            ColumnaCarnet.Name = "ColumnaCarnet";
            // 
            // ColumnaNombre
            // 
            ColumnaNombre.HeaderText = "Nombre";
            ColumnaNombre.Name = "ColumnaNombre";
            // 
            // ColumnaIdSoda
            // 
            ColumnaIdSoda.HeaderText = "Id Factura";
            ColumnaIdSoda.Name = "ColumnaIdSoda";
            // 
            // ColumnaFecha
            // 
            ColumnaFecha.HeaderText = "Fecha";
            ColumnaFecha.Name = "ColumnaFecha";
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.ActiveBorder;
            textBox1.Location = new Point(28, 93);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(110, 23);
            textBox1.TabIndex = 5;
            // 
            // buttonBuscar
            // 
            buttonBuscar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonBuscar.Location = new Point(144, 93);
            buttonBuscar.Name = "buttonBuscar";
            buttonBuscar.Size = new Size(63, 24);
            buttonBuscar.TabIndex = 6;
            buttonBuscar.Text = "Buscar";
            buttonBuscar.UseVisualStyleBackColor = true;
            // 
            // buttonAceptar
            // 
            buttonAceptar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonAceptar.ForeColor = Color.Green;
            buttonAceptar.Location = new Point(104, 325);
            buttonAceptar.Name = "buttonAceptar";
            buttonAceptar.Size = new Size(124, 35);
            buttonAceptar.TabIndex = 7;
            buttonAceptar.Text = "Aceptar";
            buttonAceptar.UseVisualStyleBackColor = true;
            // 
            // buttonCancelar
            // 
            buttonCancelar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonCancelar.ForeColor = Color.Red;
            buttonCancelar.Location = new Point(258, 325);
            buttonCancelar.Name = "buttonCancelar";
            buttonCancelar.Size = new Size(124, 35);
            buttonCancelar.TabIndex = 8;
            buttonCancelar.Text = "Cancelar";
            buttonCancelar.UseVisualStyleBackColor = true;
            buttonCancelar.Click += buttonCancelar_Click;
            // 
            // BuscadorSoda
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(499, 383);
            Controls.Add(buttonCancelar);
            Controls.Add(buttonAceptar);
            Controls.Add(buttonBuscar);
            Controls.Add(textBox1);
            Controls.Add(dataGridView1);
            Controls.Add(labelTitulo);
            Name = "BuscadorSoda";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Buscador de Soda";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitulo;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn ColumnaCarnet;
        private DataGridViewTextBoxColumn ColumnaNombre;
        private DataGridViewTextBoxColumn ColumnaIdSoda;
        private DataGridViewTextBoxColumn ColumnaFecha;
        private TextBox textBox1;
        private Button buttonBuscar;
        private Button buttonAceptar;
        private Button buttonCancelar;
    }
}