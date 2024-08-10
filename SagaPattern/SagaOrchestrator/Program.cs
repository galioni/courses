using Common.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace SagaOrchestrator;

public class Program
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Direct);
        channel.ExchangeDeclare(exchange: "payment_exchange", type: ExchangeType.Topic);
        channel.ExchangeDeclare(exchange: "inventory_exchange", type: ExchangeType.Topic);

        var consumer = new EventingBasicConsumer(channel);

        // Handle order failures and coordinate compensation actions
        void HandleOrderFailed(OrderFailed orderFailed)
        {
            Console.WriteLine($"SagaOrchestrator: Handling order failure for OrderID: {orderFailed.OrderId}, Reason: {orderFailed.Reason}");
            RefundPayment(orderFailed.OrderId);
            RestockInventory(orderFailed.OrderId);
        }

        // Handle payment failures and coordinate compensation actions
        void HandlePaymentFailed(PaymentFailed paymentFailed)
        {
            Console.WriteLine($"SagaOrchestrator: Handling payment failure for OrderID: {paymentFailed.OrderId}, Reason: {paymentFailed.Reason}");
            CancelOrder(paymentFailed.OrderId);
        }

        // Handle inventory failures and coordinate compensation actions
        void HandleInventoryFailed(InventoryFailed inventoryFailed)
        {
            Console.WriteLine($"SagaOrchestrator: Handling inventory failure for OrderID: {inventoryFailed.OrderId}, Reason: {inventoryFailed.Reason}");
            RefundPayment(inventoryFailed.OrderId);
        }

        void RefundPayment(string orderId)
        {
            Console.WriteLine($"SagaOrchestrator: Refunding payment for OrderID: {orderId}");
            // Logic to refund the payment
        }

        void RestockInventory(string orderId)
        {
            Console.WriteLine($"SagaOrchestrator: Restocking inventory for OrderID: {orderId}");
            // Logic to restock the inventory
        }

        void CancelOrder(string orderId)
        {
            Console.WriteLine($"SagaOrchestrator: Cancelling order for OrderID: {orderId}");
            // Logic to cancel the order
        }

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            if (ea.RoutingKey == "order_failed")
            {
                var orderFailed = JsonConvert.DeserializeObject<OrderFailed>(message);
                HandleOrderFailed(orderFailed);
            }
            else if (ea.RoutingKey == "payment_failed")
            {
                var paymentFailed = JsonConvert.DeserializeObject<PaymentFailed>(message);
                HandlePaymentFailed(paymentFailed);
            }
            else if (ea.RoutingKey == "inventory_failed")
            {
                var inventoryFailed = JsonConvert.DeserializeObject<InventoryFailed>(message);
                HandleInventoryFailed(inventoryFailed);
            }
        };

        channel.QueueDeclare(queue: "compensation_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: "compensation_queue", exchange: "order_exchange", routingKey: "order_failed");
        channel.QueueBind(queue: "compensation_queue", exchange: "payment_exchange", routingKey: "payment_failed");
        channel.QueueBind(queue: "compensation_queue", exchange: "inventory_exchange", routingKey: "inventory_failed");

        channel.BasicConsume(queue: "compensation_queue", autoAck: true, consumer: consumer);

        for(var i = 0; i < 10; i++)
            CreateOrder(channel);

        Console.WriteLine("SagaOrchestrator: Waiting for services to process...");
        Console.ReadLine();
    }

    private static void CreateOrder(IModel? channel)
    {
        var rnd = new Random();

        // Create orders
        var orderCreated = new OrderCreated { OrderId = Guid.NewGuid().ToString(), Amount = rnd.NextDouble() };
        var orderMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderCreated));
        channel.BasicPublish(exchange: "order_exchange", routingKey: "order_created", basicProperties: null, body: orderMessage);

        Console.WriteLine($"SagaOrchestrator: Order created - {orderCreated.OrderId}");
    }
}