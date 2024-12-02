namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BloodsoakedChampion
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void ReturnChampionToBattlefield()
      {
        var champion = C("Bloodsoaked Champion");

        Battlefield(P1, "Grizzly Bears", "Swamp", "Mountain");
        BreakZone(P1, champion);

        RunGame(1);

        Equal(Zone.Battlefield, C(champion).Zone);
      }
    }
  }
}
