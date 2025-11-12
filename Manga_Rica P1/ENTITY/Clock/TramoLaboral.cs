namespace Manga_Rica_P1.ENTITY.Clock
{
   
    public sealed class TramoLaboral
    {
        public DateTime In { get; init; }   // marca de entrada
        public DateTime Out { get; init; }  // marca de salida

        /// <summary>Minutos del tramo (maneja cruce de medianoche).</summary>
        public int Minutos
        {
            get
            {
                var outFix = Out < In ? Out.AddDays(1) : Out;
                return (int)(outFix - In).TotalMinutes;
            }
        }
    }
}
