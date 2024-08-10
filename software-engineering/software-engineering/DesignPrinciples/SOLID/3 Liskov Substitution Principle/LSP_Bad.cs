namespace DesignPrinciples.SOLID.LSP.Bad;

// Bad Example: Subclass doesn't behave like its parent.
public class Bird
{
    public virtual void Fly() => Console.WriteLine("Flying");
}

public class Penguin : Bird
{
    public override void Fly() => throw new NotSupportedException("Penguins can't fly!");
}