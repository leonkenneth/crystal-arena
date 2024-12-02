namespace CrystalArena.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class VeiledSentry
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void Becomes11Forward()
      {
        var sentry = C("Veiled Sentry");
        var shock = C("Shock");
        
        Battlefield(P1, sentry);
        Hand(P2, shock);

        Exec(
          At(Step.FirstMain)
            .Cast(shock, target: P1)
            .Verify(() =>
              {
                Equal(1, C(sentry).Power);
                Equal(1, C(sentry).Toughness);
              })
          );
      }
    }
  }
}