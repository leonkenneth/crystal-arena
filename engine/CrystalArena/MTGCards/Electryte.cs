namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Triggers;

  public class Electryte : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Electryte")
        .ManaCost("{3}{R}{R}")
        .Type("Forward - Beast")
        .Text(
          "Whenever Electryte deals combat damage to defending player, it deals damage equal to its power to each blocking forward.")
        .FlavorText("Shivan inhabitants are hardened to fire, so their predators have developed alternative weaponry.")
        .Power(3)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Electryte deals combat damage to defending player, it deals damage equal to its power to each blocking forward.";
            
            p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsDealtByOwningCard &&
                dmg.IsCombat &&
                dmg.IsDealtToPlayer));                                          

            p.Effect = () => new DealDamageToForwardsAndPlayers(
              filterForward: (e, card) => card.IsBlocker,
              amountForward: (e, card) => e.Source.OwningCard.Power.GetValueOrDefault());
          });
    }
  }
}