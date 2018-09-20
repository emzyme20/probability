using System;
using System.IO;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Serilog;

namespace Probability
{
    public class Program
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // Integration testing framework depends on this method
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();

        public static void Main(string[] args)
        {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .AddJsonFile($"appsettings.{currentEnv}.json", true)
                                .AddEnvironmentVariables()
                                .Build();

            Log.Logger =
                new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(configuration)
                    .WriteTo.Logger(
                        lc => lc
                              .Filter.ByIncludingOnly(
                                  logEvent =>
                                      logEvent.Properties.ContainsKey("AuditLogEntry"))
                              .WriteTo.File($"{Path.Combine(Directory.GetCurrentDirectory(), "bin\\AuditLog\\auditlog.log").ToString()}"))
                    .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
