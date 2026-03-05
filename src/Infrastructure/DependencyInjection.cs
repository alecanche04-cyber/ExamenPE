using FoodCampus.Application.Interfaces;
using FoodCampus.Application.UseCases;
using FoodCampus.Infrastructure.Context;
using FoodCampus.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodCampus.Infrastructure;

/// <summary>
/// Extensión de IServiceCollection para registrar todos los servicios de Infrastructure.
/// Uso desde Program.cs:
///   builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Default")!);
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        // Registrar DbContext con SQL Server como proveedor
        services.AddDbContext<FoodCampusDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sql => sql.MigrationsAssembly(typeof(FoodCampusDbContext).Assembly.FullName)));

        // Registrar repositorios concretos ligados a sus interfaces de Application
        services.AddScoped<IRestauranteRepository, RestauranteRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();

        // Registrar casos de uso de Application
        services.AddScoped<ConsultarRestaurantesUseCase>();
        services.AddScoped<RegistrarPedidoUseCase>();
        services.AddScoped<ListarPedidosPorClienteUseCase>();

        return services;
    }
}
