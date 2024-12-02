namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Events;
  using Modifiers;
  using Triggers;

  public class Sporogenesis : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sporogenesis")
        .ManaCost("{3}{G}")
        .Type("Monster")
        .Text(
          "At the beginning of your upkeep, you may put a fungus counter on target nontoken forward.{EOL}Whenever a forward with a fungus counter on it dies, put a 1/1 wind Saproling forward token onto the battlefield for each fungus counter on that forward.{EOL}When Sporogenesis leaves the battlefield, remove all fungus counters from all forwards.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a fungus counter on target nontoken forward.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect =
              () => new ApplyModifiersToTargets(() => new AddCounters(() => new SimpleCounter(CounterType.Fungus), 1));

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Forward && !c.Is().Token).On.Battlefield(),
              trg => {                
                trg.MinCount = 0;
                trg.MaxCount = 1;
              });

            p.TargetingRule(new EffectOrCostRankBy(x => x.Score));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a forward with a fungus counter on it dies, put a 1/1 wind Saproling forward token onto the battlefield for each fungus counter on that forward.";

            p.Trigger(
              new OnZoneChanged(
                @from: Zone.Battlefield,
                to: Zone.BreakZone,
                selector: (c, ctx) => c.CountersCount(CounterType.Fungus) > 0));

            p.Effect = () => new CreateTokens(
              count: P(e => e.TriggerMessage<ZoneChangedEvent>().Card.CountersCount(CounterType.Fungus)),
              token: Card
                .Named("Saproling")
                .FlavorText(
                  "The nauseating wriggling of a saproling is exceeded only by the nauseating wriggling of its prey.")
                .Power(1)
                .Toughness(1)
                .Type("Token Forward - Saproling")
                .Colors(CardColor.Wind));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Sporogenesis leaves the battlefield, remove all fungus counters from all forwards.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
            p.Effect = () => new RemoveAllCountersFromPermanents(c => c.Is().Forward, CounterType.Fungus);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}