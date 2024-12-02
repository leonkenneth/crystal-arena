namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class DragonscaleGeneral
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void PutCountersOnBear()
      {
        var bear = C("Grizzly Bears");
        Battlefield(P1, "Dragonscale General", bear);

        RunGame(1);

        Equal(4, C(bear).Power);
      }
    }
  }
}
