namespace Manga_Rica_P1.UI.Pagos
{
    partial class ActivarPagos
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
            labelSemana = new Label();
            buttonActivarPagos = new Button();
            comboSemana = new ComboBox();
            SuspendLayout();
            // 
            // labelSemana
            // 
            labelSemana.AutoSize = true;
            labelSemana.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSemana.ForeColor = Color.Green;
            labelSemana.Location = new Point(122, 49);
            labelSemana.Name = "labelSemana";
            labelSemana.Size = new Size(71, 21);
            labelSemana.TabIndex = 0;
            labelSemana.Text = "Semana";
            // 
            // buttonActivarPagos
            // 
            buttonActivarPagos.BackColor = Color.Transparent;
            buttonActivarPagos.FlatStyle = FlatStyle.Popup;
            buttonActivarPagos.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonActivarPagos.ForeColor = Color.Green;
            buttonActivarPagos.Location = new Point(93, 166);
            buttonActivarPagos.Name = "buttonActivarPagos";
            buttonActivarPagos.Size = new Size(121, 38);
            buttonActivarPagos.TabIndex = 1;
            buttonActivarPagos.Text = "Activar Pagos";
            buttonActivarPagos.UseVisualStyleBackColor = false;
            // 
            // comboSemana
            // 
            comboSemana.FormattingEnabled = true;
            comboSemana.Location = new Point(93, 110);
            comboSemana.Name = "comboSemana";
            comboSemana.Size = new Size(121, 23);
            comboSemana.TabIndex = 2;
            // 
            // ActivarPagos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(306, 260);
            Controls.Add(comboSemana);
            Controls.Add(buttonActivarPagos);
            Controls.Add(labelSemana);
            Name = "ActivarPagos";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Activar Pagos";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelSemana;
        private Button buttonActivarPagos;
        private ComboBox comboSemana;
    }
}