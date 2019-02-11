using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migrations.Versions;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Migrations
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{ Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production" }.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static IHost BuildHost(string[] args) => new HostBuilder()
            .ConfigureAppConfiguration((hostContext, configuration) =>
            {
                configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                configuration.AddEnvironmentVariables();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddOptions();

                services.AddFluentMigratorCore();

                services.AddSingleton<IConventionSet>(new DefaultConventionSet(Configuration["Database:Schema"], null));

                services.ConfigureRunner(runner =>
                {
                    var connectionstring = $"Server={Configuration["Database:Server"]};Port={Configuration["Database:Port"]};Database={Configuration["Database:Schema"]};Uid={Configuration["Database:User"]};Pwd={Configuration["Database:Password"]};";

                    runner.AddMySql5()
                        .WithGlobalConnectionString(connectionstring)
                        .ScanIn(typeof(Bootstrap).Assembly)
                        .For.Migrations()
                        .For.EmbeddedResources();
                });
            })
            .UseSerilog()
            .Build();

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                var host = BuildHost(args);

                var runner = host.Services.GetService<IMigrationRunner>();

                runner.MigrateUp();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}