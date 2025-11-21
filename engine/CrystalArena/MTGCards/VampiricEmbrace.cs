namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;
    using Triggers;

    public class VampiricEmbrace : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Vampiric Embrace")
                .ManaCost("{2}{B}{B}")
                .Type("Monster Aura")
                .Text(
                    "Enchanted forward gets +2/+2 and has flying.{EOL}Whenever a forward dealt damage by enchanted forward this turn dies, put a +1/+1 counter on that forward."
                )
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever a forward dealt damage by enchanted forward this turn dies, put a +1/+1 counter on that forward.";

                    p.Trigger(
                        new OnZoneChanged(
                            from: Zone.Battlefield,
                            to: Zone.BreakZone,
                            selector: (c, ctx) =>
                                ctx.Turn.Events.HasBeenDamagedBy(c, ctx.OwningCard.AttachedTo)
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToCard(
                            P(e => e.Source.OwningCard.AttachedTo),
                            () => new AddCounters(() => new PowerToughness(1, 1), 1)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                })
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(
                            () => new AddPowerAndToughness(2, 2),
                            () => new AddSimpleAbility(Static.Flying)
                        ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                });
        }
    }
}
