using FoodCampus.Domain.Exceptions;

namespace FoodCampus.Domain.Entities;

public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; } // FK

    public int Quantity
    {
        get;
        set
        {
            if (value <= 0)
                throw new InvalidOrderDetailQuantityException(value);
            field = value;
        }
    }

    public decimal UnitPrice
    {
        get;
        set
        {
            if (value <= 0)
                throw new InvalidOrderDetailPriceException(value);
            field = value;
        }
    }
}
