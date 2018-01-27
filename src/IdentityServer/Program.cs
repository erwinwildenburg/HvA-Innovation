using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((context, config) =>
                {
                    IHostingEnvironment env = context.HostingEnvironment;
                    config.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), true, true);
                    config.AddJsonFile(Path.Combine(AppContext.BaseDirectory, $"appsettings.{env.EnvironmentName}.json"), true, true);
                    config.AddEnvironmentVariables();
                    config.AddEnvironmentVariables("APPSETTING_");
                })
                .Build();
    }
}
