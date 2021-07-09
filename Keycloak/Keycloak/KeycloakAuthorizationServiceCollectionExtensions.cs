using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Keycloak.Keycloak
{
    public static class KeycloakAuthorizationServiceCollectionExtensions
    {
        public static IServiceCollection AddKeycloakAuthorization(this IServiceCollection services, Action<KeycloakAuthorizationOptions> configure)
        {
            services.Configure(configure);
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler, KeycloakAuthorizationHandler>();

            return services;
        }
    }
}
