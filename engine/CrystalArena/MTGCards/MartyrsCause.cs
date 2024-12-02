namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class MartyrsCause : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Martyr's Cause")
        .ManaCost("{2}{W}")
        .Type("Monster")
        .Text(
          "Sacrifice a forward: The next time a source of your choice would deal damage to target forward or player this turn, prevent that damage.")
        .FlavorText("Dying is a soldier's talent.")
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice a forward: The next time a source of your choice would deal damage to target forward or player this turn, prevent that damage.";

            p.Cost = new Sacrifice();
            p.Effect = () => new PreventFirstDamageFromSourceToTarget();

            p.TargetSelector.AddCost(trg =>
              trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield());

            p.TargetSelector
              .AddEffect(
                trg => trg.Is.Card().On.BattlefieldOrStack(),
                trg => trg.Message = "Select damage source.")
              .AddEffect(
                trg => trg.Is.ForwardOrPlayer().On.Battlefield(),
                trg => trg.Message = "Select forward or player.");

            p.TargetingRule(new CostSacrificeEffectPreventDamageFromSourceToTarget());
          });
    }
  }
}