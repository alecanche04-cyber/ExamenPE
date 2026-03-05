using System.Text.RegularExpressions;
using FoodCampus.Domain.Exceptions;

namespace FoodCampus.Domain.Entities;

public partial class Customer
{
    public int Id { get; set; }

    public string Name
    {
        get;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new CustomerNameEmptyException();
            field = value;
        }
    } = string.Empty;

    public string Email
    {
        get;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !EmailRegex().IsMatch(value))
                throw new InvalidCustomerEmailException(value);
            field = value;
        }
    } = string.Empty;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
