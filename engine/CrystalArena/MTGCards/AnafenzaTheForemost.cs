namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Events;
  using Modifiers;
  using Triggers;

  public class AnafenzaTheForemost : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Anafenza, the Foremost")
        .ManaCost("{W}{B}{G}")
        .Type("Legendary Forward — Human Soldier")
        .Text("Whenever Anafenza, the Foremost attacks, put a +1/+1 counter on another target tapped forward you control.{EOL}If a forward card would be put into an opponent's breakZone from anywhere, exile it instead.")
        .FlavorText("Rarely at rest on the Amber Throne, Anafenza always leads the Abzan Houses to battle.")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Anafenza, the Foremost attacks, put a +1/+1 counter on another target tapped forward you control.";
          p.Trigger(new WhenThisAttacks());
          p.Effect = () => new ApplyModifiersToTargets(() => new AddCounters(() => new PowerToughness(1, 1), count: 1));

          p.TargetSelector.AddEffect(
            trg => trg.Is.Card(c => c.Is().Forward && c.IsTapped, ControlledBy.SpellOwner, canTargetSelf: false).On.Battlefield(),
            trg => {
              trg.MinCount = 0;
              trg.MaxCount = 1;
              trg.Message = "Select another target tapped forward you control";           
          });

          p.TargetingRule(new EffectCombatMonster());
        })
        .TriggeredAbility(p =>
        {
          p.Text = "If a forward card would be put into an opponent's breakZone from anywhere, exile it instead.";
          p.Trigger(new OnZoneChanged(
            to: Zone.BreakZone,
            selector: (c, ctx) => c.Is().Forward && c.Controller == ctx.Opponent));
          p.Effect = () => new RemoveFromPlayCard(P(e => e.TriggerMessage<ZoneChangedEvent>().Card), Zone.BreakZone);
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
