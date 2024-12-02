namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class SilkNet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Silk Net")
        .ManaCost("{G}")
        .Type("Summon")
        .Text("Target forward gets +1/+1 and gains reach until end of turn. (It can block forwards with flying.)")
        .FlavorText("Sometimes it's possible to pull a meal out of thin air.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(1, 1) {UntilEot = true},
              () => new AddSimpleAbility(Static.Reach) {UntilEot = true})
              .SetTags(EffectTag.IncreaseToughness, EffectTag.IncreasePower, EffectTag.GainReach);

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            
            p.TimingRule(new AfterOpponentDeclaresAttackers());
            p.TargetingRule(new EffectGiveReach());
          });
    }
  }
}