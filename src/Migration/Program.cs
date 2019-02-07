using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migration.Migrations;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Migration
{
    public class Database
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string Schema { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

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

                services.Configure<Database>(hostContext.Configuration.GetSection("Database"));
            })
            .UseSerilog()
            .Build();

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                var host = BuildHost(args);

                host.
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}