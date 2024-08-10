namespace DesignPrinciples.SOLID.DIP.Good;

//What it means: Depend on abstractions, not concrete classes. This is like playing with different toys that fit into the same toy box.

// Good Example: High-level class depends on an abstraction.
public interface ISwitchable
{
    void TurnOn();
}

public class LightBulb : ISwitchable
{
    public void TurnOn()
    {
        Console.WriteLine("LightBulb turned on");
    }
}

public class Switch
{
    private ISwitchable _switchable;

    public Switch(ISwitchable switchable)
    {
        _switchable = switchable;
    }

    public void Operate()
    {
        _switchable.TurnOn();
    }
}