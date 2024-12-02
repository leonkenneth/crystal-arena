namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PathToRemovedFromPlay
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void RemoveFromPlayDragonItsControllerSearchesBackup()
      {
        var backup = C("Forest");
        var dragon = C("Shivan Dragon");

        MainDeck(P1, backup);
        Battlefield(P1, dragon);

        P2.Life = 5;
        Hand(P2, "Path To RemoveFromPlay");
        Battlefield(P2, "Plains");

        RunGame(1);

        Equal(Zone.RemovedFromPlay, C(dragon).Zone);
        Equal(Zone.Battlefield, C(backup).Zone);
      }
    }
  }
}
