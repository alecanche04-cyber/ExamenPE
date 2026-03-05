using FoodCampus.Domain.Exceptions;

namespace FoodCampus.Domain.Entities;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public TimeSpan OpeningTime
    {
        get;
        set
        {
            if (value >= ClosingTime)
                throw new InvalidRestaurantHoursException(value, ClosingTime);
            field = value;
        }
    } = TimeSpan.Zero;

    public TimeSpan ClosingTime
    {
        get;
        set
        {
            if (OpeningTime >= value)
                throw new InvalidRestaurantHoursException(OpeningTime, value);
            field = value;
        }
    } = new TimeSpan(23, 59, 59); // Default close before midnight
}
