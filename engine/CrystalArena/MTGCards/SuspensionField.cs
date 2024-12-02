namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class SuspensionField : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Suspension Field")
        .ManaCost("{1}{W}")
        .Type("Monster")
        .Text(
          "When Suspension Field enters the battlefield, you may exile target forward with toughness 3 or greater until Suspension Field leaves the battlefield. {I}(That forward returns under its owner's control.){/I}")
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

          p.Effect = () => new RemoveFromPlayTargetsUntilOwnerLeavesBattlefield();

          p.TargetSelector.AddEffect(
            trg => trg.Is.Card(c => c.Is().Forward && c.Toughness >= 3).On.Battlefield());

          p.TargetingRule(new EffectRemoveFromPlayBattlefield());
        });
    }
  }
}
