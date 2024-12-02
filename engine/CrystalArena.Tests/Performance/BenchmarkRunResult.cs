namespace CrystalArena.Tests.Performance
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Runtime.Serialization.Formatters;
  using System.Runtime.Serialization.Formatters.Binary;

  [Serializable]
  public class BenchmarkRunResult
  {
    public readonly string Description;
    public readonly string Name;
    public readonly DateTime RecordedAt = DateTime.Now;
    public readonly List<ScenarioRunResult> Runs;

    public BenchmarkRunResult(string name, string description, List<ScenarioRunResult> runs)
    {
      Description = description;
      Runs = runs;
      Name = name;
    }

    public TimeSpan Duration { get { return Runs.Aggregate(new TimeSpan(), (total, x) => total + x.Duration); } }

    public void WriteToFile(string filename)
    {
      var formatter = CreateFormatter();

      using (var stream = new FileStream(filename, FileMode.Create))
      {
        formatter.Serialize(stream, this);
      }
    }

    public static BenchmarkRunResult ReadFromFile(string filename)
    {
      var formatter = CreateFormatter();

      using (var stream = new FileStream(filename, FileMode.Open))
      {
        return (BenchmarkRunResult) formatter.Deserialize(stream);
      }
    }
    
#pragma warning disable SYSLIB0011
    private static BinaryFormatter CreateFormatter()
    {
      return new BinaryFormatter
      {
        AssemblyFormat = FormatterAssemblyStyle.Simple,
        TypeFormat = FormatterTypeStyle.TypesWhenNeeded
      };
    }
    
#pragma warning restore SYSLIB0011
  }
}