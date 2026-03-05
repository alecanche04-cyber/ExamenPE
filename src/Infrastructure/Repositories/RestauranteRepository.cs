using FoodCampus.Application.DTOs;
using FoodCampus.Application.Interfaces;
using FoodCampus.Domain.Entities;
using FoodCampus.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FoodCampus.Infrastructure.Repositories;

/// <summary>
/// Implementación concreta de IRestauranteRepository usando EF Core 10.
/// Convierte entre la entidad de dominio Restaurant y el DTO RestauranteDTO.
/// </summary>
public class RestauranteRepository(FoodCampusDbContext context) : IRestauranteRepository
{
    public async Task<IEnumerable<RestauranteDTO>> GetAllAsync()
    {
        return await context.Restaurantes
            .Select(r => new RestauranteDTO
            {
                Id = r.Id,
                Nombre = r.Name,
                HorarioApertura = r.OpeningTime,
                HorarioCierre = r.ClosingTime,
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<RestauranteDTO>> GetDisponiblesAsync(TimeSpan horaActual)
    {
        return await context.Restaurantes
            .Where(r => r.OpeningTime <= horaActual && r.ClosingTime >= horaActual)
            .Select(r => new RestauranteDTO
            {
                Id = r.Id,
                Nombre = r.Name,
                HorarioApertura = r.OpeningTime,
                HorarioCierre = r.ClosingTime,
            })
            .ToListAsync();
    }

    public async Task<RestauranteDTO?> GetByIdAsync(int id)
    {
        var entity = await context.Restaurantes.FindAsync(id);
        return entity is null ? null : ToDTO(entity);
    }

    public async Task<RestauranteDTO> CreateAsync(RestauranteDTO dto)
    {
        var entity = new Restaurant { Name = dto.Nombre };

        // Asignar ClosingTime antes que OpeningTime para respetar la validación cruzada:
        // el setter de OpeningTime valida contra ClosingTime, así el orden importa.
        entity.ClosingTime = dto.HorarioCierre;
        entity.OpeningTime = dto.HorarioApertura;

        context.Restaurantes.Add(entity);
        await context.SaveChangesAsync();

        return dto with { Id = entity.Id };
    }

    public async Task UpdateAsync(RestauranteDTO dto)
    {
        var entity = await context.Restaurantes.FindAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Restaurante con Id {dto.Id} no encontrado.");

        entity.Name = dto.Nombre;
        entity.ClosingTime = dto.HorarioCierre;
        entity.OpeningTime = dto.HorarioApertura;

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Restaurantes.FindAsync(id)
            ?? throw new KeyNotFoundException($"Restaurante con Id {id} no encontrado.");

        context.Restaurantes.Remove(entity);
        await context.SaveChangesAsync();
    }

    private static RestauranteDTO ToDTO(Restaurant r) => new()
    {
        Id = r.Id,
        Nombre = r.Name,
        HorarioApertura = r.OpeningTime,
        HorarioCierre = r.ClosingTime,
    };
}
