using Common.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Common.Extensions
{
    public static class DisplayNameExt
    {
        public static string GetDisplayName(this Enum enm)
        {
            return enm.GetType()?
          .GetMember(enm.ToString()).First()?
          .GetCustomAttribute<DisplayAttribute>()?
          .Name;
        }
    }
}
