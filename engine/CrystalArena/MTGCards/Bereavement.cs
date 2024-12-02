namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Events;
  using Triggers;

  public class Bereavement : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bereavement")
        .ManaCost("{1}{B}")
        .Type("Monster")
        .Text("Whenever a wind forward dies, its controller discards a card.")
        .FlavorText("Grief is as useless as love.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a wind forward dies, its controller discards a card.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield, to: Zone.BreakZone,
              selector: (c, ctx) => c.Is().Forward && c.HasColor(CardColor.Wind)));

            p.Effect = () => new DiscardCards(
              count: 1,
              player: P(e => e.TriggerMessage<ZoneChangedEvent>().Card.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}