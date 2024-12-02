namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class CarnivalOfSouls : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Carnival of Souls")
        .ManaCost("{1}{B}")
        .Type("Monster")
        .Text("Whenever a forward enters the battlefield, you lose 1 life and add {B} to your mana pool.")
        .FlavorText(
          "Davvol, blast those elves.' ‘Davvol, transport those troops.' No one cares that today is my birthday.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever a forward enters the battlefield, you lose 1 life and add {B} to your mana pool.";
            p.Trigger(new OnZoneChanged(
              to: Zone.Battlefield,
              selector: (c, ctx) => c.Is().Forward));

            p.Effect = () => new CompoundEffect(
              new ChangeLife(amount: -1, whos: P(e => e.Controller)),
              new AddManaToPool(Mana.Dark));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}