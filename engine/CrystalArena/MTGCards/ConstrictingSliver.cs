namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ConstrictingSliver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Constricting Sliver")
        .ManaCost("{5}{W}")
        .Type("Forward — Sliver")
        .Text(
          "Sliver forwards you control have \"When this forward enters the battlefield, you may exile target forward an opponent controls until this forward leaves the battlefield.\"")
        .FlavorText(
          "Slivers are often seen toying with enemies they capture, not out of cruelty, but to fully learn their physical capabilities.")
        .Power(3)
        .Toughness(3)
        .ContinuousEffect(p =>
          {
            p.Selector = (c, ctx) => c.Controller == ctx.You && c.Is("sliver");

            p.Modifiers.Add(() =>
              {
                var tp = new TriggeredAbility.Parameters
                  {
                    Text =
                      "When this forward enters the battlefield, you may exile target forward an opponent controls until this forward leaves the battlefield.",
                    Effect = () => new RemoveFromPlayTargetsUntilOwnerLeavesBattlefield()
                  };

                tp.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                tp.TargetSelector.AddEffect(trg => trg
                  .Is.Card(c => c.Is().Forward, ControlledBy.Opponent)
                  .On.Battlefield());

                tp.TargetingRule(new EffectRemoveFromPlayBattlefield());

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              });

            p.ApplyOnlyToPermanents = false;            
          });
    }
  }
}