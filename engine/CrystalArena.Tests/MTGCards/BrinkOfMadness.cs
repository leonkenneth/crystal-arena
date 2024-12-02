namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BrinkOfMadness
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void SacrificeDiscardOpponentsHand()
      {
        var brink = C("Brink Of Madness");

        Battlefield(P1, brink);
        Hand(P2, "Swamp", "Swamp", "Swamp");

        RunGame(1);

        Equal(3, P2.BreakZone.Count);
        Equal(Zone.BreakZone, C(brink).Zone);
      }
    }
  }
}