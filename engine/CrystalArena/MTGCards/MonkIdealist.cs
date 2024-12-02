namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class MonkIdealist : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Monk Idealist")
        .ManaCost("{2}{W}")
        .Type("Forward - Human Monk Cleric")
        .Text(
          "When Monk Idealist enters the battlefield, return target monster card from your breakZone to your hand.")
        .FlavorText("Belief is the strongest mortar.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(2)
        .Toughness(2)
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYourBreakZoneCountIs(minCount: 1, selector: c => c.Is().Monster));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Monk Idealist enters the battlefield, return target monster card from your breakZone to your hand.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ReturnToHand();
            p.TargetSelector.AddEffect(
              trg => trg.Is.Monster().In.YourBreakZone(),
              trg => trg.Message = "Select an monster in your breakZone.");

            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          });
    }
  }
}