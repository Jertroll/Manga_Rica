using Manga_Rica_P1.Entity;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Departamentos
{
    /// <summary>
    /// Reglas de negocio portadas del módulo viejo:
    /// - Nombre (Departamento) es obligatorio.
    /// - En el módulo viejo no se pedía Código; aquí, si el usuario no lo indica,
    ///   se GENERA automáticamente a partir del nombre (para cumplir con el esquema actual).
    /// </summary>
    public class AddDepartamento : Form
    {
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtCodigo = new TextBox();
        private readonly Button btnGuardar = new Button();
        private readonly Button btnCancelar = new Button();

        // (Opcional en edición) Mostrar Id solo informativo
        private readonly Label lblId = new Label();

        public Departamento Result { get; private set; } = new Departamento();

        public AddDepartamento(Departamento? seed = null)
        {
            Text = seed == null ? "Nuevo Departamento" : "Editar Departamento";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(460, 210);

            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;

            var lblNombre = new Label { Text = "Departamento", AutoSize = true, Left = 20, Top = 20 };
            txtNombre.Left = 20; txtNombre.Top = 45; txtNombre.Width = 370;

            var lblCodigo = new Label { Text = "Código", AutoSize = true, Left = 20, Top = 80 };
            txtCodigo.Left = 20; txtCodigo.Top = 105; txtCodigo.Width = 370;

            // Botones: solo altura fija; el centrado lo calcula CenterButtons()
            btnGuardar.Text = "Guardar";
            btnGuardar.Width = 100;      // opcional (o quita y usa AutoSize)
            btnGuardar.Height = 30;

            btnCancelar.Text = "Cancelar";
            btnCancelar.Width = 100;
            btnCancelar.Height = 30;

            btnGuardar.Click += (_, __) => OnSave();
            btnCancelar.Click += (_, __) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[] { lblNombre, txtNombre, lblCodigo, txtCodigo, btnGuardar, btnCancelar });

            // Precarga en edición...
            if (seed != null)
            {
                Result = new Departamento { Id = seed.Id, nombre = seed.nombre, codigo = seed.codigo };
                txtNombre.Text = seed.nombre ?? string.Empty;
                txtCodigo.Text = seed.codigo ?? string.Empty;
            }

            // ⬇️ centra botones ahora y cada vez que cambie el tamaño (DPI, fuente, etc.)
            CenterButtons();
            this.SizeChanged += (_, __) => CenterButtons();
        }

        private void CenterButtons()
        {
            const int bottomMargin = 20; // distancia al borde inferior
            const int spacing = 12;      // separación horizontal entre botones

            // Si usas AutoSize = true en los botones, esto respeta el ancho calculado
            // btnGuardar.AutoSize = true; btnCancelar.AutoSize = true;

            int totalWidth = btnGuardar.Width + spacing + btnCancelar.Width;
            int startX = Math.Max(0, (ClientSize.Width - totalWidth) / 2);
            int top = ClientSize.Height - btnGuardar.Height - bottomMargin;

            btnGuardar.Top = top;
            btnCancelar.Top = top;

            btnGuardar.Left = startX;
            btnCancelar.Left = startX + btnGuardar.Width + spacing;

            // Asegura que no “salten” con anclajes laterales
            btnGuardar.Anchor = AnchorStyles.Bottom;
            btnCancelar.Anchor = AnchorStyles.Bottom;
        }

        private void OnSave()
        {
            var nombre = (txtNombre.Text ?? "").Trim();

            // Regla del módulo viejo: Nombre requerido
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre del departamento es requerido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            // Longitudes alineadas con BLL/DAL
            if (nombre.Length > 100)
            {
                MessageBox.Show("El nombre excede 100 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            // En el viejo no se exigía Código; aquí lo generamos si lo dejan vacío
            var codigo = (txtCodigo.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                codigo = SugerirCodigoDesdeNombre(nombre);
                txtCodigo.Text = codigo; // reflejar al usuario por transparencia
            }

            if (codigo.Length > 50)
            {
                MessageBox.Show("El código excede 50 caracteres.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return;
            }

            Result.nombre = nombre;
            Result.codigo = codigo;

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Genera un código a partir del nombre:
        /// - Mayúsculas, sin acentos
        /// - Solo letras y números
        /// - Acrónimo de palabras (2 letras por palabra hasta 4 palabras) o primeras 6 del nombre
        /// - Longitud máxima 12 (luego BLL/DAL permiten hasta 50)
        /// </summary>
        private static string SugerirCodigoDesdeNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "";

            // Normalizar: quitar acentos y caracteres especiales
            string sinAcentos = RemoverDiacriticos(nombre).ToUpperInvariant();
            // Cambiar cualquier separador por espacio
            sinAcentos = Regex.Replace(sinAcentos, @"[^A-Z0-9 ]+", " ").Trim();
            var words = sinAcentos.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string baseCode;
            if (words.Length >= 2)
            {
                // tomar 2 letras por palabra hasta 4 palabras
                baseCode = string.Concat(words.Take(4).Select(w => w.Length >= 2 ? w[..2] : w));
            }
            else
            {
                // una sola palabra: primeras 6
                baseCode = words[0].Length <= 6 ? words[0] : words[0][..6];
            }

            // Fallback si quedó muy corto
            if (baseCode.Length < 3)
            {
                var compact = string.Concat(words);
                baseCode = compact.Length <= 6 ? compact : compact[..6];
            }

            // Máximo 12 para que sea legible (DAL/BLL aceptan hasta 50)
            if (baseCode.Length > 12) baseCode = baseCode[..12];

            return baseCode;
        }

        private static string RemoverDiacriticos(string texto)
        {
            var normalized = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(capacity: normalized.Length);

            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
