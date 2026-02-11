using System;
using System.Collections.Generic;
using System.Linq;
using CrystalArena.Decisions;
using Newtonsoft.Json.Linq;

namespace CrystalArena;

public class JsonFormatter
{
    public interface ISerializationInfo
    {
        SerializationContext Context { get; }
        object GetValue(string key, Type valueType);
        void AddValue(string key, object value);
        string? GetString(string key);
        int GetInt32(string key);
        int? GetNullableInt32(string key);
        bool GetBool(string key);
        
        // For nested serializable objects
        ISerializationInfo CreateNested();
        ISerializationInfo GetNested(string key);
        void AddNested(string key, Action<ISerializationInfo> serializer);
        
        // For lists of serializable objects
        List<ISerializationInfo> GetNestedList(string key);
        void AddNestedList(string key, Action<List<ISerializationInfo>> serializer);
    }
    
    public interface IStreamingContext
    {
        SerializationContext Context { get; }
    }
    
    private readonly SerializationContext _context;

    public JsonFormatter(SerializationContext context)
    {
        _context = context;
    }

    public string Serialize(DecisionResult result)
    {
        var info = new SerializationInfoImpl(_context);
        info.AddValue("$type", result.TypeName);
        result.Serialize(info);

        var jsonObject = ConvertToJObject(info);
        return jsonObject.ToString(Newtonsoft.Json.Formatting.None);
    }

    public DecisionResult Deserialize(string json)
    {
        var jsonObject = JObject.Parse(json);
        var info = ConvertFromJObject(jsonObject, _context);
        return DecisionResult.Deserialize(info);
    }

    public List<DecisionResult> DeserializeList(string json)
    {
        var jsonArray = JArray.Parse(json);
        var results = new List<DecisionResult>();

        foreach (var item in jsonArray)
        {
            var info = ConvertFromJObject((JObject)item, _context);
            results.Add(DecisionResult.Deserialize(info));
        }

        return results;
    }

    public string SerializeList(List<DecisionResult> results)
    {
        var jsonArray = new JArray();

        foreach (var result in results)
        {
            var info = new SerializationInfoImpl(_context);
            info.AddValue("$type", result.TypeName);
            result.Serialize(info);
            jsonArray.Add(ConvertToJObject(info));
        }

        return jsonArray.ToString(Newtonsoft.Json.Formatting.None);
    }

    private static JObject ConvertToJObject(SerializationInfoImpl info)
    {
        var obj = new JObject();

        foreach (var kvp in info.GetData())
        {
            if (kvp.Value is SerializationInfoImpl nested)
            {
                obj[kvp.Key] = ConvertToJObject(nested);
            }
            else if (kvp.Value is List<ISerializationInfo> nestedList)
            {
                var array = new JArray();
                foreach (var item in nestedList)
                {
                    array.Add(ConvertToJObject((SerializationInfoImpl)item));
                }
                obj[kvp.Key] = array;
            }
            else
            {
                obj[kvp.Key] = JToken.FromObject(kvp.Value);
            }
        }

        return obj;
    }

    private static SerializationInfoImpl ConvertFromJObject(JObject obj, SerializationContext context)
    {
        var info = new SerializationInfoImpl(context);

        foreach (var property in obj.Properties())
        {
            var value = property.Value;

            if (value.Type == JTokenType.Object)
            {
                info.AddValue(property.Name, ConvertFromJObject((JObject)value, context));
            }
            else if (value.Type == JTokenType.Array)
            {
                var array = (JArray)value;
                if (array.Count > 0 && array[0].Type == JTokenType.Object)
                {
                    // List of nested objects
                    var list = new List<ISerializationInfo>();
                    foreach (var item in array)
                    {
                        list.Add(ConvertFromJObject((JObject)item, context));
                    }
                    info.AddValue(property.Name, list);
                }
                else
                {
                    // Regular array - convert to List<object>
                    info.AddValue(property.Name, value.ToObject<List<object>>());
                }
            }
            else
            {
                // Primitive value
                info.AddValue(property.Name, value.ToObject<object>());
            }
        }

        return info;
    }
    
    private class SerializationInfoImpl : ISerializationInfo
    {
        private readonly Dictionary<string, object> _data = new();
        private readonly SerializationContext _context;

        public SerializationInfoImpl(SerializationContext context)
        {
            _context = context;
        }

        public Dictionary<string, object> GetData() => _data;

        public SerializationContext Context => _context;

        public object GetValue(string key, Type valueType)
        {
            var value = _data[key];

            // Handle type conversions for JSON deserialization
            if (valueType == typeof(List<int>) && value is List<object> objList)
            {
                return objList.ConvertAll(x => Convert.ToInt32(x));
            }
            if (valueType == typeof(List<List<int>>) && value is List<object> objListList)
            {
                return objListList.ConvertAll(x =>
                    ((List<object>)x).ConvertAll(y => Convert.ToInt32(y)));
            }
            if (valueType == typeof(int[]) && value is List<object> objArray)
            {
                return objArray.ConvertAll(x => Convert.ToInt32(x)).ToArray();
            }

            return value;
        }

        public void AddValue(string key, object value) => _data[key] = value;

        public string? GetString(string key) => _data.TryGetValue(key, out var value) ? value?.ToString() : null;

        public int GetInt32(string key) => Convert.ToInt32(_data[key]);

        public int? GetNullableInt32(string key)
        {
            if (!_data.TryGetValue(key, out var value) || value == null)
                return null;
            return Convert.ToInt32(value);
        }

        public bool GetBool(string key) => Convert.ToBoolean(_data[key]);

        public ISerializationInfo CreateNested() => new SerializationInfoImpl(_context);

        public ISerializationInfo GetNested(string key) => (ISerializationInfo)_data[key];

        public void AddNested(string key, Action<ISerializationInfo> serializer)
        {
            var nested = CreateNested();
            serializer(nested);
            _data[key] = nested;
        }

        public List<ISerializationInfo> GetNestedList(string key) => (List<ISerializationInfo>)_data[key];

        public void AddNestedList(string key, Action<List<ISerializationInfo>> serializer)
        {
            var list = new List<ISerializationInfo>();
            serializer(list);
            _data[key] = list;
        }

        public SerializationContext GetContext() => _context;
    }
}