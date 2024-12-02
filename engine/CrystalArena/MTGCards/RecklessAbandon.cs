namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class RecklessAbandon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Reckless Abandon")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text(
          "As an additional cost to cast Reckless Abandon, sacrifice a forward.{EOL}Reckless Abandon deals 4 damage to target forward or player.")
        .FlavorText("The climax of a warlord's career is always death.")
        .Cast(p =>
          {
            p.Cost = new AggregateCost(
              new PayMana(Mana.Fire),
              new Sacrifice());

            p.Effect = () => new DealDamageToTargets(4);
            p.TargetSelector
              .AddCost(
                trg => trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield(),
                trg => { trg.Message = "Select a forward to sacrifice."; })
              .AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

            p.TargetingRule(new CostSacrificeEffectDealDamage(4));
          });
    }
  }
}