using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JL_Migrator
{
    class Program
    {
        static void Main(string[] args) 
        {
            Thread.Sleep(5000);

            Logger.LogLine("Migrator is starting work ..", LogLevel.INFO);

            Logger.LogLine("Setting up base directory ..", LogLevel.INFO);

            try
            {
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                Logger.LogLine("Setting up completed!", LogLevel.SUCCESS);
            }
            catch (Exception ex)
            {
                Logger.LogLine(ex.Message, LogLevel.ERROR);
                return;
            }

            Logger.LogLine("Getting environment data ..", LogLevel.INFO);

            string environment = String.Empty;
            try
            {
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                Logger.LogLine($"Current environment: {environment}!", LogLevel.SUCCESS);
            }
            catch (Exception ex)
            {
                Logger.LogLine(ex.Message, LogLevel.ERROR);
                return;
            }

            Logger.LogLine($"Getting configuration ..", LogLevel.INFO);
            IConfiguration configuration = null;
            try
            {
                configuration = GetConfiguration(environment);
            }
            catch (Exception ex)
            {
                Logger.LogLine(ex.Message, LogLevel.ERROR);
                return;
            }


            Logger.LogLine("Getting connection string ..", LogLevel.INFO);
            string connectionString = String.Empty;
            try
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
                Logger.LogLine("Connection string received!", LogLevel.SUCCESS);
            }
            catch (Exception ex)
            {
                Logger.LogLine(ex.Message, LogLevel.ERROR);
                return;
            }

            if (!CheckConnection(connectionString))
            {
                Logger.LogLine("Connection is not established!", LogLevel.ERROR);
                return;
            }

            Logger.LogLine("Initializing migration service ..", LogLevel.INFO);
            try
            {
                IServiceCollection services = new ServiceCollection()
                    .AddLogging(x => x.AddFluentMigratorConsole())
                    .AddFluentMigratorCore()
                    .ConfigureRunner(x => x
                        .AddSqlServer2016()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(Assembly.GetExecutingAssembly()).For.All());

                var provider = services.BuildServiceProvider(false);
                using var scope = provider.CreateScope();

                Logger.LogLine("Starting migrations ..", LogLevel.INFO);
                scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();

                Logger.LogLine("Migrations completed", LogLevel.SUCCESS);
            }
            catch (Exception ex)
            {
                Logger.LogLine(ex.Message, LogLevel.ERROR);
                return;
            }
        }

        private static IConfiguration GetConfiguration(string environment)
        {
            if (string.IsNullOrEmpty(environment))
                throw new ArgumentNullException(nameof(environment));

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory);

            switch (environment)
            {
                case "Development":
                    configurationBuilder.AddJsonFile("appsettings-Development.json", true, true);
                    break;
                case "Production":
                    configurationBuilder.AddJsonFile("appsettings-Production.json", true, true);
                    break;
            }

            return configurationBuilder.Build();
        }

        private static bool CheckConnection(string connectionString)
        {
            try
            {
                Console.WriteLine("Connecting to: {0}", connectionString);
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = "select 1";
                    Console.WriteLine("Executing: {0}", query);

                    var command = new SqlCommand(query, connection);

                    connection.Open();
                    Console.WriteLine("SQL Connection successful.");

                    command.ExecuteScalar();
                    Console.WriteLine("SQL Query execution successful.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failure: {0}", ex.Message);
                return false;
            }
        }
    }
}
