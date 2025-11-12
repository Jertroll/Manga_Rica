
namespace Manga_Rica_P1.ENTITY.Clock
{
    /// <summary>
    /// Fila consolidada por día (dbo.calculatedAttendance).
    /// Tipos elegidos para alinearse a SQL: bigint→long, bit→bool, int→int, varchar→string, datetime→DateTime.
    /// </summary>
    public sealed class CalculatedAttendance
    {
        public long Id { get; init; }                 // id (bigint/int; usamos long por seguridad)
        public long IdEmployee { get; init; }         // idEmployee (bigint)
        public DateTime Date { get; init; }           // _date (datetime)
        public int RowType { get; init; }             // rowType (int)

        public bool DaySeventh { get; init; }         // daySeventh (bit)
        public bool DayBreak { get; init; }           // dayBreak (bit)
        public bool DayCompensatory { get; init; }    // dayCompensatory (bit)
        public bool IsHoliDay { get; init; }          // isHoliDay (bit)
        public int? IdHoliday { get; init; }          // idHoliday (int, nullable)
        public bool IsHolidayPay { get; init; }       // isHolidayPay (bit)
        public int? IdGroup { get; init; }            // idGroup (int)
        public int? TypeCalendar { get; init; }       // typeCalendar (int)
        public int? TypeSchedule { get; init; }       // typeSchedule (int)
        public string? AdditionalDescription { get; init; } // additionalDescription (varchar)
        public bool IsVacation { get; init; }         // isVacation (bit)

        public DateTime? StartEnroll { get; init; }   // startEnroll (datetime)
        public int? MachineNo1 { get; init; }         // machineNo1 (int)
        public string? NameMachine1 { get; init; }    // nameMachine1 (varchar)
        public int? VerifyMode1 { get; init; }        // verifyMode1 (int)
        public int? TypeInco1 { get; init; }          // typeInco1 (int)
        public bool NotAuto1 { get; init; }           // notAuto1 (bit)
        public bool Auto1 { get; init; }              // auto1 (bit)
        public string? Note1 { get; init; }           // note1 (varchar)
        public string? ByUsername1 { get; init; }     // byUsername1 (varchar)
        public bool IsDisability1 { get; init; }      // isDisability1 (bit)

        public DateTime? EndEnroll { get; init; }     // endEnroll (datetime)
        public int? MachineNo2 { get; init; }         // machineNo2 (int)
        public string? NameMachine2 { get; init; }    // nameMachine2 (varchar)
        public int? VerifyMode2 { get; init; }        // verifyMode2 (int)
        public int? TypeInco2 { get; init; }          // typeInco2 (int)
        public bool NotAuto2 { get; init; }           // notAuto2 (bit)
        public bool Auto2 { get; init; }              // auto2 (bit)
        public string? Note2 { get; init; }           // note2 (varchar)
        public string? ByUsername2 { get; init; }     // byusername2 (varchar)
        public bool IsDisability2 { get; init; }      // isDisability2 (bit)

        public bool IsNocturnal { get; init; }        // IsNocturnal (bit)
        public bool IsOpen { get; init; }             // IsOpen (bit)
        public bool IsInOut { get; init; }            // IsInOut (bit)
        public bool ValidateIcon { get; init; }       // validateIcon (bit)
        public bool IgnoreAbsence { get; init; }      // ignoreAbsence (bit)

        public int? IdSchedule { get; init; }         // idSchedule (int)
        public int? DurationBreak { get; init; }      // durationBreak (int) (minutos)
        public bool ValidateIncoBreak { get; init; }  // validateIncoBreak (bit)
        public bool DeductBreak { get; init; }        // deductBreak (bit)

        public DateTime? InSchedule { get; init; }    // InSchedule (datetime)
        public DateTime? OutSchedule { get; init; }   // outSchedule (datetime)
        public int? Total { get; init; }              // total (int) (minutos totales del día, según proveedor)

        public string? UserInEdition { get; init; }   // userInEdition (varchar)
        public bool IsOptionalBreak { get; init; }    // isOptionalBreak (bit)
    }
}
