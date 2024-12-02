namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class Gravedigger : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gravedigger")
        .ManaCost("{3}{B}")
        .Type("Forward — Zombie")
        .Text(
          "When Gravedigger enters the battlefield, you may return target forward card from your breakZone to your hand.")
        .FlavorText("A grave is not always for burial.")
        .Power(2)
        .Toughness(2)
        .Cast(p => p.TimingRule(new WhenYourBreakZoneCountIs(c => c.Is().Forward)))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Gravedigger enters the battlefield, you may return target forward card from your breakZone to your hand.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new ReturnToHand();

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().In.YourBreakZone());

            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));            
          });
    }
  }
}