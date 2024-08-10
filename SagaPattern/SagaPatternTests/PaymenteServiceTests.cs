using Common.Messages;
using Moq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace SagaPatternTests;

public class PaymentServiceTests : BaseTest
{
    [Fact]
    public void PaymentService_Should_ProcessPaymentSuccessfully()
    {
        // Arrange
        var paymentProcessed = new PaymentProcessed { OrderId = "1", Success = true };
        var paymentMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentProcessed));

        mockChannel.Setup(c => c.BasicPublish(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));

        var consumer = new EventingBasicConsumer(mockChannel.Object);
        consumer.Received += null;

        mockChannel.SetupAdd(m => m.BasicConsume(It.IsAny<string>(), true, "", false, false, null, It.IsAny<IBasicConsumer>()))
            .Callback<string, bool, string, bool, bool, IDictionary<string, object>, IBasicConsumer>((queue, autoAck, consumerTag, noLocal, exclusive, arguments, bc) =>
            {
                (bc as EventingBasicConsumer).HandleBasicDeliver("consumer_tag", 1, false, "exchange", "routing_key", null, paymentMessage);
            });

        // Act
        PaymentService.Program.Main(null);

        // Assert
        mockChannel.Verify(c => c.BasicPublish(
            "inventory_exchange", "inventory_updated", It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()), Times.Once);
    }

    [Fact]
    public void PaymentService_Should_HandlePaymentFailure()
    {
        // Arrange
        var paymentProcessed = new PaymentProcessed { OrderId = "1", Success = false };
        var paymentMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentProcessed));

        mockChannel.Setup(c => c.BasicPublish(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));

        var consumer = new EventingBasicConsumer(mockChannel.Object);
        consumer.Received += null;

        mockChannel.SetupAdd(m => m.BasicConsume(It.IsAny<string>(), true, "", false, false, null, It.IsAny<IBasicConsumer>()))
            .Callback<string, bool, string, bool, bool, IDictionary<string, object>, IBasicConsumer>((queue, autoAck, consumerTag, noLocal, exclusive, arguments, bc) =>
            {
                (bc as EventingBasicConsumer).HandleBasicDeliver("consumer_tag", 1, false, "exchange", "routing_key", null, paymentMessage);
            });

        // Act
        PaymentService.Program.Main(null);

        // Assert
        mockChannel.Verify(c => c.BasicPublish(
            "payment_exchange", "payment_failed", It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()), Times.Once);
    }
}
