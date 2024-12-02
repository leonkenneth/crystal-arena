namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MartyrsCause
  {
    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact (Skip = "Old card")]
      public void SacBearPreventDamageToPlayer()
      {
        var shock = C("Shock");
        
        Hand(P1, shock);
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Martyr's Cause");
        P2.Life = 2;

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P2),
          At(Step.SecondMain)
            .Verify(() =>
              {
                Equal(1, P2.BreakZone.Count);
                Equal(2, P2.Life);
              })
          );

      }
    }
  }
}