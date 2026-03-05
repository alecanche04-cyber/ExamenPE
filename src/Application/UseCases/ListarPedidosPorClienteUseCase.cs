using FoodCampus.Application.DTOs;
using FoodCampus.Application.Interfaces;

namespace FoodCampus.Application.UseCases;

/// <summary>
/// Caso de uso: Listar los pedidos de un cliente específico.
/// Valida que el cliente exista antes de consultar sus pedidos.
/// </summary>
public class ListarPedidosPorClienteUseCase(IPedidoRepository pedidoRepo, IClienteRepository clienteRepo)
{
    /// <summary>
    /// Devuelve todos los pedidos asociados al cliente indicado.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <returns>Colección de <see cref="PedidoDTO"/> del cliente.</returns>
    /// <exception cref="ArgumentException">Si el cliente no existe.</exception>
    public async Task<IEnumerable<PedidoDTO>> ExecuteAsync(int clienteId)
    {
        var cliente = await clienteRepo.GetByIdAsync(clienteId);
        if (cliente is null)
            throw new ArgumentException($"No existe un cliente con Id {clienteId}.");

        return await pedidoRepo.GetByClienteIdAsync(clienteId);
    }
}
