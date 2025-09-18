// Nueva implementacion
using Manga_Rica_P1.Entity;
using Manga_Rica_P1.ENTITY;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Empleados.Modales
{
    public partial class AddEmpleado : Form
    {
        // Nueva implementacion: el modal devuelve la entidad final
        // IMPORTANTE: asegúrate que ENTITY.Empleado sea public (no internal)
        public Empleado Result { get; private set; } = new Empleado();

        // Si quieres abrir “vacío”, usa el ctor por defecto generado por el diseñador.
        public AddEmpleado()
        {
            InitializeComponent();
            // Nueva implementacion: enganchamos eventos y cargamos combos en Load
            this.Load += AddEmpleado_Load;
        }

        // Nueva implementacion: utilidad para cargar listas por defecto (cuando no haya BLL/DAL)
        private void EnsureCombosLoaded()
        {
            // Estado civil
            if (comboBoxEstadoCivil.Items.Count == 0)
                comboBoxEstadoCivil.Items.AddRange(new object[] { "Soltero", "Casado", "Unión Libre", "Divorciado", "Viudo" });

            // Departamento (por ahora quemado)
            if (comboBoxDepartamento.Items.Count == 0)
                comboBoxDepartamento.Items.AddRange(new object[] { "Administración", "Ventas", "Producción", "RRHH", "TI" });

            // Laboró (0/1 mostrado como texto)
            if (comboBoxLaboro.Items.Count == 0)
                comboBoxLaboro.Items.AddRange(new object[] { "No", "Sí" }); // 0,1

            // Activo (0/1 mostrado como texto)
            if (comboBoxActivo.Items.Count == 0)
                comboBoxActivo.Items.AddRange(new object[] { "No", "Sí" }); // 0,1

            // Selecciones por defecto
            if (comboBoxEstadoCivil.SelectedIndex < 0 && comboBoxEstadoCivil.Items.Count > 0) comboBoxEstadoCivil.SelectedIndex = 0;
            if (comboBoxDepartamento.SelectedIndex < 0 && comboBoxDepartamento.Items.Count > 0) comboBoxDepartamento.SelectedIndex = 0;
            if (comboBoxLaboro.SelectedIndex < 0 && comboBoxLaboro.Items.Count > 0) comboBoxLaboro.SelectedIndex = 0;
            if (comboBoxActivo.SelectedIndex < 0 && comboBoxActivo.Items.Count > 0) comboBoxActivo.SelectedIndex = 1; // Activo = Sí
        }

        // Nueva implementacion: autocompleta desde una Solicitud seleccionada
        public void PrefillFromSolicitud(Solicitud s)
        {
            // Asegura que los combos tengan ítems (si llaman esto antes de Load)
            EnsureCombosLoaded();

            // Personales
            textBoxCedula.Text = s.Cedula;
            textBoxApellido1.Text = s.Primer_Apellido;
            textBoxAppelido2.Text = s.Segundo_Apellido ?? string.Empty;
            textBoxNombre.Text = s.Nombre;
            dateTimeNacimiento.Value = s.Fecha_Nacimiento == default ? DateTime.Today.AddYears(-18) : s.Fecha_Nacimiento;

            // Estado civil: intenta coincidir con el texto; si no, deja el índice 0
            var matchEstado = comboBoxEstadoCivil.Items.Cast<object>()
                .FirstOrDefault(x => string.Equals(x.ToString(), s.Estado_Civil, StringComparison.InvariantCultureIgnoreCase));
            comboBoxEstadoCivil.SelectedItem = matchEstado ?? comboBoxEstadoCivil.Items[0];

            textBoxCelular.Text = s.Celular;
            textBoxNacionalidad.Text = s.Nacionalidad;
            comboBoxLaboro.SelectedIndex = s.Laboro == 1 ? 1 : 0; // 1 = Sí, 0 = No
            textBoxDireccion.Text = s.Direccion;

            // Empleo: sugerencias por defecto, el usuario completa
            if (string.IsNullOrWhiteSpace(textBox1.Text)) textBox1.Text = "";      // Puesto
            if (string.IsNullOrWhiteSpace(textBoxSalario.Text)) textBoxSalario.Text = "0";
            dateTimePicker1.Value = DateTime.Today;              // Ingreso
            // dateTimePicker2 (Salida) lo puede dejar en la fecha mostrada
            comboBoxActivo.SelectedIndex = 1; // Activo = Sí
        }

        // Nueva implementacion: (opcional) precarga completa desde un Empleado (para editar)
        public void PrefillFromEmpleado(Empleado e)
        {
            EnsureCombosLoaded();

            textBoxCedula.Text = e.Cedula;
            textBoxApellido1.Text = e.Primer_Apellido;
            textBoxAppelido2.Text = e.Segundo_Apellido ?? string.Empty;
            textBoxNombre.Text = e.Nombre;
            dateTimeNacimiento.Value = e.Fecha_Nacimiento == default ? DateTime.Today.AddYears(-18) : e.Fecha_Nacimiento;

            var matchEstado = comboBoxEstadoCivil.Items.Cast<object>()
                .FirstOrDefault(x => string.Equals(x.ToString(), e.Estado_Civil, StringComparison.InvariantCultureIgnoreCase));
            comboBoxEstadoCivil.SelectedItem = matchEstado ?? comboBoxEstadoCivil.Items[0];

            textBoxCelular.Text = e.Celular;
            textBoxNacionalidad.Text = e.Nacionalidad;
            comboBoxLaboro.SelectedIndex = e.Laboro == 1 ? 1 : 0;
            textBoxDireccion.Text = e.Direccion;

            // Empleo
            // Nota: sin BLL/DAL usamos SelectedIndex como "Id_Departamento" demo.
            if (comboBoxDepartamento.Items.Count > 0)
                comboBoxDepartamento.SelectedIndex = Math.Max(0, Math.Min(comboBoxDepartamento.Items.Count - 1, e.Id_Departamento));
            textBoxSalario.Text = e.Salario.ToString(CultureInfo.InvariantCulture);
            textBox1.Text = e.Puesto; // (textBox1 = Puesto según tu Designer)
            dateTimePicker1.Value = e.Fecha_Ingreso == default ? DateTime.Today : e.Fecha_Ingreso;
            dateTimePicker2.Value = e.Fecha_Salida == default ? DateTime.Today : e.Fecha_Salida;
            textBoxFoto.Text = e.Foto ?? "";
            comboBoxActivo.SelectedIndex = e.Activo == 1 ? 1 : 0;
            textBoxMcNumero.Text = e.MC_Numero.ToString();
        }

        // ====== Eventos ======
        private void AddEmpleado_Load(object? sender, EventArgs e)
        {
            EnsureCombosLoaded();

            // Nueva implementacion: botones
            buttonGuardar.Click += (_, __) => GuardarYSalir();
            buttonCancelar.Click += (_, __) => DialogResult = DialogResult.Cancel;
        }

        // ====== Guardar ======
        private void GuardarYSalir()
        {
            // Validaciones mínimas
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

            if (!float.TryParse(textBoxSalario.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float salario))
            {
                MessageBox.Show("Salario inválido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxSalario.Focus(); return;
            }

            long.TryParse(textBoxMcNumero.Text, out var mc);

            // Nueva implementacion: mapear controles -> entidad Result
            Result.Cedula = textBoxCedula.Text.Trim();
            Result.Primer_Apellido = textBoxApellido1.Text.Trim();
            Result.Segundo_Apellido = string.IsNullOrWhiteSpace(textBoxAppelido2.Text) ? null : textBoxAppelido2.Text.Trim();
            Result.Nombre = textBoxNombre.Text.Trim();
            Result.Fecha_Nacimiento = dateTimeNacimiento.Value.Date;
            Result.Estado_Civil = comboBoxEstadoCivil.SelectedItem?.ToString() ?? "";
            Result.Celular = textBoxCelular.Text.Trim();
            Result.Nacionalidad = textBoxNacionalidad.Text.Trim();
            Result.Laboro = comboBoxLaboro.SelectedIndex == 1 ? 1 : 0; // Sí=1, No=0
            Result.Direccion = textBoxDireccion.Text.Trim();

            // Por ahora usamos SelectedIndex como Id_Departamento (cuando tengas DAL, mapea Id real)
            Result.Id_Departamento = Math.Max(0, comboBoxDepartamento.SelectedIndex);
            Result.Salario = salario;
            Result.Puesto = textBox1.Text.Trim(); // textBox1 = Puesto (según tu Designer)
            Result.Fecha_Ingreso = dateTimePicker1.Value.Date; // Ingreso
            Result.Fecha_Salida = dateTimePicker2.Value.Date; // Salida (puede ser default si no aplica)
            Result.Foto = textBoxFoto.Text.Trim();
            Result.Activo = comboBoxActivo.SelectedIndex == 1 ? 1 : 0; // Sí=1, No=0
            Result.MC_Numero = mc;

            DialogResult = DialogResult.OK;
        }
    }
}
