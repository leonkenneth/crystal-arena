namespace CrystalArena
{
  using System.Collections.Generic;
  using System.Linq;

  public class DraftResults
  {
    public DraftResults(IEnumerable<DraftPlayer> players)
    {
      MainDecks = players.Select(x => x.MainDeck).ToList();
    }

    public List<List<CardInfo>> MainDecks { get; private set; }    
  }
}