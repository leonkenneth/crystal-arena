namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class TimelyHordemate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Timely Hordemate")
        .ManaCost("{3}{W}")
        .Type("Forward — Human Warrior")
        .Text("{I}Raid{/I} — When Timely Hordemate enters the battlefield, if you attacked with a forward this turn, return target forward card with converted mana cost 2 or less from your breakZone to the battlefield.")
        .Power(3)
        .Toughness(2)
        .TriggeredAbility(p =>
        {
          p.Text = "When Timely Hordemate enters the battlefield, if you attacked with a forward this turn, return target forward card with converted mana cost 2 or less from your breakZone to the battlefield.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
          {
            Condition = ctx => ctx.Turn.Events.HasActivePlayerAttackedThisTurn,
          });

          p.Effect = () => new PutTargetsToBattlefield();
          p.TargetSelector.AddEffect(
            trg => trg.Is.Card(c => c.Is().Forward && c.ConvertedCost <= 2).In.YourBreakZone());
          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
        });
    }
  }
}
