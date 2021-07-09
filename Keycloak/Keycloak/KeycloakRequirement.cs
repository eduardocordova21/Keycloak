using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Keycloak
{
    public class KeycloakRequirement : IAuthorizationRequirement
    {
        public KeycloakRequirement(string policyName)
        {
            PolicyName = policyName;
        }

        public string PolicyName { get; }
    }
}
