using Common.Enums;
using Common.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Helper
{
    public class RolesAttribute : AuthorizeAttribute
    {
        public RolesAttribute(params RoleType[] roleTypes)
        {
            var roles = roleTypes.Select(r => r.GetDisplayName());
            Roles = String.Join(",", roles);
    
        }
    }
}
