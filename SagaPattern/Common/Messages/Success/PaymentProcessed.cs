namespace Common.Messages;

public class PaymentProcessed
{
    public string OrderId { get; set; } = null!;

    public bool Success { get; set; }
}
