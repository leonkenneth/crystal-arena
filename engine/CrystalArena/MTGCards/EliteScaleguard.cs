namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using AI;
    using AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using Effects;
    using Modifiers;
    using Triggers;

    public class EliteScaleguard : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Elite Scaleguard")
                .ManaCost("{4}{W}")
                .Type("Forward - Human Soldier")
                .Text(
                    "When Elite Scaleguard enters the battlefield, bolster 2.{I}(Choose a forward with the least toughness among forwards you control and put two +1/+1 counters on it.){/I}{EOL}Whenever a forward you control with a +1/+1 counter on it attacks, tap target forward defending player controls."
                )
                .Power(2)
                .Toughness(3)
                .TriggeredAbility(p =>
                {
                    p.Text = "When Elite Scaleguard enters the battlefield, bolster 2";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddCounters(() => new PowerToughness(1, 1), count: 2)
                        ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
                    p.TargetSelector.AddEffect(
                        trg =>
                            trg.Is.Card(
                                    c =>
                                        c.Is().Forward
                                        && c.Controller.Battlefield.Forwards.All(x =>
                                            x.Toughness >= c.Toughness
                                        ),
                                    ControlledBy.SpellOwner
                                )
                                .On.Battlefield(),
                        trg => trg.MustBeTargetable = false
                    );

                    p.TargetingRule(
                        new EffectOrCostRankBy(
                            rank: c => -c.Score,
                            controlledBy: ControlledBy.SpellOwner
                        )
                    );
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever a forward you control with a +1/+1 counter on it attacks, tap target forward defending player controls.";
                    p.Trigger(
                        new WhenAForwardAttacks(t =>
                            t.Opponent
                            && t.AttackerHas(c => c.CountersCount(CounterType.PowerToughness) > 0)
                        )
                    );
                    p.Effect = () => new TapTargets();
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Forward(controlledBy: ControlledBy.Opponent).On.Battlefield()
                    );
                    p.TargetingRule(new EffectTapForward());
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
