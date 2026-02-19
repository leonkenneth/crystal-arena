namespace CrystalArena
{
    using System.Linq;
    using System.Security.Policy;
    using CrystalArena.Infrastructure;
    using Newtonsoft.Json.Linq;

    public class MainDeck : OrderedZone, IMainDeckQuery
    {
        public MainDeck(Player owner)
            : base(owner) { }

        private MainDeck()
        {
            /* for state copy */
        }

        public override Zone Name
        {
            get { return Zone.MainDeck; }
        }
        public Card Top
        {
            get { return this.FirstOrDefault(); }
        }
        public Card Bottom
        {
            get { return this.LastOrDefault(); }
        }

        public override int CalculateHash(HashCalculator calc)
        {
            var visible = this.Where(x => x.IsVisibleToPlayer(Owner)).ToList();

            if (visible.Count == 0)
                return Count;

            return HashCalculator.Combine(Count, calc.Calculate(visible, true));
        }

        public override JObject DebugCalculateHash(HashCalculator calc)
        {
            var json = new JObject();
            json["hash"] = CalculateHash(calc);
            json["zone"] = Name.ToString();
            json["count"] = Count;
            var cards = new JArray();
            foreach (var card in this)
            {
                var c = new JObject();
                c["name"] = card.Name;
                c["visible"] = card.IsVisibleToPlayer(Owner);
                c["hash"] = calc.Calculate(card);
                cards.Add(c);
            }
            json["cards"] = cards;
            return json;
        }

        public void PutOnTop(Card card)
        {
            if (card.Zone == Name)
            {
                MoveToFront(card);
            }
            else
            {
                AddToFront(card);
            }
        }

        public void PutOnBottom(Card card)
        {
            if (card.Zone == Name)
            {
                MoveToEnd(card);
            }
            else
            {
                AddToEnd(card);
            }
        }
    }
}
