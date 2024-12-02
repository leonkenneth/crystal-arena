namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class NetcasterSpider
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void BlockForwardWithFlyingIncreasePower()
      {
        Battlefield(P1, "Grizzly Bears", "Kapsho Kitefins");

        P2.Life = 3;
        Battlefield(P2, "Netcaster Spider");

        RunGame(1);

        Equal(1, P2.Life);
        Equal(1, P1.Battlefield.Count);
      }

      [Fact (Skip = "Old card")]
      public void BlockForwardWithoutFlying()
      {
        Battlefield(P1, "Juggernaut");

        P2.Life = 5;
        Battlefield(P2, "Netcaster Spider");

        RunGame(1);

        Equal(5, P2.Life);
        Equal(1, P1.Battlefield.Count);
        Equal(0, P2.Battlefield.Count);
      }
    }
  }
}
