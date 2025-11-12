using System;
using System.Linq;
using Manga_Rica_P1.BLL.ReglasPago;            // ReglasDePago, TotalesSemana
using Manga_Rica_P1.DAL;                       // Repos
using Manga_Rica_P1.Entity;                    // Acumulado_Diario, etc.
using PagosRow = Manga_Rica_P1.Entity.Pagos;   // << alias para evitar choque con namespace BLL.Pagos

namespace Manga_Rica_P1.BLL.Pagos
{
    // Nueva implementación
    public sealed class PagosService
    {
        private readonly AcumuladoDiarioRepository _acumRepo;
        private readonly PagosRepository _pagosRepo;
        private readonly DeduccionesRepository _dedRepo;
        private readonly EmpleadoRepository _empRepo;
        private readonly SodaRepository _sodaRepo; // opcional si descuentas soda
        private readonly SemanaRepository _semanaRepo;

        public PagosService(
            AcumuladoDiarioRepository acumRepo,
            PagosRepository pagosRepo,
            DeduccionesRepository dedRepo,
            EmpleadoRepository empRepo,
            SodaRepository sodaRepo, // si no lo usas, quítalo también del ctor y de los campos
            SemanaRepository semanaRepo
        )
        {
            _acumRepo = acumRepo;
            _pagosRepo = pagosRepo;
            _dedRepo = dedRepo;
            _empRepo = empRepo;
            _sodaRepo = sodaRepo;
            _semanaRepo = semanaRepo;
        }

        public sealed class PagoPreview
        {
            public long IdEmpleado { get; set; }
            public int IdSemana { get; set; }

            public string? Carne { get; set; }
            public string? NombreCompleto { get; set; }
            public float SalarioHora { get; set; }

            public float Normales { get; set; }
            public float Extras { get; set; }
            public float Dobles { get; set; }
            public float Feriado { get; set; }

            public float Soda { get; set; }
            public float DeduccionUniforme { get; set; }
            public float DeduccionOtras { get; set; } = 0f;

            public float Bruto { get; set; }
            public float Neto { get; set; }
            public bool YaRegistrado { get; set; }
        }

        public List<long> GetEmpleadosPendientesIds(int idSemana)
    => _pagosRepo.GetEmpleadosPendientesBySemana(idSemana);

        public PagoPreview GetPreview(long idEmpleado, int idSemana)
        {
            var semana = _semanaRepo.GetById(idSemana)
                ?? throw new InvalidOperationException($"Semana {idSemana} no existe.");

            var desde = semana.fecha_Inicio.Date;
            var hasta = semana.fecha_Final.Date;

            var tot = CalcularTotalesSemana(idEmpleado, desde, hasta);
            var emp = _empRepo.GetById(idEmpleado)
                ?? throw new InvalidOperationException("Empleado no encontrado");

            float salarioHora = (float)emp.Salario;
            float soda = (float)_sodaRepo.SumTotalByEmpleadoYRango(idEmpleado, desde, hasta);
            float dedUniforme = (float)_dedRepo.SumSaldoPendiente(idEmpleado);

            float bruto = 0f;
            bruto += tot.Normales * salarioHora;
            bruto += tot.Extras * (salarioHora * 1.5f);
            bruto += tot.Dobles * (salarioHora * 2.0f);
            bruto += tot.Feriado * salarioHora;

            float neto = bruto - dedUniforme - soda;

            return new PagoPreview
            {
                IdEmpleado = idEmpleado,
                IdSemana = idSemana,
                Carne = emp.Carne.ToString(),
                NombreCompleto = $"{emp.Nombre} {emp.Primer_Apellido} {emp.Segundo_Apellido}".Trim(),
                SalarioHora = salarioHora,
                Normales = tot.Normales,
                Extras = tot.Extras,
                Dobles = tot.Dobles,
                Feriado = tot.Feriado,
                Soda = soda,
                DeduccionUniforme = dedUniforme,
                Bruto = bruto,
                Neto = neto,
                YaRegistrado = _pagosRepo.ExistsRegistrado(idEmpleado, idSemana)
            };
        }


        /// <summary>
        /// Suma la semana desde Acumulado_Diario y aplica ReglasDePago (48h / dobles→normales si &lt;7 días).
        /// Devuelve TotalesSemana listos para calcular dinero.
        /// </summary>
        public TotalesSemana CalcularTotalesSemana(long idEmpleado, DateTime inicio, DateTime fin)
        {
            var dias = _acumRepo.ListByEmpleadoYRango(idEmpleado, inicio.Date, fin.Date);

            float normales = dias.Sum(x => x.Normales);
            float extras = dias.Sum(x => x.Extras);
            float dobles = dias.Sum(x => x.Dobles);
            float feriado = dias.Sum(x => x.Feriado);

            // Días trabajados: hay trabajo si Normales>0 o Dobles>0 (según módulo viejo)
            int diasTrabajados = dias.Count(d => d.Normales > 0f || d.Dobles > 0f);

            return ReglasDePago.AplicarReglasSemanales(normales, extras, dobles, feriado, diasTrabajados);
        }

        /// <summary>
        /// Calcula totales + dinero y registra el pago (equivalente al Registrar_Click del módulo viejo).
        /// </summary>
        public void RegistrarPagoSemana(long idEmpleado, int idSemana, DateTime fechaCorte)
        {
            var semana = _semanaRepo.GetById(idSemana)
        ?? throw new InvalidOperationException($"Semana {idSemana} no existe.");


            var desde = semana.fecha_Inicio.Date;
            var hasta = semana.fecha_Final.Date;

            var tot = CalcularTotalesSemana(idEmpleado, desde, hasta);

            // 3) Datos de salario y deducciones
            var emp = _empRepo.GetById(idEmpleado) ?? throw new InvalidOperationException("Empleado no encontrado");
            float salarioHora = (float)emp.Salario;

            double sodaDouble = _sodaRepo.SumTotalByEmpleadoYRango(idEmpleado, desde, hasta);

            float soda = (float)sodaDouble;

            // Deducciones (saldo pendiente) -> double en repo, convierto a float
            float dedUniforme = (float)_dedRepo.SumSaldoPendiente(idEmpleado);
            float dedOtras = 0f; // según tu UI/reglas

            // 4) Cálculo de dinero (igual a tu VB)
            float bruto = 0f;
            bruto += tot.Normales * salarioHora;
            bruto += tot.Extras * (salarioHora * 1.5f);
            bruto += tot.Dobles * (salarioHora * 2.0f);
            bruto += tot.Feriado * salarioHora;

            float neto = bruto - dedUniforme - soda - dedOtras;

            // 5) Persistencia en Pagos
            var row = new PagosRow
            {
                Id_Empleado = idEmpleado,
                Id_Semana = idSemana,                 // << propiedad correcta en tu entidad
                Fecha = fechaCorte.Date,
                Horas_Normales = tot.Normales,
                Horas_Extras = tot.Extras,
                Horas_Dobles = tot.Dobles,
                Feriadas = tot.Feriado,
                Deduccion_Soda = soda,
                Deduccion_Uniforme = dedUniforme,
                Deduccion_Otras = dedOtras,
                Salario_Bruto = bruto,
                Salario_Neto = neto,
                Registrado = true,
                Id_Usuario = 0 
            };


            var yaRegistrado = _pagosRepo.ExistsRegistrado(idEmpleado, idSemana);

            _pagosRepo.Upsert(row);

            if (!yaRegistrado && row.Deduccion_Uniforme > 0f)
            {
                _dedRepo.AplicarContraSaldo(idEmpleado, row.Deduccion_Uniforme);
            }
        }
    }
}
