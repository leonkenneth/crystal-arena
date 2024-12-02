namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class CollateralDamage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Collateral Damage")
        .ManaCost("{R}")
        .Type("Summon")
        .Text(
          "As an additional cost to cast Collateral Damage, sacrifice a forward.{EOL}Collateral Damage deals 3 damage to target forward or player.")
        .FlavorText("It is much easier to create fire than to contain it.")
        .Cast(p =>
          {
            p.Cost = new AggregateCost(
              new PayMana("{R}".Parse()),
              new Sacrifice());

            p.TargetSelector.AddCost(
              trg => trg.Is.Card(c => c.Is().Forward, ControlledBy.SpellOwner).On.Battlefield(),
              trg => trg.Message = "Select a forward to sacrifice.");

            p.Text = "Collateral Damage deals 3 damage to target forward or player.";
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

            p.TargetingRule(new CostSacrificeEffectDealDamage(3));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new WhenStackIsNotEmpty()));
          });
    }
  }
}