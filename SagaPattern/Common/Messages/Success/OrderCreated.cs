namespace Common.Messages;

public class OrderCreated
{
    public string OrderId { get; set; } = null!;

    public Double Amount { get; set; }
}