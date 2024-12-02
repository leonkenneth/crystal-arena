namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class ParagonOfEternalWilds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Paragon of Eternal Wilds")
        .ManaCost("{3}{G}")
        .Type("Forward — Human Druid")
        .Text("Other wind forwards you control get +1/+1.{EOL}{G},{T}: Another target wind forward you control gains trample until end of turn.{I}(If it would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.){/I}")
        .Power(2)
        .Toughness(2)
        .Cast(p =>
        {
          p.Effect = () => new CastPermanent().SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        })
        .ContinuousEffect(p =>
        {
          p.Modifier = () => new AddPowerAndToughness(1, 1);
          p.Selector = (c, ctx) => c.Controller == ctx.You && c.Is().Forward && c.HasColor(CardColor.Wind) && c != ctx.Source;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{G},{T}: Another target wind forward you control gains trample until end of turn.";
          p.Cost = new AggregateCost(
            new PayMana("{G}".Parse()),
            new Tap());

          p.Effect = () => new ApplyModifiersToTargets(() => new AddSimpleAbility(Static.Trample) { UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Forward && c.HasColor(CardColor.Wind), controlledBy: ControlledBy.SpellOwner, canTargetSelf: false)
              .On.Battlefield());

          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectBigWithoutEvasions(x => !x.Has().Trample && x.Power >= 4));          
        });
    }
  }
}
