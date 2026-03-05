namespace FoodCampus.Domain.Exceptions;

public class CustomerNameEmptyException() 
    : DomainException("Customer name cannot be empty.");

public class InvalidCustomerEmailException(string email) 
    : DomainException($"Customer email '{email}' is invalid.");
