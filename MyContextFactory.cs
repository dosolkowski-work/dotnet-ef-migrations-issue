using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace dotnet_ef_migrations_issue;

internal class MyContextFactory : IDesignTimeDbContextFactory<MyContext>
{
    public MyContext CreateDbContext(string[] args)
    {
        NpgsqlConnectionStringBuilder builder = new()
        {
            Host = "localhost",
            Port = 5432,
            Database = "testing",
            // we don't really care about connecting to an actual database
        };

        var options = new DbContextOptionsBuilder<MyContext>()
            .UseNpgsql(builder.ConnectionString)
            .Options;

        return new(options);
    }
}