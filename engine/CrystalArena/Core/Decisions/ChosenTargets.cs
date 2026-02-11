namespace CrystalArena.Decisions
{
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class ChosenTargets : DecisionResult
    {
        public ChosenTargets(Targets targets)
        {
            Targets = targets;
        }

        public Targets Targets { get; private set; }

        public bool HasTargets
        {
            get { return Targets != null && Targets.Count > 0; }
        }

        public static ChosenTargets None()
        {
            return new ChosenTargets(new Targets());
        }

        public override string TypeName => nameof(ChosenTargets);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            if (Targets != null)
            {
                var targetsJson = new JObject();
                Targets.WriteJson(targetsJson, ctx);
                json["targets"] = targetsJson;
            }
        }

        internal static new ChosenTargets ReadJson(JObject json, SerializationContext ctx)
        {
            var targetsToken = json["targets"];
            if (targetsToken != null && targetsToken.Type != JTokenType.Null)
            {
                return new ChosenTargets(Targets.ReadJson((JObject)targetsToken, ctx));
            }
            return new ChosenTargets(new Targets());
        }
    }
}
