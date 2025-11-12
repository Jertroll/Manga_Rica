// Nueva implementacion
namespace Manga_Rica_P1.ENTITY.Clock
{
    /// <summary>
    /// Marca cruda del reloj (dbo.GLogs).
    /// </summary>
    public sealed class GLog
    {
        public DateTime DateTime { get; init; }   // _datetime (datetime)
        public string EnrollNo { get; init; } = ""; // enrollNo (varchar)
        public int State { get; init; }           // state (int) si aplica: 0/1 o códigos del proveedor
        public int? VerifyMode { get; init; }     // verifyMode (int?) si existe en tu esquema
        public int? InOutMode { get; init; }      // inOutMode (int?) si existe
        public int? MachineNo { get; init; }      // machineNo (int?) si existe
    }
}
