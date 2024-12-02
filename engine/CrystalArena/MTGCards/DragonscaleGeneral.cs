namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class DragonscaleGeneral : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dragonscale General")
        .ManaCost("{3}{W}")
        .Type("Forward - Human Warrior")
        .Text("At the beginning of your end step, bolster X, where X is the number of tapped forwards you control.{I}(Choose a forward with the least toughness among forwards you control and put X +1/+1 counters on it.){/I}")
        .FlavorText("\"Dragons seek war. I bring it to them.\"")
        .Power(2)
        .Toughness(3)
        .TriggeredAbility(p =>
        {
          p.Text = "At the beginning of your end step, bolster X, where X is the number of tapped forwards you control.";
          p.Trigger(new OnStepStart(Step.EndOfTurn));
          p.Effect = () => new Put11CountersOnTargets(P(e => e.Controller.Battlefield.Forwards.Count(x => x.IsTapped)));
          
          p.TargetSelector.AddEffect(
            trg => trg.Is.Card(c =>
              c.Is().Forward && c.Controller.Battlefield.Forwards.All(x => x.Toughness >= c.Toughness),
              ControlledBy.SpellOwner).On.Battlefield(),
            trg => trg.MustBeTargetable = false);

          p.TargetingRule(new EffectOrCostRankBy(rank: c => -c.Score, controlledBy: ControlledBy.SpellOwner));
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
