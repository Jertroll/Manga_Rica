
using Manga_Rica_P1.Entity; // si luego quieres agregar overloads que reciban Acumulado_Diario

namespace Manga_Rica_P1.BLL.ReglasPago
{
    /// <summary>
    /// Totales semanales luego de aplicar reglas de negocio (48h, mover extras/dobles, etc.).
    /// Usamos float para respetar 1:1 los tipos de tu BD actual.
    /// </summary>
    public sealed class TotalesSemana
    {
        public float Normales;
        public float Extras;
        public float Dobles;
        public float Feriado;
    }

    /// <summary>
    /// Reglas de negocio de semana (equivalentes al Calculos/Calculos2 del módulo viejo).
    /// Clase estática porque no tiene estado ni dependencias.
    /// </summary>
    public static class ReglasDePago
    {
        /// <summary>
        /// Aplica las reglas semanales:
        /// 1) Completar 48h moviendo de Extras→Normales.
        /// 2) Si aún falta y no hay Extras, tomar de Dobles→Normales (solo si trabajó &lt;7 días).
        /// 3) Si trabajó &lt;7 días, las Dobles pasan a Normales (no hay dobles esa semana).
        /// </summary>
        public static TotalesSemana AplicarReglasSemanales(
            float normales, float extras, float dobles, float feriado, int diasTrabajados)
        {
            var r = new TotalesSemana
            {
                Normales = normales,
                Extras = extras,
                Dobles = dobles,
                Feriado = feriado
            };

            // 1) Relleno hasta 48 horas con Extras
            var variable = 48f - r.Normales;
            if (variable > 0f && r.Extras > 0f)
            {
                var m = System.Math.Min(variable, r.Extras);
                r.Normales += m;
                r.Extras -= m;
                variable -= m;
            }

            // 2) Si aún falta, no hay Extras y trabajó <7 días ⇒ mover de Dobles a Normales
            if (variable > 0f && r.Extras == 0f && diasTrabajados < 7)
            {
                var m = System.Math.Min(variable, r.Dobles);
                r.Normales += m;
                r.Dobles -= m;
                variable -= m;
            }

            // 3) Si trabajó <7 días no hay dobles ⇒ todo dobles pasa a normales
            if (diasTrabajados < 7)
            {
                r.Normales += r.Dobles;
                r.Dobles = 0f;
            }

            return r;
        }
    }
}
