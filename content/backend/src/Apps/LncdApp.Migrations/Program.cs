using System;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using LeanCode.EFMigrator;
using LncdApp.DomainName.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LncdApp.Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        new Migrator().Run(args);
    }
}

internal class Migrator : LeanCode.EFMigrator.Migrator
{
    protected override void MigrateAll()
    {
        Migrate<DomainNameDbContext, DomainNameDbContextFactory>();
        Migrate<PersistedGrantDbContext, PersistedGrantDbContextFactory>();
    }
}

internal class DomainNameDbContextFactory : BaseFactory<DomainNameDbContext, DomainNameDbContextFactory> { }

public class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
{
    protected virtual string AssemblyName => typeof(PersistedGrantDbContextFactory).Assembly.GetName().Name!;

    public PersistedGrantDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>()
            .UseLoggerFactory(new ServiceCollection()
                .AddLogging(cfg => cfg.AddConsole())
                .BuildServiceProvider()
                .GetRequiredService<ILoggerFactory>())
            .UseSqlServer(
                MigrationsConfig.GetConnectionString()
                ?? throw new InvalidOperationException("Connection string missing"),
                cfg => cfg.MigrationsAssembly(AssemblyName));

        return new PersistedGrantDbContext(
            builder.Options,
            new OperationalStoreOptions
            {
                DefaultSchema = "auth",
            });
    }
}
