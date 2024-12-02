namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;

  public class SteamBlast : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Steam Blast")
        .ManaCost("{2}{R}")
        .Type("Sorcery")
        .Text(
          "Steam Blast deals 2 damage to each forward and each player.")
        .FlavorText(
          "The viashino knew of the cracked pipes but deliberately left them unmended to bolster the rig's defenses.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToForwardsAndPlayers(
              amountPlayer: 2,
              amountForward: 2);
          });
    }
  }
}