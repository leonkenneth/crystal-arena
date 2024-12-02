namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class ParagonOfNewDawns : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Paragon of New Dawns")
        .ManaCost("{3}{W}")
        .Type("Forward — Human Soldier")
        .Text("Other light forwards you control get +1/+1.{EOL}{W},{T}: Another target light forward you control gains brave until end of turn.{I}(Attacking doesn't cause it to tap.){/I}")
        .Power(2)
        .Toughness(2)
        .Cast(p =>
        {
          p.Effect = () => new CastPermanent().SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        })
        .ContinuousEffect(p =>
        {
          p.Modifier = () => new AddPowerAndToughness(1, 1);
          p.Selector = (c, ctx) => c.Controller == ctx.You && c.Is().Forward && c.HasColor(CardColor.Light) && c != ctx.Source;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{W},{T}: Another target light forward you control gains brave until end of turn.";
          p.Cost = new AggregateCost(new PayMana("{W}".Parse()), new Tap());
          p.Effect = () => new ApplyModifiersToTargets(() => new AddSimpleAbility(Static.Brave){ UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Forward && c.HasColor(CardColor.Light), controlledBy: ControlledBy.SpellOwner, canTargetSelf: false)
              .On.Battlefield());

          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectOrCostRankBy(c => c.Has().Brave ? c.Score : -c.Score, controlledBy: ControlledBy.SpellOwner));
        });
    }
  }
}
