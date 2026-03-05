namespace FoodCampus.Domain.Exceptions;

public class InvalidRestaurantHoursException(TimeSpan openingTime, TimeSpan closingTime) 
    : DomainException($"Opening time ({openingTime}) must be earlier than closing time ({closingTime}).");
