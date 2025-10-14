namespace Manga_Rica_P1.UI.CierreDiario
{
    partial class CierreDiario
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
            monthCalendarCierreDiario = new MonthCalendar();
            labelTitulo = new Label();
            checkBoxDomingo = new CheckBox();
            checkBoxFeriado = new CheckBox();
            buttonCierreDiario = new Button();
            SuspendLayout();
            // 
            // monthCalendarCierreDiario
            // 
            monthCalendarCierreDiario.Location = new Point(112, 63);
            monthCalendarCierreDiario.Name = "monthCalendarCierreDiario";
            monthCalendarCierreDiario.TabIndex = 0;
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 14F);
            labelTitulo.Location = new Point(134, 29);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(190, 25);
            labelTitulo.TabIndex = 1;
            labelTitulo.Text = "Selecciona una fecha";
            // 
            // checkBoxDomingo
            // 
            checkBoxDomingo.AutoSize = true;
            checkBoxDomingo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBoxDomingo.ForeColor = Color.Green;
            checkBoxDomingo.Location = new Point(247, 237);
            checkBoxDomingo.Name = "checkBoxDomingo";
            checkBoxDomingo.Size = new Size(77, 19);
            checkBoxDomingo.TabIndex = 2;
            checkBoxDomingo.Text = "Domingo";
            checkBoxDomingo.UseVisualStyleBackColor = true;
            // 
            // checkBoxFeriado
            // 
            checkBoxFeriado.AutoSize = true;
            checkBoxFeriado.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBoxFeriado.ForeColor = Color.Green;
            checkBoxFeriado.Location = new Point(134, 237);
            checkBoxFeriado.Name = "checkBoxFeriado";
            checkBoxFeriado.Size = new Size(67, 19);
            checkBoxFeriado.TabIndex = 3;
            checkBoxFeriado.Text = "Feriado";
            checkBoxFeriado.UseVisualStyleBackColor = true;
            // 
            // buttonCierreDiario
            // 
            buttonCierreDiario.FlatStyle = FlatStyle.Popup;
            buttonCierreDiario.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonCierreDiario.ForeColor = Color.Green;
            buttonCierreDiario.Location = new Point(169, 274);
            buttonCierreDiario.Name = "buttonCierreDiario";
            buttonCierreDiario.Size = new Size(108, 37);
            buttonCierreDiario.TabIndex = 4;
            buttonCierreDiario.Text = "Cerrar Dia";
            buttonCierreDiario.UseVisualStyleBackColor = true;
            // 
            // CierreDiario
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(459, 322);
            Controls.Add(buttonCierreDiario);
            Controls.Add(checkBoxFeriado);
            Controls.Add(checkBoxDomingo);
            Controls.Add(labelTitulo);
            Controls.Add(monthCalendarCierreDiario);
            Name = "CierreDiario";
            Text = "CierreDiario";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MonthCalendar monthCalendarCierreDiario;
        private Label labelTitulo;
        private CheckBox checkBoxDomingo;
        private CheckBox checkBoxFeriado;
        private Button buttonCierreDiario;
    }
}