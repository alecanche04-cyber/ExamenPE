using FoodCampus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodCampus.Infrastructure.Mappings;

/// <summary>
/// Configuración Fluent API para la entidad Order → tabla "Pedidos".
/// Define shadow properties para las FKs que no existen en el dominio puro.
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();

        builder.Property(o => o.OrderDate)
            .IsRequired()
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        // Shadow property: FK hacia la tabla Clientes
        builder.Property<int>("CustomerId").IsRequired();

        // Shadow property: FK hacia la tabla Restaurantes
        builder.Property<int>("RestaurantId").IsRequired();

        // Relación: Order (N) → Customer (1)
        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey("CustomerId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Relación: Order (1) → OrderDetail (N), con eliminación en cascada
        builder.HasMany(o => o.Details)
            .WithOne()
            .HasForeignKey(d => d.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
