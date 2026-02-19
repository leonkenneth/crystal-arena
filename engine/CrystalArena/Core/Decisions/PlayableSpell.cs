namespace CrystalArena.Decisions
{
    using Newtonsoft.Json.Linq;

    public class PlayableSpell : Playable
    {
        public PlayableSpell() { }

        public override void Play()
        {
            Card.Cast(Index, ActivationParameters);
        }

        public override string ToString()
        {
            return string.Format("spell {0} of {1}", Index, Card);
        }

        public void WriteJson(JObject json, SerializationContext ctx)
        {
            json["card"] = Card.Id;
            json["index"] = Index;
            var paramsJson = new JObject();
            ActivationParameters.WriteJson(paramsJson, ctx);
            json["parameters"] = paramsJson;
        }

        public static PlayableSpell ReadJson(JObject json, SerializationContext ctx)
        {
            var spell = new PlayableSpell();
            spell.Card = (Card)ctx.Recorder.GetObject(json["card"]!.Value<int>());
            spell.Index = json["index"]!.Value<int>();
            var paramsJson = (JObject)json["parameters"]!;
            spell.ActivationParameters = ActivationParameters.ReadJson(paramsJson, ctx);
            return spell;
        }
    }
}
