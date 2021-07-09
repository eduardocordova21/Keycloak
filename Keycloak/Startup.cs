using Keycloak.Keycloak;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Keycloak
{
    public class Startup
    {
        private const string _roleClaimType = "role";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtOptions = Configuration.GetSection("JwtBearer").Get<JwtBearerOptions>();

            services.AddCors(options => options.AddDefaultPolicy(policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.Authority = jwtOptions.Authority;
                        options.Authority = jwtOptions.Audience;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters.NameClaimType = "DEFINIR UMA ROLE!";
                        options.TokenValidationParameters.RoleClaimType = _roleClaimType;
                    });

            services.AddTransient<IClaimsTransformation>(_ => new KeycloakRolesClaimsTransformation(_roleClaimType, jwtOptions.Audience));

            services.AddAuthorization(options => {
                options.AddPolicy(Policies.CanAccess, policy => policy.RequiresKeycloakEntitlement("Dashboard", "Access"));
            }).AddKeycloakAuthorization(options =>
            {
                options.Audience = jwtOptions.Audience;
                options.TokenEndpoint = $"{jwtOptions.Authority}/protocol/openid-connect/token";
            });

            services.AddControllersWithViews();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
