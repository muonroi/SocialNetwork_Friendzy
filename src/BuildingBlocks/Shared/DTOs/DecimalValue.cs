namespace Shared.DTOs;

public class DecimalValue
{
    public long Units { get; set; }
    public int Nanos { get; set; }

    public decimal ToDecimal()
    {
        return Units + (Nanos / 1e9m);
    }
}