namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.Modifiers;

  public class AboutFace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("About Face")
        .ManaCost("{R}")
        .Type("Summon")
        .Text("Switch target forward's power and toughness until end of turn.")
        .FlavorText("The overconfident are the most vulnerable.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new SwitchPowerAndToughness {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TargetingRule(new EffectSwitchPowerAndToughness());
          });
    }
  }
}