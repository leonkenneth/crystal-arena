namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class PillarOfLight : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Pillar of Light")
                .ManaCost("{2}{W}")
                .Type("Summon")
                .Text("RemoveFromPlay target forward with toughness 4 or greater.")
                .FlavorText(
                    "\"The vaulted ceiling of our faith rests upon such pillars.\"—Darugand, banisher priest"
                )
                .Cast(p =>
                {
                    p.Effect = () => new RemoveFromPlayTargets();

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(card => card.Is().Forward && card.Toughness >= 4)
                            .On.Battlefield()
                    );

                    p.TargetingRule(new EffectRemoveFromPlayBattlefield());
                    p.TimingRule(
                        new TargetRemovalTimingRule().RemovalTags(
                            EffectTag.RemoveFromPlay,
                            EffectTag.ForwardsOnly
                        )
                    );
                });
        }
    }
}
