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
            panelToolbar = new Panel();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            dataGridUsuarios = new DataGridView();
            panelToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridUsuarios).BeginInit();
            SuspendLayout();
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = Color.WhiteSmoke;
            panelToolbar.Controls.Add(btnNuevo);
            panelToolbar.Controls.Add(btnEditar);
            panelToolbar.Controls.Add(btnEliminar);
            panelToolbar.Dock = DockStyle.Right;
            panelToolbar.Location = new Point(711, 0);
            panelToolbar.Margin = new Padding(0);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new Size(89, 600);
            panelToolbar.TabIndex = 0;
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = Color.ForestGreen;
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
            btnEditar.BackColor = Color.DeepSkyBlue;
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
            btnEliminar.BackColor = Color.Crimson;
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
            dataGridUsuarios.Location = new Point(0, 0);
            dataGridUsuarios.Name = "dataGridUsuarios";
            dataGridUsuarios.ReadOnly = true;
            dataGridUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridUsuarios.Size = new Size(711, 600);
            dataGridUsuarios.TabIndex = 1;
            // 
            // UserView
            // 
            Controls.Add(dataGridUsuarios);
            Controls.Add(panelToolbar);
            Name = "UserView";
            Size = new Size(800, 600);
            Load += UserView_Load;
            panelToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridUsuarios).EndInit();
            ResumeLayout(false);
        }
    }
}
