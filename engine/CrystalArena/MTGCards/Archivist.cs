namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class Archivist : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Archivist")
                .ManaCost("{2}{U}{U}")
                .Type("Forward Human Wizard")
                .Text("{T}: Draw a card.")
                .FlavorText("Some do. Some teach. The rest look it up.")
                .Power(1)
                .Toughness(1)
                .ActivatedAbility(p =>
                {
                    p.Text = "{T}: Draw a card.";
                    p.Cost = new Tap();
                    p.Effect = () => new DrawCards(1);

                    p.TimingRule(new OnFirstMain());
                    p.TimingRule(new WhenStackIsEmpty());
                });
        }
    }
}
