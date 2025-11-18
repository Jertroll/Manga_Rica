# Clock lookup linkage

El sistema vincula cada empleado de Planilla con su usuario del reloj marcador usando el campo `MC_Numero`:

- **Obtención del identificador del reloj**: `AcumuladoDiarioRepository` lee `MC_Numero` desde la tabla de empleados y lo traduce a `employees.code` del reloj antes de pedir horas (`Manga_Rica P1/DAL/AcumuladoDiarioRepository.cs`).
- **Consulta de horas consolidadas**: `CalculatedAttendanceRepository.GetByEmployeeCode` filtra por `MC_Numero` al convertirlo en `@code` y compararlo con `TRY_CONVERT(bigint, e.code)` dentro de la base Clock.
- **Repositorios principales**: Tanto `EmpleadoRepository` como los servicios de horas (`HorasService`, `AttendanceCalculationService`) exponen `MC_Numero` para búsquedas y reportes.

Por lo tanto, toda interacción con la base del reloj parte del mapeo `Empleado.Id ↔ MC_Numero ↔ employees.code`, que asegura que cada consulta diaria o semanal recupere las marcas correctas del colaborador.
