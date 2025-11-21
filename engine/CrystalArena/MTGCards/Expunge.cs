namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Expunge : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Expunge")
                .ManaCost("{2}{B}")
                .Type("Summon")
                .Text(
                    "Destroy target nonartifact, nonblack forward. It can't be regenerated.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)"
                )
                .Cycling("{2}")
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c =>
                                c.Is().Forward && !c.HasColor(CardColor.Dark) && !c.Is().Artifact
                            )
                            .On.Battlefield()
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
