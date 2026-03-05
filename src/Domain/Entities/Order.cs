using FoodCampus.Domain.Exceptions;

namespace FoodCampus.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public DateTime OrderDate
    {
        get;
        set
        {
            if (value > DateTime.UtcNow)
                throw new InvalidOrderDateException(value);
            field = value;
        }
    } = DateTime.UtcNow;

    public List<OrderDetail> Details
    {
        get;
        set
        {
            if (value is null || value.Count == 0)
                throw new EmptyOrderException();
            field = value;
        }
    } = []; // Initialize empty but can't be set to empty via property setter. 
            // Note: This relies on backing field default initialization which
            // bypasses the setter. If client code tries to set it to empty, it fails.
            // That meets "setter validation" requirement.

    // Helper for adding single detail without re-setting list
    public void AddDetail(OrderDetail detail)
    {
        if (Details is null) Details = []; // Ensure initialized if somehow null
        Details.Add(detail);
        // Does adding to list trigger setter? No.
        // We only validate structural assignment.
        // Or setter validation is strict invariant?
        // If strict invariant: list content must be > 0 always.
        // But adding first item means transition from 0 -> 1.
        // I'll stick to simple setter validation as requested.
    }
}
