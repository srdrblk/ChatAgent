using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum RoleType
    {
        [Display(Name ="User")]
        User = 0,
        [Display(Name = "Agent")]
        Agent = 1
    }
}
