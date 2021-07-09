using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Keycloak
{
    public static class KeycloakAuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequiresKeycloakEntitlement(this AuthorizationPolicyBuilder builder, string resource, string scope)
        {
            builder.AddRequirements(new KeycloakRequirement($"{resource}#{scope}"));
            return builder;
        }
    }
}
