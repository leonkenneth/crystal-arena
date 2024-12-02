namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class VividCreek
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DoNotUseGiveAnyMana()
      {
        var creek = C("Vivid Creek");
        var baloth = C("Ravenous Baloth");

        Hand(P1, creek, baloth);
        Battlefield(P1, "Forest", "Rootbound Crag", "Swamp");

        RunGame(3);

        Equal(2, C(creek).Counters);
        Equal(Zone.Battlefield, C(creek).Zone);
        Equal(Zone.Battlefield, C(baloth).Zone);
      }

      [Fact (Skip = "Old card")]
      public void UseAnyManaAbility()
      {
        var creek = C("Vivid Creek");
        var baloth = C("Ravenous Baloth");

        Hand(P1, creek, baloth);
        Battlefield(P1, "Swamp", "Rootbound Crag", "Swamp");

        RunGame(3);

        Equal(1, C(creek).Counters);
        Equal(Zone.Battlefield, C(creek).Zone);
        Equal(Zone.Battlefield, C(baloth).Zone);
      }
    }
  }
}