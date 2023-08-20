using Common.Enums;
using Dtos;

namespace Business.IServices
{
    public interface IAuthenticationService
    {
        AuthenticationDto Authenticate(string fullName, RoleType roleType);
    }
}
