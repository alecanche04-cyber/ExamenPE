namespace FoodCampus.Application.DTOs;

/// <summary>
/// DTO con los datos de un pedido, incluyendo la lista de detalles asociados.
/// </summary>
public record PedidoDTO
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public int RestauranteId { get; set; }
    public List<DetallePedidoDTO> Detalles { get; set; } = [];
}
