using Manga_Rica_P1.BLL;
using Manga_Rica_P1.BLL.Pagos;
using Manga_Rica_P1.BLL.Session;
using Manga_Rica_P1.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Manga_Rica_P1.BLL.Pagos.PagosService;

namespace Manga_Rica_P1.UI.Pagos
{
    public partial class RegistroPagos : UserControl
    {
        private readonly PagosService _pagosService;
        private readonly SemanasService _semanasService;
        private readonly EmpleadosService _empleadosService;
        private readonly ToolTip toolTip1;

        private int? _idSemanaSel;
        private long? _idEmpleadoSel;
        private PagoPreview? _previewActual;
        private readonly IAppSession _session;

        // >>> Factores usados para el cálculo del bruto desde horas
        private const float FACTOR_NORMAL = 1.0f;
        private const float FACTOR_EXTRA = 1.5f;
        private const float FACTOR_DOBLE = 2.0f;
        private const float FACTOR_FERIADO = 2.0f;

        public RegistroPagos(
            PagosService pagosService,
            SemanasService semanasService,
            EmpleadosService empleadosService,
            IAppSession session)
        {
            InitializeComponent();

            CarneColumna.DataPropertyName = "Carne";
            NombreColumna.DataPropertyName = "Nombre";
            Apellido1Columna.DataPropertyName = "Apellido";

            _pagosService = pagosService ?? throw new ArgumentNullException(nameof(pagosService));
            _semanasService = semanasService ?? throw new ArgumentNullException(nameof(semanasService));
            _empleadosService = empleadosService ?? throw new ArgumentNullException(nameof(empleadosService));
            _session = session ?? throw new ArgumentNullException(nameof(session));
            toolTip1 = new ToolTip();

            // ---------------- Eventos de recálculo ----------------

            // Cambios que afectan directamente el cálculo de BRUTO (salario + horas)
            textBoxSalario.TextChanged += (_, __) => RecalcularBrutoYNetoDesdeUI();
            textBoxHorasNormales.TextChanged += (_, __) => RecalcularBrutoYNetoDesdeUI();
            textBoxHorasExtras.TextChanged += (_, __) => RecalcularBrutoYNetoDesdeUI();
            textBoxHorasDobles.TextChanged += (_, __) => RecalcularBrutoYNetoDesdeUI();
            textBoxFeriados.TextChanged += (_, __) => RecalcularBrutoYNetoDesdeUI();

            // Cambios de deducciones también disparan recálculo
            textBoxSoda.TextChanged += (_, __) => RecalcularBrutoYNetoDesdeUI();
            textBoxUniforme.TextChanged += (_, __) => RecalcularBrutoYNetoDesdeUI();
            textBoxOtras.TextChanged += (_, __) => RecalcularBrutoYNetoDesdeUI();

            // Si se edita manualmente el BRUTO, sólo recalculamos el NETO
            textBoxPagoBruto.TextChanged += (_, __) => RecalcularSoloNetoDesdeUI();

            comboBoxSemana.SelectedIndexChanged += (_, __) => CargarPendientesDeSemana();
            dataGridViewEmpleados.CellDoubleClick += (_, __) => SeleccionarEmpleadoDeGrilla();
            textBoxCarnet.KeyDown += TextBoxCarnet_KeyDown;
        }

        private void RegistroPagos_Load(object sender, EventArgs e)
        {
            CargarSemanas();
        }

        // ----------------- Carga de catálogos / listas -----------------

        private void CargarSemanas()
        {
            var semanas = _semanasService.GetAll().ToList();
            comboBoxSemana.DisplayMember = "semana";
            comboBoxSemana.ValueMember = "Id";
            comboBoxSemana.DataSource = semanas;

            if (semanas.Count > 0)
            {
                comboBoxSemana.SelectedIndex = 0;
                _idSemanaSel = semanas[0].Id;
            }
        }

        private void CargarPendientesDeSemana()
        {
            LimpiarCampos();

            if (comboBoxSemana.SelectedValue is int idSemana)
            {
                _idSemanaSel = idSemana;

                var ids = _pagosService.GetEmpleadosPendientesIds(idSemana);
                var filas = new BindingList<dynamic>();
                foreach (var id in ids)
                {
                    var emp = _empleadosService.GetById(id);
                    if (emp == null) continue;
                    filas.Add(new
                    {
                        Carne = emp.Carne,
                        Nombre = emp.Nombre,
                        Apellido = emp.Primer_Apellido,
                        Id = emp.Id
                    });
                }

                dataGridViewEmpleados.AutoGenerateColumns = false;
                dataGridViewEmpleados.DataSource = filas;
            }
        }

        // ----------------- Búsqueda / selección -----------------

        private void TextBoxCarnet_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            var carnet = textBoxCarnet.Text?.Trim();
            if (string.IsNullOrEmpty(carnet)) return;

            // Busca en la grilla y selecciona la fila
            foreach (DataGridViewRow row in dataGridViewEmpleados.Rows)
            {
                if (row.Cells["CarneColumna"].Value?.ToString() == carnet)
                {
                    row.Selected = true;
                    dataGridViewEmpleados.CurrentCell = row.Cells[0];
                    SeleccionarEmpleadoDeGrilla();
                    return;
                }
            }

            // Si no está en pendientes, intentamos cargarlo igual
            var emp = _empleadosService.GetByCarne(carnet);
            if (emp != null)
            {
                _idEmpleadoSel = emp.Id;
                CargarPreview();
            }
        }

        private void SeleccionarEmpleadoDeGrilla()
        {
            if (dataGridViewEmpleados.CurrentRow is null) return;

            var boundObj = (dynamic?)dataGridViewEmpleados.CurrentRow.DataBoundItem;
            if (boundObj == null) return;
            _idEmpleadoSel = (long)boundObj.Id;
            textBoxCarnet.Text = boundObj.Carne.ToString() ?? "";
            CargarPreview();
        }

        // ----------------- Cálculo / mostrador -----------------

        private void CargarPreview()
        {
            if (_idEmpleadoSel is null || _idSemanaSel is null) return;

            _previewActual = _pagosService.GetPreview(_idEmpleadoSel.Value, _idSemanaSel.Value);

            textBoxNombre.Text = _previewActual.NombreCompleto;
            textBoxSalario.Text = _previewActual.SalarioHora.ToString("0.00");

            textBoxHorasNormales.Text = _previewActual.Normales.ToString("0.##");
            textBoxHorasExtras.Text = _previewActual.Extras.ToString("0.##");
            textBoxHorasDobles.Text = _previewActual.Dobles.ToString("0.##");
            textBoxFeriados.Text = _previewActual.Feriado.ToString("0.##");

            textBoxSoda.Text = _previewActual.Soda.ToString("0.00");
            textBoxUniforme.Text = _previewActual.DeduccionUniforme.ToString("0.00");
            textBoxOtras.Text = _previewActual.DeduccionOtras.ToString("0.00");

            textBoxPagoBruto.Text = _previewActual.Bruto.ToString("0.00");
            textBoxPagoNeto.Text = _previewActual.Neto.ToString("0.00");

            // Cargar foto del empleado (mismo enfoque que en Soda)
            var emp = _empleadosService.GetById(_idEmpleadoSel.Value);
            if (emp != null)
            {
                CargarFotoEmpleado(emp.Foto);
            }
            else
            {
                CargarFotoEmpleado(null);
            }

            // (Opcional) mostrar aviso si ya estaba registrado
            if (_previewActual.YaRegistrado)
                toolTip1.SetToolTip(buttonGuardar, "Este pago ya estaba registrado. No se volverán a aplicar saldos.");
        }

        private float ParseFloat(string? s)
            => float.TryParse(s, out var v) ? v : 0f;

        // >>> Recalcula BRUTO y luego NETO en función de salario + horas + deducciones
        private void RecalcularBrutoYNetoDesdeUI()
        {
            // Salario por hora
            float salario = ParseFloat(textBoxSalario.Text);

            // Horas
            float hN = ParseFloat(textBoxHorasNormales.Text);
            float hE = ParseFloat(textBoxHorasExtras.Text);
            float hD = ParseFloat(textBoxHorasDobles.Text);
            float hF = ParseFloat(textBoxFeriados.Text);

            // Bruto según reglas
            float bruto = salario * (
                  FACTOR_NORMAL * hN
                + FACTOR_EXTRA * hE
                + FACTOR_DOBLE * hD
                + FACTOR_FERIADO * hF
            );

            textBoxPagoBruto.Text = bruto.ToString("0.00");

            // Y con ese bruto recalculamos el neto
            RecalcularSoloNetoDesdeUI();
        }

        // Recalcula sólo el NETO (asumiendo que PagoBruto ya es correcto)
        private void RecalcularSoloNetoDesdeUI()
        {
            float bruto = ParseFloat(textBoxPagoBruto.Text);
            float u = ParseFloat(textBoxUniforme.Text);
            float s = ParseFloat(textBoxSoda.Text);
            float o = ParseFloat(textBoxOtras.Text);

            textBoxPagoNeto.Text = (bruto - u - s - o).ToString("0.00");
        }

        /// <summary>
        /// Lee y valida todos los valores numéricos de la UI.
        /// Devuelve false si hay algún error de validación.
        /// </summary>
        private bool TryLeerValoresDesdeUi(
            out float salario,
            out float horasNormales,
            out float horasExtras,
            out float horasDobles,
            out float feriados,
            out float dedSoda,
            out float dedUniforme,
            out float dedOtras,
            out float pagoBruto,
            out float pagoNeto,
            out string mensajeError)
        {
            salario = horasNormales = horasExtras = horasDobles = feriados =
                dedSoda = dedUniforme = dedOtras = pagoBruto = pagoNeto = 0f;
            mensajeError = string.Empty;

            if (!ParseCampo(textBoxSalario, "Salario por hora", out salario, ref mensajeError)) return false;
            if (!ParseCampo(textBoxHorasNormales, "Horas normales", out horasNormales, ref mensajeError)) return false;
            if (!ParseCampo(textBoxHorasExtras, "Horas extras", out horasExtras, ref mensajeError)) return false;
            if (!ParseCampo(textBoxHorasDobles, "Horas dobles", out horasDobles, ref mensajeError)) return false;
            if (!ParseCampo(textBoxFeriados, "Feriados", out feriados, ref mensajeError)) return false;
            if (!ParseCampo(textBoxSoda, "Soda", out dedSoda, ref mensajeError)) return false;
            if (!ParseCampo(textBoxUniforme, "Uniforme", out dedUniforme, ref mensajeError)) return false;
            if (!ParseCampo(textBoxOtras, "Otras deducciones", out dedOtras, ref mensajeError)) return false;
            if (!ParseCampo(textBoxPagoBruto, "Pago bruto", out pagoBruto, ref mensajeError)) return false;
            if (!ParseCampo(textBoxPagoNeto, "Pago neto", out pagoNeto, ref mensajeError)) return false;

            return true;
        }

        /// <summary>
        /// Helper estático para validar y parsear un TextBox a float.
        /// </summary>
        private static bool ParseCampo(
            TextBox txt,
            string nombreCampo,
            out float valor,
            ref string mensajeError)
        {
            valor = 0f;
            var texto = txt.Text?.Trim();

            // Vacío se interpreta como 0
            if (string.IsNullOrEmpty(texto))
            {
                valor = 0f;
                return true;
            }

            if (!float.TryParse(texto, out valor))
            {
                mensajeError = $"El campo '{nombreCampo}' no tiene un número válido.";
                txt.Focus();
                txt.SelectAll();
                return false;
            }

            if (valor < 0)
            {
                mensajeError = $"El campo '{nombreCampo}' no puede ser negativo.";
                txt.Focus();
                txt.SelectAll();
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            _idEmpleadoSel = null;
            _previewActual = null;

            textBoxNombre.Clear();
            textBoxSalario.Clear();
            textBoxHorasNormales.Clear();
            textBoxHorasExtras.Clear();
            textBoxHorasDobles.Clear();
            textBoxFeriados.Clear();
            textBoxSoda.Clear();
            textBoxUniforme.Clear();
            textBoxOtras.Clear();
            textBoxPagoBruto.Clear();
            textBoxPagoNeto.Clear();

            // Limpiar imagen del empleado liberando recursos
            if (pictureBoxEmpleado.Image != null)
            {
                pictureBoxEmpleado.Image.Dispose();
                pictureBoxEmpleado.Image = null;
            }

            toolTip1.RemoveAll();
        }

        // ----------- Carga de foto de empleado -----------

        private void CargarFotoEmpleado(string? rutaFoto)
        {
            try
            {
                // Limpiar imagen anterior
                if (pictureBoxEmpleado.Image != null)
                {
                    pictureBoxEmpleado.Image.Dispose();
                    pictureBoxEmpleado.Image = null;
                }

                // Si no hay ruta, no mostramos nada
                if (string.IsNullOrEmpty(rutaFoto))
                {
                    return;
                }

                string? rutaEncontrada = null;

                string[] rutasAProbar = {
                    rutaFoto,
                    Path.Combine(Application.StartupPath, rutaFoto),
                    Path.Combine(Application.StartupPath, "Imagenes", rutaFoto),
                    Path.Combine(Application.StartupPath, "Imagenes", "Empleados", rutaFoto)
                };

                foreach (string ruta in rutasAProbar)
                {
                    if (File.Exists(ruta))
                    {
                        rutaEncontrada = ruta;
                        break;
                    }
                }

                if (rutaEncontrada == null)
                {
                    // No se encontró la imagen
                    return;
                }

                using (var fileStream = new FileStream(rutaEncontrada, FileMode.Open, FileAccess.Read))
                {
                    pictureBoxEmpleado.Image = new System.Drawing.Bitmap(fileStream);
                }

                pictureBoxEmpleado.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                pictureBoxEmpleado.Image = null;
                System.Diagnostics.Debug.WriteLine($"Error cargando foto empleado (Pagos): {ex.Message}");
            }
        }

        // ----------------- Guardar -----------------

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (_idEmpleadoSel is null || _idSemanaSel is null)
            {
                MessageBox.Show("Seleccione un empleado y una semana.", "Pagos",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var usuario = _session.CurrentUser
                ?? throw new InvalidOperationException("No hay usuario autenticado.");

            if (!TryLeerValoresDesdeUi(
                    out var salario,
                    out var horasNormales,
                    out var horasExtras,
                    out var horasDobles,
                    out var feriados,
                    out var dedSoda,
                    out var dedUniforme,
                    out var dedOtras,
                    out var pagoBruto,
                    out var pagoNeto,
                    out var mensajeError))
            {
                if (!string.IsNullOrEmpty(mensajeError))
                {
                    MessageBox.Show(
                        mensajeError,
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                return;
            }

            // Registrar usando exactamente los valores que hay en la UI
            _pagosService.RegistrarPagoSemanaManual(
                idEmpleado: _idEmpleadoSel.Value,
                idSemana: _idSemanaSel.Value,
                fechaCorte: DateTime.Today,
                idUsuario: usuario.Id,
                salarioHora: salario,
                horasNormales: horasNormales,
                horasExtras: horasExtras,
                horasDobles: horasDobles,
                feriados: feriados,
                dedSoda: dedSoda,
                dedUniforme: dedUniforme,
                dedOtras: dedOtras,
                salarioBruto: pagoBruto,
                salarioNeto: pagoNeto
            );

            MessageBox.Show("Pago registrado.", "Pagos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            CargarPendientesDeSemana();
            LimpiarCampos();
        }
    }
}
