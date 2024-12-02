namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class WardenOfTheBeyond
  {
    public class Predefined : PredefinedScenario
    {
      [Fact (Skip = "Old card")]
      public void RemoveFromPlayCardGive22()
      {
        var warden = C("Warden Of The Beyond");
        var spell = C("Pillar of Light");
        var wall = C("Wall of Frost");

        Hand(P1, spell);
        Battlefield(P1, warden);
        Battlefield(P2, wall);

        Exec(
          At(Step.FirstMain)
            .Cast(spell, target: wall)
            .Verify(() =>
            {              
              Equal(1, P2.RemovedFromPlay.Count());
              Equal(4, C(warden).Power);
            })
          );
      }
    }
  }
}
