namespace DesignPrinciples.SOLID.OCP.Bad;

// Bad Example: Modifying the class to add new features.
public class AreaCalculator
{
    public double CalculateRectangleArea(double width, double height) => width * height;

    public double CalculateCircleArea(double radius) => Math.PI * radius * radius;
}