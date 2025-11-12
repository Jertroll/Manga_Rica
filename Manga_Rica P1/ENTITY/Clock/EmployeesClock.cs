// Nueva implementacion
namespace Manga_Rica_P1.ENTITY.Clock
{
    /// <summary>
    /// Empleado en la BD del reloj (dbo.employees).
    /// Sirve para mapear code ↔ id (bigint) ↔ IdDevice/enrollNo (varchar).
    /// </summary>
    public sealed class EmployeesClock
    {
        public long Id { get; init; }           // dbo.employees.id (bigint)
        public string Code { get; init; } = ""; // dbo.employees.code (varchar)
        public string IdDevice { get; init; } = ""; // dbo.employees.IdDevice (varchar) → suele ser enrollNo en GLogs
    }
}
