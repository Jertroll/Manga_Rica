namespace Manga_Rica_P1.ENTITY.Attendance
{
    public enum FuenteHoras { GLogs, CalculatedAttendance }
    public enum MotivoDecision
    {
        Ok_DiffDentroTolerancia,
        CA_Anomalo,
        GLogs_Incompleto,
        Desacuerdo_MayorQueTolerancia
    }
    public enum DiaTipo { Normal, Domingo, Feriado }

    /// <summary>
    /// Resultado diario tras reconciliar (dominio): lo que luego mapearás a Acumulado_Diario.
    /// </summary>
    public sealed class AsistenciaDia
    {
        public long IdEmpleado { get; init; } // Id de planilla o mapeado desde reloj
        public DateTime Fecha { get; init; }  // solo fecha
        public int MinNeto { get; init; }     // minutos netos del día (post-reglas)
        public FuenteHoras Fuente { get; init; }
        public MotivoDecision Motivo { get; init; }
        public int DiffMin { get; init; }     // |GLogs - CA| en minutos
        public DiaTipo TipoDia { get; init; }

        // Auditoría opcional para UI
        public IReadOnlyList<Manga_Rica_P1.ENTITY.Clock.TramoLaboral> TramosUsados { get; init; }
            = Array.Empty<Manga_Rica_P1.ENTITY.Clock.TramoLaboral>();

        public int? MinNeto_GLogs { get; init; }
        public int? MinNeto_CA { get; init; }
    }
}
