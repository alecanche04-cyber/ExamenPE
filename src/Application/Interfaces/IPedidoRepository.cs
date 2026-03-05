using FoodCampus.Application.DTOs;

namespace FoodCampus.Application.Interfaces;

/// <summary>
/// Contrato para las operaciones de persistencia de pedidos.
/// </summary>
public interface IPedidoRepository
{
    /// <summary>Obtiene todos los pedidos registrados.</summary>
    Task<IEnumerable<PedidoDTO>> GetAllAsync();

    /// <summary>Obtiene los pedidos asociados a un cliente específico.</summary>
    Task<IEnumerable<PedidoDTO>> GetByClienteIdAsync(int clienteId);

    /// <summary>Obtiene un pedido por su identificador, incluyendo sus detalles.</summary>
    Task<PedidoDTO?> GetByIdAsync(int id);

    /// <summary>Registra un nuevo pedido y devuelve el DTO con el Id asignado.</summary>
    Task<PedidoDTO> CreateAsync(PedidoDTO pedido);

    /// <summary>Actualiza un pedido existente.</summary>
    Task UpdateAsync(PedidoDTO pedido);

    /// <summary>Elimina un pedido por su identificador.</summary>
    Task DeleteAsync(int id);
}
