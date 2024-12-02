namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class EasternPaladin
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact (Skip = "Old card")]
      public void DestroyVerdantForce()
      {
        Battlefield(P1, "Verdant Force");
        Battlefield(P2, "Eastern Paladin", "Swamp", "Swamp");

        Exec(
          At(Step.Upkeep, turn: 2)
            .Verify(() => Equal(1, P1.BreakZone.Count()))
          );
      }
    }
  }
}