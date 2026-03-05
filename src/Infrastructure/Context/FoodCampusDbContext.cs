using FoodCampus.Domain.Entities;
using FoodCampus.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace FoodCampus.Infrastructure.Context;

/// <summary>
/// DbContext principal de FoodCampus. Expone los DbSets de todas las entidades
/// y aplica las configuraciones Fluent API definidas en Infrastructure/Mappings.
/// </summary>
public class FoodCampusDbContext(DbContextOptions<FoodCampusDbContext> options) : DbContext(options)
{
    /// <summary>Tabla de restaurantes.</summary>
    public DbSet<Restaurant> Restaurantes => Set<Restaurant>();

    /// <summary>Tabla de pedidos.</summary>
    public DbSet<Order> Pedidos => Set<Order>();

    /// <summary>Tabla de detalles de pedido.</summary>
    public DbSet<OrderDetail> DetallesPedido => Set<OrderDetail>();

    /// <summary>Tabla de clientes.</summary>
    public DbSet<Customer> Clientes => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RestaurantConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}
