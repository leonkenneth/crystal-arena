namespace CrystalArena.Decisions
{
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class ChosenOptions : DecisionResult
    {
        private readonly List<object> _options = new List<object>();

        public ChosenOptions(params object[] options)
        {
            _options.AddRange(options);
        }

        public IList<object> Options
        {
            get { return _options; }
        }

        public override string TypeName => nameof(ChosenOptions);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            json["options"] = JArray.FromObject(_options);
        }

        internal static new ChosenOptions ReadJson(JObject json, SerializationContext ctx)
        {
            var options = json["options"]!.ToObject<List<object>>()!;
            return new ChosenOptions(options.ToArray());
        }
    }
}
