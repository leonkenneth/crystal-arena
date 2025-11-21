namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class VinesOfVastwood : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Vines of Vastwood")
                .ManaCost("{G}")
                .Type("Summon")
                .Text(
                    "{Kicker} {G}{EOL}Target forward can't be the target of spells or abilities your opponents control this turn. If Vines of Vastwood was kicked, that forward gets +4/+4 until end of turn."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddSimpleAbility(Static.Hexproof) { UntilEot = true }
                        ).SetTags(EffectTag.Shroud);

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TargetingRule(new EffectProtect());
                })
                .Cast(p =>
                {
                    p.Text = p.KickerDescription;
                    p.Cost = new PayMana("{G}{G}".Parse());
                    p.Effect = () =>
                        new ApplyModifiersToTargets(
                            () => new AddSimpleAbility(Static.Hexproof) { UntilEot = true },
                            () => new AddPowerAndToughness(4, 4) { UntilEot = true }
                        ).SetTags(
                            EffectTag.Shroud,
                            EffectTag.IncreasePower,
                            EffectTag.IncreaseToughness
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new PumpTargetCardTimingRule());
                    p.TargetingRule(new EffectPumpSummon(4, 4));
                });
        }
    }
}
