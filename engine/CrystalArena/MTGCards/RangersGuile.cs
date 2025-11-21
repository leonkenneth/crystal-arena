namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class RangersGuile : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Ranger's Guile")
                .ManaCost("{G}")
                .Type("Summon")
                .Text(
                    "Target forward you control gets +1/+1 and gains hexproof until end of turn. {I}(It can't be the target of spells or abilities your opponents control.){/I}"
                )
                .FlavorText(
                    "\"You don't survive in the wild by standing in plain sight.\"{EOL}—Garruk Wildspeaker"
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ApplyModifiersToTargets(
                            () => new AddPowerAndToughness(1, 1) { UntilEot = true },
                            () => new AddSimpleAbility(Static.Hexproof) { UntilEot = true }
                        ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Forward(controlledBy: ControlledBy.SpellOwner).On.Battlefield()
                    );

                    p.TargetingRule(new EffectPumpSummon(1, 1));
                    p.TimingRule(new PumpTargetCardTimingRule());
                });
        }
    }
}
