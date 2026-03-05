using FoodCampus.Application.DTOs;
using FoodCampus.Application.Interfaces;

namespace FoodCampus.Application.UseCases;

/// <summary>
/// Caso de uso: Consultar todos los restaurantes disponibles.
/// Un restaurante está disponible si su horario abarca la hora actual.
/// </summary>
public class ConsultarRestaurantesUseCase(IRestauranteRepository restauranteRepo)
{
    /// <summary>
    /// Devuelve la lista de restaurantes disponibles en este momento.
    /// </summary>
    /// <returns>Colección de <see cref="RestauranteDTO"/> con los restaurantes disponibles.</returns>
    public async Task<IEnumerable<RestauranteDTO>> ExecuteAsync()
    {
        var horaActual = DateTime.UtcNow.TimeOfDay;
        return await restauranteRepo.GetDisponiblesAsync(horaActual);
    }

    /// <summary>
    /// Devuelve todos los restaurantes sin filtrar por disponibilidad.
    /// </summary>
    public async Task<IEnumerable<RestauranteDTO>> ExecuteAllAsync()
    {
        return await restauranteRepo.GetAllAsync();
    }
}
