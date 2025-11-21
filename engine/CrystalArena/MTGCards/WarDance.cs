namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class WarDance : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("War Dance")
                .ManaCost("{G}")
                .Type("Monster")
                .Text(
                    "At the beginning of your upkeep, you may put a verse counter on War Dance.{EOL}Sacrifice War Dance: Target forward gets +X/+X until end of turn, where X is the number of verse counters on War Dance."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of your upkeep, you may put a verse counter on War Dance.";

                    p.Trigger(new OnStepStart(step: Step.Upkeep));

                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new AddCounters(() => new SimpleCounter(CounterType.Verse), 1)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "Sacrifice War Dance: Target forward gets +X/+X until end of turn, where X is the number of verse counters on War Dance.";
                    p.Cost = new Sacrifice();
                    p.Effect = () => new Add11ForEachCounter();
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new WhenCardHasCounters(minCount: 3, onlyAtEot: false));
                    p.TimingRule(new PumpTargetCardTimingRule());
                    p.TargetingRule(new EffectPumpSummon(3, 3));
                });
        }
    }
}
