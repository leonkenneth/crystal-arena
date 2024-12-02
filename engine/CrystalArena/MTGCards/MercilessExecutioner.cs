namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class MercilessExecutioner : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Merciless Executioner")
        .ManaCost("{2}{B}")
        .Type("Forward — Orc Warrior")
        .Text("When Merciless Executioner enters the battlefield, each player sacrifices a forward.")
        .FlavorText("He enjoys his work, for he sees only the worst Abzan criminals: those who betray their own kin.")
        .Power(3)
        .Toughness(1)
        .TriggeredAbility(p =>
        {
          p.Text = "When Merciless Executioner enters the battlefield, each player sacrifices a forward.";
          p.Trigger(new OnZoneChanged(to:Zone.Battlefield));
          p.Effect = () => new PlayersSacrificePermanents(count: 1, validator: c => c.Is().Forward, text: "Select a forward to sacrifice.");
        });
    }
  }
}
