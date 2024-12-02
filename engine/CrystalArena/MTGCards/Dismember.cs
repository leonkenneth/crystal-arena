namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Dismember : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dismember")
        .ManaCost("{1}{BP}{BP}")
        .Type("Summon")
        .Text("{I}({BP} can be paid with either {B} or 2 life.){/I}{EOL}Target forward gets -5/-5 until end of turn.")
        .FlavorText("\"You serve Phyrexia. Your pieces would better serve Phyrexia elsewhere.\"{EOL}—Azax-Azog, the Demon Thane")
        .Cast(p =>
        {
          p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(-5, -5) { UntilEot = true }) { ToughnessReduction = 5 };

          p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
          p.TargetingRule(new EffectReduceToughness(5));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.ReduceToughness));
        });
    }
  }
}
