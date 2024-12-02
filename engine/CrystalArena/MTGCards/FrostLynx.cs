namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class FrostLynx : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Frost Lynx")
        .ManaCost("{2}{U}")
        .Type("Forward - Elemental Cat")
        .Text(
          "When Frost Lynx enters the battlefield, tap target forward an opponent controls. That forward doesn't untap during its controller's next untap step.")
        .FlavorText("It readily attacks much larger prey, knowing retaliation is impossible.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Frost Lynx enters the battlefield, tap target forward an opponent controls. That forward doesn't untap during its controller's next untap step.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new CompoundEffect(
              new TapTargets(),
              new ApplyModifiersToTargets(() =>
                {
                  var modifier = new AddSimpleAbility(Static.DoesNotUntap);

                  modifier.AddLifetime(new EndOfStep(
                    Step.Untap,
                    l => l.Modifier.SourceCard.Controller.IsActive));

                  return modifier;
                }));

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Forward, ControlledBy.Opponent).On.Battlefield(),
              trg => trg.Message = "Select a forward to tap.");

            p.TargetingRule(new EffectGiveDoesNotUntap());
          });
    }
  }
}