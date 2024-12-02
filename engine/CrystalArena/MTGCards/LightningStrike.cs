namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class LightningStrike : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lightning Strike")
        .ManaCost("{1}{R}")
        .Type("Summon")
        .Text("Lightning Strike deals 3 damage to target forward or player.")
        .FlavorText("To wield lightning is to tame chaos.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(3));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}