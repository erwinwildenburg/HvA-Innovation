using API.Configuration;
using API.Helpers;
using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable CORS
            services.AddCors();

            // Enable Application Insights
            services.AddApplicationInsightsTelemetry(Configuration);

            // Enable and bind to strongly-typed configuration
            services.AddOptions();
            services.Configure<StorageHandlerSettings>(Configuration.GetSection(nameof(AppSettings.StorageHandler)));

            // Register the provider managers
            services.AddSingleton<StorageHandlerManager>();

            // Register the provider helpers
            services.AddSingleton<StorageHandlerHelper>();

            // Add a connection to the database
			//services.AddEntityFrameworkMySql().AddDbContext<MySqlDbContext>();
			//services.AddScoped<ApplicationDbContext, MySqlDbContext>(f => f.GetService<MySqlDbContext>());

            // Authenticate with IdentityServer4
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.Authority = Configuration.GetValue<string>("Authentication:Authority");
                    options.Audience = Configuration.GetValue<string>("Authentication:ApiName");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false
                    };
                });

            // Add framework services
            services.AddMvcCore()
                .AddAuthorization()
                .AddDataAnnotations()
                .AddJsonFormatters(options =>
                {
                    options.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "HvA Innovation API", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "API.xml"));
                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = new Uri(new Uri(Configuration.GetValue<string>("Swagger:IdpUrl")), "/connect/authorize").ToString(),
                    Scopes = new Dictionary<string, string>
                    {
                        { "api", "This scope value requests access to the API." }
                    }
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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

            // Add CORS options
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                );

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PowerShell DSC Cloud API");
                c.ConfigureOAuth2("swagger", null, null, "Swagger UI");
            });

            app.UseAuthentication();

            app.UseMvc();

            // Auto provision the database with required values
            //ApplicationDbContextInitializer.Initialize(dbContext);

            // Resolve some DI classes to make sure they're ready to go when needed and
            // forces any possible resolution errors to invoke earlier rather than later
            serviceProvider.GetRequiredService<StorageHandlerHelper>();
        }
    }
}
