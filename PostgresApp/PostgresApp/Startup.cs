using System;

using Evolve.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Npgsql;

namespace PostgresApp
{
    public sealed class Startup
    {
        public Startup()
        {
            var csb = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Port = 5432,

                Database = "postgres",
                Username = "postgres",
                Password = "postgres",
            };

            ConnectionString = csb.ToString();
        }

        public string ConnectionString { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Migrate(app.ApplicationServices, ConnectionString);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }

        private void Migrate(IServiceProvider serviceProvider, string connectionString)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();

            try
            {
                using var connection = new NpgsqlConnection(connectionString);

                var evolve = new Evolve.Evolve(connection, msg => logger.LogInformation(msg))
                {
                    Locations = new[] {"migrations"},
                    Command = CommandOptions.Erase,
                    IsEraseDisabled = false,
                };

                evolve.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogCritical("Database migration failed.", ex);
                throw;
            }
        }
    }
}