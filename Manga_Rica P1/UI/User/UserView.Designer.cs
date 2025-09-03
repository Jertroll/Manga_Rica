namespace Manga_Rica_P1.UI.User
{
    partial class UserView
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelToolbar;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private DataGridView dataGridUsuarios;
        private Panel panelHeader;
        private Label lblTitulo;

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
            panelHeader = new Panel();
            lblTitulo = new Label();
            panelToolbar = new Panel();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            dataGridUsuarios = new DataGridView();
            panelHeader.SuspendLayout();
            panelToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridUsuarios).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(240, 192, 47);
            panelHeader.BorderStyle = BorderStyle.Fixed3D;
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(800, 40);
            panelHeader.TabIndex = 2;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(194, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new Padding(10, 10, 0, 0);
            lblTitulo.Size = new Size(196, 40);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Listado de Usuarios";
            lblTitulo.TextAlign = ContentAlignment.TopCenter;
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = Color.WhiteSmoke;
            panelToolbar.Controls.Add(btnNuevo);
            panelToolbar.Controls.Add(btnEditar);
            panelToolbar.Controls.Add(btnEliminar);
            panelToolbar.Dock = DockStyle.Right;
            panelToolbar.Location = new Point(711, 40);
            panelToolbar.Margin = new Padding(0);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new Size(89, 560);
            panelToolbar.TabIndex = 0;
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = Color.FromArgb(230, 135, 45);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(6, 22);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(75, 30);
            btnNuevo.TabIndex = 0;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            // 
            // btnEditar
            // 
            btnEditar.BackColor = Color.FromArgb(124, 179, 66);
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(6, 73);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(75, 30);
            btnEditar.TabIndex = 1;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(211, 47, 47);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(6, 124);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(75, 30);
            btnEliminar.TabIndex = 2;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // dataGridUsuarios
            // 
            dataGridUsuarios.AllowUserToAddRows = false;
            dataGridUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridUsuarios.BackgroundColor = Color.White;
            dataGridUsuarios.Dock = DockStyle.Fill;
            dataGridUsuarios.Location = new Point(0, 40);
            dataGridUsuarios.Name = "dataGridUsuarios";
            dataGridUsuarios.ReadOnly = true;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(50, 130, 56);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(250, 251, 250);
            dataGridUsuarios.RowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridUsuarios.Size = new Size(711, 560);
            dataGridUsuarios.TabIndex = 1;
            // 
            // UserView
            // 
            Controls.Add(dataGridUsuarios);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);
            Name = "UserView";
            Size = new Size(800, 600);
            Load += UserView_Load;
            panelHeader.ResumeLayout(false);
            panelToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridUsuarios).EndInit();
            ResumeLayout(false);
        }
    }
}
