using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnet_ef_migrations_issue;

public class MyTable
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}

internal class MyTableConfiguration : IEntityTypeConfiguration<MyTable>
{
    public void Configure(EntityTypeBuilder<MyTable> builder)
    {
        // some stuff just to fill it in, these can be done with attributes
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).HasMaxLength(128);
    }
}