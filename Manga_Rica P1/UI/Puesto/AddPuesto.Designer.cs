using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Puesto
{
    partial class AddPuesto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPuesto));
            panel1 = new Panel();
            btnCancelar = new Button();
            btnAgregar = new Button();
            textBoxPuesto = new TextBox();
            labelPuesto = new Label();
            textBoxDescripcion = new TextBox();
            labelDescripcion = new Label();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveBorder;
            panel1.Controls.Add(btnCancelar);
            panel1.Controls.Add(btnAgregar);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 270);
            panel1.Name = "panel1";
            panel1.Size = new Size(590, 52);
            panel1.TabIndex = 0;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Red;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(299, 7);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 35);
            btnCancelar.TabIndex = 2;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnAgregar
            // 
            btnAgregar.BackColor = Color.LimeGreen;
            btnAgregar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAgregar.ForeColor = SystemColors.ControlLightLight;
            btnAgregar.Location = new Point(189, 7);
            btnAgregar.Name = "btnAgregar";
            btnAgregar.Size = new Size(100, 35);
            btnAgregar.TabIndex = 1;
            btnAgregar.Text = "Agregar";
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.Click += btnAgregar_Click;
            // 
            // textBoxPuesto
            // 
            textBoxPuesto.Location = new Point(315, 62);
            textBoxPuesto.Name = "textBoxPuesto";
            textBoxPuesto.Size = new Size(197, 23);
            textBoxPuesto.TabIndex = 0;
            // 
            // labelPuesto
            // 
            labelPuesto.AutoSize = true;
            labelPuesto.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelPuesto.Location = new Point(315, 38);
            labelPuesto.Name = "labelPuesto";
            labelPuesto.Size = new Size(57, 21);
            labelPuesto.TabIndex = 10;
            labelPuesto.Text = "Puesto";
            // 
            // textBoxDescripcion
            // 
            textBoxDescripcion.Location = new Point(315, 133);
            textBoxDescripcion.Multiline = true;
            textBoxDescripcion.Name = "textBoxDescripcion";
            textBoxDescripcion.Size = new Size(197, 119);
            textBoxDescripcion.TabIndex = 1;
            // 
            // labelDescripcion
            // 
            labelDescripcion.AutoSize = true;
            labelDescripcion.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelDescripcion.Location = new Point(315, 109);
            labelDescripcion.Name = "labelDescripcion";
            labelDescripcion.Size = new Size(91, 21);
            labelDescripcion.TabIndex = 12;
            labelDescripcion.Text = "Descripción";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(25, 32);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(238, 220);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 14;
            pictureBox1.TabStop = false;
            // 
            // AddPuesto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(590, 322);
            Controls.Add(pictureBox1);
            Controls.Add(textBoxDescripcion);
            Controls.Add(labelDescripcion);
            Controls.Add(textBoxPuesto);
            Controls.Add(labelPuesto);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AddPuesto";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Agregar Puesto";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button btnCancelar;
        private Button btnAgregar;
        private TextBox textBoxPuesto;
        private Label labelPuesto;
        private TextBox textBoxDescripcion;
        private Label labelDescripcion;
        private PictureBox pictureBox1;
    }
}
