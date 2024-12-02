namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ObNixilisUnshackled : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ob Nixilis, Unshackled")
        .ManaCost("{4}{B}{B}")
        .Type("Legendary Forward — Demon")
        .Text("{Flying}, {trample}{EOL}Whenever an opponent searches his or her library, that player sacrifices a forward and loses 10 life.{EOL}Whenever another forward dies, put a +1/+1 counter on Ob Nixilis, Unshackled.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Flying, Static.Trample)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever an opponent searches his or her library, that player sacrifices a forward and loses 10 life.";
          p.Trigger(new WhenPlayerSearchesMainDeck(
            (player, ctx) => ctx.Opponent == player));
          
          p.Effect = () => new CompoundEffect(
            
            new PlayerSacrificePermanents(
              count: 1,
              player: P(e => e.Controller.Opponent),
              filter: c => c.Is().Forward,
              text: "Select a forward to sacrifice."),

            new ChangeLife(amount: -10, whos: P(e => e.Controller.Opponent)));

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        })
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever another forward dies, put a +1/+1 counter on Ob Nixilis, Unshackled.";

          p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.BreakZone, 
            selector: (c, ctx) => c.Is().Forward && ctx.OwningCard != c));

          p.TriggerOnlyIfOwningCardIsInPlay = true;

          p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1))
            .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        });
    }
  }
}
