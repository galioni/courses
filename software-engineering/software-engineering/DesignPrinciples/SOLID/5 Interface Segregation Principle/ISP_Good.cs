namespace DesignPrinciples.SOLID.ISP.Good;

//What it means: Don’t force a class to implement things it doesn’t need. It's like giving each toy only the buttons it needs to work.

// Good Example: Separate interfaces for different responsibilities.
public interface IWorkable
{
    void Work();
}

public interface IFeedable
{
    void Eat();
}

public class HumanWorker : IWorkable, IFeedable
{
    public void Work() => Console.WriteLine("Human working");

    public void Eat() => Console.WriteLine("Human eating");
}

public class RobotWorker : IWorkable
{
    public void Work() => Console.WriteLine("Robot working");
}