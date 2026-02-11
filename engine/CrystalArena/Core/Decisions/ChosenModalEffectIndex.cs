namespace CrystalArena.Decisions
{
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class ChosenModalEffectIndex : DecisionResult
    {
        private readonly List<int> _indices = new List<int>();

        public ChosenModalEffectIndex(params int[] indices)
        {
            _indices.AddRange(indices);
        }

        public IList<int> Indices
        {
            get { return _indices; }
        }

        public override string TypeName => nameof(ChosenModalEffectIndex);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            json["indices"] = new JArray(_indices);
        }

        internal static new ChosenModalEffectIndex ReadJson(JObject json, SerializationContext ctx)
        {
            var indices = json["indices"]!.ToObject<List<int>>()!;
            return new ChosenModalEffectIndex(indices.ToArray());
        }
    }
}
