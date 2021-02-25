using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Npgsql;

namespace EFApp.DataAccess
{
public sealed class MyContextFactory : IDesignTimeDbContextFactory<MyContext>
{
    public MyContext CreateDbContext(string[] args)
    {
        var csb = new NpgsqlConnectionStringBuilder
        {
            Host = "127.0.0.1",
            Port = 5432,

            Database = "postgres",
            Username = "postgres",
            Password = "postgres",
        };

        var builder = new DbContextOptionsBuilder<MyContext>()
            .UseNpgsql(csb.ToString())
            .UseSnakeCaseNamingConvention();

        return new MyContext(builder.Options);
    }
}
}