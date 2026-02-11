namespace CrystalArena.Decisions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class ChosenCards : DecisionResult, IEnumerable<Card>
    {
        private readonly List<Card> _cards = new List<Card>();

        public ChosenCards() { }

        public ChosenCards(Card card)
        {
            _cards.Add(card);
        }

        public ChosenCards(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
        }

        public static ChosenCards None
        {
            get { return new ChosenCards(); }
        }
        public int Count
        {
            get { return _cards.Count; }
        }

        public Card this[int index]
        {
            get { return _cards[index]; }
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string TypeName => nameof(ChosenCards);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            json["cards"] = new JArray(_cards.Select(x => x.Id));
        }

        internal static new ChosenCards ReadJson(JObject json, SerializationContext ctx)
        {
            var cardIds = json["cards"]!.ToObject<List<int>>()!;
            var cards = cardIds.Select(id => (Card)ctx.Recorder.GetObject(id));
            return new ChosenCards(cards);
        }

        public void Add(Card card)
        {
            _cards.Add(card);
        }

        public static implicit operator ChosenCards(List<ITarget> cards)
        {
            return new ChosenCards(cards.Select(x => x.Card()));
        }

        public static implicit operator ChosenCards(List<Card> cards)
        {
            return new ChosenCards(cards);
        }
    }
}
