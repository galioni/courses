namespace Common.Messages;
public class PaymentFailed
{
    public string OrderId { get; set; } = null!;

    public string Reason { get; set; } = null!;
}