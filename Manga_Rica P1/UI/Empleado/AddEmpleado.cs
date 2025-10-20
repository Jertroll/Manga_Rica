using Manga_Rica_P1.BLL;
using Manga_Rica_P1.Entity;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EmpleadoEntity = Manga_Rica_P1.Entity.Empleado;
using SolicitudEntity = Manga_Rica_P1.Entity.Solicitudes;

namespace Manga_Rica_P1.UI.Empleados.Modales
{
    public partial class AddEmpleado : Form
    {
        // Resultado que leerá el caller después de DialogResult.OK
        public EmpleadoEntity Result { get; private set; } = new EmpleadoEntity();

        private readonly DepartamentosService _departamentosService;
        private string? _selectedPhotoPath;
        private const string FotosFolderName = "FotosEmpleados";

        // Si se hace PrefillFromEmpleado antes de cargar DataSource, guardamos el Id pendiente
        private int? _pendingDepartamentoId;

        // ===== Constructor =====
        // OBLIGATORIO pasar DepartamentosService para llenar el combo desde la BD.
        public AddEmpleado(DepartamentosService departamentosService)
        {
            InitializeComponent();
            _departamentosService = departamentosService
             ?? throw new ArgumentNullException(nameof(departamentosService));
            this.Load += AddEmpleado_Load;
        }

        // ===== Helpers =====
        /// <summary>
        /// Copia/convierte la imagen a &lt;app&gt;\FotosEmpleados (JPG 400x400) y retorna la ruta RELATIVA para la BD.
        /// </summary>
        private string? SaveLocalPhoto(string? sourcePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
                    return null;

                var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FotosFolderName);
                Directory.CreateDirectory(root);

                var baseName = string.IsNullOrWhiteSpace(Result?.Cedula) ? Guid.NewGuid().ToString("N") : Result.Cedula;
                var fileName = $"{baseName}_{DateTime.Now:yyyyMMddHHmmss}.jpg";
                var destFullPath = Path.Combine(root, fileName);

                using (var src = Image.FromFile(sourcePath))
                using (var dst = new Bitmap(400, 400))
                using (var g = Graphics.FromImage(dst))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    // Crop centrado para llenar 400x400
                    var scale = Math.Max(400f / src.Width, 400f / src.Height);
                    var w = (int)Math.Round(400 / scale);
                    var h = (int)Math.Round(400 / scale);
                    var x = (src.Width - w) / 2;
                    var y = (src.Height - h) / 2;
                    var srcRect = new Rectangle(x, y, w, h);

                    g.Clear(Color.White);
                    g.DrawImage(src, new Rectangle(0, 0, 400, 400), srcRect, GraphicsUnit.Pixel);

                    var jpegCodec = ImageCodecInfo.GetImageEncoders().First(c => c.MimeType == "image/jpeg");
                    using var encParams = new EncoderParameters(1);
                    encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L);
                    dst.Save(destFullPath, jpegCodec, encParams);
                }

                return Path.Combine(FotosFolderName, fileName).Replace('\\', '/');
            }
            catch
            {
                return null;
            }
        }

        private void EnsureCombosLoaded()
        {
            // Estado civil (constante)
            if (comboBoxEstadoCivil.Items.Count == 0)
                comboBoxEstadoCivil.Items.AddRange(new object[] { "Soltero", "Casado", "Unión Libre", "Divorciado", "Viudo" });

            // Laboró (constante)
            if (comboBoxLaboro.Items.Count == 0)
                comboBoxLaboro.Items.AddRange(new object[] { "No", "Sí" });

            // Activo (constante)
            if (comboBoxActivo.Items.Count == 0)
                comboBoxActivo.Items.AddRange(new object[] { "No", "Sí" });

            // Selecciones por defecto
            if (comboBoxEstadoCivil.SelectedIndex < 0 && comboBoxEstadoCivil.Items.Count > 0) comboBoxEstadoCivil.SelectedIndex = 0;
            if (comboBoxLaboro.SelectedIndex < 0 && comboBoxLaboro.Items.Count > 0) comboBoxLaboro.SelectedIndex = 0;
            if (comboBoxActivo.SelectedIndex < 0 && comboBoxActivo.Items.Count > 0) comboBoxActivo.SelectedIndex = 1; // Activo = Sí
        }

        // Nueva implementacion
        private void LimpiarFotoEmpleado()
        {
            if (fotoEmpleado == null) return;
            if (fotoEmpleado.Image != null)
            {
                fotoEmpleado.Image.Dispose();
                fotoEmpleado.Image = null;
            }
        }

        // Nueva implementacion
        private void CargarFotoEmpleadoDesdeRuta(string? rutaFoto)
        {
            try
            {
                if (fotoEmpleado == null) return;

                // libera imagen previa para evitar locks
                LimpiarFotoEmpleado();

                if (string.IsNullOrWhiteSpace(rutaFoto))
                {
                    // opcional: placeholder
                    // fotoEmpleado.Image = Properties.Resources.placeholder_user;
                    return;
                }

                string? rutaEncontrada = null;
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;

                // mismas heurísticas que en Uniforme
                string[] rutasAProbar =
                {
                    rutaFoto,                                                     // ruta directa
                    Path.Combine(baseDir, rutaFoto),                              // relativa a la app
                    Path.Combine(baseDir, "Imagenes", rutaFoto),                  // /Imagenes
                    Path.Combine(baseDir, "Imagenes", "Empleados", rutaFoto),     // /Imagenes/Empleados
                    Path.Combine(baseDir, rutaFoto.Replace('/', Path.DirectorySeparatorChar)) // normalizada
                };

                foreach (var r in rutasAProbar)
                {
                    if (File.Exists(r))
                    {
                        rutaEncontrada = r;
                        break;
                    }
                }

                if (rutaEncontrada == null)
                {
                    // opcional: placeholder
                    // fotoEmpleado.Image = Properties.Resources.placeholder_user;
                    return;
                }

                // carga sin bloquear el archivo
                using (var fs = new FileStream(rutaEncontrada, FileMode.Open, FileAccess.Read))
                {
                    fotoEmpleado.Image = new Bitmap(fs);
                }

                // dimensionado igual que Uniforme (yo recomiendo Zoom para no deformar)
                fotoEmpleado.SizeMode = PictureBoxSizeMode.Zoom; // usa StretchImage si quieres paridad exacta
                // fotoEmpleado.BorderStyle = BorderStyle.FixedSingle; // opcional
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cargando foto empleado: {ex.Message}");
                LimpiarFotoEmpleado();
            }
        }

        private void LoadDepartamentosDesdeBD()
        {
            // Consulta SIEMPRE a la BD (sin fallback a items quemados)
            var lista = _departamentosService.GetAll()
                        .OrderBy(d => d.nombre)   // si tu entidad usa "Nombre" cambia a d.Nombre
                        .Select(d => new { d.Id, Nombre = d.nombre })
                        .ToList();

            comboBoxDepartamento.DataSource = null;
            comboBoxDepartamento.Items.Clear();

            comboBoxDepartamento.DataSource = lista;
            comboBoxDepartamento.DisplayMember = "Nombre";
            comboBoxDepartamento.ValueMember = "Id";

            if (lista.Count == 0)
            {
                buttonGuardar.Enabled = false;
                MessageBox.Show("No hay departamentos en la base de datos. No es posible continuar.", "Departamentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                buttonGuardar.Enabled = true;

                if (_pendingDepartamentoId.HasValue)
                {
                    comboBoxDepartamento.SelectedValue = _pendingDepartamentoId.Value;
                    _pendingDepartamentoId = null;
                }
                else
                {
                    comboBoxDepartamento.SelectedIndex = 0;
                }
            }
        }

        // ===== Prefills =====
        public void PrefillFromSolicitud(Manga_Rica_P1.Entity.Solicitudes s)
        {
            EnsureCombosLoaded();

            // Carné no viene en Solicitud: lo dejamos vacío para que lo ingrese el usuario
            if (textBoxCarnet != null) textBoxCarnet.Text = string.Empty;

            textBoxCedula.Text = s.Cedula;
            textBoxApellido1.Text = s.Primer_Apellido;
            textBoxAppelido2.Text = s.Segundo_Apellido ?? string.Empty;
            textBoxNombre.Text = s.Nombre;
            dateTimeNacimiento.Value = s.Fecha_Nacimiento == default ? DateTime.Today.AddYears(-18) : s.Fecha_Nacimiento;

            var matchEstado = comboBoxEstadoCivil.Items.Cast<object>()
                .FirstOrDefault(x => string.Equals(x.ToString(), s.Estado_Civil, StringComparison.InvariantCultureIgnoreCase));
            comboBoxEstadoCivil.SelectedItem = matchEstado ?? comboBoxEstadoCivil.Items[0];

            textBoxTelefono.Text = s.Telefono ?? string.Empty;
            textBoxCelular.Text = s.Celular;
            textBoxNacionalidad.Text = s.Nacionalidad;
            comboBoxLaboro.SelectedIndex = s.Laboro == 1 ? 1 : 0;
            textBoxDireccion.Text = s.Direccion;

            if (string.IsNullOrWhiteSpace(textBoxPuestoLaboro.Text)) textBoxPuestoLaboro.Text = "";
            if (string.IsNullOrWhiteSpace(textBoxSalario.Text)) textBoxSalario.Text = "0";
            dateTimePicker1.Value = DateTime.Today;
            comboBoxActivo.SelectedIndex = 1;
        }

        public void PrefillFromEmpleado(Manga_Rica_P1.Entity.Empleado e)
        {
            EnsureCombosLoaded();

            if (textBoxCarnet != null) textBoxCarnet.Text = e.Carne.ToString();

            textBoxCedula.Text = e.Cedula;
            textBoxApellido1.Text = e.Primer_Apellido;
            textBoxAppelido2.Text = e.Segundo_Apellido ?? string.Empty;
            textBoxNombre.Text = e.Nombre;
            dateTimeNacimiento.Value = e.Fecha_Nacimiento == default ? DateTime.Today.AddYears(-18) : e.Fecha_Nacimiento;

            var matchEstado = comboBoxEstadoCivil.Items.Cast<object>()
                .FirstOrDefault(x => string.Equals(x.ToString(), e.Estado_Civil, StringComparison.InvariantCultureIgnoreCase));
            comboBoxEstadoCivil.SelectedItem = matchEstado ?? comboBoxEstadoCivil.Items[0];

            textBoxTelefono.Text = e.Telefono ?? string.Empty;
            textBoxCelular.Text = e.Celular;
            textBoxNacionalidad.Text = e.Nacionalidad;
            comboBoxLaboro.SelectedIndex = e.Laboro == 1 ? 1 : 0;
            textBoxDireccion.Text = e.Direccion;

            // El DataSource se configura en Load; guardamos el Id para aplicarlo luego
            _pendingDepartamentoId = e.Id_Departamento;
            if (comboBoxDepartamento.DataSource != null)
            {
                comboBoxDepartamento.SelectedValue = e.Id_Departamento;
                _pendingDepartamentoId = null;
            }

            textBoxSalario.Text = e.Salario.ToString(CultureInfo.InvariantCulture);
            textBoxPuestoLaboro.Text = e.Puesto;
            dateTimePicker1.Value = e.Fecha_Ingreso == default ? DateTime.Today : e.Fecha_Ingreso;
            dateTimePicker2.Value = e.Fecha_Salida == default ? DateTime.Today : e.Fecha_Salida;
            comboBoxActivo.SelectedIndex = e.Activo == 1 ? 1 : 0;
            textBoxMcNumero.Text = e.MC_Numero.ToString();

            // Nueva implementacion: usar el helper para cargar la foto (mismas heurísticas que Uniforme)
            Result.Foto = e.Foto; // conservar ruta existente si no se reemplaza
            CargarFotoEmpleadoDesdeRuta(e.Foto);
        }

        // ===== Load / Botones =====
        private void AddEmpleado_Load(object? sender, EventArgs e)
        {
            EnsureCombosLoaded();
            LoadDepartamentosDesdeBD(); // SIEMPRE desde BD

            buttonGuardar.Click += (_, __) => GuardarYSalir();
            buttonCancelar.Click += (_, __) => DialogResult = DialogResult.Cancel;

            // Nueva implementacion: asegurar dimensionado coherente del PictureBox
            if (fotoEmpleado != null)
            {
                fotoEmpleado.SizeMode = PictureBoxSizeMode.Zoom; // usa StretchImage si quieres paridad exacta con Uniforme
            }
        }

        // ===== Guardar =====
        private void GuardarYSalir()
        {
            // Validaciones
            if (comboBoxDepartamento.DataSource == null || comboBoxDepartamento.Items.Count == 0)
            {
                MessageBox.Show("No hay departamentos cargados. Verifique la tabla de Departamentos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxCedula.Text))
            {
                MessageBox.Show("Cédula requerida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCedula.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                MessageBox.Show("Nombre requerido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxNombre.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(textBoxApellido1.Text))
            {
                MessageBox.Show("Primer Apellido requerido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxApellido1.Focus(); return;
            }

            // Carné (obligatorio)
            if (textBoxCarnet == null || string.IsNullOrWhiteSpace(textBoxCarnet.Text) || !long.TryParse(textBoxCarnet.Text, out var carne) || carne <= 0)
            {
                MessageBox.Show("Número de carné inválido u obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCarnet?.Focus(); return;
            }

            if (!float.TryParse(textBoxSalario.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float salario))
            {
                MessageBox.Show("Salario inválido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxSalario.Focus(); return;
            }

            long.TryParse(textBoxMcNumero.Text, out var mc);

            // Mapear controles -> entidad
            Result.Carne = carne;
            Result.Cedula = textBoxCedula.Text.Trim();
            Result.Primer_Apellido = textBoxApellido1.Text.Trim();
            Result.Segundo_Apellido = string.IsNullOrWhiteSpace(textBoxAppelido2.Text) ? null : textBoxAppelido2.Text.Trim();
            Result.Nombre = textBoxNombre.Text.Trim();
            Result.Fecha_Nacimiento = dateTimeNacimiento.Value.Date;
            Result.Estado_Civil = comboBoxEstadoCivil.SelectedItem?.ToString() ?? "";
            Result.Telefono = textBoxTelefono.Text.Trim();
            Result.Celular = textBoxCelular.Text.Trim();
            Result.Nacionalidad = textBoxNacionalidad.Text.Trim();
            Result.Laboro = comboBoxLaboro.SelectedIndex == 1 ? 1 : 0;
            Result.Direccion = textBoxDireccion.Text.Trim();

            // Departamento: SIEMPRE desde DataSource
            Result.Id_Departamento = Convert.ToInt32(comboBoxDepartamento.SelectedValue ?? 0);

            Result.Salario = salario;
            Result.Puesto = textBoxPuestoLaboro.Text.Trim();
            Result.Fecha_Ingreso = dateTimePicker1.Value.Date;
            Result.Fecha_Salida = dateTimePicker2.Value.Date;
            Result.Activo = comboBoxActivo.SelectedIndex == 1 ? 1 : 0;
            Result.MC_Numero = mc;

            // Foto
            var savedRelative = SaveLocalPhoto(_selectedPhotoPath);
            if (!string.IsNullOrWhiteSpace(savedRelative))
                Result.Foto = savedRelative;
            else if (string.IsNullOrWhiteSpace(Result.Foto))
                Result.Foto = "";

            DialogResult = DialogResult.OK;
        }

        // ===== UI: Buscar foto =====
        private void buttonBuscarFoto_Click(object sender, EventArgs e)
        {
            using var imagenSelector = new OpenFileDialog
            {
                Title = "Seleccione una imagen",
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Todos los archivos|*.*"
            };

            if (imagenSelector.ShowDialog() == DialogResult.OK)
            {
                _selectedPhotoPath = imagenSelector.FileName;
                try
                {
                    // Carga sin bloquear el archivo seleccionado
                    using var fs = new FileStream(_selectedPhotoPath, FileMode.Open, FileAccess.Read);
                    using var temp = Image.FromStream(fs);
                    fotoEmpleado.Image = new Bitmap(temp);

                    // Nueva implementacion: asegurar dimensionado coherente al previsualizar
                    fotoEmpleado.SizeMode = PictureBoxSizeMode.Zoom; // o StretchImage si quieres paridad exacta
                }
                catch
                {
                    MessageBox.Show("No se pudo cargar la imagen seleccionada.", "Imagen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _selectedPhotoPath = null;
                }
            }
        }
    }
}
