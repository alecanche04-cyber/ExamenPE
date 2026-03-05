using FoodCampus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodCampus.Infrastructure.Mappings;

/// <summary>
/// Configuración Fluent API para la entidad OrderDetail → tabla "DetallesPedido".
/// Incluye la shadow property "ProductName" que no existe en la entidad de dominio.
/// </summary>
public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("DetallesPedido");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).ValueGeneratedOnAdd();

        builder.Property(d => d.OrderId).IsRequired();

        // Shadow property: nombre del producto pedido
        builder.Property<string>("ProductName")
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(d => d.Quantity)
            .IsRequired()
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        builder.Property(d => d.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
    }
}
