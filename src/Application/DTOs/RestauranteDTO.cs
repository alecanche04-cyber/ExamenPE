namespace FoodCampus.Application.DTOs;

/// <summary>
/// DTO con los datos de un restaurante para transferencia entre capas.
/// </summary>
public record RestauranteDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public TimeSpan HorarioApertura { get; set; }
    public TimeSpan HorarioCierre { get; set; }
}
