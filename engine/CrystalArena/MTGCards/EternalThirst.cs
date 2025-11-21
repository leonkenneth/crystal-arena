namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;
    using Triggers;

    public class EternalThirst : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Eternal Thirst")
                .ManaCost("{1}{B}")
                .Type("Monster — Aura")
                .Text(
                    "Enchant forward{EOL}Enchanted forward has lifelink and \"Whenever a forward an opponent controls dies, put a +1/+1 counter on this forward.\" {I}(Damage dealt by a forward with lifelink also causes its controller to gain that much life.){/I}"
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(
                            () => new AddSimpleAbility(Static.Lifelink),
                            () =>
                            {
                                var tp = new TriggeredAbility.Parameters()
                                {
                                    Text =
                                        "Whenever a forward an opponent controls dies, put a +1/+1 counter on this forward.",
                                    Effect = () =>
                                        new ApplyModifiersToSelf(() =>
                                            new AddCounters(
                                                () => new PowerToughness(1, 1),
                                                count: 1
                                            )
                                        ).SetTags(
                                            EffectTag.IncreasePower,
                                            EffectTag.IncreaseToughness
                                        ),
                                };

                                tp.Trigger(
                                    new OnZoneChanged(
                                        from: Zone.Battlefield,
                                        to: Zone.BreakZone,
                                        selector: (c, ctx) =>
                                            c.Is().Forward && c.Controller == ctx.Opponent
                                    )
                                );

                                return new AddTriggeredAbility(new TriggeredAbility(tp));
                            }
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                });
        }
    }
}
