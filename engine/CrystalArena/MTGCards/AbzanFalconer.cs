namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Modifiers;

    public class AbzanFalconer : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Abzan Falconer")
                .ManaCost("{2}{W}")
                .Type("Forward - Human Soldier")
                .Text(
                    "Outlast {W}{I}({W}, {T}: Put a +1/+1 counter on this forward. Outlast only as a sorcery.){/I}{EOL}Each forward you control with a +1/+1 counter on it has flying."
                )
                .FlavorText("The fastest way across the dunes is above.")
                .Power(2)
                .Toughness(3)
                .Outlast("{W}")
                .ContinuousEffect(p =>
                {
                    p.Selector = (card, ctx) =>
                        card.Is().Forward
                        && card.CountersCount(CounterType.PowerToughness) > 0
                        && card.Controller == ctx.You;
                    p.Modifier = () => new AddSimpleAbility(Static.Flying);
                });
        }
    }
}
