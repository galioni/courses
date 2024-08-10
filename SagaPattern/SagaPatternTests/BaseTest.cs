using Moq;
using RabbitMQ.Client;

namespace SagaPatternTests;

public class BaseTest
{
    protected Mock<IModel> mockChannel;
    protected Mock<IConnection> mockConnection;
    protected Mock<IConnectionFactory> mockFactory;

    public BaseTest()
    {
        mockChannel = new Mock<IModel>();
        mockConnection = new Mock<IConnection>();
        mockFactory = new Mock<IConnectionFactory>();

        mockFactory.Setup(f => f.CreateConnection()).Returns(mockConnection.Object);
        mockConnection.Setup(c => c.CreateModel()).Returns(mockChannel.Object);
    }
}