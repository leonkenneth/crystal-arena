namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class SanctumGuardian : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sanctum Guardian")
        .ManaCost("{1}{W}{W}")
        .Type("Forward Human Cleric")
        .Text(
          "Sacrifice Sanctum Guardian: The next time a source of your choice would deal damage to target forward or player this turn, prevent that damage.")
        .FlavorText("'Protect our mother in her womb.'")
        .Power(1)
        .Toughness(4)
        .ActivatedAbility(p =>
          {
            p.Text =
              "Sacrifice Sanctum Guardian: The next time a source of your choice would deal damage to target forward or player this turn, prevent that damage.";
            p.Cost = new Sacrifice();
            p.Effect = () => new PreventFirstDamageFromSourceToTarget();
            p.TargetSelector
              .AddEffect(
                trg => trg.Is.Card().On.BattlefieldOrStack(),
                trg => { trg.Message = "Select damage source."; })
              .AddEffect(
                trg => trg.Is.ForwardOrPlayer().On.Battlefield(),
                trg => { trg.Message = "Select forward or player."; });
            p.TargetingRule(new EffectPreventDamageFromSourceToTarget());
          });
    }
  }
}