namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class ParagonOfFierceDefiance : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Paragon of Fierce Defiance")
        .ManaCost("{3}{R}")
        .Type("Forward — Human Warrior")
        .Text(
          "Other fire forwards you control get +1/+1.{EOL}{R},{T}: Another target fire forward you control gains haste until end of turn. {I}(It can attack and {T} this turn.){/I}")
        .Power(2)
        .Toughness(2)
        .Cast(
          p => { p.Effect = () => new CastPermanent().SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness); })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddPowerAndToughness(1, 1);
            p.Selector =
              (c, ctx) => c.Controller == ctx.You && c.Is().Forward && c.HasColor(CardColor.Fire) && c != ctx.Source;
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{R},{T}: Another target fire forward you control gains haste until end of turn.";
            p.Cost = new AggregateCost(
              new PayMana("{R}".Parse()),
              new Tap());

            p.Effect = () => new ApplyModifiersToTargets(() => new AddSimpleAbility(Static.Haste) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Forward && c.HasColor(CardColor.Fire), ControlledBy.SpellOwner,
                canTargetSelf: false)
              .On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, ControlledBy.SpellOwner));
          });
    }
  }
}