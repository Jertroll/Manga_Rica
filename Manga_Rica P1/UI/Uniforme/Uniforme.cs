using Manga_Rica_P1.UI.Soda;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manga_Rica_P1.BLL;
using Manga_Rica_P1.BLL.Session;

namespace Manga_Rica_P1.UI.Uniforme
{
    public partial class Uniforme : Form
    {
        private readonly DeduccionesService _deduccionesService;
        private readonly IAppSession _session;
        
        // Variables de estado (equivalentes al código legacy)
        private bool _modoEdicion = false;
        private long? _deduccionActualId = null;
        private long? deduccionCargadaId = null;
        private List<DetalleUniformeTemp> _detallesTemporales = new();
        private EmpleadoInfoUniforme? _empleadoActual = null;

        public Uniforme(DeduccionesService deduccionesService, IAppSession session)
        {
            InitializeComponent();
            _deduccionesService = deduccionesService ?? throw new ArgumentNullException(nameof(deduccionesService));
            _session = session ?? throw new ArgumentNullException(nameof(session));
            
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            // Equivalente a FrmDeduccionesUniforme_Load del código legacy
            CambiarEstado(true); // Habilitar desde el inicio
            buttonGuardar.Enabled = true;
            
            // Configurar DataGridView para detalles
            ConfigurarDataGridView();
            
            // Cargar próximo consecutivo
            VerificarConsecutivo();
            
            // Llenar combo de artículos UNIFORMES
            LlenarArticulos();
            
            // Configurar fecha actual
            comboBoxFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            
            // Configurar cantidad por defecto
            textBoxCantidad.Text = "1";
            
            // Focus inicial en carné para empezar el flujo
            textBoxCarnet.Focus();
            
            // Configurar eventos adicionales
            ConfigurarEventos();
        }

        private void ConfigurarEventos()
        {
            // Eventos del carnet
            textBoxCarnet.KeyDown += TextBoxCarnet_KeyDown;
            textBoxCarnet.KeyPress += TextBoxCarnet_KeyPress;
            
            // Eventos de cantidad
            textBoxCantidad.KeyDown += TextBoxCantidad_KeyDown;
            textBoxCantidad.KeyPress += TextBoxCantidad_KeyPress;
            textBoxCantidad.TextChanged += TextBoxCantidad_TextChanged;
            textBoxCantidad.Leave += TextBoxCantidad_Leave;
            
            // Eventos del combo de artículos
            comboBoxArticulos.SelectedIndexChanged += ComboBoxArticulos_SelectedIndexChanged;
            comboBoxArticulos.KeyDown += ComboBoxArticulos_KeyDown;
            
            // Eventos de botones
            buttonAgregar.Click += ButtonAgregar_Click;
            buttonGuardar.Click += ButtonGuardar_Click;
            
            // Eventos del DataGridView
            dataGridView1.KeyDown += DataGridView1_KeyDown;
        }

        private void ConfigurarDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cantidad",
                HeaderText = "Cantidad",
                DataPropertyName = "Cantidad",
                Width = 80
            });
            
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Codigo",
                HeaderText = "Código",
                DataPropertyName = "Codigo",
                Width = 80
            });
            
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Descripcion",
                HeaderText = "Descripción",
                DataPropertyName = "Descripcion",
                Width = 200
            });
            
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Precio",
                HeaderText = "Precio",
                DataPropertyName = "Precio",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
            
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Total",
                HeaderText = "Total",
                DataPropertyName = "Total",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });
        }

        private void LlenarArticulos()
        {
            try
            {
                var articulos = _deduccionesService.GetArticulosUniformes();
                comboBoxArticulos.DataSource = articulos;
                comboBoxArticulos.DisplayMember = "Descripcion";
                comboBoxArticulos.ValueMember = "Id";
                comboBoxArticulos.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar artículos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VerificarConsecutivo()
        {
            try
            {
                var proximoNumero = _deduccionesService.GetNextDeduccionId();
                textBoxNumFactura.Text = proximoNumero.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener consecutivo: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CambiarEstado(bool estado)
        {
            // Equivalente a Cambia_Estado del código legacy
            textBoxCarnet.Enabled = estado;
            textBoxNombre.Enabled = false;  // Siempre deshabilitado, se llena automáticamente
            textBoxNumFactura.Enabled = false;  // Siempre deshabilitado, se genera automáticamente
            textBoxTotal.Enabled = false;  // Siempre deshabilitado, se calcula automáticamente
            comboBoxFecha.Enabled = false;  // Siempre deshabilitado, se usa fecha actual
            comboBoxArticulos.Enabled = estado && _empleadoActual != null; // Solo si hay empleado
            textBoxCantidad.Enabled = estado && _empleadoActual != null;
            textBoxSubTotal.Enabled = false;  // Siempre deshabilitado, se calcula automáticamente
            textBoxSaldo.Enabled = false;  // Siempre deshabilitado, se calcula automáticamente
            buttonAgregar.Enabled = estado && _empleadoActual != null;
            
            // En modo normal, limpiar estado de anulación
            if (estado)
            {
                this.BackColor = SystemColors.Control;  // Color por defecto
                this.Text = "Uniforme - Deducciones";
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToDeleteRows = true;
            }
        }

        private void LimpiarFormulario()
        {
            // Equivalente a Limpia() del código legacy
            textBoxCarnet.Text = "";
            textBoxNombre.Text = "";
            textBoxCantidad.Text = "1"; // Cantidad por defecto
            textBoxSubTotal.Text = "";
            textBoxTotal.Text = "";
            textBoxSaldo.Text = "";
            if (pictureBoxEmpleado != null)
                pictureBoxEmpleado.Image = null;

            comboBoxFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            
            _detallesTemporales.Clear();
            ActualizarDataGridView();
            
            _empleadoActual = null;
            _deduccionActualId = null;
            
            comboBoxArticulos.SelectedIndex = -1;
            
            // Actualizar estado de controles
            CambiarEstado(true);
        }

        private void LimpiarDetalles()
        {
            textBoxCantidad.Text = "1"; // Resetear a cantidad por defecto
            textBoxSubTotal.Text = "";
            comboBoxArticulos.SelectedIndex = -1;
        }

        // =========================================================
        //  Eventos de controles
        // =========================================================

        private void TextBoxCarnet_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarEmpleado(textBoxCarnet.Text);
            }
        }

        private void TextBoxCarnet_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Solo permitir números y backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void TextBoxCantidad_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(textBoxCantidad.Text))
            {
                CalcularSubTotal();
                AgregarArticulo();
            }
        }

        private void TextBoxCantidad_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Solo permitir números y backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void ComboBoxArticulos_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Calcular automáticamente cuando se selecciona un artículo
            CalcularSubTotal();
            
            // Si no hay cantidad, poner 1 por defecto
            if (string.IsNullOrEmpty(textBoxCantidad.Text) && comboBoxArticulos.SelectedItem != null)
            {
                textBoxCantidad.Text = "1";
            }
            
            // Si ya hay cantidad y artículo, enfocar en botón agregar
            if (comboBoxArticulos.SelectedItem != null && !string.IsNullOrEmpty(textBoxCantidad.Text))
            {
                buttonAgregar.Focus();
            }
        }

        private void ComboBoxArticulos_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && comboBoxArticulos.SelectedItem != null)
            {
                // Si presiona Enter en el combo, ir a cantidad
                textBoxCantidad.Focus();
                textBoxCantidad.SelectAll(); // Seleccionar todo para poder cambiar rápido
            }
        }

        private void TextBoxCantidad_TextChanged(object? sender, EventArgs e)
        {
            // Calcular automáticamente al cambiar cantidad
            if (comboBoxArticulos.SelectedItem != null)
            {
                CalcularSubTotal();
            }
        }

        private void TextBoxCantidad_Leave(object? sender, EventArgs e)
        {
            // Si sale del campo sin valor, poner 1 por defecto
            if (string.IsNullOrEmpty(textBoxCantidad.Text))
            {
                textBoxCantidad.Text = "1";
            }
            CalcularSubTotal();
        }

        private void CalcularSubTotal()
        {
            try
            {
                if (comboBoxArticulos.SelectedItem != null)
                {
                    // Obtener el artículo seleccionado directamente
                    var articuloSeleccionado = comboBoxArticulos.SelectedItem as ArticuloUniforme;
                    if (articuloSeleccionado == null) return;
                    
                    var articuloId = articuloSeleccionado.Id;
                    
                    // Si no hay cantidad o es inválida, usar 1 por defecto para mostrar el precio
                    int cantidad = 1;
                    if (!string.IsNullOrEmpty(textBoxCantidad.Text))
                    {
                        if (!int.TryParse(textBoxCantidad.Text, out cantidad) || cantidad <= 0)
                        {
                            cantidad = 1;
                        }
                    }
                    
                    var subtotal = _deduccionesService.CalcularSubtotal(articuloId, cantidad);
                    textBoxSubTotal.Text = subtotal.ToString("N2");
                }
                else
                {
                    textBoxSubTotal.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al calcular subtotal: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSubTotal.Text = "";
            }
        }

        private void ButtonAgregar_Click(object? sender, EventArgs e)
        {
            AgregarArticulo();
        }

        private void AgregarArticulo()
        {
            try
            {
                // Validar campos
                if (comboBoxArticulos.SelectedItem == null)
                {
                    MessageBox.Show("Debe Escoger algún Artículo válido para esta Deducción !!!!", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(textBoxCantidad.Text, out int cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Debe Ingresar una Cantidad para esta Deducción !!!!", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_empleadoActual == null)
                {
                    MessageBox.Show("Debe Ingresar un Empleado Válido !!!!", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el artículo seleccionado directamente
                var articuloSeleccionado = comboBoxArticulos.SelectedItem as ArticuloUniforme;
                if (articuloSeleccionado == null) return;
                
                var articuloId = articuloSeleccionado.Id;

                // Validar con el servicio
                var (isValid, errorMessage) = _deduccionesService.ValidarDetalle(articuloId, cantidad, _empleadoActual.Id);
                if (!isValid)
                {
                    MessageBox.Show(errorMessage, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar que no esté duplicado
                if (_detallesTemporales.Any(d => d.Codigo == articuloId))
                {
                    MessageBox.Show("Este artículo ya fue agregado", "Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear detalle temporal
                var detalle = _deduccionesService.CrearDetalleTemporal(articuloId, cantidad);
                _detallesTemporales.Add(detalle);

                // Actualizar interfaz
                ActualizarDataGridView();
                CalcularTotal();
                LimpiarDetalles();
                comboBoxArticulos.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar artículo: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _detallesTemporales;
            
            // Asegurar que las columnas estén correctamente configuradas
            if (dataGridView1.Columns.Count > 0)
            {
                // Configurar formato de las columnas
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    switch (column.DataPropertyName)
                    {
                        case "Cantidad":
                            column.HeaderText = "Cantidad";
                            column.Width = 80;
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "Codigo":
                            column.HeaderText = "Código";
                            column.Width = 80;
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case "Descripcion":
                            column.HeaderText = "Descripción";
                            column.Width = 200;
                            break;
                        case "Precio":
                            column.HeaderText = "Precio";
                            column.Width = 100;
                            column.DefaultCellStyle.Format = "N2";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        case "Total":
                            column.HeaderText = "Total";
                            column.Width = 100;
                            column.DefaultCellStyle.Format = "N2";
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                    }
                }
            }
        }

        private void CalcularTotal()
        {
            try
            {
                var total = _deduccionesService.CalcularTotalDetalles(_detallesTemporales);
                textBoxTotal.Text = total.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al calcular total: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_KeyDown(object? sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete && dataGridView1.CurrentRow != null)
                {
                    if (_modoEdicion)
                    {
                        MessageBox.Show("No es posible editar una deducción !!!", "Información", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    var result = MessageBox.Show("¿Desea quitar este artículo?", "Confirmar", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        var index = dataGridView1.CurrentRow.Index;
                        if (index >= 0 && index < _detallesTemporales.Count)
                        {
                            _detallesTemporales.RemoveAt(index);
                            ActualizarDataGridView();
                            CalcularTotal();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar artículo: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_empleadoActual == null)
                {
                    MessageBox.Show("Debe seleccionar un empleado", "Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_detallesTemporales.Count == 0)
                {
                    MessageBox.Show("Debe Tener Al Menos un Artículo !!!!", "Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_session.CurrentUser == null)
                {
                    MessageBox.Show("No hay usuario autenticado", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Registrar la deducción
                var deduccionId = _deduccionesService.RegistrarDeduccion(
                    _empleadoActual.Id, 
                    _detallesTemporales, 
                    _session.CurrentUser.Id);

                MessageBox.Show("Deducción Actualizada Satisfactoriamente....", "Éxito", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Preparar para siguiente deducción
                LimpiarFormulario();
                CambiarEstado(true);
                buttonGuardar.Enabled = true;
                VerificarConsecutivo();
                LlenarArticulos();
                textBoxCarnet.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar deducción: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var buscarForm = new BuscadorUniforme(_deduccionesService))
                {
                    var result = buscarForm.ShowDialog(this);
                    if (result == DialogResult.OK && buscarForm.DeduccionSeleccionadaId.HasValue)
                    {
                        CargarDeduccionExistente(buscarForm.DeduccionSeleccionadaId.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar deducción: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDeduccionExistente(long id)
        {
            try
            {
                var deduccion = _deduccionesService.CargarDeduccion(id);
                if (deduccion == null)
                {
                    MessageBox.Show("No se pudo cargar la deducción seleccionada", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Modo edición
                _modoEdicion = true;
                _deduccionActualId = id;
                deduccionCargadaId = id;  // Para el botón anular

                // Cargar datos del empleado
                _empleadoActual = deduccion.Empleado;
                textBoxCarnet.Text = deduccion.Empleado.Carne.ToString();
                textBoxNombre.Text = deduccion.Empleado.NombreCompleto;

                // Cargar foto del empleado
                if (pictureBoxEmpleado != null)
                {
                    CargarFotoEmpleado(deduccion.Empleado.Foto);
                }

                // Cargar datos de la deducción
                textBoxNumFactura.Text = deduccion.Id.ToString();
                textBoxTotal.Text = deduccion.Total.ToString("N2");
                textBoxSaldo.Text = deduccion.Saldo.ToString("N2");

                comboBoxFecha.Text = deduccion.Fecha.ToString("dd/MM/yyyy");

                // Cargar detalles
                _detallesTemporales.Clear();
                _detallesTemporales.AddRange(deduccion.Detalles);
                ActualizarDataGridView();

                // Cambiar a modo consulta/anulación
                CambiarAModoConsultaAnulacion(deduccion.Anulada);
                
                // Mostrar información al usuario
                if (deduccion.Anulada)
                {
                    MessageBox.Show("Deducción cargada. Esta deducción ya está ANULADA.", "Información", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Deducción cargada. Solo puede ANULAR esta deducción.", "Información", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar deducción: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configura el formulario para modo consulta/anulación solamente
        /// </summary>
        private void CambiarAModoConsultaAnulacion(bool yaAnulada)
        {
            // Deshabilitar todos los controles de edición
            textBoxCarnet.Enabled = false;
            textBoxNombre.Enabled = false;          // No editar nombre
            textBoxNumFactura.Enabled = false;      // No editar ID factura
            textBoxTotal.Enabled = false;           // No editar total
            textBoxSaldo.Enabled = false;           // No editar saldo
            comboBoxFecha.Enabled = false;          // No editar fecha
            buttonBuscarEmpleado.Enabled = false;
            comboBoxArticulos.Enabled = false;
            textBoxCantidad.Enabled = false;
            textBoxSubTotal.Enabled = false;
            buttonAgregar.Enabled = false;
            buttonGuardar.Visible = false;          // Ocultar guardar
            btnBuscar.Enabled = true;              // Permitir buscar otras deducciones
            
            // Configurar botón anular según estado
            buttonAnular.Visible = !yaAnulada;     // Solo mostrar si no está anulada
            buttonAnular.Enabled = !yaAnulada;     // Solo habilitar si no está anulada
            
            // Deshabilitar edición del DataGridView
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToDeleteRows = false;
            
            // Cambiar color de fondo para indicar modo consulta
            this.BackColor = yaAnulada ? Color.LightCoral : Color.LightYellow;
            
            // Mostrar en el título que está en modo consulta
            if (yaAnulada)
            {
                this.Text = "Uniforme - Deducción ANULADA (Solo Consulta)";
            }
            else
            {
                this.Text = "Uniforme - Deducción Existente (Solo Anulación)";
            }
        }

        private void buttonBuscarEmpleado_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxCarnet.Text))
            {
                BuscarEmpleado(textBoxCarnet.Text);
            }
            else
            {
                MessageBox.Show("Ingrese un número de carnet para buscar", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCarnet.Focus();
            }
        }

        private void BuscarEmpleado(string carnet)
        {
            try
            {
                if (string.IsNullOrEmpty(carnet) || !long.TryParse(carnet, out long carnetLong))
                {
                    MessageBox.Show("Empleado NO Registrado !!!!!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LimpiarDatosEmpleado();
                    return;
                }

                var empleado = _deduccionesService.BuscarEmpleadoPorCarne(carnetLong);
                if (empleado == null)
                {
                    MessageBox.Show("Empleado NO Registrado !!!!!", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LimpiarDatosEmpleado();
                    return;
                }

                // AUTOCOMPLETAR datos del empleado
                _empleadoActual = empleado;
                textBoxNombre.Text = empleado.NombreCompleto;
                textBoxCarnet.Text = empleado.Carne.ToString();

                // Cargar foto del empleado si existe el PictureBox
                if (pictureBoxEmpleado != null)
                {
                    CargarFotoEmpleado(empleado.Foto);
                }

                // Habilitar controles para continuar el flujo
                CambiarEstado(true);
                
                // Enfocar en ComboBox de artículos para continuar flujo
                comboBoxArticulos.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar empleado: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarDatosEmpleado()
        {
            _empleadoActual = null;
            textBoxNombre.Text = "";
            if (pictureBoxEmpleado != null)
                pictureBoxEmpleado.Image = null;
            CambiarEstado(true); // Mantener carné habilitado
            textBoxCarnet.Focus();
        }

        private void CargarFotoEmpleado(string? rutaFoto)
        {
            try
            {
                // Solo ejecutar si existe el control pictureBoxEmpleado
                if (pictureBoxEmpleado == null) return;

                // Limpiar imagen anterior
                if (pictureBoxEmpleado.Image != null)
                {
                    pictureBoxEmpleado.Image.Dispose();
                    pictureBoxEmpleado.Image = null;
                }

                // Verificar si hay ruta de foto
                if (string.IsNullOrEmpty(rutaFoto))
                {
                    pictureBoxEmpleado.Image = null;
                    return;
                }

                string? rutaEncontrada = null;

                // Intentar diferentes rutas para encontrar la imagen
                string[] rutasAProbrar = {
                    rutaFoto,  // Ruta directa
                    Path.Combine(Application.StartupPath, rutaFoto),  // Ruta desde aplicación
                    Path.Combine(Application.StartupPath, "Imagenes", rutaFoto),  // Ruta en carpeta Imagenes
                    Path.Combine(Application.StartupPath, "Imagenes", "Empleados", rutaFoto)  // Ruta en subcarpeta
                };

                foreach (string ruta in rutasAProbrar)
                {
                    if (System.IO.File.Exists(ruta))
                    {
                        rutaEncontrada = ruta;
                        break;
                    }
                }

                if (rutaEncontrada == null)
                {
                    pictureBoxEmpleado.Image = null;
                    return;
                }

                // Cargar la imagen
                using (var fileStream = new FileStream(rutaEncontrada, FileMode.Open, FileAccess.Read))
                {
                    pictureBoxEmpleado.Image = new Bitmap(fileStream);
                }
                
                // Configurar el modo de visualización
                pictureBoxEmpleado.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar imagen vacía
                if (pictureBoxEmpleado != null)
                    pictureBoxEmpleado.Image = null;
                // Opcional: Log del error para debugging
                System.Diagnostics.Debug.WriteLine($"Error cargando foto empleado: {ex.Message}");
            }
        }

        private void buttonAnular_Click(object sender, EventArgs e)
        {
            try
            {
                if (deduccionCargadaId == null)
                {
                    MessageBox.Show("No hay una deducción cargada para anular.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmacion = MessageBox.Show(
                    "¿Está seguro que desea anular esta deducción de uniforme?",
                    "Confirmar Anulación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    if (_session.CurrentUser == null)
                    {
                        MessageBox.Show("No hay usuario autenticado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    _deduccionesService.AnularDeduccion(deduccionCargadaId.Value, _session.CurrentUser.Id);
                    MessageBox.Show("Deducción anulada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Limpiar el formulario
                    LimpiarFormulario();
                    CambiarAModoNuevo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al anular la deducción: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CambiarAModoNuevo()
        {
            // Cambiar a modo nuevo
            deduccionCargadaId = null;
            _modoEdicion = false;
            
            // Habilitar controles para nueva deducción
            textBoxCarnet.Enabled = true;
            comboBoxArticulos.Enabled = true;
            textBoxCantidad.Enabled = true;
            buttonAgregar.Enabled = true;
            buttonGuardar.Visible = true;
            buttonAnular.Visible = false;
            
            // Cambiar colores a modo normal
            BackColor = SystemColors.Control;
            foreach (Control ctrl in Controls)
            {
                if (ctrl is TextBox || ctrl is ComboBox)
                {
                    ctrl.BackColor = SystemColors.Window;
                }
            }
        }
    }
}
