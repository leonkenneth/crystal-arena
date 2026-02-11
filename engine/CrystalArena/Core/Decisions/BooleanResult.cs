namespace CrystalArena.Decisions
{
    using Newtonsoft.Json.Linq;

    public class BooleanResult : DecisionResult
    {
        public BooleanResult(bool value)
        {
            IsTrue = value;
        }

        public bool IsTrue { get; private set; }

        public override string TypeName => nameof(BooleanResult);

        public static implicit operator BooleanResult(bool value)
        {
            return new BooleanResult(value);
        }

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            json["IsTrue"] = IsTrue;
        }

        internal static new BooleanResult ReadJson(JObject json, SerializationContext ctx)
        {
            var value = json["IsTrue"]!.Value<bool>();
            return new BooleanResult(value);
        }
    }
}
