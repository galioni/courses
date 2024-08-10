using Common.Messages;
using Moq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace SagaPatternTests;

public class OrderServiceTests : BaseTest
{
    [Fact]
    public void OrderService_Should_ProcessOrderSuccessfully()
    {
        // Arrange
        var orderCreated = new OrderCreated { OrderId = "1", Amount = 100.00M };
        var orderMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderCreated));

        mockChannel.Setup(c => c.BasicPublish(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));

        var consumer = new EventingBasicConsumer(mockChannel.Object);
        consumer.Received += null;

        mockChannel.SetupAdd(m => m.BasicConsume(It.IsAny<string>(), true, "", false, false, null, It.IsAny<IBasicConsumer>()))
            .Callback<string, bool, string, bool, bool, IDictionary<string, object>, IBasicConsumer>((queue, autoAck, consumerTag, noLocal, exclusive, arguments, bc) =>
            {
                (bc as EventingBasicConsumer).HandleBasicDeliver("consumer_tag", 1, false, "exchange", "routing_key", null, orderMessage);
            });

        // Act
        OrderService.Program.Main(null);

        // Assert
        mockChannel.Verify(c => c.BasicPublish(
            "payment_exchange", "payment_processed", It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()), Times.Once);
    }

    [Fact]
    public void OrderService_Should_HandleOrderFailure()
    {
        // Arrange
        var orderCreated = new OrderCreated { OrderId = "1", Amount = 100.00M };
        var orderMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(orderCreated));

        mockChannel.Setup(c => c.BasicPublish(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));

        var consumer = new EventingBasicConsumer(mockChannel.Object);
        consumer.Received += null;

        mockChannel.SetupAdd(m => m.BasicConsume(It.IsAny<string>(), true, "", false, false, null, It.IsAny<IBasicConsumer>()))
            .Callback<string, bool, string, bool, bool, IDictionary<string, object>, IBasicConsumer>((queue, autoAck, consumerTag, noLocal, exclusive, arguments, bc) =>
            {
                (bc as EventingBasicConsumer).HandleBasicDeliver("consumer_tag", 1, false, "exchange", "routing_key", null, orderMessage);
            });

        // Set the OrderService to simulate a failure
        OrderService.Program.SetOrderProcessingSuccess(false);

        // Act
        OrderService.Program.Main(null);

        // Assert
        mockChannel.Verify(c => c.BasicPublish(
            "order_exchange", "order_failed", It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()), Times.Once);
    }
}
