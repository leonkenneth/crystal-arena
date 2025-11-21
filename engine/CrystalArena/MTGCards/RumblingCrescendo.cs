namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class RumblingCrescendo : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Rumbling Crescendo")
                .ManaCost("{3}{R}{R}")
                .Type("Monster")
                .Text(
                    "At the beginning of your upkeep, you may put a verse counter on Rumbling Crescendo.{EOL}{R}, Sacrifice Rumbling Crescendo: Destroy up to X target backups, where X is the number of verse counters on Rumbling Crescendo."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of your upkeep, you may put a verse counter on Rumbling Crescendo.";
                    p.Trigger(new OnStepStart(Step.Upkeep));
                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new AddCounters(() => new SimpleCounter(CounterType.Verse), 1)
                        );
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{R}, Sacrifice Rumbling Crescendo: Destroy up to X target backups, where X is the number of verse counters on Rumbling Crescendo.";

                    p.Cost = new AggregateCost(new PayMana(Mana.Fire), new Sacrifice());

                    p.Effect = () => new DestroyTargetPermanents();

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Card(c => c.Is().Backup).On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = 0;
                            trg.GetMaxCount = cp => cp.OwningCard.CountersCount();
                        }
                    );

                    p.TimingRule(new WhenCardHasCounters(minCount: 3, onlyAtEot: false));
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectDestroy());
                });
        }
    }
}
