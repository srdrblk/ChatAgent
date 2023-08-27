using Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum TeamShiftType
    {
        None = 0,
        [WorkHour(Start: 9, End: 17)]
        [Display(Name = "Day Shift Team")]
        DayShift = 1,
        [WorkHour(Start: 17, End: 1)]
        [Display(Name = "Evening Shift Team")]
        EveningShift = 2,
        [WorkHour(Start: 1, End: 9)]
        [Display(Name = "Night Shift Team")]
        NightShift = 3,
        [WorkHour(Start: 0, End: 24)]
        [Display(Name = "Overflow Shift Team")]
        Overflow = 4

    }
}
