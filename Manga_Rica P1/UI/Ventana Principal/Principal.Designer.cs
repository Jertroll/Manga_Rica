
namespace Manga_Rica_P1.UI.Ventana_Principal
{
    partial class Principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            panel1 = new Panel();
            LabelTitulo = new Label();
            pictureBox1 = new PictureBox();
            flowLayoutPanelSideBar = new FlowLayoutPanel();
            menuConfContenedor = new FlowLayoutPanel();
            panel2 = new Panel();
            btnConfiguraciones = new Button();
            panel7 = new Panel();
            btnUsuarios = new Button();
            panel11 = new Panel();
            btnDepartamentos = new Button();
            panel10 = new Panel();
            btnSemanas = new Button();
            panel9 = new Panel();
            btnArticulos = new Button();
            menuPlanillaContenedor = new FlowLayoutPanel();
            panel12 = new Panel();
            btnPlanilla = new Button();
            panel13 = new Panel();
            btnEmpleado = new Button();
            panel14 = new Panel();
            btnEntrada = new Button();
            panel15 = new Panel();
            btnSalidas = new Button();
            panel16 = new Panel();
            btnCierreDiario = new Button();
            panel6 = new Panel();
            btnSolicitudesPlanilla = new Button();
            menuDeduccionesContenedor = new FlowLayoutPanel();
            panel3 = new Panel();
            btnDeducciones = new Button();
            panel17 = new Panel();
            btnSoda = new Button();
            panel18 = new Panel();
            btnUniforme = new Button();
            panel19 = new Panel();
            btnReporteEmpleadoDeducciones = new Button();
            panel20 = new Panel();
            btnReporteGneralDeducciones = new Button();
            menuPagosContenedor = new FlowLayoutPanel();
            panel4 = new Panel();
            btnPagosPrincipal = new Button();
            panel5 = new Panel();
            btnActivarPagos = new Button();
            panel21 = new Panel();
            btnPagosSubmenu = new Button();
            menuReportesContenedor = new FlowLayoutPanel();
            panel24 = new Panel();
            btnReporte = new Button();
            panel25 = new Panel();
            btnEmpleadosReporte = new Button();
            panel26 = new Panel();
            btnPlanillaReportes = new Button();
            panel27 = new Panel();
            btnSodaReportes = new Button();
            botonSalirContenedor = new Panel();
            btnSalir = new Button();
            panelPrincipal = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            flowLayoutPanelSideBar.SuspendLayout();
            menuConfContenedor.SuspendLayout();
            panel2.SuspendLayout();
            panel7.SuspendLayout();
            panel11.SuspendLayout();
            panel10.SuspendLayout();
            panel9.SuspendLayout();
            menuPlanillaContenedor.SuspendLayout();
            panel12.SuspendLayout();
            panel13.SuspendLayout();
            panel14.SuspendLayout();
            panel15.SuspendLayout();
            panel16.SuspendLayout();
            panel6.SuspendLayout();
            menuDeduccionesContenedor.SuspendLayout();
            panel3.SuspendLayout();
            panel17.SuspendLayout();
            panel18.SuspendLayout();
            panel19.SuspendLayout();
            panel20.SuspendLayout();
            menuPagosContenedor.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel21.SuspendLayout();
            menuReportesContenedor.SuspendLayout();
            panel24.SuspendLayout();
            panel25.SuspendLayout();
            panel26.SuspendLayout();
            panel27.SuspendLayout();
            botonSalirContenedor.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(LabelTitulo);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(922, 37);
            panel1.TabIndex = 0;
            // 
            // LabelTitulo
            // 
            LabelTitulo.AutoSize = true;
            LabelTitulo.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LabelTitulo.Location = new Point(61, 9);
            LabelTitulo.Name = "LabelTitulo";
            LabelTitulo.Size = new Size(116, 20);
            LabelTitulo.TabIndex = 1;
            LabelTitulo.Text = "Manga Rica S.A ";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(43, 32);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // flowLayoutPanelSideBar
            // 
            flowLayoutPanelSideBar.BackColor = Color.FromArgb(0, 221, 0);
            flowLayoutPanelSideBar.Controls.Add(menuConfContenedor);
            flowLayoutPanelSideBar.Controls.Add(menuPlanillaContenedor);
            flowLayoutPanelSideBar.Controls.Add(menuDeduccionesContenedor);
            flowLayoutPanelSideBar.Controls.Add(menuPagosContenedor);
            flowLayoutPanelSideBar.Controls.Add(menuReportesContenedor);
            flowLayoutPanelSideBar.Controls.Add(botonSalirContenedor);
            flowLayoutPanelSideBar.Dock = DockStyle.Left;
            flowLayoutPanelSideBar.ForeColor = SystemColors.ControlText;
            flowLayoutPanelSideBar.Location = new Point(0, 37);
            flowLayoutPanelSideBar.Name = "flowLayoutPanelSideBar";
            flowLayoutPanelSideBar.Padding = new Padding(0, 30, 0, 0);
            flowLayoutPanelSideBar.Size = new Size(198, 473);
            flowLayoutPanelSideBar.TabIndex = 1;
            // 
            // menuConfContenedor
            // 
            menuConfContenedor.BackColor = Color.White;
            menuConfContenedor.Controls.Add(panel2);
            menuConfContenedor.Controls.Add(panel7);
            menuConfContenedor.Controls.Add(panel11);
            menuConfContenedor.Controls.Add(panel10);
            menuConfContenedor.Controls.Add(panel9);
            menuConfContenedor.ForeColor = Color.White;
            menuConfContenedor.Location = new Point(0, 30);
            menuConfContenedor.Margin = new Padding(0);
            menuConfContenedor.Name = "menuConfContenedor";
            menuConfContenedor.Size = new Size(202, 46);
            menuConfContenedor.TabIndex = 8;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnConfiguraciones);
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(202, 46);
            panel2.TabIndex = 3;
            // 
            // btnConfiguraciones
            // 
            btnConfiguraciones.BackColor = Color.FromArgb(0, 221, 0);
            btnConfiguraciones.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnConfiguraciones.ForeColor = Color.White;
            btnConfiguraciones.Image = (Image)resources.GetObject("btnConfiguraciones.Image");
            btnConfiguraciones.ImageAlign = ContentAlignment.MiddleLeft;
            btnConfiguraciones.Location = new Point(-29, -23);
            btnConfiguraciones.Name = "btnConfiguraciones";
            btnConfiguraciones.Padding = new Padding(40, 0, 0, 0);
            btnConfiguraciones.Size = new Size(231, 89);
            btnConfiguraciones.TabIndex = 2;
            btnConfiguraciones.Text = "       Configuraciones";
            btnConfiguraciones.TextAlign = ContentAlignment.MiddleLeft;
            btnConfiguraciones.UseVisualStyleBackColor = false;
            btnConfiguraciones.Click += btnConfiguraciones_Click;
            // 
            // panel7
            // 
            panel7.Controls.Add(btnUsuarios);
            panel7.Location = new Point(0, 46);
            panel7.Margin = new Padding(0);
            panel7.Name = "panel7";
            panel7.Size = new Size(202, 46);
            panel7.TabIndex = 9;
            // 
            // btnUsuarios
            // 
            btnUsuarios.BackColor = Color.FromArgb(224, 224, 224);
            btnUsuarios.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUsuarios.ForeColor = Color.Black;
            btnUsuarios.Image = (Image)resources.GetObject("btnUsuarios.Image");
            btnUsuarios.ImageAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.Location = new Point(-32, -20);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Padding = new Padding(40, 0, 0, 0);
            btnUsuarios.Size = new Size(273, 89);
            btnUsuarios.TabIndex = 2;
            btnUsuarios.Text = "       Usuarios";
            btnUsuarios.TextAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.UseVisualStyleBackColor = false;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // panel11
            // 
            panel11.Controls.Add(btnDepartamentos);
            panel11.Location = new Point(0, 92);
            panel11.Margin = new Padding(0);
            panel11.Name = "panel11";
            panel11.Size = new Size(202, 46);
            panel11.TabIndex = 13;
            // 
            // btnDepartamentos
            // 
            btnDepartamentos.BackColor = Color.FromArgb(224, 224, 224);
            btnDepartamentos.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDepartamentos.ForeColor = Color.Black;
            btnDepartamentos.Image = (Image)resources.GetObject("btnDepartamentos.Image");
            btnDepartamentos.ImageAlign = ContentAlignment.MiddleLeft;
            btnDepartamentos.Location = new Point(-32, -20);
            btnDepartamentos.Name = "btnDepartamentos";
            btnDepartamentos.Padding = new Padding(40, 0, 0, 0);
            btnDepartamentos.Size = new Size(273, 89);
            btnDepartamentos.TabIndex = 2;
            btnDepartamentos.Text = "       Departamentos";
            btnDepartamentos.TextAlign = ContentAlignment.MiddleLeft;
            btnDepartamentos.UseVisualStyleBackColor = false;
            // 
            // panel10
            // 
            panel10.Controls.Add(btnSemanas);
            panel10.Location = new Point(0, 138);
            panel10.Margin = new Padding(0);
            panel10.Name = "panel10";
            panel10.Size = new Size(202, 46);
            panel10.TabIndex = 12;
            // 
            // btnSemanas
            // 
            btnSemanas.BackColor = Color.FromArgb(224, 224, 224);
            btnSemanas.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSemanas.ForeColor = Color.Black;
            btnSemanas.Image = (Image)resources.GetObject("btnSemanas.Image");
            btnSemanas.ImageAlign = ContentAlignment.MiddleLeft;
            btnSemanas.Location = new Point(-32, -20);
            btnSemanas.Name = "btnSemanas";
            btnSemanas.Padding = new Padding(40, 0, 0, 0);
            btnSemanas.Size = new Size(273, 89);
            btnSemanas.TabIndex = 2;
            btnSemanas.Text = "       Semanas";
            btnSemanas.TextAlign = ContentAlignment.MiddleLeft;
            btnSemanas.UseVisualStyleBackColor = false;
            // 
            // panel9
            // 
            panel9.Controls.Add(btnArticulos);
            panel9.Location = new Point(0, 184);
            panel9.Margin = new Padding(0);
            panel9.Name = "panel9";
            panel9.Size = new Size(202, 46);
            panel9.TabIndex = 11;
            // 
            // btnArticulos
            // 
            btnArticulos.BackColor = Color.FromArgb(224, 224, 224);
            btnArticulos.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnArticulos.ForeColor = Color.Black;
            btnArticulos.Image = (Image)resources.GetObject("btnArticulos.Image");
            btnArticulos.ImageAlign = ContentAlignment.MiddleLeft;
            btnArticulos.Location = new Point(-32, -20);
            btnArticulos.Name = "btnArticulos";
            btnArticulos.Padding = new Padding(40, 0, 0, 0);
            btnArticulos.Size = new Size(273, 89);
            btnArticulos.TabIndex = 2;
            btnArticulos.Text = "       Articulos";
            btnArticulos.TextAlign = ContentAlignment.MiddleLeft;
            btnArticulos.UseVisualStyleBackColor = false;
            // 
            // menuPlanillaContenedor
            // 
            menuPlanillaContenedor.BackColor = Color.White;
            menuPlanillaContenedor.Controls.Add(panel12);
            menuPlanillaContenedor.Controls.Add(panel13);
            menuPlanillaContenedor.Controls.Add(panel14);
            menuPlanillaContenedor.Controls.Add(panel15);
            menuPlanillaContenedor.Controls.Add(panel16);
            menuPlanillaContenedor.Controls.Add(panel6);
            menuPlanillaContenedor.ForeColor = Color.White;
            menuPlanillaContenedor.Location = new Point(0, 76);
            menuPlanillaContenedor.Margin = new Padding(0);
            menuPlanillaContenedor.Name = "menuPlanillaContenedor";
            menuPlanillaContenedor.Size = new Size(202, 46);
            menuPlanillaContenedor.TabIndex = 11;
            // 
            // panel12
            // 
            panel12.Controls.Add(btnPlanilla);
            panel12.Location = new Point(0, 0);
            panel12.Margin = new Padding(0);
            panel12.Name = "panel12";
            panel12.Size = new Size(202, 46);
            panel12.TabIndex = 3;
            // 
            // btnPlanilla
            // 
            btnPlanilla.BackColor = Color.FromArgb(0, 221, 0);
            btnPlanilla.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPlanilla.ForeColor = Color.White;
            btnPlanilla.Image = (Image)resources.GetObject("btnPlanilla.Image");
            btnPlanilla.ImageAlign = ContentAlignment.MiddleLeft;
            btnPlanilla.Location = new Point(-29, -20);
            btnPlanilla.Name = "btnPlanilla";
            btnPlanilla.Padding = new Padding(40, 0, 0, 0);
            btnPlanilla.Size = new Size(231, 89);
            btnPlanilla.TabIndex = 2;
            btnPlanilla.Text = "       Planilla";
            btnPlanilla.TextAlign = ContentAlignment.MiddleLeft;
            btnPlanilla.UseVisualStyleBackColor = false;
            // 
            // panel13
            // 
            panel13.Controls.Add(btnEmpleado);
            panel13.Location = new Point(0, 46);
            panel13.Margin = new Padding(0);
            panel13.Name = "panel13";
            panel13.Size = new Size(202, 46);
            panel13.TabIndex = 9;
            // 
            // btnEmpleado
            // 
            btnEmpleado.BackColor = Color.FromArgb(224, 224, 224);
            btnEmpleado.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnEmpleado.ForeColor = Color.Black;
            btnEmpleado.Image = (Image)resources.GetObject("btnEmpleado.Image");
            btnEmpleado.ImageAlign = ContentAlignment.MiddleLeft;
            btnEmpleado.Location = new Point(-32, -20);
            btnEmpleado.Name = "btnEmpleado";
            btnEmpleado.Padding = new Padding(40, 0, 0, 0);
            btnEmpleado.Size = new Size(273, 89);
            btnEmpleado.TabIndex = 2;
            btnEmpleado.Text = "       Empleados";
            btnEmpleado.TextAlign = ContentAlignment.MiddleLeft;
            btnEmpleado.UseVisualStyleBackColor = false;
            // 
            // panel14
            // 
            panel14.Controls.Add(btnEntrada);
            panel14.Location = new Point(0, 92);
            panel14.Margin = new Padding(0);
            panel14.Name = "panel14";
            panel14.Size = new Size(202, 46);
            panel14.TabIndex = 13;
            // 
            // btnEntrada
            // 
            btnEntrada.BackColor = Color.FromArgb(224, 224, 224);
            btnEntrada.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnEntrada.ForeColor = Color.Black;
            btnEntrada.Image = (Image)resources.GetObject("btnEntrada.Image");
            btnEntrada.ImageAlign = ContentAlignment.MiddleLeft;
            btnEntrada.Location = new Point(-32, -20);
            btnEntrada.Name = "btnEntrada";
            btnEntrada.Padding = new Padding(40, 0, 0, 0);
            btnEntrada.Size = new Size(273, 89);
            btnEntrada.TabIndex = 2;
            btnEntrada.Text = "       Entradas";
            btnEntrada.TextAlign = ContentAlignment.MiddleLeft;
            btnEntrada.UseVisualStyleBackColor = false;
            // 
            // panel15
            // 
            panel15.Controls.Add(btnSalidas);
            panel15.Location = new Point(0, 138);
            panel15.Margin = new Padding(0);
            panel15.Name = "panel15";
            panel15.Size = new Size(202, 46);
            panel15.TabIndex = 12;
            // 
            // btnSalidas
            // 
            btnSalidas.BackColor = Color.FromArgb(224, 224, 224);
            btnSalidas.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSalidas.ForeColor = Color.Black;
            btnSalidas.Image = (Image)resources.GetObject("btnSalidas.Image");
            btnSalidas.ImageAlign = ContentAlignment.MiddleLeft;
            btnSalidas.Location = new Point(-32, -20);
            btnSalidas.Name = "btnSalidas";
            btnSalidas.Padding = new Padding(40, 0, 0, 0);
            btnSalidas.Size = new Size(273, 89);
            btnSalidas.TabIndex = 2;
            btnSalidas.Text = "       Salidas";
            btnSalidas.TextAlign = ContentAlignment.MiddleLeft;
            btnSalidas.UseVisualStyleBackColor = false;
            // 
            // panel16
            // 
            panel16.Controls.Add(btnCierreDiario);
            panel16.Location = new Point(0, 184);
            panel16.Margin = new Padding(0);
            panel16.Name = "panel16";
            panel16.Size = new Size(202, 46);
            panel16.TabIndex = 11;
            // 
            // btnCierreDiario
            // 
            btnCierreDiario.BackColor = Color.FromArgb(224, 224, 224);
            btnCierreDiario.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCierreDiario.ForeColor = Color.Black;
            btnCierreDiario.Image = (Image)resources.GetObject("btnCierreDiario.Image");
            btnCierreDiario.ImageAlign = ContentAlignment.MiddleLeft;
            btnCierreDiario.Location = new Point(-32, -20);
            btnCierreDiario.Name = "btnCierreDiario";
            btnCierreDiario.Padding = new Padding(40, 0, 0, 0);
            btnCierreDiario.Size = new Size(273, 89);
            btnCierreDiario.TabIndex = 2;
            btnCierreDiario.Text = "       Cierre Diario";
            btnCierreDiario.TextAlign = ContentAlignment.MiddleLeft;
            btnCierreDiario.UseVisualStyleBackColor = false;
            // 
            // panel6
            // 
            panel6.Controls.Add(btnSolicitudesPlanilla);
            panel6.Location = new Point(0, 230);
            panel6.Margin = new Padding(0);
            panel6.Name = "panel6";
            panel6.Size = new Size(202, 46);
            panel6.TabIndex = 15;
            // 
            // btnSolicitudesPlanilla
            // 
            btnSolicitudesPlanilla.BackColor = Color.FromArgb(224, 224, 224);
            btnSolicitudesPlanilla.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSolicitudesPlanilla.ForeColor = Color.Black;
            btnSolicitudesPlanilla.Image = (Image)resources.GetObject("btnSolicitudesPlanilla.Image");
            btnSolicitudesPlanilla.ImageAlign = ContentAlignment.MiddleLeft;
            btnSolicitudesPlanilla.Location = new Point(-32, -20);
            btnSolicitudesPlanilla.Name = "btnSolicitudesPlanilla";
            btnSolicitudesPlanilla.Padding = new Padding(40, 0, 0, 0);
            btnSolicitudesPlanilla.Size = new Size(273, 89);
            btnSolicitudesPlanilla.TabIndex = 2;
            btnSolicitudesPlanilla.Text = "       Solicitudes";
            btnSolicitudesPlanilla.TextAlign = ContentAlignment.MiddleLeft;
            btnSolicitudesPlanilla.UseVisualStyleBackColor = false;
            // 
            // menuDeduccionesContenedor
            // 
            menuDeduccionesContenedor.BackColor = Color.White;
            menuDeduccionesContenedor.Controls.Add(panel3);
            menuDeduccionesContenedor.Controls.Add(panel17);
            menuDeduccionesContenedor.Controls.Add(panel18);
            menuDeduccionesContenedor.Controls.Add(panel19);
            menuDeduccionesContenedor.Controls.Add(panel20);
            menuDeduccionesContenedor.ForeColor = Color.White;
            menuDeduccionesContenedor.Location = new Point(0, 122);
            menuDeduccionesContenedor.Margin = new Padding(0);
            menuDeduccionesContenedor.Name = "menuDeduccionesContenedor";
            menuDeduccionesContenedor.Size = new Size(202, 46);
            menuDeduccionesContenedor.TabIndex = 12;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnDeducciones);
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(202, 46);
            panel3.TabIndex = 3;
            // 
            // btnDeducciones
            // 
            btnDeducciones.BackColor = Color.FromArgb(0, 221, 0);
            btnDeducciones.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDeducciones.ForeColor = Color.White;
            btnDeducciones.Image = (Image)resources.GetObject("btnDeducciones.Image");
            btnDeducciones.ImageAlign = ContentAlignment.MiddleLeft;
            btnDeducciones.Location = new Point(-29, -20);
            btnDeducciones.Name = "btnDeducciones";
            btnDeducciones.Padding = new Padding(40, 0, 0, 0);
            btnDeducciones.Size = new Size(231, 89);
            btnDeducciones.TabIndex = 2;
            btnDeducciones.Text = "       Deducciones";
            btnDeducciones.TextAlign = ContentAlignment.MiddleLeft;
            btnDeducciones.UseVisualStyleBackColor = false;
            // 
            // panel17
            // 
            panel17.Controls.Add(btnSoda);
            panel17.Location = new Point(0, 46);
            panel17.Margin = new Padding(0);
            panel17.Name = "panel17";
            panel17.Size = new Size(202, 46);
            panel17.TabIndex = 9;
            // 
            // btnSoda
            // 
            btnSoda.BackColor = Color.FromArgb(224, 224, 224);
            btnSoda.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSoda.ForeColor = Color.Black;
            btnSoda.Image = (Image)resources.GetObject("btnSoda.Image");
            btnSoda.ImageAlign = ContentAlignment.MiddleLeft;
            btnSoda.Location = new Point(-32, -20);
            btnSoda.Name = "btnSoda";
            btnSoda.Padding = new Padding(40, 0, 0, 0);
            btnSoda.Size = new Size(273, 89);
            btnSoda.TabIndex = 2;
            btnSoda.Text = "       Soda";
            btnSoda.TextAlign = ContentAlignment.MiddleLeft;
            btnSoda.UseVisualStyleBackColor = false;
            // 
            // panel18
            // 
            panel18.Controls.Add(btnUniforme);
            panel18.Location = new Point(0, 92);
            panel18.Margin = new Padding(0);
            panel18.Name = "panel18";
            panel18.Size = new Size(202, 46);
            panel18.TabIndex = 13;
            // 
            // btnUniforme
            // 
            btnUniforme.BackColor = Color.FromArgb(224, 224, 224);
            btnUniforme.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUniforme.ForeColor = Color.Black;
            btnUniforme.Image = (Image)resources.GetObject("btnUniforme.Image");
            btnUniforme.ImageAlign = ContentAlignment.MiddleLeft;
            btnUniforme.Location = new Point(-32, -20);
            btnUniforme.Name = "btnUniforme";
            btnUniforme.Padding = new Padding(40, 0, 0, 0);
            btnUniforme.Size = new Size(273, 89);
            btnUniforme.TabIndex = 2;
            btnUniforme.Text = "       Uniforme";
            btnUniforme.TextAlign = ContentAlignment.MiddleLeft;
            btnUniforme.UseVisualStyleBackColor = false;
            // 
            // panel19
            // 
            panel19.Controls.Add(btnReporteEmpleadoDeducciones);
            panel19.Location = new Point(0, 138);
            panel19.Margin = new Padding(0);
            panel19.Name = "panel19";
            panel19.Size = new Size(202, 46);
            panel19.TabIndex = 12;
            // 
            // btnReporteEmpleadoDeducciones
            // 
            btnReporteEmpleadoDeducciones.BackColor = Color.FromArgb(224, 224, 224);
            btnReporteEmpleadoDeducciones.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReporteEmpleadoDeducciones.ForeColor = Color.Black;
            btnReporteEmpleadoDeducciones.Image = (Image)resources.GetObject("btnReporteEmpleadoDeducciones.Image");
            btnReporteEmpleadoDeducciones.ImageAlign = ContentAlignment.MiddleLeft;
            btnReporteEmpleadoDeducciones.Location = new Point(-32, -20);
            btnReporteEmpleadoDeducciones.Name = "btnReporteEmpleadoDeducciones";
            btnReporteEmpleadoDeducciones.Padding = new Padding(40, 0, 0, 0);
            btnReporteEmpleadoDeducciones.Size = new Size(273, 89);
            btnReporteEmpleadoDeducciones.TabIndex = 2;
            btnReporteEmpleadoDeducciones.Text = "       Reporte  Empleado";
            btnReporteEmpleadoDeducciones.TextAlign = ContentAlignment.MiddleLeft;
            btnReporteEmpleadoDeducciones.UseVisualStyleBackColor = false;
            // 
            // panel20
            // 
            panel20.Controls.Add(btnReporteGneralDeducciones);
            panel20.Location = new Point(0, 184);
            panel20.Margin = new Padding(0);
            panel20.Name = "panel20";
            panel20.Size = new Size(202, 46);
            panel20.TabIndex = 11;
            // 
            // btnReporteGneralDeducciones
            // 
            btnReporteGneralDeducciones.BackColor = Color.FromArgb(224, 224, 224);
            btnReporteGneralDeducciones.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReporteGneralDeducciones.ForeColor = Color.Black;
            btnReporteGneralDeducciones.Image = (Image)resources.GetObject("btnReporteGneralDeducciones.Image");
            btnReporteGneralDeducciones.ImageAlign = ContentAlignment.MiddleLeft;
            btnReporteGneralDeducciones.Location = new Point(-32, -20);
            btnReporteGneralDeducciones.Name = "btnReporteGneralDeducciones";
            btnReporteGneralDeducciones.Padding = new Padding(40, 0, 0, 0);
            btnReporteGneralDeducciones.Size = new Size(273, 89);
            btnReporteGneralDeducciones.TabIndex = 2;
            btnReporteGneralDeducciones.Text = "       Reporte General";
            btnReporteGneralDeducciones.TextAlign = ContentAlignment.MiddleLeft;
            btnReporteGneralDeducciones.UseVisualStyleBackColor = false;
            // 
            // menuPagosContenedor
            // 
            menuPagosContenedor.BackColor = Color.White;
            menuPagosContenedor.Controls.Add(panel4);
            menuPagosContenedor.Controls.Add(panel5);
            menuPagosContenedor.Controls.Add(panel21);
            menuPagosContenedor.ForeColor = Color.White;
            menuPagosContenedor.Location = new Point(0, 168);
            menuPagosContenedor.Margin = new Padding(0);
            menuPagosContenedor.Name = "menuPagosContenedor";
            menuPagosContenedor.Size = new Size(202, 46);
            menuPagosContenedor.TabIndex = 13;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnPagosPrincipal);
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(0);
            panel4.Name = "panel4";
            panel4.Size = new Size(202, 46);
            panel4.TabIndex = 3;
            // 
            // btnPagosPrincipal
            // 
            btnPagosPrincipal.BackColor = Color.FromArgb(0, 221, 0);
            btnPagosPrincipal.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPagosPrincipal.ForeColor = Color.White;
            btnPagosPrincipal.Image = (Image)resources.GetObject("btnPagosPrincipal.Image");
            btnPagosPrincipal.ImageAlign = ContentAlignment.MiddleLeft;
            btnPagosPrincipal.Location = new Point(-29, -20);
            btnPagosPrincipal.Name = "btnPagosPrincipal";
            btnPagosPrincipal.Padding = new Padding(40, 0, 0, 0);
            btnPagosPrincipal.Size = new Size(231, 89);
            btnPagosPrincipal.TabIndex = 2;
            btnPagosPrincipal.Text = "       Pagos";
            btnPagosPrincipal.TextAlign = ContentAlignment.MiddleLeft;
            btnPagosPrincipal.UseVisualStyleBackColor = false;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnActivarPagos);
            panel5.Location = new Point(0, 46);
            panel5.Margin = new Padding(0);
            panel5.Name = "panel5";
            panel5.Size = new Size(202, 46);
            panel5.TabIndex = 9;
            // 
            // btnActivarPagos
            // 
            btnActivarPagos.BackColor = Color.FromArgb(224, 224, 224);
            btnActivarPagos.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnActivarPagos.ForeColor = Color.Black;
            btnActivarPagos.Image = (Image)resources.GetObject("btnActivarPagos.Image");
            btnActivarPagos.ImageAlign = ContentAlignment.MiddleLeft;
            btnActivarPagos.Location = new Point(-32, -20);
            btnActivarPagos.Name = "btnActivarPagos";
            btnActivarPagos.Padding = new Padding(40, 0, 0, 0);
            btnActivarPagos.Size = new Size(273, 89);
            btnActivarPagos.TabIndex = 2;
            btnActivarPagos.Text = "       Activar Pagos";
            btnActivarPagos.TextAlign = ContentAlignment.MiddleLeft;
            btnActivarPagos.UseVisualStyleBackColor = false;
            // 
            // panel21
            // 
            panel21.Controls.Add(btnPagosSubmenu);
            panel21.Location = new Point(0, 92);
            panel21.Margin = new Padding(0);
            panel21.Name = "panel21";
            panel21.Size = new Size(202, 46);
            panel21.TabIndex = 13;
            // 
            // btnPagosSubmenu
            // 
            btnPagosSubmenu.BackColor = Color.FromArgb(224, 224, 224);
            btnPagosSubmenu.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPagosSubmenu.ForeColor = Color.Black;
            btnPagosSubmenu.Image = (Image)resources.GetObject("btnPagosSubmenu.Image");
            btnPagosSubmenu.ImageAlign = ContentAlignment.MiddleLeft;
            btnPagosSubmenu.Location = new Point(-32, -20);
            btnPagosSubmenu.Name = "btnPagosSubmenu";
            btnPagosSubmenu.Padding = new Padding(40, 0, 0, 0);
            btnPagosSubmenu.Size = new Size(273, 89);
            btnPagosSubmenu.TabIndex = 2;
            btnPagosSubmenu.Text = "       Pagos";
            btnPagosSubmenu.TextAlign = ContentAlignment.MiddleLeft;
            btnPagosSubmenu.UseVisualStyleBackColor = false;
            // 
            // menuReportesContenedor
            // 
            menuReportesContenedor.BackColor = Color.White;
            menuReportesContenedor.Controls.Add(panel24);
            menuReportesContenedor.Controls.Add(panel25);
            menuReportesContenedor.Controls.Add(panel26);
            menuReportesContenedor.Controls.Add(panel27);
            menuReportesContenedor.ForeColor = Color.White;
            menuReportesContenedor.Location = new Point(0, 214);
            menuReportesContenedor.Margin = new Padding(0);
            menuReportesContenedor.Name = "menuReportesContenedor";
            menuReportesContenedor.Size = new Size(202, 46);
            menuReportesContenedor.TabIndex = 14;
            // 
            // panel24
            // 
            panel24.Controls.Add(btnReporte);
            panel24.Location = new Point(0, 0);
            panel24.Margin = new Padding(0);
            panel24.Name = "panel24";
            panel24.Size = new Size(202, 46);
            panel24.TabIndex = 3;
            // 
            // btnReporte
            // 
            btnReporte.BackColor = Color.FromArgb(0, 221, 0);
            btnReporte.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReporte.ForeColor = Color.White;
            btnReporte.Image = (Image)resources.GetObject("btnReporte.Image");
            btnReporte.ImageAlign = ContentAlignment.MiddleLeft;
            btnReporte.Location = new Point(-29, -20);
            btnReporte.Name = "btnReporte";
            btnReporte.Padding = new Padding(40, 0, 0, 0);
            btnReporte.Size = new Size(231, 89);
            btnReporte.TabIndex = 2;
            btnReporte.Text = "       Reportes";
            btnReporte.TextAlign = ContentAlignment.MiddleLeft;
            btnReporte.UseVisualStyleBackColor = false;
            // 
            // panel25
            // 
            panel25.Controls.Add(btnEmpleadosReporte);
            panel25.Location = new Point(0, 46);
            panel25.Margin = new Padding(0);
            panel25.Name = "panel25";
            panel25.Size = new Size(202, 46);
            panel25.TabIndex = 9;
            // 
            // btnEmpleadosReporte
            // 
            btnEmpleadosReporte.BackColor = Color.FromArgb(224, 224, 224);
            btnEmpleadosReporte.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnEmpleadosReporte.ForeColor = Color.Black;
            btnEmpleadosReporte.Image = (Image)resources.GetObject("btnEmpleadosReporte.Image");
            btnEmpleadosReporte.ImageAlign = ContentAlignment.MiddleLeft;
            btnEmpleadosReporte.Location = new Point(-32, -20);
            btnEmpleadosReporte.Name = "btnEmpleadosReporte";
            btnEmpleadosReporte.Padding = new Padding(40, 0, 0, 0);
            btnEmpleadosReporte.Size = new Size(273, 89);
            btnEmpleadosReporte.TabIndex = 2;
            btnEmpleadosReporte.Text = "       Empleados              >";
            btnEmpleadosReporte.TextAlign = ContentAlignment.MiddleLeft;
            btnEmpleadosReporte.UseVisualStyleBackColor = false;
            btnEmpleadosReporte.Click += btnEmpleadosReporte_Click;
            // 
            // panel26
            // 
            panel26.Controls.Add(btnPlanillaReportes);
            panel26.Location = new Point(0, 92);
            panel26.Margin = new Padding(0);
            panel26.Name = "panel26";
            panel26.Size = new Size(202, 46);
            panel26.TabIndex = 13;
            // 
            // btnPlanillaReportes
            // 
            btnPlanillaReportes.BackColor = Color.FromArgb(224, 224, 224);
            btnPlanillaReportes.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPlanillaReportes.ForeColor = Color.Black;
            btnPlanillaReportes.Image = (Image)resources.GetObject("btnPlanillaReportes.Image");
            btnPlanillaReportes.ImageAlign = ContentAlignment.MiddleLeft;
            btnPlanillaReportes.Location = new Point(-32, -20);
            btnPlanillaReportes.Name = "btnPlanillaReportes";
            btnPlanillaReportes.Padding = new Padding(40, 0, 0, 0);
            btnPlanillaReportes.Size = new Size(273, 89);
            btnPlanillaReportes.TabIndex = 2;
            btnPlanillaReportes.Text = "       Planilla                     >";
            btnPlanillaReportes.TextAlign = ContentAlignment.MiddleLeft;
            btnPlanillaReportes.UseVisualStyleBackColor = false;
            btnPlanillaReportes.Click += btnPlanillaReportes_Click;
            // 
            // panel27
            // 
            panel27.Controls.Add(btnSodaReportes);
            panel27.Location = new Point(0, 138);
            panel27.Margin = new Padding(0);
            panel27.Name = "panel27";
            panel27.Size = new Size(202, 46);
            panel27.TabIndex = 12;
            // 
            // btnSodaReportes
            // 
            btnSodaReportes.BackColor = Color.FromArgb(224, 224, 224);
            btnSodaReportes.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSodaReportes.ForeColor = Color.Black;
            btnSodaReportes.Image = (Image)resources.GetObject("btnSodaReportes.Image");
            btnSodaReportes.ImageAlign = ContentAlignment.MiddleLeft;
            btnSodaReportes.Location = new Point(-32, -20);
            btnSodaReportes.Name = "btnSodaReportes";
            btnSodaReportes.Padding = new Padding(40, 0, 0, 0);
            btnSodaReportes.Size = new Size(273, 89);
            btnSodaReportes.TabIndex = 2;
            btnSodaReportes.Text = "       Soda";
            btnSodaReportes.TextAlign = ContentAlignment.MiddleLeft;
            btnSodaReportes.UseVisualStyleBackColor = false;
            // 
            // botonSalirContenedor
            // 
            botonSalirContenedor.Controls.Add(btnSalir);
            botonSalirContenedor.Location = new Point(0, 260);
            botonSalirContenedor.Margin = new Padding(0);
            botonSalirContenedor.Name = "botonSalirContenedor";
            botonSalirContenedor.Size = new Size(202, 46);
            botonSalirContenedor.TabIndex = 10;
            // 
            // btnSalir
            // 
            btnSalir.BackColor = Color.Firebrick;
            btnSalir.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSalir.ForeColor = Color.White;
            btnSalir.Image = (Image)resources.GetObject("btnSalir.Image");
            btnSalir.ImageAlign = ContentAlignment.MiddleLeft;
            btnSalir.Location = new Point(-32, -20);
            btnSalir.Name = "btnSalir";
            btnSalir.Padding = new Padding(40, 0, 0, 0);
            btnSalir.Size = new Size(234, 89);
            btnSalir.TabIndex = 2;
            btnSalir.Text = "       Salir";
            btnSalir.TextAlign = ContentAlignment.MiddleLeft;
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;
            // 
            // panelPrincipal
            // 
            panelPrincipal.BackColor = Color.FromArgb(238, 238, 238, 238);
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.Location = new Point(198, 37);
            panelPrincipal.Name = "panelPrincipal";
            panelPrincipal.Size = new Size(724, 473);
            panelPrincipal.TabIndex = 2;
            // 
            // Principal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(922, 510);
            Controls.Add(panelPrincipal);
            Controls.Add(flowLayoutPanelSideBar);
            Controls.Add(panel1);
            Name = "Principal";
            Text = " ";
            Load += Principal_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            flowLayoutPanelSideBar.ResumeLayout(false);
            menuConfContenedor.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel9.ResumeLayout(false);
            menuPlanillaContenedor.ResumeLayout(false);
            panel12.ResumeLayout(false);
            panel13.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel15.ResumeLayout(false);
            panel16.ResumeLayout(false);
            panel6.ResumeLayout(false);
            menuDeduccionesContenedor.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel17.ResumeLayout(false);
            panel18.ResumeLayout(false);
            panel19.ResumeLayout(false);
            panel20.ResumeLayout(false);
            menuPagosContenedor.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel21.ResumeLayout(false);
            menuReportesContenedor.ResumeLayout(false);
            panel24.ResumeLayout(false);
            panel25.ResumeLayout(false);
            panel26.ResumeLayout(false);
            panel27.ResumeLayout(false);
            botonSalirContenedor.ResumeLayout(false);
            ResumeLayout(false);
        }



        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private Label LabelTitulo;
        private FlowLayoutPanel flowLayoutPanelSideBar;
        private Button btnConfiguraciones;
        private Panel panel2;
        private Button btnPlanilla; // <-- Deja solo UNA declaración de btnPlanilla aquí
        private Button btnReportes;
        private FlowLayoutPanel menuConfContenedor;
        private Panel panel7;
        private Button btnUsuarios;
        private Panel botonSalirContenedor;
        private Button btnSalir;
        private Panel panel9;
        private Button btnArticulos;
        private Panel panel10;
        private Button btnSemanas;
        private Panel panel11;
        private Button btnDepartamentos;
        private FlowLayoutPanel menuPlanillaContenedor;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel12;
        private Panel panel13;
        private Button btnEmpleado;
        private Panel panel14;
        private Button btnEntrada;
        private Panel panel15;
        private Button btnSalidas;
        private Panel panel16;
        private Button btnCierreDiario;
        private FlowLayoutPanel menuDeduccionesContenedor;
        private Panel panel3;
        private Button btnDeducciones;
        private Panel panel17;
        private Button btnSoda;
        private Panel panel18;
        private Button btnUniforme;
        private Panel panel19;
        private Button btnReporteEmpleadoDeducciones;
        private Panel panel20;
        private Button btnReporteGneralDeducciones;
        private FlowLayoutPanel menuPagosContenedor;
        private Panel panel4;
        private Button btnPagosPrincipal;
        private Panel panel5;
        private Button btnActivarPagos;
        private Panel panel21;
        private Button btnPagosSubmenu;
        private FlowLayoutPanel menuReportesContenedor;
        private Panel panel24;
        private Button btnReporte;
        private Panel panel25;
        private Button btnEmpleadosReporte;
        private Panel panel26;
        private Button btnPlanillaReportes;
        private Panel panel27;
        private Button btnSodaReportes;
        private Panel panel6;
        private Button btnSolicitudesPlanilla;
        private Panel panelPrincipal;
    }
}