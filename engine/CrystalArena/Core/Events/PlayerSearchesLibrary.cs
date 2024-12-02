namespace CrystalArena.Events
{
  public class PlayerSearchesMainDeck
  {
    public readonly Player Player;

    public PlayerSearchesMainDeck(Player player)
    {
      Player = player;
    }
  }
}
