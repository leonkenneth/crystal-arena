namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class OppressiveRays : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Oppressive Rays")
        .ManaCost("{W}")
        .Type("Monster - Aura")
        .Text(
          "Enchant forward{EOL}Enchanted forward can't attack or block unless its controller pays {3}.{EOL}Activated abilities of enchanted forward cost {3} more to activate.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new IncreaseCombatCost(3),
              () => new AddCostModifier(new ChangeManaCostOfEnchantedForwardsAbilities(3)))
              .SetTags(EffectTag.CombatDisabler);

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCannotBlockAttack());
          });
    }
  }
}