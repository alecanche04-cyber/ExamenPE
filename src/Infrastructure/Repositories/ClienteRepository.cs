using FoodCampus.Application.DTOs;
using FoodCampus.Application.Interfaces;
using FoodCampus.Domain.Entities;
using FoodCampus.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FoodCampus.Infrastructure.Repositories;

/// <summary>
/// Implementación concreta de IClienteRepository usando EF Core 10.
/// Convierte entre la entidad de dominio Customer y el DTO ClienteDTO.
/// </summary>
public class ClienteRepository(FoodCampusDbContext context) : IClienteRepository
{
    public async Task<IEnumerable<ClienteDTO>> GetAllAsync()
    {
        return await context.Clientes
            .Select(c => new ClienteDTO
            {
                Id = c.Id,
                Nombre = c.Name,
                Correo = c.Email,
            })
            .ToListAsync();
    }

    public async Task<ClienteDTO?> GetByIdAsync(int id)
    {
        var entity = await context.Clientes.FindAsync(id);
        return entity is null ? null : ToDTO(entity);
    }

    public async Task<ClienteDTO?> GetByCorreoAsync(string correo)
    {
        var entity = await context.Clientes
            .FirstOrDefaultAsync(c => c.Email == correo);
        return entity is null ? null : ToDTO(entity);
    }

    public async Task<ClienteDTO> CreateAsync(ClienteDTO dto)
    {
        var entity = new Customer
        {
            Name = dto.Nombre,
            Email = dto.Correo,
        };

        context.Clientes.Add(entity);
        await context.SaveChangesAsync();

        return dto with { Id = entity.Id };
    }

    public async Task UpdateAsync(ClienteDTO dto)
    {
        var entity = await context.Clientes.FindAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Cliente con Id {dto.Id} no encontrado.");

        entity.Name = dto.Nombre;
        entity.Email = dto.Correo;

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Clientes.FindAsync(id)
            ?? throw new KeyNotFoundException($"Cliente con Id {id} no encontrado.");

        context.Clientes.Remove(entity);
        await context.SaveChangesAsync();
    }

    private static ClienteDTO ToDTO(Customer c) => new()
    {
        Id = c.Id,
        Nombre = c.Name,
        Correo = c.Email,
    };
}
