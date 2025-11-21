namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class HerosDownfall : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hero's Downfall")
                .ManaCost("{1}{B}{B}")
                .Type("Summon")
                .Text("Destroy target forward or planeswalker.")
                .FlavorText("Destiny exalts a chosen few, but even heroes break.")
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents();

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Forward || c.Is().Planeswalker, ControlledBy.Any)
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
