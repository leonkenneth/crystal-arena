namespace CrystalArena.Decisions
{
    using Newtonsoft.Json.Linq;

    public class Ordering : DecisionResult
    {
        public Ordering(params int[] indices)
        {
            Indices = indices;
        }

        public int[] Indices { get; private set; }

        public override string TypeName => nameof(Ordering);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            json["indices"] = new JArray(Indices);
        }

        internal static new Ordering ReadJson(JObject json, SerializationContext ctx)
        {
            var indices = json["indices"]!.ToObject<int[]>()!;
            return new Ordering(indices);
        }
    }
}
