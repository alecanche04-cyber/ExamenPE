using FoodCampus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodCampus.Infrastructure.Mappings;

/// <summary>
/// Configuración Fluent API para la entidad Customer → tabla "Clientes".
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        // PreferFieldDuringConstruction: evita que el setter valide durante
        // la materialización cuando EF Core aún no ha asignado todos los campos.
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200)
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(320)
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        // El correo debe ser único en la base de datos
        builder.HasIndex(c => c.Email).IsUnique();
    }
}
