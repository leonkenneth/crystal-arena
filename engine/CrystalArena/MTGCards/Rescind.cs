namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Rescind : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Rescind")
                .ManaCost("{1}{U}{U}")
                .Type("Summon")
                .Text(
                    "Return target permanent to its owner's hand.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)"
                )
                .Cycling("{2}")
                .Cast(p =>
                {
                    p.Effect = () => new ReturnToHand();
                    p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());

                    p.TargetingRule(new EffectBounce());
                    p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Bounce));
                });
        }
    }
}
