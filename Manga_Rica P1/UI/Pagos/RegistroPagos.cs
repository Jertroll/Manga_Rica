using Manga_Rica_P1.BLL;
using Manga_Rica_P1.BLL.Pagos;
using Manga_Rica_P1.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public RegistroPagos(
            PagosService pagosService,
            SemanasService semanasService,
            EmpleadosService empleadosService)
        {
            InitializeComponent();

            CarneColumna.DataPropertyName = "Carne";
            NombreColumna.DataPropertyName = "Nombre";
            Apellido1Columna.DataPropertyName = "Apellido";

            _pagosService = pagosService;
            _semanasService = semanasService;
            _empleadosService = empleadosService;
            toolTip1 = new ToolTip();

            // UX: campos calculados no editables
            textBoxHorasNormales.ReadOnly = true;
            textBoxHorasExtras.ReadOnly = true;
            textBoxHorasDobles.ReadOnly = true;
            textBoxFeriados.ReadOnly = true;
            textBoxSalario.ReadOnly = true;
            textBoxPagoBruto.ReadOnly = true;
            textBoxPagoNeto.ReadOnly = true;

            // Editables como en el sistema viejo
            textBoxSoda.TextChanged += (_, __) => RecalcularNetoDesdeUI();
            textBoxUniforme.TextChanged += (_, __) => RecalcularNetoDesdeUI();
            textBoxOtras.TextChanged += (_, __) => RecalcularNetoDesdeUI();

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
            textBoxCarnet.Text = (string?)boundObj.Carne ?? "";
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

            // (Opcional) mostrar aviso si ya estaba registrado
            if (_previewActual.YaRegistrado)
                toolTip1.SetToolTip(buttonGuardar, "Este pago ya estaba registrado. No se volverán a aplicar saldos.");
        }

        private float ParseFloat(string? s)
            => float.TryParse(s, out var v) ? v : 0f;

        private void RecalcularNetoDesdeUI()
        {
            if (_previewActual == null) return;

            float bruto = ParseFloat(textBoxPagoBruto.Text);
            float u = ParseFloat(textBoxUniforme.Text);
            float s = ParseFloat(textBoxSoda.Text);
            float o = ParseFloat(textBoxOtras.Text);

            textBoxPagoNeto.Text = (bruto - u - s - o).ToString("0.00");
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
            pictureBoxEmpleado.Image = null;
            
            toolTip1.RemoveAll();
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

            _pagosService.RegistrarPagoSemana(_idEmpleadoSel.Value, _idSemanaSel.Value, DateTime.Today);

            MessageBox.Show("Pago registrado.", "Pagos",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            CargarPendientesDeSemana();
            LimpiarCampos();
        }
    }
}
