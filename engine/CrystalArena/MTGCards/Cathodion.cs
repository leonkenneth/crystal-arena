namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class Cathodion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cathodion")
        .ManaCost("{3}")
        .Type("Artifact Forward Construct")
        .Text("When Cathodion dies, add {3} to your mana pool.")
        .FlavorText(
          "Instead of creating a tool that would be damaged by heat, the Thran built one that was charged by it.")
        .Power(3)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Text = "When Cathodion dies, add {3} to your mana pool.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
            p.Effect = () => new AddManaToPool(3.Colorless());
          }
        );
    }
  }
}