using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Keycloak.Keycloak
{
    public class KeycloakAuthorizationHandler : AuthorizationHandler<KeycloakRequirement>
    {
        private readonly IOptions<KeycloakAuthorizationOptions> _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KeycloakAuthorizationHandler(IOptions<KeycloakAuthorizationOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, KeycloakRequirement requirement)
        {
            var options = _options.Value;
            var httpContext = _httpContextAccessor.HttpContext;
            var auth = await httpContext.AuthenticateAsync(options.RequiredScheme);

            if (!auth.Succeeded)
            {
                context.Fail();
                return;
            }

            var data = new Dictionary<string, string>();
            data.Add("grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket");
            data.Add("response_mode", "decision");
            data.Add("audience", options.Audience);
            data.Add("permission", $"{requirement.PolicyName}");

            var client = new HttpClient(options.BackchannelHandler);
            var token = await httpContext.GetTokenAsync(options.RequiredScheme, "access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(options.TokenEndpoint, new FormUrlEncodedContent(data));

            if (response.IsSuccessStatusCode)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}
