namespace CrystalArena.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class EndlessWurm
  {
    public class Ai : AiScenario
    {
      [Fact (Skip = "Old card")]
      public void SacrificeWurm()
      {
        Battlefield(P2, "Endless Wurm");
        RunGame(2);

        Equal(1, P2.BreakZone.Count());
      }

      [Fact (Skip = "Old card")]
      public void SacrificeWurmPacifism()
      {
        var wurm = C("Endless Wurm");
        Battlefield(P2, wurm.IsEnchantedWith("Pacifism"), "Seal of Fire");

        RunGame(2);

        Equal(Zone.BreakZone, C(wurm).Zone);
      }

      [Fact (Skip = "Old card")]
      public void SacrificeRancor()
      {
        Battlefield(P2, C("Endless Wurm").IsEnchantedWith("Rancor"), "Forest");

        RunGame(2);
        Equal(9, P1.Life);
      }
    }
  }
}