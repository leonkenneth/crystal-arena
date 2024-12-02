namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class Scrapheap : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scrapheap")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("Whenever an artifact or monster is put into your breakZone from the battlefield, you gain 1 life.")
        .FlavorText("Junk heaps have rats, but scrapheaps have goblins.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever an artifact or monster is put into your breakZone from the battlefield, you gain 1 life.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.BreakZone,
              selector: (c, ctx) => c.Owner == ctx.You && (c.Is().Artifact || c.Is().Monster)));

            p.Effect = () => new ChangeLife(amount: 1, whos: P(e => e.Controller));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}