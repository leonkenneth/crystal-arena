namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class Humble : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Humble")
        .ManaCost("{1}{W}")
        .Type("Summon")
        .Text("Target forward loses all abilities and becomes 0/1 until end of turn.")
        .FlavorText("It is not your place to rule, Radiant. It may not even be mine.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new DisableAllAbilities(activated: true, simple: true, triggered: true) {UntilEot = true},
              () => new SetPowerAndToughness(0, 1) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Humble, combatOnly: true));
          });
    }
  }
}