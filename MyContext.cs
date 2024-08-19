using Microsoft.EntityFrameworkCore;

namespace dotnet_ef_migrations_issue;

public class MyContext(DbContextOptions<MyContext> options) : DbContext(options)
{
    public DbSet<MyTable> MyTables => Set<MyTable>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Apply all configuration from IEntityTypeConfiguration<T>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyContext).Assembly);
    }
}