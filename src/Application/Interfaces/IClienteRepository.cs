using FoodCampus.Application.DTOs;

namespace FoodCampus.Application.Interfaces;

/// <summary>
/// Contrato para las operaciones de persistencia de clientes.
/// </summary>
public interface IClienteRepository
{
    /// <summary>Obtiene todos los clientes registrados.</summary>
    Task<IEnumerable<ClienteDTO>> GetAllAsync();

    /// <summary>Obtiene un cliente por su identificador.</summary>
    Task<ClienteDTO?> GetByIdAsync(int id);

    /// <summary>Busca un cliente por su correo electrónico.</summary>
    Task<ClienteDTO?> GetByCorreoAsync(string correo);

    /// <summary>Registra un nuevo cliente y devuelve el DTO con el Id asignado.</summary>
    Task<ClienteDTO> CreateAsync(ClienteDTO cliente);

    /// <summary>Actualiza los datos de un cliente existente.</summary>
    Task UpdateAsync(ClienteDTO cliente);

    /// <summary>Elimina un cliente por su identificador.</summary>
    Task DeleteAsync(int id);
}
