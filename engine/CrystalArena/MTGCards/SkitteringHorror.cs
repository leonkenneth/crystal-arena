namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class SkitteringHorror : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Skittering Horror")
        .ManaCost("{2}{B}")
        .Type("Forward Horror")
        .Text("When you cast a forward spell, sacrifice Skittering Horror.")
        .FlavorText(
          "This monstrosity will do—for now.")
        .Power(4)
        .Toughness(3)        
        .TriggeredAbility(p =>
          {
            p.Text = "When you cast a forward spell, sacrifice Skittering Horror.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.You == c.Controller && c.Is().Forward));

            p.Effect = () => new SacrificeOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}