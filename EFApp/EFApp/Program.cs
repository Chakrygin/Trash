using System;
using System.Text.Json;
using System.Threading.Tasks;

using EFApp.DataAccess;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Npgsql;

namespace EFApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            services.AddDbContext<MyContext>((sp, builder) =>
            {
                var csb = new NpgsqlConnectionStringBuilder
                {
                    Host = "127.0.0.1",
                    Port = 5432,

                    Database = "postgres",
                    Username = "postgres",
                    Password = "postgres",
                };

                builder
                    .UseNpgsql(csb.ToString())
                    .UseSnakeCaseNamingConvention();

                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                builder.UseLoggerFactory(loggerFactory);
                builder.EnableSensitiveDataLogging();
            });

            using var serviceProvider = services.BuildServiceProvider();

            {
                using var scope = serviceProvider.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<MyContext>();
            }
        }

        private static void Print(object value)
        {
            var json = JsonSerializer.Serialize(value, new JsonSerializerOptions {WriteIndented = true});

            Console.WriteLine(json);
            Console.WriteLine();
        }
    }
}