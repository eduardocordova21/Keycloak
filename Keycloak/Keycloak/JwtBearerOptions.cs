using System;

namespace Keycloak
{
    internal class JwtBearerOptions
    {
        public string Authority { get; set; } = String.Empty;

        public string Audience { get; set; } = String.Empty;
    }
}