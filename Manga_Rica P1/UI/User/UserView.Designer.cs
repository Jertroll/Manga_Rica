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

        private Panel panelPaginador;
        private Button btnFirst;
        private Button btnPrev;
        private Button btnNext;
        private Button btnLast;
        private Label lblPageInfo;
        private Label lblTam;
        private ComboBox cboPageSize;

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
            panelHeader = new Panel();
            lblTitulo = new Label();
            panelToolbar = new Panel();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            dataGridUsuarios = new DataGridView();
            panelPaginador = new Panel();
            btnFirst = new Button();
            btnPrev = new Button();
            btnNext = new Button();
            btnLast = new Button();
            lblPageInfo = new Label();
            lblTam = new Label();
            cboPageSize = new ComboBox();
            panelHeader.SuspendLayout();
            panelToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridUsuarios).BeginInit();
            panelPaginador.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(230, 135, 45);
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
            lblTitulo.ForeColor = Color.White;
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
            panelToolbar.BorderStyle = BorderStyle.FixedSingle;
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
            dataGridUsuarios.Location = new Point(0, 40);
            dataGridUsuarios.Name = "dataGridUsuarios";
            dataGridUsuarios.ReadOnly = true;
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(224, 224, 224);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridUsuarios.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dataGridUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridUsuarios.Size = new Size(711, 520);
            dataGridUsuarios.TabIndex = 1;
            dataGridUsuarios.CellContentClick += dataGridUsuarios_CellContentClick;
            // 
            // panelPaginador
            // 
            panelPaginador.BackColor = Color.WhiteSmoke;
            panelPaginador.BorderStyle = BorderStyle.FixedSingle;
            panelPaginador.Controls.Add(btnFirst);
            panelPaginador.Controls.Add(btnPrev);
            panelPaginador.Controls.Add(btnNext);
            panelPaginador.Controls.Add(btnLast);
            panelPaginador.Controls.Add(lblPageInfo);
            panelPaginador.Controls.Add(lblTam);
            panelPaginador.Controls.Add(cboPageSize);
            panelPaginador.Dock = DockStyle.Bottom;
            panelPaginador.Location = new Point(0, 560);
            panelPaginador.Name = "panelPaginador";
            panelPaginador.Padding = new Padding(8, 5, 8, 5);
            panelPaginador.Size = new Size(711, 40);
            panelPaginador.TabIndex = 2;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(10, 6);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(40, 28);
            btnFirst.TabIndex = 0;
            btnFirst.Text = "⏮";
            btnFirst.Click += btnFirst_Click;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(55, 6);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(40, 28);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "◀";
            btnPrev.Click += btnPrev_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(100, 6);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(40, 28);
            btnNext.TabIndex = 2;
            btnNext.Text = "▶";
            btnNext.Click += btnNext_Click;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(145, 6);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(40, 28);
            btnLast.TabIndex = 3;
            btnLast.Text = "⏭";
            btnLast.Click += btnLast_Click;
            // 
            // lblPageInfo
            // 
            lblPageInfo.AutoSize = true;
            lblPageInfo.Location = new Point(200, 11);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(38, 15);
            lblPageInfo.TabIndex = 4;
            lblPageInfo.Text = "1 de 1";
            // 
            // lblTam
            // 
            lblTam.AutoSize = true;
            lblTam.Location = new Point(290, 11);
            lblTam.Name = "lblTam";
            lblTam.Size = new Size(53, 15);
            lblTam.TabIndex = 5;
            lblTam.Text = "Tamaño:";
            // 
            // cboPageSize
            // 
            cboPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPageSize.Location = new Point(350, 8);
            cboPageSize.Name = "cboPageSize";
            cboPageSize.Size = new Size(60, 23);
            cboPageSize.TabIndex = 6;
            cboPageSize.SelectionChangeCommitted += cboPageSize_SelectionChangeCommitted;
            // 
            // UserView
            // 
            Controls.Add(dataGridUsuarios);
            Controls.Add(panelPaginador);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);
            Name = "UserView";
            Size = new Size(800, 600);
            Load += UserView_Load;
            panelHeader.ResumeLayout(false);
            panelToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridUsuarios).EndInit();
            panelPaginador.ResumeLayout(false);
            panelPaginador.PerformLayout();
            ResumeLayout(false);
        }
    }
}
