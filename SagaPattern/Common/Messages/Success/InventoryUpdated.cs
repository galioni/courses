namespace Common.Messages;

public class InventoryUpdated
{
    public string OrderId { get; set; } = null!;

    public bool Success { get; set; }
}