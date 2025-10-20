using Manga_Rica_P1.BLL;
using Manga_Rica_P1.BLL.Session;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Pagos
{
    public partial class ActivarPagos : Form
    {
        private readonly ActivarPagosService _service;
        private readonly SemanasService _semanasService;
        private readonly IAppSession _session;

        public ActivarPagos(ActivarPagosService service, SemanasService semanasService, IAppSession session)
        {
            InitializeComponent();
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _semanasService = semanasService ?? throw new ArgumentNullException(nameof(semanasService));
            _session = session ?? throw new ArgumentNullException(nameof(session));

            // Eventos
            Load += ActivarPagos_Load;
            buttonActivarPagos.Click += ButtonActivarPagos_Click;

            // UX
            AcceptButton = buttonActivarPagos;
        }

        private void ActivarPagos_Load(object? sender, EventArgs e)
        {
            CargarSemanas();
        }

        private void CargarSemanas()
        {
            try
            {
                // Puedes usar el de ActivarPagosService o el de SemanasService.
                // Aquí uso el de ActivarPagosService para mantener una fuente única.
                var semanas = _service.GetSemanas(); // List<SemanaDto> { Id, Semana }

                comboSemana.DataSource = semanas;
                comboSemana.DisplayMember = "Semana";
                comboSemana.ValueMember = "Id";
                if (semanas.Any())
                    comboSemana.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar semanas: {ex.Message}", "Semanas",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonActivarPagos.Enabled = false;
            }
        }

        private void ButtonActivarPagos_Click(object? sender, EventArgs e)
        {
            if (comboSemana.SelectedValue is null)
            {
                MessageBox.Show("Debe seleccionar una semana.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboSemana.Focus();
                return;
            }

            if (_session.CurrentUser is null)
            {
                MessageBox.Show("No hay usuario autenticado.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idSemana = (int)comboSemana.SelectedValue;
            var idUsuario = _session.CurrentUser.Id;

            try
            {
                buttonActivarPagos.Enabled = false;
                Cursor = Cursors.WaitCursor;

                var (insertados, total) = _service.ActivarSemana(idSemana, idUsuario);

                MessageBox.Show(
                    $"Pagos activados.\n\nEmpleados activos: {total}\nRegistros creados: {insertados}",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (InvalidOperationException iex)
            {
                // Regla de negocio: ya estaba activada
                MessageBox.Show(iex.Message, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al activar pagos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                buttonActivarPagos.Enabled = true;
            }
        }
    }
}
