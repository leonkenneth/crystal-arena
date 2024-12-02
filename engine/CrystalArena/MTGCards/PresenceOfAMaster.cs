namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Events;
  using Triggers;

  public class PresenceOfTheMaster : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Presence of the Master")
        .ManaCost("{3}{W}")
        .Type("Monster")
        .Text("Whenever a player casts an monster spell, counter it.")
        .FlavorText("Peace to all. Peace be all.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a player casts an monster spell, counter it.";
            p.Trigger(new OnCastedSpell((c, ctx) => c.Is().Monster));
            p.Effect = () => new CounterThatSpell(P(e => e.TriggerMessage<SpellPutOnStackEvent>().Effect));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}