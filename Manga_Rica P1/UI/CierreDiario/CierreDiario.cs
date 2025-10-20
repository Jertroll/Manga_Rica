using System;
using System.Windows.Forms;
using Manga_Rica_P1.BLL;

namespace Manga_Rica_P1.UI.CierreDiario
{
    public partial class CierreDiario : Form
    {
        private readonly CierreDiarioService _service;

        // <- Recibe el servicio como los otros módulos
        public CierreDiario(CierreDiarioService service)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            _service = service ?? throw new ArgumentNullException(nameof(service));

            buttonCierreDiario.Click += buttonCierreDiario_Click;
        }

        private void buttonCierreDiario_Click(object? sender, EventArgs e)
        {
            var fecha = monthCalendarCierreDiario.SelectionStart.Date;
            var feriado = checkBoxFeriado.Checked;
            var domingo = checkBoxDomingo.Checked;

            try
            {
                var (insertados, total) = _service.CerrarDia(fecha, feriado, domingo);
                MessageBox.Show(
                    $"Cierre diario registrado para {fecha:d}.\n" +
                    $"Empleados procesados: {total}\n" +
                    $"Filas insertadas: {insertados}",
                    "Cierre Diario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cierre Diario",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
