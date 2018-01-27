using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace API.Models
{
    public class MySqlDbContext : ApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), true, true)
                .AddEnvironmentVariables()
                .AddEnvironmentVariables("APPSETTING_")
                .Build();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
        }
    }

    /// <inheritdoc />
    public class MySqlDbContextFactory : IDesignTimeDbContextFactory<MySqlDbContext>
    {
        public readonly ApplicationDbContextSettings Options;

        public MySqlDbContextFactory() { }

        public MySqlDbContextFactory(IOptions<ApplicationDbContextSettings> options)
        {
            Options = options.Value;
        }

        public MySqlDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MySqlDbContext> optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), true, true)
                .AddEnvironmentVariables()
                .AddEnvironmentVariables("APPSETTING_")
                .Build();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));

            return new MySqlDbContext();
        }
    }
}
