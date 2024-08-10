namespace DesignPrinciples.SOLID.DIP.Bad;

//What it means: Depend on abstractions, not concrete classes. This is like playing with different toys that fit into the same toy box.

// Bad Example: High-level class depends on a low-level class.
public class LightBulb
{
    public void TurnOn()
    {
        Console.WriteLine("LightBulb turned on");
    }
}

public class Switch
{
    private LightBulb _lightBulb;

    public Switch()
    {
        _lightBulb = new LightBulb();
    }

    public void Operate()
    {
        _lightBulb.TurnOn();
    }
}