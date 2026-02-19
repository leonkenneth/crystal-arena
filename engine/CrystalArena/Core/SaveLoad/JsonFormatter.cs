using System.Collections.Generic;
using System.Linq;
using CrystalArena.Decisions;
using Newtonsoft.Json.Linq;

namespace CrystalArena;

public class JsonFormatter
{
    private readonly SerializationContext _context;

    public JsonFormatter(SerializationContext context)
    {
        _context = context;
    }

    public JObject Serialize(DecisionResult result)
    {
        var json = new JObject();
        json["$type"] = result.TypeName;
        result.WriteJson(json, _context);
        return json;
    }

    public DecisionResult Deserialize(JObject jsonObject)
    {
        return DecisionResult.ReadJson(jsonObject, _context);
    }

    public string SerializeList(List<DecisionResult> results)
    {
        var array = new JArray();
        foreach (var result in results)
        {
            var json = new JObject();
            json["$type"] = result.TypeName;
            result.WriteJson(json, _context);
            array.Add(json);
        }
        return array.ToString(Newtonsoft.Json.Formatting.None);
    }

    public List<DecisionResult> DeserializeList(string json)
    {
        var array = JArray.Parse(json);
        return array.Select(item => DecisionResult.ReadJson((JObject)item, _context)).ToList();
    }
}
