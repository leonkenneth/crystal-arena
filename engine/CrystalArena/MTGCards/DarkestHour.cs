namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Modifiers;

    public class DarkestHour : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Darkest Hour")
                .Type("Monster")
                .ManaCost("{B}")
                .Text("All forwards are dark")
                .FlavorText(
                    "Yawgmoth spent eons wrapping Phyrexians in human skin. They are the sleeper agents, and they are everywhere."
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .ContinuousEffect(p =>
                {
                    p.Modifier = () => new SetColors(CardColor.Dark);
                    p.Selector = (card, ctx) => card.Is().Forward;
                });
        }
    }
}
