namespace DesignPrinciples.SOLID.OCP.Good;

//What it means: Classes should be open for extension but closed for modification. Imagine you want to add a new toy to your collection without changing your old ones.

// Good Example: Extend the class without changing existing code.
public abstract class Shape
{
    public abstract double CalculateArea();
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public override double CalculateArea() => Width * Height;
}

public class Circle : Shape
{
    public double Radius { get; set; }

    public override double CalculateArea() => Math.PI * Radius * Radius;
}

public class AreaCalculator
{
    public double CalculateArea(Shape shape) => shape.CalculateArea();
}