namespace CrystalArena.Effects
{
  using System.Linq;

  public class EachPlayerShufflesHandAndBreakZoneIntoMainDeckAndDrawsCards : Effect
  {
    private readonly int _count;
    private readonly bool _onlyYouDraw;

    private EachPlayerShufflesHandAndBreakZoneIntoMainDeckAndDrawsCards() {}

    public EachPlayerShufflesHandAndBreakZoneIntoMainDeckAndDrawsCards(int count, bool onlyYouDraw = false)
    {
      _count = count;
      _onlyYouDraw = onlyYouDraw;
    }

    protected override void ResolveEffect()
    {
      ShuffleIntoMainDeck(Players.Active);
      ShuffleIntoMainDeck(Players.Passive);
    }

    private void ShuffleIntoMainDeck(Player player)
    {
      foreach (var card in player.Hand.ToList())
      {
        player.PutOnBottomOfMainDeck(card);
      }

      foreach (var card in player.BreakZone.ToList())
      {
        player.PutOnBottomOfMainDeck(card);
      }

      player.ShuffleMainDeck();

      if (!_onlyYouDraw || player == Controller)
      {
        player.DrawCards(_count);
      }  
    }
  }
}