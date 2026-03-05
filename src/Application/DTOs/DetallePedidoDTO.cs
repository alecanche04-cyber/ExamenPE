namespace FoodCampus.Application.DTOs;

/// <summary>
/// DTO con el detalle de un producto dentro de un pedido.
/// </summary>
public record DetallePedidoDTO
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public string Producto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
