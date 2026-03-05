using FoodCampus.API;
using FoodCampus.Infrastructure;
using FoodCampus.Infrastructure.Context;
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
    bool conectado = await db.Database.CanConnectAsync();
    Console.WriteLine(conectado
        ? "CONECTADO correctamente a ExamenPE en Somee."
        : "ERROR: No se pudo conectar. Verifica las credenciales en appsettings.json.");
    if (!conectado) return;
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR de conexión: {ex.Message}");
    return;
}

await host.StartAsync();

using (var scope = host.Services.CreateScope())
{
    var menu = scope.ServiceProvider.GetRequiredService<Menu>();
    await menu.EjecutarAsync();
}

await host.StopAsync();
