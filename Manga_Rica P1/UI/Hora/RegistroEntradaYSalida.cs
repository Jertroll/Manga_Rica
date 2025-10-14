// Nueva implementacion
using System;
using System.Windows.Forms;
using Manga_Rica_P1.BLL;  // ← para HorasService

namespace Manga_Rica_P1.UI.Hora
{
    public partial class RegistroEntradaYSalida : Form
    {
        // Cache para precarga
        private long? _carne;
        private string _nombre = string.Empty;
        private string _ape1 = string.Empty;
        private string _ape2 = string.Empty;

        // Nueva implementacion: dependencias
        private readonly HorasService _horasService;
        private readonly int _usuarioId;

        // Nueva implementacion: ctor inyectado
        public RegistroEntradaYSalida(HorasService horasService, int usuarioActualId)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            _horasService = horasService ?? throw new ArgumentNullException(nameof(horasService));
            _usuarioId = usuarioActualId;

            // Defaults de fecha/hora (evita mezclar fechas de hoy con horas del control)
            dateTimePickerFechaEntrada.Value = DateTime.Today;
            dateTimePickerRegistroEntrada.Value = DateTime.Now;
            dateTimePickerSalida.Value = DateTime.Today;
            dateTimePickerRegistroSalida.Value = DateTime.Now;

            // Wire-up de botones
            buttonRegistrarEntrada.Click += ButtonRegistrarEntrada_Click;
            buttonRegistrarSalida.Click += ButtonRegistrarSalida_Click;
            buttonCancelarEntrada.Click += (_, __) => Close();
            buttonCancelarSalida.Click += (_, __) => Close();

            // Buscar (rellena solo el carné en ambos tabs)
            BttnBuscarEmpleado.Click += (_, __) =>
            {
                if (long.TryParse(textBoxBuscarEmpleado.Text?.Trim(), out var carne))
                {
                    textBoxCarneEntrada.Text = carne.ToString();
                    textBoxCarneSalida.Text = carne.ToString();
                }
                else
                {
                    MessageBox.Show("Ingrese un número de carné válido.", "Asistencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            // Al cambiar de tab, re-aplica precarga
            tabControlSalida.SelectedIndexChanged += (_, __) => ApplyPrefillToBothTabs();
        }

        /// <summary>
        /// Precarga datos del empleado y los aplica a ambos tabs (Entrada y Salida).
        /// </summary>
        public void PrefillEmpleado(long carne, string nombre, string ape1, string ape2)
        {
            _carne = carne;
            _nombre = nombre ?? string.Empty;
            _ape1 = ape1 ?? string.Empty;
            _ape2 = ape2 ?? string.Empty;

            ApplyPrefillToBothTabs();
        }

        // Aplica la cache a los controles de Entrada y Salida
        private void ApplyPrefillToBothTabs()
        {
            if (_carne == null) return;

            // --- Tab Entrada ---
            if (textBoxCarneEntrada != null) textBoxCarneEntrada.Text = _carne.Value.ToString();
            if (textBoxNombreEntrada != null) textBoxNombreEntrada.Text = _nombre;
            if (textBoxApellido1Entrada != null) textBoxApellido1Entrada.Text = _ape1;
            // (si quieres segundo apellido, agrégalo al designer y asígnalo aquí)

            // --- Tab Salida ---
            if (textBoxCarneSalida != null) textBoxCarneSalida.Text = _carne.Value.ToString();
            if (textBoxNombreSalida != null) textBoxNombreSalida.Text = _nombre;
            if (textBoxApellidoSalida != null) textBoxApellidoSalida.Text = _ape1;
        }

        // Nueva implementacion: click Registrar ENTRADA
        private void ButtonRegistrarEntrada_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!long.TryParse(textBoxCarneEntrada.Text?.Trim(), out var carne))
                    throw new InvalidOperationException("Carne inválido.");

                var f = dateTimePickerFechaEntrada.Value.Date;
                var h = dateTimePickerRegistroEntrada.Value;
                // Combina fecha seleccionada + hora seleccionada (evita “fecha de hoy” del control Time)
                var horaEntrada = new DateTime(f.Year, f.Month, f.Day, h.Hour, h.Minute, 0);

                _horasService.RegistrarEntrada(carne, f, horaEntrada, _usuarioId);
                MessageBox.Show("Entrada registrada correctamente.", "Asistencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al registrar entrada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Nueva implementacion: click Registrar SALIDA
        private void ButtonRegistrarSalida_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!long.TryParse(textBoxCarneSalida.Text?.Trim(), out var carne))
                    throw new InvalidOperationException("Carne inválido.");

                var f = dateTimePickerSalida.Value.Date;
                var h = dateTimePickerRegistroSalida.Value;
                var horaSalida = new DateTime(f.Year, f.Month, f.Day, h.Hour, h.Minute, 0);

                _horasService.RegistrarSalida(carne, horaSalida);
                MessageBox.Show("Salida registrada correctamente.", "Asistencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al registrar salida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // (Tu método vacío del click de tab se puede eliminar si no se usa)
        private void tabPageEntrada_Click(object sender, EventArgs e) { }
    }
}
