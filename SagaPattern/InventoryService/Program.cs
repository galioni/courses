using Common.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace InventoryService;

public class Program
{
    public static bool IsInventoryUpdateSuccessful = true;

    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "inventory_exchange", type: ExchangeType.Topic);
        channel.QueueDeclare(queue: "inventory_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: "inventory_queue", exchange: "inventory_exchange", routingKey: "inventory_updated");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var inventoryUpdated = JsonConvert.DeserializeObject<InventoryUpdated>(message);

            Console.WriteLine($"InventoryService: Inventory updated for order - {inventoryUpdated.OrderId}");

            if (IsInventoryUpdateSuccessful)
            {
                // Successfully updated inventory, no further action needed
            }
            else
            {
                var inventoryFailed = new InventoryFailed { OrderId = inventoryUpdated.OrderId, Reason = "Inventory update failed" };
                var inventoryFailedMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(inventoryFailed));
                channel.BasicPublish(exchange: "inventory_exchange", routingKey: "inventory_failed", basicProperties: null, body: inventoryFailedMessage);
            }
        };

        channel.BasicConsume(queue: "inventory_queue", autoAck: true, consumer: consumer);
        Console.WriteLine("InventoryService: Listening for messages...");
        Console.ReadLine();
    }

    public static void SetInventoryUpdateSuccess(bool success)
    {
        IsInventoryUpdateSuccessful = success;
    }
}