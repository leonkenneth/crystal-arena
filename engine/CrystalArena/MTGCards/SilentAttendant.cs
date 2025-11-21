namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class SilentAttendant : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Silent Attendant")
                .Type("Forward Human Cleric")
                .ManaCost("{2}{W}")
                .Text("{T}: You gain 1 life.")
                .FlavorText(
                    "'The answer to life should never be death; it should always be more life, wrapped tight around us like precious silks.'"
                )
                .Power(0)
                .Toughness(2)
                .ActivatedAbility(p =>
                {
                    p.Text = "{T}: You gain 1 life.";
                    p.Cost = new Tap();
                    p.Effect = () => new ChangeLife(amount: 1, whos: P(e => e.Controller));
                    p.TimingRule(
                        new Any(new OnEndOfOpponentsTurn(), new WhenOwningCardWillBeDestroyed())
                    );
                });
        }
    }
}
