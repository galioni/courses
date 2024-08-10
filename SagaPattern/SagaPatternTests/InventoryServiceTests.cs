using Common.Messages;
using Moq;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace SagaPatternTests
{
    public class InventoryServiceTests : BaseTest
    {
        [Fact]
        public void InventoryService_Should_UpdateInventorySuccessfully()
        {
            // Arrange
            var inventoryUpdated = new InventoryUpdated { OrderId = "1", Success = true };
            var inventoryMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(inventoryUpdated));

            mockChannel.Setup(c => c.BasicPublish(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));

            var consumer = new EventingBasicConsumer(mockChannel.Object);
            consumer.Received += null;

            mockChannel.SetupAdd(m => m.BasicConsume(It.IsAny<string>(), true, "", false, false, null, It.IsAny<IBasicConsumer>()))
                .Callback<string, bool, string, bool, bool, IDictionary<string, object>, IBasicConsumer>((queue, autoAck, consumerTag, noLocal, exclusive, arguments, bc) =>
                {
                    (bc as EventingBasicConsumer).HandleBasicDeliver("consumer_tag", 1, false, "exchange", "routing_key", null, inventoryMessage);
                });

            // Act
            InventoryService.Program.Main(null);

            // Assert
            mockChannel.Verify(c => c.BasicPublish(
                "inventory_exchange", "inventory_failed", It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()), Times.Never);
        }

        [Fact]
        public void InventoryService_Should_HandleInventoryFailure()
        {
            // Arrange
            var inventoryUpdated = new InventoryUpdated { OrderId = "1", Success = false };
            var inventoryMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(inventoryUpdated));

            mockChannel.Setup(c => c.BasicPublish(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));

            var consumer = new EventingBasicConsumer(mockChannel.Object);
            consumer.Received += null;

            mockChannel.SetupAdd(m => m.BasicConsume(It.IsAny<string>(), true, "", false, false, null, It.IsAny<IBasicConsumer>()))
                .Callback<string, bool, string, bool, bool, IDictionary<string, object>, IBasicConsumer>((queue, autoAck, consumerTag, noLocal, exclusive, arguments, bc) =>
                {
                    (bc as EventingBasicConsumer).HandleBasicDeliver("consumer_tag", 1, false, "exchange", "routing_key", null, inventoryMessage);
                });

            // Act
            InventoryService.Program.Main(null);

            // Assert
            mockChannel.Verify(c => c.BasicPublish(
                "inventory_exchange", "inventory_failed", It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()), Times.Once);
        }
    }
}