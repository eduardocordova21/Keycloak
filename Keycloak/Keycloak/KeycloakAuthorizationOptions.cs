using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Keycloak.Keycloak
{
    public class KeycloakAuthorizationOptions
    {
        public string RequiredScheme { get; set; } = JwtBearerDefaults.AuthenticationScheme;

        public string TokenEndpoint { get; set; }

        public HttpMessageHandler BackchannelHandler { get; set; } = new HttpClientHandler();

        public string Audience { get; set; }
    }
}
