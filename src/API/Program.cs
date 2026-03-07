using FoodCampus.API;
using FoodCampus.Infrastructure;
using FoodCampus.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var connectionString = ctx.Configuration.GetConnectionString("FoodCampusDB")
            ?? throw new InvalidOperationException("La cadena de conexión 'FoodCampusDB' no está configurada.");

        services.AddInfrastructure(connectionString);
        services.AddScoped<Menu>();
    })
    .Build();

Console.WriteLine("=== FoodCampus ===");
Console.Write("Verificando conexión a la base de datos... ");

try
{
    var db = host.Services.GetRequiredService<FoodCampusDbContext>();

    // Open a real connection so the CLI surfaces the exact SQL error.
    await db.Database.OpenConnectionAsync();
    await db.Database.CloseConnectionAsync();

    Console.WriteLine("CONECTADO correctamente a ExamenPE en Somee.");
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR de conexión: {ex.Message}");

    var inner = ex.InnerException;
    while (inner is not null)
    {
        Console.WriteLine($"Detalle: {inner.Message}");
        inner = inner.InnerException;
    }

    return;
}

await host.StartAsync();

using (var scope = host.Services.CreateScope())
{
    var menu = scope.ServiceProvider.GetRequiredService<Menu>();
    await menu.EjecutarAsync();
}

await host.StopAsync();
