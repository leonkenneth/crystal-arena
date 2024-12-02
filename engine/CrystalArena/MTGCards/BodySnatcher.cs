namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class BodySnatcher : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Body Snatcher")
        .ManaCost("{2}{B}{B}")
        .Type("Forward Minion")
        .Text(
          "When Body Snatcher enters the battlefield, exile it unless you discard a forward card.{EOL}When Body Snatcher dies, exile Body Snatcher and return target forward card from your breakZone to the battlefield.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(2)
        .Toughness(2)
        .Cast(p => p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.Is().Forward)))
        .TriggeredAbility(p =>
          {
            p.Text = "When Body Snatcher enters the battlefield, exile it unless you discard a forward card.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new RemoveFromPlayOwnerUnlessYouDiscardForwardCard();
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Body Snatcher dies, exile Body Snatcher and return target forward card from your breakZone to the battlefield.";

            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));

            p.Effect = () => new CompoundEffect(
              new RemoveFromPlayOwner(),
              new PutTargetsToBattlefield());

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().In.YourBreakZone());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          });
    }
  }
}