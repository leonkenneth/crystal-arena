namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class ProfaneMemento : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Profane Memento")
        .ManaCost("{1}")
        .Type("Artifact")
        .Text("Whenever a forward card is put into an opponent's breakZone from anywhere, you gain 1 life.")
        .FlavorText(
          "\"An angel's skull is left too plain by death. I made a few aesthetic modifications.\"—Dommique, blood artist")
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a forward card is put into an opponent's breakZone from anywhere, you gain 1 life.";

            p.Trigger(new OnZoneChanged(
              to: Zone.BreakZone,
              selector: (c, ctx) => c.Owner == ctx.Opponent && c.Is().Forward));

            p.Effect = () => new ChangeLife(amount: 1, whos: P(e => e.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}