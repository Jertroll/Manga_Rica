namespace Manga_Rica_P1.UI.User
{
    partial class UserView
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dataGridUsuarios;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridUsuarios = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridUsuarios).BeginInit();
            SuspendLayout();
            // 
            // dataGridUsuarios
            // 
            dataGridUsuarios.AllowUserToAddRows = false;
            dataGridUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridUsuarios.BackgroundColor = Color.White;
            dataGridUsuarios.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(50, 130, 56);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(50, 130, 56);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridUsuarios.Dock = DockStyle.Fill;
            dataGridUsuarios.EnableHeadersVisualStyles = false;
            dataGridUsuarios.Location = new Point(0, 0);
            dataGridUsuarios.Name = "dataGridUsuarios";
            dataGridUsuarios.ReadOnly = true;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(224, 224, 224);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridUsuarios.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dataGridUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridUsuarios.Size = new Size(800, 600);
            dataGridUsuarios.TabIndex = 1;
            // 
            // UserView
            // 
            Controls.Add(dataGridUsuarios);
            Name = "UserView";
            Size = new Size(800, 600);
            ((System.ComponentModel.ISupportInitialize)dataGridUsuarios).EndInit();
            ResumeLayout(false);
        }
    }
}
