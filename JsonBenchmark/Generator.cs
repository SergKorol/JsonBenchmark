namespace JsonBenchmark;

public static class Generator
{
    public static List<Person> GeneratePersons(int count)
    {
        var persons = new List<Person>();
        for (int i = 0; i < count; i++)
        {
            persons.Add(GeneratePerson());
        }
        return persons;
    }

    private static Person GeneratePerson()
    {
        var random = new Random();
        var randomNumber = random.Next(1, 100);
        var person = new Person
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Age = randomNumber,
            Languages = ["English", "French", "Spanish"],
            YearsExperience = random.NextDouble(),
            OpenToWork = randomNumber % 2 == 0
        };
        return person;
    }
}