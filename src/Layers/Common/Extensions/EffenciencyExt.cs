using Common.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Common.Extensions
{
    public static class EffenciencyExt
    {
        public static double? GetEffenciency(this Enum enm)
        {
            return enm.GetType()?
          .GetMember(enm.ToString()).First()?
          .GetCustomAttribute<Efficiency>()?
          .Value;
        }
    }
}
