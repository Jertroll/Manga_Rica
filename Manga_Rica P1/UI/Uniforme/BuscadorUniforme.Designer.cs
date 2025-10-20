namespace Manga_Rica_P1.UI.Uniforme
{
    partial class BuscadorUniforme
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            buttonCancelar = new Button();
            buttonAceptar = new Button();
            buttonBuscar = new Button();
            textBox1 = new TextBox();
            dataGridView1 = new DataGridView();
            ColumnaCarnet = new DataGridViewTextBoxColumn();
            ColumnaNombre = new DataGridViewTextBoxColumn();
            ColumnaIdUniforme = new DataGridViewTextBoxColumn();
            ColumnaFecha = new DataGridViewTextBoxColumn();
            ColumnaTotal = new DataGridViewTextBoxColumn();
            ColumnaAnulada = new DataGridViewCheckBoxColumn();
            labelTitulo = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // buttonCancelar
            // 
            buttonCancelar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonCancelar.ForeColor = Color.Red;
            buttonCancelar.Location = new Point(420, 430);
            buttonCancelar.Name = "buttonCancelar";
            buttonCancelar.Size = new Size(124, 35);
            buttonCancelar.TabIndex = 14;
            buttonCancelar.Text = "Cancelar";
            buttonCancelar.UseVisualStyleBackColor = true;
            buttonCancelar.Click += buttonCancelar_Click;
            // 
            // buttonAceptar
            // 
            buttonAceptar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonAceptar.ForeColor = Color.Green;
            buttonAceptar.Location = new Point(256, 430);
            buttonAceptar.Name = "buttonAceptar";
            buttonAceptar.Size = new Size(124, 35);
            buttonAceptar.TabIndex = 13;
            buttonAceptar.Text = "Aceptar";
            buttonAceptar.UseVisualStyleBackColor = true;
            // 
            // buttonBuscar
            // 
            buttonBuscar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonBuscar.Location = new Point(250, 115);
            buttonBuscar.Name = "buttonBuscar";
            buttonBuscar.Size = new Size(80, 28);
            buttonBuscar.TabIndex = 12;
            buttonBuscar.Text = "Buscar";
            buttonBuscar.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Window;
            textBox1.Font = new Font("Segoe UI", 10F);
            textBox1.Location = new Point(20, 115);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(220, 25);
            textBox1.TabIndex = 11;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColumnaCarnet, ColumnaNombre, ColumnaIdUniforme, ColumnaFecha, ColumnaTotal, ColumnaAnulada });
            dataGridView1.Location = new Point(20, 159);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(760, 250);
            dataGridView1.TabIndex = 10;
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
            // ColumnaIdUniforme
            // 
            ColumnaIdUniforme.HeaderText = "Id Factura";
            ColumnaIdUniforme.Name = "ColumnaIdUniforme";
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
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.ForeColor = Color.FromArgb(0, 102, 204);
            labelTitulo.Location = new Point(240, 30);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(320, 30);
            labelTitulo.TabIndex = 9;
            labelTitulo.Text = "Buscador de Deducciones - Uniformes";
            // 
            // BuscadorUniforme
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
            Name = "BuscadorUniforme";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Buscador Uniforme";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonCancelar;
        private Button buttonAceptar;
        private Button buttonBuscar;
        private TextBox textBox1;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn ColumnaCarnet;
        private DataGridViewTextBoxColumn ColumnaNombre;
        private DataGridViewTextBoxColumn ColumnaIdUniforme;
        private DataGridViewTextBoxColumn ColumnaFecha;
        private DataGridViewTextBoxColumn ColumnaTotal;
        private DataGridViewCheckBoxColumn ColumnaAnulada;
        private Label labelTitulo;
    }
}