using System.Reflection;
using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable Application Insights
            services.AddApplicationInsightsTelemetry(Configuration);

            // Configure IdentityServer4
            string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer(options =>
            {
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
            })
            .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        string connectionString = Configuration.GetConnectionString("DefaultConnection");
                        builder.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    };

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
            .AddInMemoryClients(Configuration.GetSection("Clients"))
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddDeveloperSigningCredential()
            .AddExtensionGrantValidator<Extensions.ExtensionGrantValidator>()
            .AddExtensionGrantValidator<Extensions.NoSubjectExtensionGrantValidator>()
            .AddJwtBearerClientAuthentication()
            .AddAppAuthRedirectUriValidator()
            .AddTestUsers(TestUsers.Users);

            // Configure external authentication
            services.AddOidcStateDataFormatterCache("aad", "aad");
            services.AddAuthentication()
                .AddOpenIdConnect("aad", "Azure AD", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                    options.Authority = Configuration.GetValue<string>("Authentication:AzureAD:Authority");
                    options.ClientId = Configuration.GetValue<string>("Authentication:AzureAD:ClientId");
                    options.ResponseType = OpenIdConnectResponseType.IdToken;
                    options.CallbackPath = new PathString("/signin-aad");
                    options.SignedOutCallbackPath = new PathString("/signout-callback-aad");
                    options.RemoteSignOutPath = new PathString("/signout-aad");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Fix authentication when a proxy server is in place
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.Use(async (context, next) =>
            {
                // Correctly set request scheme for Azure Webapp on Linux and Amazon AWS
                if (!string.IsNullOrEmpty(context.Request.Headers["X-ARR-SSL"]) || !string.IsNullOrEmpty(context.Request.Headers["X-Forwarded-Proto"]) && context.Request.Headers["X-Forwarded-Proto"].Equals("https"))
                    context.Request.Scheme = "https";

                await next.Invoke();
            });

            // Add IdentityServer4
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
