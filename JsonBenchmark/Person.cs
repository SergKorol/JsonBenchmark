namespace JsonBenchmark;

public record Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public double YearsExperience { get; set; }
    public List<string> Languages { get; set; }
    public bool OpenToWork { get; set; }
}