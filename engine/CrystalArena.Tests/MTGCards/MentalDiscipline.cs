namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class MentalDiscipline
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void Discard3Cards()
      {
        Hand(P1, "Island", "Island", "Island");
        Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Island", "Mental Discipline");
        
        RunGame(2);

        Equal(3, P1.BreakZone.Count);
      }
    }
  }
}