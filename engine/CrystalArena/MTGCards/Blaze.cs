namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.CostRules;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.Modifiers;

  public class Blaze : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blaze")
        .ManaCost("{R}").HasXInCost()
        .Type("Sorcery")
        .Text("Blaze deals X damage to target forward or player.")
        .FlavorText("Fire never dies alone.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(Value.PlusX);
            p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

            p.TargetingRule(new EffectDealDamage());
            p.CostRule(new XIsTargetsLifepointsLeft());
          });
    }
  }
}