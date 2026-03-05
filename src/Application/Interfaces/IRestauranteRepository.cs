using FoodCampus.Application.DTOs;

namespace FoodCampus.Application.Interfaces;

/// <summary>
/// Contrato para las operaciones de persistencia de restaurantes.
/// </summary>
public interface IRestauranteRepository
{
    /// <summary>Obtiene todos los restaurantes registrados.</summary>
    Task<IEnumerable<RestauranteDTO>> GetAllAsync();

    /// <summary>Obtiene los restaurantes cuyo horario incluye el momento actual.</summary>
    Task<IEnumerable<RestauranteDTO>> GetDisponiblesAsync(TimeSpan horaActual);

    /// <summary>Obtiene un restaurante por su identificador.</summary>
    Task<RestauranteDTO?> GetByIdAsync(int id);

    /// <summary>Crea un nuevo restaurante y devuelve el DTO con el Id asignado.</summary>
    Task<RestauranteDTO> CreateAsync(RestauranteDTO restaurante);

    /// <summary>Actualiza los datos de un restaurante existente.</summary>
    Task UpdateAsync(RestauranteDTO restaurante);

    /// <summary>Elimina un restaurante por su identificador.</summary>
    Task DeleteAsync(int id);
}
