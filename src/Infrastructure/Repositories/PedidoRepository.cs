using FoodCampus.Application.DTOs;
using FoodCampus.Application.Interfaces;
using FoodCampus.Domain.Entities;
using FoodCampus.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FoodCampus.Infrastructure.Repositories;

/// <summary>
/// Implementación concreta de IPedidoRepository usando EF Core 10.
/// Gestiona la persistencia de Orders y sus OrderDetails.
///
/// Nota de diseño: Las propiedades "CustomerId", "RestaurantId" y "ProductName"
/// son shadow properties configuradas en Fluent API porque las entidades de dominio
/// puro no las exponen. Se accede a ellas mediante EF.Property&lt;T&gt;() y
/// context.Entry(entity).Property().CurrentValue.
/// </summary>
public class PedidoRepository(FoodCampusDbContext context) : IPedidoRepository
{
    public async Task<IEnumerable<PedidoDTO>> GetAllAsync()
    {
        // Proyección directa a DTO usando EF.Property para leer shadow properties
        return await context.Pedidos
            .Include(o => o.Details)
            .Select(o => new PedidoDTO
            {
                Id = o.Id,
                Fecha = o.OrderDate,
                ClienteId = EF.Property<int>(o, "CustomerId"),
                RestauranteId = EF.Property<int>(o, "RestaurantId"),
                Detalles = o.Details.Select(d => new DetallePedidoDTO
                {
                    Id = d.Id,
                    PedidoId = d.OrderId,
                    Producto = EF.Property<string>(d, "ProductName"),
                    Cantidad = d.Quantity,
                    PrecioUnitario = d.UnitPrice,
                }).ToList(),
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PedidoDTO>> GetByClienteIdAsync(int clienteId)
    {
        return await context.Pedidos
            .Include(o => o.Details)
            .Where(o => EF.Property<int>(o, "CustomerId") == clienteId)
            .Select(o => new PedidoDTO
            {
                Id = o.Id,
                Fecha = o.OrderDate,
                ClienteId = clienteId,
                RestauranteId = EF.Property<int>(o, "RestaurantId"),
                Detalles = o.Details.Select(d => new DetallePedidoDTO
                {
                    Id = d.Id,
                    PedidoId = d.OrderId,
                    Producto = EF.Property<string>(d, "ProductName"),
                    Cantidad = d.Quantity,
                    PrecioUnitario = d.UnitPrice,
                }).ToList(),
            })
            .ToListAsync();
    }

    public async Task<PedidoDTO?> GetByIdAsync(int id)
    {
        var entity = await context.Pedidos
            .Include(o => o.Details)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (entity is null) return null;

        var clienteId = context.Entry(entity).Property<int>("CustomerId").CurrentValue;
        var restauranteId = context.Entry(entity).Property<int>("RestaurantId").CurrentValue;

        return new PedidoDTO
        {
            Id = entity.Id,
            Fecha = entity.OrderDate,
            ClienteId = clienteId,
            RestauranteId = restauranteId,
            Detalles = entity.Details.Select(d => new DetallePedidoDTO
            {
                Id = d.Id,
                PedidoId = d.OrderId,
                Producto = context.Entry(d).Property<string>("ProductName").CurrentValue ?? string.Empty,
                Cantidad = d.Quantity,
                PrecioUnitario = d.UnitPrice,
            }).ToList(),
        };
    }

    public async Task<PedidoDTO> CreateAsync(PedidoDTO dto)
    {
        var entity = new Order { OrderDate = dto.Fecha };

        // Añadir detalles a la entidad antes de persistir
        foreach (var detalleDto in dto.Detalles)
        {
            entity.AddDetail(new OrderDetail
            {
                Quantity = detalleDto.Cantidad,
                UnitPrice = detalleDto.PrecioUnitario,
            });
        }

        // context.Add realiza graph traversal y registra la entidad y sus detalles
        context.Pedidos.Add(entity);

        // Asignar shadow properties de FK sobre las entidades ya rastreadas
        context.Entry(entity).Property("CustomerId").CurrentValue = dto.ClienteId;
        context.Entry(entity).Property("RestaurantId").CurrentValue = dto.RestauranteId;

        foreach (var (detalle, detalleDto) in entity.Details.Zip(dto.Detalles))
            context.Entry(detalle).Property("ProductName").CurrentValue = detalleDto.Producto;

        await context.SaveChangesAsync();

        return dto with { Id = entity.Id };
    }

    public async Task UpdateAsync(PedidoDTO dto)
    {
        var entity = await context.Pedidos
            .Include(o => o.Details)
            .FirstOrDefaultAsync(o => o.Id == dto.Id)
            ?? throw new KeyNotFoundException($"Pedido con Id {dto.Id} no encontrado.");

        entity.OrderDate = dto.Fecha;
        context.Entry(entity).Property("CustomerId").CurrentValue = dto.ClienteId;
        context.Entry(entity).Property("RestaurantId").CurrentValue = dto.RestauranteId;

        // Eliminar detalles anteriores explícitamente (EF Core no usa proxy de colección)
        var detallesAnteriores = entity.Details.ToList();
        context.DetallesPedido.RemoveRange(detallesAnteriores);
        entity.Details.Clear();

        // Agregar los nuevos detalles
        foreach (var detalleDto in dto.Detalles)
        {
            var detalle = new OrderDetail
            {
                OrderId = entity.Id,
                Quantity = detalleDto.Cantidad,
                UnitPrice = detalleDto.PrecioUnitario,
            };
            context.DetallesPedido.Add(detalle); // registrar en el tracker explícitamente
            context.Entry(detalle).Property("ProductName").CurrentValue = detalleDto.Producto;
            entity.AddDetail(detalle);
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Pedidos.FindAsync(id)
            ?? throw new KeyNotFoundException($"Pedido con Id {id} no encontrado.");

        context.Pedidos.Remove(entity);
        await context.SaveChangesAsync();
    }
}
