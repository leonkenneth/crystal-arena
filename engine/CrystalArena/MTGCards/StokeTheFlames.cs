namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class StokeTheFlames : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Stoke the Flames")
        .ManaCost("{2}{R}{R}")
        .Type("Summon")
        .Text("{Convoke} {I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Stoke the Flames deals 4 damage to target forward or player.")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
        {
          p.Effect = () => new DealDamageToTargets(4);
          p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
          p.TargetingRule(new EffectDealDamage(4));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
        });
    }
  }
}
