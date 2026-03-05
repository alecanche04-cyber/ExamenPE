using FoodCampus.Application.DTOs;
using FoodCampus.Application.Interfaces;

namespace FoodCampus.Application.UseCases;

/// <summary>
/// Caso de uso: Registrar un nuevo pedido.
/// Valida que el pedido tenga al menos un detalle y que el cliente exista.
/// </summary>
public class RegistrarPedidoUseCase(IPedidoRepository pedidoRepo, IClienteRepository clienteRepo)
{
    /// <summary>
    /// Registra el pedido si supera las validaciones de negocio.
    /// </summary>
    /// <param name="pedido">DTO con los datos del pedido a registrar.</param>
    /// <returns>DTO del pedido registrado con su Id asignado.</returns>
    /// <exception cref="ArgumentException">Si el pedido no tiene detalles o el cliente no existe.</exception>
    public async Task<PedidoDTO> ExecuteAsync(PedidoDTO pedido)
    {
        if (pedido.Detalles is null || pedido.Detalles.Count == 0)
            throw new ArgumentException("El pedido debe contener al menos un detalle.");

        var cliente = await clienteRepo.GetByIdAsync(pedido.ClienteId);
        if (cliente is null)
            throw new ArgumentException($"No existe un cliente con Id {pedido.ClienteId}.");

        return await pedidoRepo.CreateAsync(pedido);
    }
}
