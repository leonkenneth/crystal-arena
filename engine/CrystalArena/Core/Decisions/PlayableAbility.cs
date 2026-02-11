namespace CrystalArena.Decisions
{
    using Newtonsoft.Json.Linq;

    public class PlayableAbility : Playable
    {
        public PlayableAbility() { }

        public override void Play()
        {
            Card.ActivateAbility(Index, ActivationParameters);
        }

        public override string ToString()
        {
            return string.Format("ability {0} of {1}", Index, Card);
        }

        public void WriteJson(JObject json, SerializationContext ctx)
        {
            json["card"] = Card.Id;
            json["index"] = Index;
            var paramsJson = new JObject();
            ActivationParameters.WriteJson(paramsJson, ctx);
            json["parameters"] = paramsJson;
        }

        public static PlayableAbility ReadJson(JObject json, SerializationContext ctx)
        {
            var ability = new PlayableAbility();
            ability.Card = (Card)ctx.Recorder.GetObject(json["card"]!.Value<int>());
            ability.Index = json["index"]!.Value<int>();
            var paramsJson = (JObject)json["parameters"]!;
            ability.ActivationParameters = ActivationParameters.ReadJson(paramsJson, ctx);
            return ability;
        }
    }
}
