using Common.Attributes;
using System.Reflection;

namespace Common.Extensions
{
    public static class WorkHourExt
    {
        public static int? GetStartHour(this Enum enm)
        {
            return enm.GetType()?
          .GetMember(enm.ToString()).First()?
          .GetCustomAttribute<WorkHour>()?
          .Start;
        }
        public static int? GetEndHour(this Enum enm)
        {
            return enm.GetType()?
          .GetMember(enm.ToString()).First()?
          .GetCustomAttribute<WorkHour>()?
          .Start;
        }
    }
}
