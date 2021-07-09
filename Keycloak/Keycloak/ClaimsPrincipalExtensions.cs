using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Keycloak.Keycloak
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsUser(this ClaimsPrincipal principal)
        {
            return principal.IsInRole(Roles.User);
        }
    }
}
