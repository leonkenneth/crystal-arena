namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Hibernation : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hibernation")
                .ManaCost("{2}{U}")
                .Type("Summon")
                .Text("Return all wind permanents to their owners' hands.")
                .FlavorText(
                    "On its way to the cave, the armadillo brushed by a sapling. It awoke to find a full-grown tree blocking its path."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ReturnAllPermanentsToHand((c) => c.HasColor(CardColor.Wind));
                    p.TimingRule(new Any(new OnFirstMain(), new AfterOpponentDeclaresAttackers()));
                });
        }
    }
}
