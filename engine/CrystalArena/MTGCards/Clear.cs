namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Clear : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Clear")
                .ManaCost("{1}{W}")
                .Type("Summon")
                .Text(
                    "Destroy target monster.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)"
                )
                .Cycling("{2}")
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(trg => trg.Is.Monster().On.Battlefield());

                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Destroy));
                });
        }
    }
}
