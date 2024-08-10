namespace DesignPrinciples.SOLID.SRP.Bad;

// Bad Example: This class does too much!
public class Report
{
    public string Title { get; set; }

    // Generates the report data
    public string GenerateReport() => "Report data";

    // Prints the report
    public void PrintReport(string data) => Console.WriteLine(data);
}