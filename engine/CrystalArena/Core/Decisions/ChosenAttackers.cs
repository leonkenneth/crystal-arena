namespace CrystalArena.Decisions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Newtonsoft.Json.Linq;

    [Copyable]
    public class ChosenAttackers : DecisionResult, IEnumerable<ChosenAttackers.Attacker>
    {
        private readonly List<Attacker> _attackers = new List<Attacker>();

        public List<Attacker> Attackers
        {
            get { return _attackers; }
        }

        public ChosenAttackers() { }

        public ChosenAttackers(IEnumerable<Card> attackers, Card assignedPlaneswalker = null)
        {
            _attackers.AddRange(attackers.Select(c => new Attacker(c, assignedPlaneswalker)));
        }

        public int Count
        {
            get { return _attackers.Count; }
        }

        public IEnumerator<Attacker> GetEnumerator()
        {
            return _attackers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Card attacker, Card planeswalker)
        {
            _attackers.Add(new Attacker(attacker, planeswalker));
        }

        public void Remove(Card attacker)
        {
            var found = _attackers.FirstOrDefault(x => x.Card == attacker);
            if (found != null)
            {
                _attackers.Remove(found);
            }
        }

        public override string TypeName => nameof(ChosenAttackers);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            var array = new JArray();
            foreach (var attacker in _attackers)
            {
                var attackerJson = new JObject();
                attacker.WriteJson(attackerJson);
                array.Add(attackerJson);
            }
            json["attackers"] = array;
        }

        internal static new ChosenAttackers ReadJson(JObject json, SerializationContext ctx)
        {
            var result = new ChosenAttackers();
            var attackersList = (JArray)json["attackers"]!;
            foreach (var item in attackersList)
            {
                result._attackers.Add(Attacker.ReadJson((JObject)item, ctx));
            }
            return result;
        }

        [Copyable]
        public class Attacker
        {
            public readonly Card Card;
            public readonly Card Planeswalker;

            private Attacker() { }

            public Attacker(Card card, Card planeswalker)
            {
                Card = card;
                Planeswalker = planeswalker;
            }

            public void WriteJson(JObject json)
            {
                json["card"] = Card.Id;
                json["planeswalker"] = Planeswalker?.Id;
            }

            public static Attacker ReadJson(JObject json, SerializationContext ctx)
            {
                var cardId = json["card"]!.Value<int>();
                var planeswalkerId =
                    json["planeswalker"]?.Type != JTokenType.Null
                        ? json["planeswalker"]?.Value<int>()
                        : null;

                var card = (Card)ctx.Recorder.GetObject(cardId);
                var planeswalker = planeswalkerId.HasValue
                    ? (Card)ctx.Recorder.GetObject(planeswalkerId.Value)
                    : null;
                return new Attacker(card, planeswalker);
            }
        }
    }
}
