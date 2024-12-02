namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class LastDitchEffort : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Last-Ditch Effort")
        .ManaCost("{R}")
        .Type("Summon")
        .Text(
          "Sacrifice any number of forwards. Last-Ditch Effort deals that much damage to target forward or player.")
        .FlavorText("If you're gonna lose, at least make sure they don't win as much.")
        .Cast(p =>
          {
            p.Effect = () => new SacrificeToDealDamageToTarget(c => c.Is().Forward);
            p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(dp => dp.Controller.Battlefield.Forwards.Count()));

            p.TimingRule(new WhenYouHavePermanents(c => c.Is().Forward, minCount: 1));
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
          });
    }
  }
}