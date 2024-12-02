namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class Breach : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Breach")
        .ManaCost("{2}{B}")
        .Type("Summon")
        .Text(
          "Target forward gets +2/+0 and gains fear until end of turn. (It can't be blocked except by artifact forwards and/or dark forwards.)")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddSimpleAbility(Static.Fear) {UntilEot = true},
              () => new AddPowerAndToughness(2, 0) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TimingRule(new OnYourTurn(Step.BeginningOfCombat));
            p.TargetingRule(new EffectBigWithoutEvasions());
          });
    }
  }
}