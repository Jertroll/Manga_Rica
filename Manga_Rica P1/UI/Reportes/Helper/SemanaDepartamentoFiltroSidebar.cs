using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Reportes.Shared
{
    /// <summary>
    /// Sidebar de filtros para reportes por semana + departamento.
    /// </summary>
    public partial class SemanaDepartamentoFiltroSidebar : UserControl
    {
        /// <summary>
        /// Evento que el formulario escuchará cuando se pulse "Generar reporte".
        /// Entrega la semana seleccionada y el departamento (Id + Nombre).
        /// </summary>
        public event EventHandler<GenerarReporteSemanaDepartamentoEventArgs>? GenerarReporte;

        /// <summary>Título del panel (barra verde superior).</summary>
        public string Titulo
        {
            get => lblTitulo.Text;
            set => lblTitulo.Text = value;
        }

        /// <summary>Texto de la etiqueta para la semana.</summary>
        public string EtiquetaSemana
        {
            get => lblSemana.Text;
            set => lblSemana.Text = value;
        }

        /// <summary>Texto de la etiqueta para el departamento.</summary>
        public string EtiquetaDepartamento
        {
            get => lblDepartamento.Text;
            set => lblDepartamento.Text = value;
        }

        /// <summary>Texto del botón principal.</summary>
        public string TextoBoton
        {
            get => btnGenerar.Text;
            set => btnGenerar.Text = value;
        }

        // ====== Binding de Semana ======

        /// <summary>
        /// DataSource del combo de semanas.
        /// Suele ser una lista con propiedades como Id, Semana, etc.
        /// </summary>
        public object? DataSourceSemana
        {
            get => cboSemana.DataSource;
            set => cboSemana.DataSource = value;
        }

        public string DisplayMemberSemana
        {
            get => cboSemana.DisplayMember;
            set => cboSemana.DisplayMember = value;
        }

        public string ValueMemberSemana
        {
            get => cboSemana.ValueMember;
            set => cboSemana.ValueMember = value;
        }

        /// <summary>
        /// Devuelve la semana seleccionada como int? si se puede convertir.
        /// </summary>
        public int? SemanaSeleccionadaNumero
        {
            get
            {
                if (cboSemana.SelectedValue is int i)
                    return i;

                if (int.TryParse(cboSemana.SelectedValue?.ToString(), out var n))
                    return n;

                return null;
            }
        }

        // ====== Binding de Departamento ======

        /// <summary>
        /// DataSource del combo de departamentos.
        /// Generalmente una lista de entidades Departamento.
        /// </summary>
        public object? DataSourceDepartamento
        {
            get => cboDepartamento.DataSource;
            set => cboDepartamento.DataSource = value;
        }

        public string DisplayMemberDepartamento
        {
            get => cboDepartamento.DisplayMember;
            set => cboDepartamento.DisplayMember = value;
        }

        public string ValueMemberDepartamento
        {
            get => cboDepartamento.ValueMember;
            set => cboDepartamento.ValueMember = value;
        }

        /// <summary>
        /// Id del departamento seleccionado como int? (según SelectedValue).
        /// </summary>
        public int? DepartamentoSeleccionadoId
        {
            get
            {
                if (cboDepartamento.SelectedValue is int i)
                    return i;

                if (int.TryParse(cboDepartamento.SelectedValue?.ToString(), out var n))
                    return n;

                return null;
            }
        }

        /// <summary>
        /// Nombre del departamento seleccionado (texto visible del combo).
        /// </summary>
        public string? DepartamentoSeleccionadoNombre
        {
            get => cboDepartamento.Text;
        }

        public SemanaDepartamentoFiltroSidebar()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            var semanaNum = SemanaSeleccionadaNumero;
            var deptoId = DepartamentoSeleccionadoId;
            var deptoNombre = DepartamentoSeleccionadoNombre;

            var args = new GenerarReporteSemanaDepartamentoEventArgs(semanaNum, deptoId, deptoNombre);
            GenerarReporte?.Invoke(this, args);
        }

        /// <summary>
        /// Argumentos para el evento GenerarReporte: semana + departamento.
        /// </summary>
        public sealed class GenerarReporteSemanaDepartamentoEventArgs : EventArgs
        {
            public int? SemanaNumero { get; }
            public int? DepartamentoId { get; }
            public string? DepartamentoNombre { get; }

            public GenerarReporteSemanaDepartamentoEventArgs(
                int? semanaNumero,
                int? departamentoId,
                string? departamentoNombre)
            {
                SemanaNumero = semanaNumero;
                DepartamentoId = departamentoId;
                DepartamentoNombre = departamentoNombre;
            }
        }
    }
}
