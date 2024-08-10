using Common.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OrderService;

public class Program
{
    public static bool IsOrderProcessingSuccessful = true;

    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Direct);
        channel.QueueDeclare(queue: "order_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: "order_queue", exchange: "order_exchange", routingKey: "order_created");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var orderCreated = JsonConvert.DeserializeObject<OrderCreated>(message);

            Console.WriteLine($"OrderService: Order received - {orderCreated.OrderId}");

            if (IsOrderProcessingSuccessful)
            {
                var paymentProcessed = new PaymentProcessed { OrderId = orderCreated.OrderId, Success = true };
                var paymentMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentProcessed));
                channel.BasicPublish(exchange: "payment_exchange", routingKey: "payment_processed", basicProperties: null, body: paymentMessage);
            }
            else
            {
                var orderFailed = new OrderFailed { OrderId = orderCreated.OrderId, Reason = "Order processing failed" };
                var orderFailedMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderFailed));
                channel.BasicPublish(exchange: "order_exchange", routingKey: "order_failed", basicProperties: null, body: orderFailedMessage);
            }
        };

        channel.BasicConsume(queue: "order_queue", autoAck: true, consumer: consumer);
        Console.WriteLine("OrderService: Listening for messages...");
        Console.ReadLine();
    }

    public static void SetOrderProcessingSuccess(bool success)
    {
        IsOrderProcessingSuccessful = success;
    }
}