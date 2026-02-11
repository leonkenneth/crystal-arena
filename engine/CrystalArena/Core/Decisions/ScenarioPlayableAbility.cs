namespace CrystalArena.Decisions
{
    using Newtonsoft.Json.Linq;

    public class ScenarioPlayableAbility : PlayableAbility
    {
        public ScenarioPlayableAbility() { }

        public override void Play()
        {
            // so we don't have to have it available
            // to shorten scenario definition
            AddActivationCostToManaPool();

            base.Play();
        }

        private void AddActivationCostToManaPool()
        {
            var manaCost = Card.GetActivatedAbilityManaCost(Index);

            if (ActivationParameters.X.HasValue)
            {
                manaCost = manaCost.Add(ActivationParameters.X.Value.Colorless());
            }

            Card.Controller.AddManaToManaPool(manaCost);
        }

        public new void WriteJson(JObject json, SerializationContext ctx)
        {
            json["card"] = Card.Id;
            json["index"] = Index;
            var paramsJson = new JObject();
            ActivationParameters.WriteJson(paramsJson, ctx);
            json["parameters"] = paramsJson;
        }

        public static new ScenarioPlayableAbility ReadJson(JObject json, SerializationContext ctx)
        {
            var ability = new ScenarioPlayableAbility();
            ability.Card = (Card)ctx.Recorder.GetObject(json["card"]!.Value<int>());
            ability.Index = json["index"]!.Value<int>();
            var paramsJson = (JObject)json["parameters"]!;
            ability.ActivationParameters = ActivationParameters.ReadJson(paramsJson, ctx);
            return ability;
        }
    }
}
