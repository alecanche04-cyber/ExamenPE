using FoodCampus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FoodCampus.Infrastructure.Context;

namespace FoodCampus.Infrastructure.Mappings;

/// <summary>
/// Configuración Fluent API para la entidad Restaurant → tabla "Restaurantes".
/// </summary>
public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable("Restaurantes");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(200);

        // PreferFieldDuringConstruction: EF Core usa el backing field (<field>) 
        // al materializar entidades desde DB, evitando que se dispare la validación
        // cruzada (OpeningTime ↔ ClosingTime) durante la reconstrucción del objeto.
        builder.Property(r => r.OpeningTime)
            .IsRequired()
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        builder.Property(r => r.ClosingTime)
            .IsRequired()
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        // Relación: Restaurant (1) → Order (N)
        // La FK "RestaurantId" es una shadow property definida en OrderConfiguration.
        builder.HasMany<Order>()
            .WithOne()
            .HasForeignKey("RestaurantId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
