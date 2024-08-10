using Common.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PaymentService;

public class Program
{
    public static bool IsPaymentProcessingSuccessful = true;

    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "payment_exchange", type: ExchangeType.Topic);
        channel.QueueDeclare(queue: "payment_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: "payment_queue", exchange: "payment_exchange", routingKey: "payment_processed");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var paymentProcessed = JsonConvert.DeserializeObject<PaymentProcessed>(message);

            Console.WriteLine($"PaymentService: Payment processed for order - {paymentProcessed.OrderId}");

            if (IsPaymentProcessingSuccessful)
            {
                var inventoryUpdated = new InventoryUpdated { OrderId = paymentProcessed.OrderId, Success = true };
                var inventoryMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(inventoryUpdated));
                channel.BasicPublish(exchange: "inventory_exchange", routingKey: "inventory_updated", basicProperties: null, body: inventoryMessage);
            }
            else
            {
                var paymentFailed = new PaymentFailed { OrderId = paymentProcessed.OrderId, Reason = "Payment processing failed" };
                var paymentFailedMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentFailed));
                channel.BasicPublish(exchange: "payment_exchange", routingKey: "payment_failed", basicProperties: null, body: paymentFailedMessage);
            }
        };

        channel.BasicConsume(queue: "payment_queue", autoAck: true, consumer: consumer);
        Console.WriteLine("PaymentService: Listening for messages...");
        Console.ReadLine();
    }

    public static void SetPaymentProcessingSuccess(bool success)
    {
        IsPaymentProcessingSuccessful = success;
    }
}