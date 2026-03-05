namespace FoodCampus.Application.DTOs;

/// <summary>
/// DTO con los datos públicos de un cliente.
/// </summary>
public record ClienteDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
}
