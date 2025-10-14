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

namespace Manga_Rica_P1.UI.Uniforme
{
    public partial class BuscadorUniforme : Form
    {
        private readonly DeduccionesService _deduccionesService;
        private List<DeduccionResumenUniforme> _deducciones = new();

        public long? DeduccionSeleccionadaId { get; private set; }

        public BuscadorUniforme(DeduccionesService deduccionesService)
        {
            InitializeComponent();
            _deduccionesService = deduccionesService ?? throw new ArgumentNullException(nameof(deduccionesService));
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            // Configurar DataGridView
            dataGridView1.AutoGenerateColumns = false;
            // Las columnas ya están definidas en el Designer
            
            // Configurar propiedades de binding para las columnas
            ColumnaCarnet.DataPropertyName = "Carne";
            ColumnaNombre.DataPropertyName = "Nombre";
            ColumnaIdUniforme.DataPropertyName = "Id";
            ColumnaFecha.DataPropertyName = "Fecha";
            ColumnaTotal.DataPropertyName = "Total";
            ColumnaAnulada.DataPropertyName = "Anulada";
            
            // Configurar formato de fecha y total
            ColumnaFecha.DefaultCellStyle.Format = "dd/MM/yyyy";
            ColumnaTotal.DefaultCellStyle.Format = "N2";
            ColumnaTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Cargar datos iniciales
            CargarDeducciones();

            // Eventos
            buttonBuscar.Click += ButtonBuscar_Click;
            buttonAceptar.Click += ButtonAceptar_Click;
            textBox1.KeyDown += TextBox1_KeyDown;
            dataGridView1.DoubleClick += DataGridView1_DoubleClick;
        }

        private void CargarDeducciones(string? filtro = null)
        {
            try
            {
                _deducciones = _deduccionesService.BuscarDeduccionesSimple(filtro);
                dataGridView1.DataSource = _deducciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar deducciones: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonBuscar_Click(object? sender, EventArgs e)
        {
            var filtro = string.IsNullOrWhiteSpace(textBox1.Text) ? null : textBox1.Text.Trim();
            CargarDeducciones(filtro);
        }

        private void TextBox1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ButtonBuscar_Click(sender, e);
            }
        }

        private void ButtonAceptar_Click(object? sender, EventArgs e)
        {
            SeleccionarDeduccion();
        }

        private void DataGridView1_DoubleClick(object? sender, EventArgs e)
        {
            SeleccionarDeduccion();
        }

        private void SeleccionarDeduccion()
        {
            if (dataGridView1.CurrentRow != null && 
                dataGridView1.CurrentRow.DataBoundItem is DeduccionResumenUniforme deduccion)
            {
                DeduccionSeleccionadaId = deduccion.Id;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una deducción", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            DeduccionSeleccionadaId = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
