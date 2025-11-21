namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Swat : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Swat")
                .ManaCost("{1}{B}{B}")
                .Type("Summon")
                .Text(
                    "Destroy target forward with power 2 or less.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)"
                )
                .Cycling("{2}")
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Forward && c.Power <= 2).On.Battlefield()
                    );

                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(
                        new TargetRemovalTimingRule().RemovalTags(
                            EffectTag.Destroy,
                            EffectTag.ForwardsOnly
                        )
                    );
                });
        }
    }
}
