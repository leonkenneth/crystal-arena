namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;

  public class WildFire : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wildfire")
        .ManaCost("{4}{R}{R}")
        .Type("Sorcery")
        .Text("Each player sacrifices four backups. Wildfire deals 4 damage to each forward.")
        .FlavorText("'Shiv hatched from a shell of stone around a yolk of flame.'—Viashino myth")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new PlayersSacrificePermanents(count: 4, validator: c => c.Is().Backup, text: "Select backups to sacrifice."),
              new DealDamageToForwardsAndPlayers(amountForward: 4));
          });
    }
  }
}