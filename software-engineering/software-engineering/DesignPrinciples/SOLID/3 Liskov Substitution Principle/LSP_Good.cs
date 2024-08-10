namespace DesignPrinciples.SOLID.LSP.Good;

//What it means: Subclasses should be able to replace their parent classes without breaking your code.Like how any toy car can be replaced with another toy car, and it still works the same way.

// Good Example: Using a base class that fits all subclasses.
public abstract class Bird
{
    public abstract void Move();
}

public class Sparrow : Bird
{
    public override void Move() => Console.WriteLine("Flying");
}

public class Penguin : Bird
{
    public override void Move() => Console.WriteLine("Swimming");
}