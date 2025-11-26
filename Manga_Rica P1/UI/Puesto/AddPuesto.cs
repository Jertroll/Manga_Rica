using System;
using System.Windows.Forms;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.UI.Puesto
{
    public partial class AddPuesto : Form
    {
        /// <summary>
        /// Resultado del formulario: el Puesto capturado/ editado.
        /// </summary>
        public PuestoEntity Result { get; private set; }

        // Constructor para "Nuevo"
        public AddPuesto()
        {
            InitializeComponent();

            // Valor por defecto
            Result = new PuestoEntity();

            Text = "Agregar Puesto";
            btnAgregar.Text = "Agregar";

            WireEvents();
        }

        // Constructor para "Editar"
        public AddPuesto(PuestoEntity existente) : this()
        {
            if (existente is null)
                throw new ArgumentNullException(nameof(existente));

            // Ajustar título y texto del botón
            Text = "Editar Puesto";
            btnAgregar.Text = "Guardar";

            // Cargar valores en los controles
            textBoxPuesto.Text = existente.puesto;
            textBoxDescripcion.Text = existente.descripcion;

            // Inicializar el Result con el Id original (importante para Update)
            Result = new PuestoEntity
            {
                Id = existente.Id,
                puesto = existente.puesto,
                descripcion = existente.descripcion
            };
        }

        private void WireEvents()
        {
            btnCancelar.Click += (_, __) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            btnAgregar.Click += btnAgregar_Click;
        }

        private void btnAgregar_Click(object? sender, EventArgs e)
        {
            var nombre = textBoxPuesto.Text?.Trim() ?? "";
            var desc = textBoxDescripcion.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show(this,
                    "El nombre del puesto es obligatorio.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                textBoxPuesto.Focus();
                return;
            }
            Result.puesto = nombre;
            Result.descripcion = desc;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }
    }
}
