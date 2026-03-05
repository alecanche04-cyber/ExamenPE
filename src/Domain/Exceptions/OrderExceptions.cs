namespace FoodCampus.Domain.Exceptions;

public class InvalidOrderDateException(DateTime date) 
    : DomainException($"Order date cannot be in the future (was {date}).");

public class EmptyOrderException() 
    : DomainException("Order must have at least one detail.");

public class InvalidOrderDetailQuantityException(int quantity) 
    : DomainException($"Order detail quantity must be greater than zero (was {quantity}).");

public class InvalidOrderDetailPriceException(decimal price) 
    : DomainException($"Order detail unit price must be greater than zero (was {price}).");
