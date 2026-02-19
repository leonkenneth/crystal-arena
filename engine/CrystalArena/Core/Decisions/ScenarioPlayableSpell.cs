namespace CrystalArena.Decisions
{
    using CrystalArena.Infrastructure;
    using Newtonsoft.Json.Linq;

    public class ScenarioPlayableSpell : PlayableSpell
    {
        public ScenarioPlayableSpell() { }

        public override void Play()
        {
            Asrt.True(Card != null, "Did you forget to add card to players hand?");

            // so we don't have to have it available
            // to shorten scenario definition
            AddCastingCostToManaPool();

            base.Play();
        }

        private void AddCastingCostToManaPool()
        {
            var manaCost = Card.GetSpellManaCost(Index);
            if (ActivationParameters.X.HasValue)
                manaCost = manaCost.Add(ActivationParameters.X.Value.Colorless());

            Controller.AddManaToManaPool(manaCost);
        }

        public new void WriteJson(JObject json, SerializationContext ctx)
        {
            json["card"] = Card.Id;
            json["index"] = Index;
            var paramsJson = new JObject();
            ActivationParameters.WriteJson(paramsJson, ctx);
            json["parameters"] = paramsJson;
        }

        public static new ScenarioPlayableSpell ReadJson(JObject json, SerializationContext ctx)
        {
            var spell = new ScenarioPlayableSpell();
            spell.Card = (Card)ctx.Recorder.GetObject(json["card"]!.Value<int>());
            spell.Index = json["index"]!.Value<int>();
            var paramsJson = (JObject)json["parameters"]!;
            spell.ActivationParameters = ActivationParameters.ReadJson(paramsJson, ctx);
            return spell;
        }
    }
}
