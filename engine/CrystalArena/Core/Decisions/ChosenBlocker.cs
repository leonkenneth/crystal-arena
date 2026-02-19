namespace CrystalArena.Decisions
{
    using Infrastructure;
    using Newtonsoft.Json.Linq;

    [Copyable]
    public class ChosenBlocker : DecisionResult
    {
        public static readonly ChosenBlocker None = new();
        public Card? Blocker;

        private ChosenBlocker() { }

        public ChosenBlocker(Card? card = null)
        {
            Blocker = card;
        }

        public override string TypeName => nameof(ChosenBlocker);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            json["card"] = Blocker?.Id;
        }

        internal static new ChosenBlocker ReadJson(JObject json, SerializationContext ctx)
        {
            var cardToken = json["card"];
            if (cardToken != null && cardToken.Type != JTokenType.Null)
            {
                var cardId = cardToken.Value<int>();
                var card = (Card)ctx.Recorder.GetObject(cardId);
                return new ChosenBlocker(card);
            }

            return new ChosenBlocker();
        }
    }
}
