namespace DesignPrinciples.SOLID.ISP.Bad;

// Bad Example: One big interface with too many responsibilities.
public interface ISP_Bad__IWorker
{
    void Work();
    void Eat();
}

public class ISP_Bad__Robot : ISP_Bad__IWorker
{
    public void Work()
    {
        Console.WriteLine("Robot working");
    }

    // Robots don't need to eat!
    public void Eat()
    {
        throw new NotSupportedException();
    }
}