namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class DragonBlood : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Dragon Blood")
                .ManaCost("{3}")
                .Type("Artifact")
                .Text("{3},{T} : Put a +1/+1 counter on target forward.")
                .FlavorText("Fire in the blood, fire in the belly.")
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .ActivatedAbility(p =>
                {
                    p.Text = "{3},{T} : Put a +1/+1 counter on target forward.";
                    p.Cost = new AggregateCost(new PayMana(3.Colorless()), new Tap());

                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddCounters(() => new PowerToughness(1, 1), count: 1)
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new PumpTargetCardTimingRule(untilEot: false));
                    p.TargetingRule(new EffectPumpSummon(1, 1, untilEot: false));
                });
        }
    }
}
