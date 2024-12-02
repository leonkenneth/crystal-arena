namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Ulcerate
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void KillHorror()
      {
        var horror = C("Skittering Horror");
        Battlefield(P1, horror);
        Hand(P2, "Ulcerate");
        Battlefield(P2, "Swamp");


        RunGame(1);
        Equal(Zone.BreakZone, C(horror).Zone);
        Equal(17, P2.Life);
      }
    }
  }
}