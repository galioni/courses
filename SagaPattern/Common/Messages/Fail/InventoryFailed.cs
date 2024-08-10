namespace Common.Messages;

public class InventoryFailed
{
    public string OrderId { get; set; } = null!;

    public string Reason { get; set; } = null!;
}