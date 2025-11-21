namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class HonorsReward : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Honor's Reward")
                .ManaCost("{2}{W}")
                .Type("Summon")
                .Text(
                    "You gain 4 life. Bolster 2.{I}(Choose a forward with the least toughness among forwards you control and put two +1/+1 counters on it.){/I}"
                )
                .FlavorText(
                    "It seldom rains in Abzan backups. When it does, the khan marks the occasion by honoring the families of the fallen."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CompoundEffect(
                            new ChangeLife(amount: 4, whos: P(e => e.Controller)),
                            new ApplyModifiersToTargets(() =>
                                new AddCounters(() => new PowerToughness(1, 1), count: 2)
                            )
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

                    p.TimingRule(
                        new Any(new OnEndOfOpponentsTurn(), new WhenYourLifeCanBecomeZero())
                    );
                });
        }
    }
}
