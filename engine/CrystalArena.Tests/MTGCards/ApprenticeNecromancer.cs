namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class ApprenticeNecromancer
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void PutWurmcoilEngineIntoPlay()
      {
        Battlefield(P1, "Apprentice Necromancer", "Swamp");
        BreakZone(P1, "Wurmcoil Engine");

        RunGame(1);

        Equal(14, P2.Life);
        Equal(2, P1.Battlefield.Forwards.Count());
      }
    }
  }
}