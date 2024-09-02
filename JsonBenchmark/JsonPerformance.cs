using System.Text;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace JsonBenchmark;

public class JsonPerformance
{
    private List<Person> Persons { get; } = Generator.GeneratePersons(10000);
    private string? Json { get; set; }
    
    [Benchmark]
    public void NewtonJsonWrite()
    {
        Json = JsonConvert.SerializeObject(Persons);
    }
    
    [Benchmark]
    public void FrameworkJsonWrite()
    {
        Json = JsonSerializer.Serialize(Persons);
    }
    
    [Benchmark]
    public void ManualJsonWrite()
    {
        var builder = new StringBuilder();
        var writer = new StringWriter(builder);
        using (var textWriter = new JsonTextWriter(writer))
        {
            textWriter.WriteStartArray();
            foreach (var person in Persons)
            {
                textWriter.WriteStartObject();
    
                textWriter.WritePropertyName("FirstName");
                textWriter.WriteValue(person.FirstName);
    
                textWriter.WritePropertyName("LastName");
                textWriter.WriteValue(person.LastName);
    
                textWriter.WritePropertyName("Age");
                textWriter.WriteValue(person.Age);
    
                textWriter.WritePropertyName("YearsExperience");
                textWriter.WriteValue(person.YearsExperience);
    
                textWriter.WritePropertyName("Languages");
                textWriter.WriteStartArray();
                foreach (var language in person.Languages)
                {
                    textWriter.WriteValue(language);
                }
                textWriter.WriteEndArray();
    
                textWriter.WritePropertyName("OpenToWork");
                textWriter.WriteValue(person.OpenToWork);
    
                textWriter.WriteEndObject();
            }
            textWriter.WriteEndArray();
        }

        Json = builder.ToString();
    }
    
    [Benchmark]
    public void NewtonJsonRead()
    {
        if (Json == null) return;
        var persons = JsonConvert.DeserializeObject<List<Person>>(Json);
    }
    
    [Benchmark]
    public void FrameworkJsonRead()
    {
        if (Json == null) return;
        var persons = JsonSerializer.Deserialize<List<Person>>(Json);
    }

    [Benchmark]
    public void ManualJsonRead()
    {
        if (Json == null) return;
        var persons = new List<Person>();
        using var reader = new StringReader(Json);
        using var jsonReader = new JsonTextReader(reader);
        jsonReader.Read();
        while (jsonReader.Read())
        {
            if (jsonReader.TokenType == JsonToken.StartObject)
            {
                var person = new Person();
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName)
                    {
                        var propertyName = jsonReader.Value?.ToString();
                        jsonReader.Read();
                        switch (propertyName)
                        {
                            case "FirstName":
                                person.FirstName = jsonReader.Value?.ToString() ?? string.Empty;
                                break;
                            case "LastName":
                                person.LastName = jsonReader.Value?.ToString() ?? string.Empty;
                                break;
                            case "Age":
                                person.Age = Convert.ToInt32(jsonReader.Value);
                                break;
                            case "YearsExperience":
                                person.YearsExperience = Convert.ToInt32(jsonReader.Value);
                                break;
                            case "Languages":
                                person.Languages = jsonReader.Value?.ToString()?.Split(',').ToList() ?? new List<string>();
                                break;
                            case "OpenToWork":
                                person.OpenToWork = Convert.ToBoolean(jsonReader.Value);
                                break;
                        }
                    }
                    else if (jsonReader.TokenType == JsonToken.EndObject)
                    {
                        break;
                    }
                }
                persons.Add(person);
            }
            else if (jsonReader.TokenType == JsonToken.EndArray)
            {
                break;
            }
        }
    }
}

