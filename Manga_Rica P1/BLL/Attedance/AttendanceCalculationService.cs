// Nueva implementacion
// Archivo: BLL/Attendance/AttendanceCalculationService.cs

using System;
using System.Collections.Generic;
using System.Linq;

using Manga_Rica_P1.DAL.Clock;
using Manga_Rica_P1.ENTITY.Attendance; // AsistenciaDia, enums
using Manga_Rica_P1.ENTITY.Clock;      // EmployeesClock, GLog, CalculatedAttendance, TramoLaboral
using Manga_Rica_P1.Entity;            // Acumulado_Diario (tu entidad real de planilla)

namespace Manga_Rica_P1.BLL.Attendance
{
    /// <summary>
    /// Nueva implementacion
    /// Servicio BLL que calcula minutos netos diarios a partir de GLogs y/o calculatedAttendance,
    /// reconcilia diferencias y mapea a horas Normales/Extras/Dobles/Feriado (reglas del módulo viejo).
    /// </summary>
    public sealed class AttendanceCalculationService
    {
        private readonly EmployeesClockRepository _empRepo;
        private readonly GLogsRepository _glogsRepo;
        private readonly CalculatedAttendanceRepository _caRepo;

        /// <summary>
        /// Minutos permitidos de diferencia para aceptar CA vs GLogs.
        /// </summary>
        private readonly int _toleranciaMin;

        public AttendanceCalculationService(
            EmployeesClockRepository empRepo,
            GLogsRepository glogsRepo,
            CalculatedAttendanceRepository caRepo,
            int toleranciaMin = 10 // puedes ajustarlo según tu realidad
        )
        {
            _empRepo = empRepo;
            _glogsRepo = glogsRepo;
            _caRepo = caRepo;
            _toleranciaMin = toleranciaMin;
        }

        // =========================
        // Lecturas / Cálculos base
        // =========================

        /// <summary>
        /// Nueva implementacion
        /// Construye tramos IN→OUT (pares) con marcas ordenadas.
        /// Empareja 1-2, 3-4…; si hay impar, ignora la última (jornada abierta).
        /// </summary>
        private static List<TramoLaboral> BuildTramosOrdenados(List<GLog> marcasOrdenadas)
        {
            var tramos = new List<TramoLaboral>();
            for (int i = 0; i + 1 < marcasOrdenadas.Count; i += 2)
            {
                var entrada = marcasOrdenadas[i].DateTime;
                var salida = marcasOrdenadas[i + 1].DateTime;
                tramos.Add(new TramoLaboral { In = entrada, Out = salida });
            }
            return tramos;
        }

        /// <summary>
        /// Nueva implementacion
        /// Minutos netos del día a partir de GLogs (excluye almuerzo naturalmente si se marcó salida/entrada).
        /// </summary>
        private int ComputeNetFromGLogs(string enrollNo, DateTime dia)
        {
            var desde = dia.Date;
            var hasta = dia.Date.AddDays(1);

            var marcas = _glogsRepo.GetMarksByEnrollNo(enrollNo, desde, hasta, ascending: true);
            if (marcas.Count == 0) return 0;

            var tramos = BuildTramosOrdenados(marcas);
            var total = tramos.Sum(t => t.Minutos);
            return Math.Max(0, total);
        }

        /// <summary>
        /// Nueva implementacion
        /// Minutos netos desde calculatedAttendance para un día.
        /// </summary>
        private int? ComputeNetFromCA(long idEmployeeClock, DateTime dia, out CalculatedAttendance? caRow)
        {
            var list = _caRepo.GetByEmployeeId(idEmployeeClock, dia.Date, dia.Date);
            caRow = list.FirstOrDefault(x => x.Date.Date == dia.Date);
            if (caRow == null) return null;

            int? net = null;

            if (caRow.StartEnroll.HasValue && caRow.EndEnroll.HasValue)
            {
                var start = caRow.StartEnroll.Value;
                var end = caRow.EndEnroll.Value;
                if (end < start) end = end.AddDays(1);
                net = (int)(end - start).TotalMinutes;
            }
            else if (caRow.Total.HasValue)
            {
                net = caRow.Total.Value;
            }

            if (net.HasValue && caRow.DeductBreak && caRow.DurationBreak.HasValue)
                net = Math.Max(0, net.Value - caRow.DurationBreak.Value);

            return net;
        }

        // =========================
        // Reglas de negocio (día)
        // =========================

        /// <summary>
        /// Nueva implementacion
        /// Clasifica el día usando señales de CA si existen; si no, fallback al calendario.
        /// </summary>
        private static DiaTipo ClasificarDia(DateTime dia, CalculatedAttendance? ca)
        {
            if (ca != null)
            {
                if (ca.IsHoliDay) return DiaTipo.Feriado;
                if (ca.DaySeventh) return DiaTipo.Domingo;
            }
            return dia.DayOfWeek == DayOfWeek.Sunday ? DiaTipo.Domingo : DiaTipo.Normal;
        }

        /// <summary>
        /// Nueva implementacion
        /// Convierte minutos netos → horas separadas por tipo de día (mismas reglas del módulo viejo).
        /// </summary>
        private static (double normales, double extras, double dobles, double feriado)
            MapearMinutosAHorasPorTipo(int minNeto, DiaTipo tipo)
        {
            double horas = minNeto / 60.0;

            switch (tipo)
            {
                case DiaTipo.Domingo:
                    return (0, 0, horas, 0);

                case DiaTipo.Feriado:
                    if (horas >= 8) return (8, 0, horas - 8, 8);
                    return (0, 0, 0, horas);

                default: // Normal
                    if (horas >= 8) return (8, horas - 8, 0, 0);
                    return (horas, 0, 0, 0);
            }
        }

        // =========================
        // Reconciliación híbrida
        // =========================

        /// <summary>
        /// Nueva implementacion
        /// Decide la fuente (GLogs vs CA) con tolerancia y señales de anomalía.
        /// </summary>
        private AsistenciaDia ReconciliarDia(
            long idEmpleadoPlanilla,
            long idEmployeeClock,
            string enrollNo,
            DateTime dia)
        {
            var minG = ComputeNetFromGLogs(enrollNo, dia);                 // siempre calculamos GLogs
            var minC = ComputeNetFromCA(idEmployeeClock, dia, out var ca); // CA puede venir null
            var tipo = ClasificarDia(dia, ca);

            FuenteHoras fuente;
            MotivoDecision motivo;
            int diff = minC.HasValue ? Math.Abs(minG - minC.Value) : minG;

            bool caAnomala = false;
            if (ca != null)
            {
                if (ca.IsOpen || (minC.HasValue && minC.Value > 20 * 60)) // jornada abierta o >20h
                    caAnomala = true;
            }

            if (minC.HasValue && !caAnomala)
            {
                if (diff <= _toleranciaMin)
                {
                    fuente = FuenteHoras.CalculatedAttendance;
                    motivo = MotivoDecision.Ok_DiffDentroTolerancia;

                    return new AsistenciaDia
                    {
                        IdEmpleado = idEmpleadoPlanilla,
                        Fecha = dia.Date,
                        MinNeto = minC.Value,
                        Fuente = fuente,
                        Motivo = motivo,
                        DiffMin = diff,
                        TipoDia = tipo,
                        MinNeto_GLogs = minG,
                        MinNeto_CA = minC
                    };
                }
                else
                {
                    fuente = FuenteHoras.GLogs;
                    motivo = MotivoDecision.Desacuerdo_MayorQueTolerancia;
                }
            }
            else
            {
                fuente = FuenteHoras.GLogs;
                motivo = MotivoDecision.CA_Anomalo;
            }

            return new AsistenciaDia
            {
                IdEmpleado = idEmpleadoPlanilla,
                Fecha = dia.Date,
                MinNeto = minG,
                Fuente = fuente,
                Motivo = motivo,
                DiffMin = diff,
                TipoDia = tipo,
                MinNeto_GLogs = minG,
                MinNeto_CA = minC
            };
        }

        // =========================
        // API pública por día / rango
        // =========================

        /// <summary>
        /// Nueva implementacion
        /// Calcula un día para un empleado identificado en la BD del reloj por "code".
        /// Retorna entidad de dominio AsistenciaDia (para auditoría/UI).
        /// </summary>
        public AsistenciaDia CalcularDiaPorCode(long idEmpleadoPlanilla, string codeClock, DateTime dia)
        {
            var emp = _empRepo.GetByCode(codeClock)
                ?? throw new InvalidOperationException($"Empleado reloj no encontrado para code={codeClock}");

            var enrollNo = string.IsNullOrWhiteSpace(emp.IdDevice) ? emp.Code : emp.IdDevice;

            return ReconciliarDia(idEmpleadoPlanilla, emp.Id, enrollNo, dia.Date);
        }

        /// <summary>
        /// Nueva implementacion (Opción 2)
        /// Calcula un rango y devuelve:
        ///   - detalle: AsistenciaDia (minutos + metadatos por día)
        ///   - acumulado: lista de Acumulado_Diario (tu entidad real, lista para persistir)
        /// </summary>
        public (List<AsistenciaDia> detalle, List<Acumulado_Diario> acumulado)
            CalcularRangoPorCode(long idEmpleadoPlanilla, string codeClock, DateTime desde, DateTime hasta)
        {
            if (hasta < desde) throw new ArgumentException("Rango inválido: 'hasta' < 'desde'.");

            var emp = _empRepo.GetByCode(codeClock)
                ?? throw new InvalidOperationException($"Empleado reloj no encontrado para code={codeClock}");

            var enrollNo = string.IsNullOrWhiteSpace(emp.IdDevice) ? emp.Code : emp.IdDevice;

            var detalle = new List<AsistenciaDia>();
            var acumulado = new List<Acumulado_Diario>();

            for (var d = desde.Date; d <= hasta.Date; d = d.AddDays(1))
            {
                var a = ReconciliarDia(idEmpleadoPlanilla, emp.Id, enrollNo, d);
                detalle.Add(a);

                var (norm, ext, dob, fer) = MapearMinutosAHorasPorTipo(a.MinNeto, a.TipoDia);

                // IMPORTANTE: tu entidad usa float → casteo explícito
                acumulado.Add(new Acumulado_Diario
                {
                    Id_Empleado = idEmpleadoPlanilla,
                    Fecha = d,
                    Normales = (float)norm,
                    Extras = (float)ext,
                    Dobles = (float)dob,
                    Feriado = (float)fer
                });
            }

            return (detalle, acumulado);
        }
    }
}
