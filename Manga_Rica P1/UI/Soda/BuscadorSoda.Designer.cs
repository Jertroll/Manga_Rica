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
            ColumnaTotal = new DataGridViewTextBoxColumn();
            ColumnaAnulada = new DataGridViewCheckBoxColumn();
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
            labelTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.ForeColor = Color.FromArgb(34, 139, 34);
            labelTitulo.Location = new Point(260, 30);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(280, 30);
            labelTitulo.TabIndex = 3;
            labelTitulo.Text = "Buscador de Deducciones - Soda";
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
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColumnaCarnet, ColumnaNombre, ColumnaIdSoda, ColumnaFecha, ColumnaTotal, ColumnaAnulada });
            dataGridView1.Location = new Point(20, 159);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(760, 250);
            dataGridView1.TabIndex = 4;
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.AllowUserToResizeRows = true;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
            // ColumnaTotal
            // 
            ColumnaTotal.HeaderText = "Total";
            ColumnaTotal.Name = "ColumnaTotal";
            // 
            // ColumnaAnulada
            // 
            ColumnaAnulada.HeaderText = "Anulada";
            ColumnaAnulada.Name = "ColumnaAnulada";
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Window;
            textBox1.Font = new Font("Segoe UI", 10F);
            textBox1.Location = new Point(20, 115);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(220, 25);
            textBox1.TabIndex = 5;
            // 
            // buttonBuscar
            // 
            buttonBuscar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonBuscar.Location = new Point(250, 115);
            buttonBuscar.Name = "buttonBuscar";
            buttonBuscar.Size = new Size(80, 28);
            buttonBuscar.TabIndex = 6;
            buttonBuscar.Text = "Buscar";
            buttonBuscar.UseVisualStyleBackColor = true;
            // 
            // buttonAceptar
            // 
            buttonAceptar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonAceptar.ForeColor = Color.Green;
            buttonAceptar.Location = new Point(256, 430);
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
            buttonCancelar.Location = new Point(420, 430);
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
            ClientSize = new Size(800, 490);
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
        private DataGridViewTextBoxColumn ColumnaTotal;
        private DataGridViewCheckBoxColumn ColumnaAnulada;
        private TextBox textBox1;
        private Button buttonBuscar;
        private Button buttonAceptar;
        private Button buttonCancelar;
    }
}