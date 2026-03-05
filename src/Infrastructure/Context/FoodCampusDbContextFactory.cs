using FoodCampus.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FoodCampus.Infrastructure;

/// <summary>
/// Fábrica en tiempo de diseño utilizada exclusivamente por las herramientas de EF Core
/// (dotnet ef migrations add / dotnet ef database update).
/// No se usa en tiempo de ejecución.
///
/// Uso:
///   dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/API
///   dotnet ef database update            --project src/Infrastructure --startup-project src/API
/// </summary>
public class FoodCampusDbContextFactory : IDesignTimeDbContextFactory<FoodCampusDbContext>
{
    public FoodCampusDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FoodCampusDbContext>();

        // Cadena de conexión usada únicamente en tiempo de diseño (migraciones)
        const string connectionString =
            "Data Source=ExamenPE.mssql.somee.com;" +
            "Initial Catalog=ExamenPE;" +
            "User Id=AleUTM;" +
            "Password=4qsRMippJN77K@3;" +
            "TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(
            connectionString,
            sql => sql.MigrationsAssembly(typeof(FoodCampusDbContext).Assembly.FullName));

        return new FoodCampusDbContext(optionsBuilder.Options);
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          