// Nueva implementacion
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Manga_Rica_P1.Entity;

// ✅ Alias para la entidad, así evitamos choques con namespaces
using EntitySemana = Manga_Rica_P1.Entity.Semana;

namespace Manga_Rica_P1.UI.Semanas.Modales
{
    public class AddSemana : Form
    {
        private readonly ComboBox cboSemana = new ComboBox();
        private readonly DateTimePicker dtInicio = new DateTimePicker();
        private readonly DateTimePicker dtFinal = new DateTimePicker();
        private readonly Button btnGuardar = new Button();
        private readonly Button btnCancelar = new Button();

        // ✅ El modal devuelve directamente la entidad (alias)
        public EntitySemana Result { get; private set; }

        // ✅ Usa el alias en la firma
        public AddSemana(EntitySemana seed = null)
        {
            var esEdicion = seed != null;
            Text = esEdicion ? "Editar Semana" : "Nueva Semana";

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(460, 230);

            var lblSemana = new Label { Text = "Semana (1–20)", AutoSize = true, Left = 20, Top = 20 };
            cboSemana.Left = 20; cboSemana.Top = 45; cboSemana.Width = 120;
            cboSemana.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSemana.Items.AddRange(Enumerable.Range(1, 20).Cast<object>().ToArray());

            var lblInicio = new Label { Text = "Fecha inicio", AutoSize = true, Left = 160, Top = 20 };
            dtInicio.Left = 160; dtInicio.Top = 45; dtInicio.Width = 260; dtInicio.Format = DateTimePickerFormat.Short;

            var lblFinal = new Label { Text = "Fecha final", AutoSize = true, Left = 160, Top = 85 };
            dtFinal.Left = 160; dtFinal.Top = 110; dtFinal.Width = 260; dtFinal.Format = DateTimePickerFormat.Short;

            btnGuardar.Text = "Guardar";
            btnGuardar.Left = 260; btnGuardar.Top = 170; btnGuardar.Width = 80;

            btnCancelar.Text = "Cancelar";
            btnCancelar.Left = 350; btnCancelar.Top = 170; btnCancelar.Width = 80;

            btnGuardar.Click += (_, __) => OnSave();
            btnCancelar.Click += (_, __) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[] { lblSemana, cboSemana, lblInicio, dtInicio, lblFinal, dtFinal, btnGuardar, btnCancelar });

            // Inicializar Result
            Result = esEdicion
                ? new EntitySemana { Id = seed.Id, semana = seed.semana, fecha_Inicio = seed.fecha_Inicio, fecha_Final = seed.fecha_Final }
                : new EntitySemana();

            if (esEdicion)
            {
                cboSemana.SelectedItem = seed.semana;
                dtInicio.Value = seed.fecha_Inicio == default ? DateTime.Today : seed.fecha_Inicio;
                dtFinal.Value = seed.fecha_Final == default ? DateTime.Today : seed.fecha_Final;
            }
            else
            {
                cboSemana.SelectedIndex = 0; // semana 1
                dtInicio.Value = DateTime.Today;
                dtFinal.Value = DateTime.Today;
            }
        }

        private void OnSave()
        {
            if (cboSemana.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un número de semana (1–20).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSemana.DroppedDown = true;
                return;
            }

            var inicio = dtInicio.Value.Date;
            var final = dtFinal.Value.Date;
            if (final < inicio)
            {
                MessageBox.Show("La fecha final no puede ser menor que la fecha de inicio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtFinal.Focus();
                return;
            }

            // Asignar directamente sobre la entidad (alias)
            Result.semana = (int)cboSemana.SelectedItem;
            Result.fecha_Inicio = inicio;
            Result.fecha_Final = final;

            DialogResult = DialogResult.OK;
        }
    }
}
