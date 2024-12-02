namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class SpectraWard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Spectra Ward")
        .ManaCost("{3}{W}{W}")
        .Type("Monster - Aura")
        .Text("Enchant forward{EOL}Enchanted forward gets +2/+2 and has protection from all colors. This effect doesn't remove Auras. {I}(It can't be blocked, targeted, or dealt damage by anything that's light, water, dark, fire, or wind.){/I}")
        .Cast(p =>
        {
          p.Effect = () => new Attach(
            () => new AddPowerAndToughness(2, 2),
            () => new AddProtectionFromColors(L(CardColor.Dark, CardColor.Water, CardColor.Wind, CardColor.Fire, CardColor.Light)))
            .SetTags(EffectTag.IncreasePower, EffectTag.IncreasePower);

          p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectCombatMonster());
        });
    }
  }
}
