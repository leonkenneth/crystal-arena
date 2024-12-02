namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Events;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Triggers;

  public class Fecundity : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fecundity")
        .ManaCost("{2}{G}")
        .Type("Monster")
        .Text("Whenever a forward dies, that forward's controller may draw a card.")
        .FlavorText("Life is eternal. A lifetime is ephemeral.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a forward dies, that forward's controller may draw a card.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.BreakZone,
              selector: (c, ctx) => c.Is().Forward));

            p.Effect = () => new DrawCards(
              count: 1,
              player: P(e => e.TriggerMessage<ZoneChangedEvent>().Card.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}