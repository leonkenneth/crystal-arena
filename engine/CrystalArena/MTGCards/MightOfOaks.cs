namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class MightOfOaks : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Might of Oaks")
                .ManaCost("{3}{G}")
                .Type("Summon")
                .Text("Target forward gets +7/+7 until end of turn.")
                .FlavorText("Suddenly, she couldn't see the acorns for the trees.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddPowerAndToughness(7, 7) { UntilEot = true }
                        ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new PumpTargetCardTimingRule());
                    p.TargetingRule(new EffectPumpSummon(7, 7));
                });
        }
    }
}
