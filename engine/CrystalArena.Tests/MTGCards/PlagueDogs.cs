namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PlagueDogs
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyOpponentsForwards()
      {
        Battlefield(P1, "Plague Dogs", "Plague Dogs", "Swamp", "Swamp");
        Battlefield(P2, "Llanowar Elves", "Birds of Paradise", "Elvish Lyrist", "Llanowar Elves", "Savannah Lions");

        RunGame(2);

        Equal(1, P1.Hand.Count);
        Equal(5, P2.BreakZone.Count);
      }
    }
  }
}