using System;
using System.Linq;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Reglas de negocio del Cierre Diario (idénticas al software viejo).
    /// </summary>
    public sealed class CierreDiarioService
    {
        private readonly IAcumuladoDiarioRepository _repo;

        public CierreDiarioService(IAcumuladoDiarioRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Ejecuta el cierre del día para todos los empleados activos.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public (int registrosInsertados, int empleadosProcesados) CerrarDia(
            DateTime fecha, bool esFeriado, bool esDomingo)
        {
            if (esDomingo && esFeriado)
                throw new InvalidOperationException("No se puede marcar 'Feriado' y 'Domingo' a la vez.");

            if (_repo.ExisteCierre(fecha))
                throw new InvalidOperationException("El cierre de este día ya fue registrado.");

            var empleados = _repo.GetEmpleadosActivosIds().ToList();
            int registros = 0;

            foreach (var empId in empleados)
            {
                var horas = _repo.GetHorasTrabajadasEnteras(empId, fecha);

                double normales = 0, extras = 0, dobles = 0, feriado = 0;

                if (esDomingo)
                {
                    // Domingo: todo va como dobles
                    dobles = horas;
                }
                else if (esFeriado)
                {
                    // Feriado: 8 normales, hasta 8 a feriado, el resto dobles
                    normales = 8;
                    if (horas >= 8)
                    {
                        feriado = 8;
                        dobles = horas - 8;
                    }
                    else if (horas > 0)
                    {
                        feriado = horas;
                    }
                }
                else
                {
                    // Día normal: 8 normales, resto extras
                    if (horas >= 8) { normales = 8; extras = horas - 8; }
                    else { normales = horas; }
                }

                var fila = new Acumulado_Diario
                {
                    Id_Empleado = empId,
                    Fecha = fecha.Date,
                    Normales = (float)normales,
                    Extras = (float)extras,
                    Dobles = (float)dobles,
                    Feriado = (float)feriado
                };

                _repo.Insert(fila);
                registros++;
            }

            return (registros, empleados.Count);
        }
    }
}
