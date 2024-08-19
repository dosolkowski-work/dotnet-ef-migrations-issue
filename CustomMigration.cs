using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.Internal;

namespace dotnet_ef_migrations_issue;

public class MyMigrationDiffer(
    IRelationalTypeMappingSource typeMappingSource,
    IMigrationsAnnotationProvider migrationsAnnotationProvider,
    IRowIdentityMapFactory rowIdentityMapFactory,
    CommandBatchPreparerDependencies commandBatchPreparerDependencies)
    : MigrationsModelDiffer(typeMappingSource, migrationsAnnotationProvider, rowIdentityMapFactory, commandBatchPreparerDependencies)
{
    public override IReadOnlyList<MigrationOperation> GetDifferences(IRelationalModel? source, IRelationalModel? target)
    {
        Console.WriteLine(">>> USING MY DIFFER");
        return base.GetDifferences(source, target);
    }

    protected override IEnumerable<MigrationOperation> Diff(ITable source, ITable target, DiffContext diffContext)
    {
        Console.WriteLine($">>> Comparing {source.Name} to {target.Name}");

        // Note: neither source nor target appears to have the annotation we explicitly set
        foreach (var annotation in source.GetAnnotations())
        {
            Console.WriteLine($"Source has {annotation.Name} = {annotation.Value}");
        }

        foreach (var annotation in target.GetAnnotations())
        {
            Console.WriteLine($"Target has {annotation.Name} = {annotation.Value}");
        }

        // Note: base doesn't generate any operations at all
        foreach (var op in base.Diff(source, target, diffContext))
        {
            Console.WriteLine($">>> Generated {op.GetType().Name} for {source.Name} -> {target.Name}");
            yield return op;
        }
    }
}

// Note: this never seems to be used
public class MyAnnotationProvider(RelationalAnnotationProviderDependencies dependencies)
    : NpgsqlAnnotationProvider(dependencies)
{
    public override IEnumerable<IAnnotation> For(ITable table, bool designTime)
    {
        Console.WriteLine($">>>>>> Generating annotations for {table.Name}");

        foreach (var annotation in base.For(table, designTime))
        {
            Console.WriteLine($"- {annotation.Name} = {annotation.Value}");
            yield return annotation;
        }
    }
}

public class MyDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        Console.WriteLine(">>> Customizing services from main database");

        services.AddSingleton<IMigrationsModelDiffer, MyMigrationDiffer>();
        services.RemoveAll<IRelationalAnnotationProvider>();
        services.AddSingleton<IRelationalAnnotationProvider, MyAnnotationProvider>();
    }
}