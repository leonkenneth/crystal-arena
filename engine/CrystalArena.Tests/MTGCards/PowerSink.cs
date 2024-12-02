namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class PowerSink
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void CounterForce()
      {
        var force = C("Verdant Force");

        Hand(P1, force);
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
        Hand(P2, "Power Sink");
        Battlefield(P2, "Island", "Island", "Island");

        RunGame(1);

        Equal(Zone.BreakZone, C(force).Zone);
        Equal(8, P1.Battlefield.Backups.Count(x => x.IsTapped));
      }
    }
  }
}