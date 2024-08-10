namespace DesignPrinciples.SOLID.SRP.Good;

//What it means: Each class should have one job, and only one job. Just like a toy car is made for driving, not for flying.

// Good Example: Separate the responsibilities.
public class ReportGenerator
{
    public string GenerateReport() => "Report data";
}

public class ReportPrinter
{
    public void PrintReport(string data) => Console.WriteLine(data);
}