namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class GoForTheThroat : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Go for the Throat")
                .ManaCost("{1}{B}")
                .Type("Summon")
                .Text("Destroy target nonartifact forward.")
                .FlavorText("Having flesh is increasingly a liability on Mirrodin.")
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Forward && !c.Is().Artifact).On.Battlefield()
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
