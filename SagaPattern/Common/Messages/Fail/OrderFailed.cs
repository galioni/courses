namespace Common.Messages;

public class OrderFailed
{
    public string OrderId { get; set; } = null!;

    public string Reason { get; set; } = null!;
}