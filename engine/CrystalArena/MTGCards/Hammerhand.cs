namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;
    using Triggers;

    public class Hammerhand : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hammerhand")
                .ManaCost("{R}")
                .Type("Monster — Aura")
                .Text(
                    "Enchant forward{EOL}When Hammerhand enters the battlefield, target forward can't block this turn.{EOL}Enchanted forward gets +1/+1 and has haste. {I}(It can attack and no matter when it came under your control.){/I}"
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(
                            () => new AddPowerAndToughness(1, 1),
                            () => new AddSimpleAbility(Static.Haste)
                        ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Hammerhand enters the battlefield, target forward can't block this turn.";

                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddSimpleAbility(Static.CannotBlock) { UntilEot = true }
                        );
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TargetingRule(new EffectCannotBlockAttack(blockOnly: true));
                });
        }
    }
}
